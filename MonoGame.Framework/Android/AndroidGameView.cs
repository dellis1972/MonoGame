using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using OpenTK.Graphics;
using OpenTK.Platform;
using OpenTK.Platform.Android;
using All = OpenTK.Graphics.ES11.All;
using ES11 = OpenTK.Graphics.ES11;
using ES20 = OpenTK.Graphics.ES20;
using Javax.Microedition.Khronos.Egl;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Android.Media;

namespace Microsoft.Xna.Framework
{

	[CLSCompliant (false)]
	public class AndroidGameView : SurfaceView, ISurfaceHolderCallback, View.IOnTouchListener
	{

		bool disposed = false;
		ISurfaceHolder mHolder;
		Size size;
		object lockObject = new object ();

		bool surfaceAvailable;

		int surfaceWidth;
		int surfaceHeight;

		bool glSurfaceAvailable;
		bool glContextAvailable;
		bool lostglContext;
		private bool isPaused;
		private bool isExited = false;
		System.Diagnostics.Stopwatch stopWatch;
		double tick = 0;

		bool loaded = false;

		Task renderTask;
		CancellationTokenSource cts = null;
		private readonly AndroidTouchEventManager _touchManager;
		private readonly AndroidGameWindow _gameWindow;
		private readonly Game _game;

		const int EglContextClientVersion = 0x3098;

		public bool TouchEnabled
		{
			get { return _touchManager.Enabled; }
			set
			{
				_touchManager.Enabled = value;
				SetOnTouchListener(value ? this : null);
			}
		}

		public bool IsResuming { get; private set; }

		public AndroidGameView (Context context, AndroidGameWindow gameWindow, Game game)
			: base (context)
		{
			_gameWindow = gameWindow;
			_game = game;
			_touchManager = new AndroidTouchEventManager(gameWindow);
			Init ();
		}

		private void Init ()
		{
			// default
			mHolder = Holder;
			// Add callback to get the SurfaceCreated etc events
			mHolder.AddCallback (this);
			mHolder.SetType (SurfaceType.Gpu);
		}

		public void SurfaceChanged (ISurfaceHolder holder, global::Android.Graphics.Format format, int width, int height)
		{
			lock (lockObject) {
				Log.Verbose ("AndroidGameView", "SurfaceChanged");
				surfaceWidth = Width;
				surfaceHeight = Height;

				if (_game.GraphicsDevice != null)
					_game.graphicsDeviceManager.ResetClientBounds();
			}
		}

		public void SurfaceCreated (ISurfaceHolder holder)
		{
			lock (lockObject) {
				Log.Verbose ("AndroidGameView", "SurfaceCreated");
				surfaceWidth = Width;
				surfaceHeight = Height;
				surfaceAvailable = true;
				Monitor.PulseAll (lockObject);
			}
		}

		public void SurfaceDestroyed (ISurfaceHolder holder)
		{
			lock (lockObject) {
				Log.Verbose ("AndroidGameView", "SurfaceDestroyed");
				surfaceAvailable = false;
				Monitor.PulseAll (lockObject);
				while (glSurfaceAvailable) {
					Monitor.Wait (lockObject);
				}
			}
		}

		public bool OnTouch (View v, MotionEvent e)
		{
			_touchManager.OnTouchEvent(e);
			return true;
		}

		public virtual void SwapBuffers ()
		{
			EnsureUndisposed ();
			if (!egl.EglSwapBuffers (eglDisplay, eglSurface)) {
				if (egl.EglGetError () == EGL11.EglContextLost) {
					if (lostglContext)
						System.Diagnostics.Debug.WriteLine ("Lost EGL context" + GetErrorAsString ());
					lostglContext = true;
				}
			}

		}

		public virtual void MakeCurrent ()
		{
			EnsureUndisposed ();
			if (!egl.EglMakeCurrent (eglDisplay, eglSurface,
				    eglSurface, eglContext)) {
				System.Diagnostics.Debug.WriteLine ("Error Make Current" + GetErrorAsString ());
			}

		}

		public virtual void ClearCurrent()
		{
			EnsureUndisposed ();
			if (!egl.EglMakeCurrent (eglDisplay, EGL10.EglNoSurface,
				EGL10.EglNoSurface, EGL10.EglNoContext)) {
				System.Diagnostics.Debug.WriteLine ("Error Clearing Current" + GetErrorAsString ());
			}
		}

		double updates;

		public bool LogFPS { get; set; }
		public bool RenderOnUIThread {get;set;}

		public virtual void Run ()
		{
			cts = new CancellationTokenSource ();
			if (LogFPS) {
				targetFps = currentFps = 0;
				avgFps = 1;
			}
			updates = 0;
			var syncContext = new SynchronizationContext ();
			renderTask = Task.Factory.StartNew (() => {
				if (RenderOnUIThread) {
					syncContext.Send((s) => {
						RenderLoop (cts.Token);
					}, null);
				} else
					RenderLoop (cts.Token);
			}, cts.Token)
				.ContinueWith ((t) => {
				OnStopped (EventArgs.Empty);
			});
		}

		public virtual void Run (double updatesPerSecond)
		{
			cts = new CancellationTokenSource ();
			if (LogFPS) {
				avgFps = targetFps = currentFps = updatesPerSecond;
			}
			updates = 1000 / updatesPerSecond;
			var syncContext = new SynchronizationContext ();
			renderTask = Task.Factory.StartNew (() => {
				if (RenderOnUIThread) {
					syncContext.Send((s) => {
						RenderLoop (cts.Token);
					}, null);
				} else
					RenderLoop (cts.Token);
			}, cts.Token);
		}

		public virtual void Pause ()
		{
			EnsureUndisposed ();
			lock (lockObject) {
				isPaused = true;
				Monitor.PulseAll (lockObject);
			}
		}

		public virtual void Resume ()
		{
			EnsureUndisposed ();
			lock (lockObject) {
				if (isPaused) {
					isPaused = false;
					Monitor.PulseAll (lockObject);
				}
				try {
					if (!IsFocused)
						RequestFocus ();
				} catch {
				}
			}
		}

		protected override void Dispose (bool disposing)
		{
			if (disposing) {
				Stop ();
			}
			base.Dispose (disposing);
		}

		public async void Stop ()
		{
			Log.Verbose ("AndroidGameView", "Stop() Called!!!!!");
			EnsureUndisposed ();
			if (cts != null) {
				lock (lockObject) {
					Monitor.PulseAll (lockObject);
				}
				cts.Cancel ();
				while (!isExited) {
					lock (lockObject) {
						Monitor.PulseAll (lockObject);

					}
					await Task.Delay (100);
				}
				//renderTask.Wait ();
				Log.Verbose ("AndroidGameView", "Stop() Completed!!!!!");
			}
		}

		FrameEventArgs renderEventArgs = new FrameEventArgs ();

		protected void RenderLoop (CancellationToken token)
		{
			Threading.ResetThread (Thread.CurrentThread.ManagedThreadId);
			try {
				stopWatch = System.Diagnostics.Stopwatch.StartNew ();
				tick = 0;
				prevUpdateTime = DateTime.Now;
				while (!cts.IsCancellationRequested) {
					if (!IsGLSurfaceAvailable ()) {
						break;
					}

					Threading.ResetThread (Thread.CurrentThread.ManagedThreadId);
					try {
						RunIteration (token);
					} catch (MonoGameGLException ex) {
						Log.Error ("AndroidGameView", "GL Exception occured during RunIteration {0}", ex.Message);
					}

					if (updates > 0) {
						var t = updates - (stopWatch.Elapsed.TotalMilliseconds - tick);
						if (t > 0) {
							if (LogFPS) {
								Log.Verbose ("AndroidGameView", "took {0:F2}ms, should take {1:F2}ms, sleeping for {2:F2}", stopWatch.Elapsed.TotalMilliseconds - tick, updates, t);
							}
							if (token.IsCancellationRequested)
								return;
						}
					}
				}
				Log.Verbose ("AndroidGameView", "RenderLoop exited");
			} catch (Exception ex) {
				Log.Error ("AndroidGameView", ex.ToString ());
			} finally {
				lock (lockObject) {
					isExited = true;
					cts = null;
					if (glSurfaceAvailable)
						DestroyGLSurface ();
					if (glContextAvailable) {
						DestroyGLContext ();
						ContextLostInternal ();
					}
				}
			}
		}

		DateTime prevUpdateTime;
		DateTime prevRenderTime;
		DateTime curUpdateTime;
		DateTime curRenderTime;
		FrameEventArgs updateEventArgs = new FrameEventArgs ();

		void UpdateFrameInternal (FrameEventArgs e)
		{
			OnUpdateFrame (e);
			if (UpdateFrame != null)
				UpdateFrame (this, e);
		}

		protected virtual void OnUpdateFrame (FrameEventArgs e)
		{

		}

		// this method is called on the main thread
		void RunIteration (CancellationToken token)
		{
			if (token.IsCancellationRequested)
				return;

			curUpdateTime = DateTime.Now;
			if (prevUpdateTime.Ticks != 0) {
				var t = (curUpdateTime - prevUpdateTime).TotalMilliseconds;
				updateEventArgs.Time = t < 0 ? 0 : t;
			}
			try {
				UpdateFrameInternal (updateEventArgs);
			} catch (Content.ContentLoadException) {
				// ignore it..
			}

			prevUpdateTime = curUpdateTime;

			curRenderTime = DateTime.Now;
			if (prevRenderTime.Ticks == 0) {
				var t = (curRenderTime - prevRenderTime).TotalMilliseconds;
				renderEventArgs.Time = t < 0 ? 0 : t;
			}

			RenderFrameInternal (renderEventArgs);
			prevRenderTime = curRenderTime;

		}

		void RenderFrameInternal (FrameEventArgs e)
		{
			if (LogFPS) {
				Mark ();
			}
			OnRenderFrame (e);
			if (RenderFrame != null)
				RenderFrame (this, e);
		}

		protected virtual void OnRenderFrame (FrameEventArgs e)
		{

		}

		int frames = 0;
		double prev = 0;
		double avgFps = 0;
		double currentFps = 0;
		double targetFps = 0;

		void Mark ()
		{
			double cur = stopWatch.Elapsed.TotalMilliseconds;
			if (cur < 2000) {
				return;
			}
			frames++;

			if (cur - prev >= 995) {
				avgFps = 0.8 * avgFps + 0.2 * frames;

				Log.Verbose ("AndroidGameView", "frames {0} elapsed {1}ms {2:F2} fps",
					frames,
					cur - prev,
					avgFps);

				frames = 0;
				prev = cur;
			}
		}

		protected void EnsureUndisposed ()
		{
			if (disposed)
				throw new ObjectDisposedException ("");
		}

		protected void DestroyGLContext ()
		{
			Log.Verbose ("AndroidGameView", "DestroyGLContext");
			if (eglContext != null) {
				if (!egl.EglDestroyContext (eglDisplay, eglContext))
					throw new Exception ("Could not destroy EGL context" + GetErrorAsString ());
				eglContext = null;
			}
			if (eglDisplay != null) {
				if (!egl.EglTerminate (eglDisplay))
					throw new Exception ("Could not terminate EGL connection" + GetErrorAsString ());
				eglDisplay = null;
			}

			glContextAvailable = false;

		}

		void DestroyGLSurfaceInternal ()
		{
			if (!(eglSurface == null || eglSurface == EGL10.EglNoSurface)) {
				if (!egl.EglMakeCurrent (eglDisplay, EGL10.EglNoSurface,
					    EGL10.EglNoSurface, EGL10.EglNoContext)) {
					Log.Verbose ("AndroidGameView", "Could not unbind EGL surface" + GetErrorAsString ());
				}

				if (!egl.EglDestroySurface (eglDisplay, eglSurface)) {
					Log.Verbose ("AndroidGameView", "Could not destroy EGL surface" + GetErrorAsString ());
				}
			}
			eglSurface = null;
		}

		protected virtual void DestroyGLSurface ()
		{
			Log.Verbose ("AndroidGameView", "DestroyGLSurface");
			DestroyGLSurfaceInternal ();
			glSurfaceAvailable = false;
			Monitor.PulseAll (lockObject);
		}

		internal struct SurfaceConfig {
			public int Red;
			public int Green;
			public int Blue;
			public int Alpha;
			public int Depth;
			public int Stencil;

			public int[] ToConfigAttribs() {

				return new int[] {
					EGL11.EglRedSize, Red,
					EGL11.EglGreenSize, Green,
					EGL11.EglBlueSize, Blue,
					EGL11.EglAlphaSize, Alpha,
					EGL11.EglDepthSize, Depth,
					EGL11.EglStencilSize, Stencil,
					EGL11.EglRenderableType, 4,
					EGL11.EglNone
				};
			}

			public override string ToString ()
			{
				return string.Format ("Red:{0} Green:{1} Blue:{2} Alpha:{3} Depth:{4} Stencil:{5}", Red, Green, Blue, Alpha, Depth, Stencil);
			}
		}

		protected void CreateGLContext ()
		{
			Log.Verbose ("AndroidGameView", "CreateGLContext");
			lostglContext = false;

			egl = EGLContext.EGL.JavaCast<IEGL10> ();

			eglDisplay = egl.EglGetDisplay (EGL10.EglDefaultDisplay);
			if (eglDisplay == EGL10.EglNoDisplay)
				throw new Exception ("Could not get EGL display" + GetErrorAsString ());

			int[] version = new int[2];
			if (!egl.EglInitialize (eglDisplay, version))
				throw new Exception ("Could not initialize EGL display" + GetErrorAsString ());

			int depth = 0;
			int stencil = 0;
			switch (_game.graphicsDeviceManager.PreferredDepthStencilFormat)
			{
				case DepthFormat.Depth16:
					depth = 16;
					break;
				case DepthFormat.Depth24:
					depth = 24;
					break;
				case DepthFormat.Depth24Stencil8:
					depth = 24;
					stencil = 8;
					break;
				case DepthFormat.None:
					break;
			}

			List<SurfaceConfig> configs = new List<SurfaceConfig> ();
			if (depth > 0) {
				configs.Add (new SurfaceConfig () { Red = 8, Green = 8, Blue = 8, Alpha = 8, Depth = depth, Stencil = stencil });
				configs.Add (new SurfaceConfig () { Red = 5, Green = 6, Blue = 5, Depth = depth, Stencil = stencil });
				configs.Add (new SurfaceConfig () { Depth = depth, Stencil = stencil });
				if (depth > 16) {
					configs.Add (new SurfaceConfig () { Red = 8, Green = 8, Blue = 8, Alpha = 8, Depth = 16 });
					configs.Add (new SurfaceConfig () { Red = 5, Green = 6, Blue = 5, Depth = 16 });
					configs.Add (new SurfaceConfig () { Depth = 16 });
				}
			} else {
				configs.Add (new SurfaceConfig () { Red = 8, Green = 8, Blue = 8, Alpha = 8});
				configs.Add (new SurfaceConfig () { Red = 5, Green = 6, Blue = 5 });
			}
			configs.Add (new SurfaceConfig () { Red = 4, Green = 4, Blue = 4, Alpha = 0, Depth = 0, Stencil = 0 });
			int[] numConfigs = new int[1];
			EGLConfig[] results = new EGLConfig[1];


			foreach (var config in configs) {

				if (!egl.EglChooseConfig (eglDisplay, config.ToConfigAttribs(), results, 1, numConfigs)) {
					continue;
				}
				Log.Verbose ("AndroidGameView", string.Format("Selected Config : {0}",config)); 
				break;
			}

			if (numConfigs [0] == 0)
				throw new Exception ("No valid EGL configs found" + GetErrorAsString ());
			eglConfig = results [0];

			int[] contextAttribs = new int[] { EglContextClientVersion, 2, EGL10.EglNone };
			eglContext = egl.EglCreateContext (eglDisplay, eglConfig, EGL10.EglNoContext, contextAttribs);
			if (eglContext == null || eglContext == EGL10.EglNoContext) {
				eglContext = null;
				throw new Exception ("Could not create EGL context" + GetErrorAsString ());
			}

			glContextAvailable = true;
		}

		private string GetErrorAsString ()
		{
			switch (egl.EglGetError ()) {
			case EGL10.EglSuccess:
				return "Success";

			case EGL10.EglNotInitialized:
				return "Not Initialized";

			case EGL10.EglBadAccess:
				return "Bad Access";
			case EGL10.EglBadAlloc:
				return "Bad Allocation";
			case EGL10.EglBadAttribute:
				return "Bad Attribute";
			case EGL10.EglBadConfig:
				return "Bad Config";
			case EGL10.EglBadContext:
				return "Bad Context";
			case EGL10.EglBadCurrentSurface:
				return "Bad Current Surface";
			case EGL10.EglBadDisplay:
				return "Bad Display";
			case EGL10.EglBadMatch:
				return "Bad Match";
			case EGL10.EglBadNativePixmap:
				return "Bad Native Pixmap";
			case EGL10.EglBadNativeWindow:
				return "Bad Native Window";
			case EGL10.EglBadParameter:
				return "Bad Parameter";
			case EGL10.EglBadSurface:
				return "Bad Surface";

			default:
				return "Unknown Error";
			}
		}


		protected void CreateGLSurface ()
		{

			if (!glSurfaceAvailable)
				try {
					Log.Verbose ("AndroidGameView", "CreateGLSurface");
					// If there is an existing surface, destroy the old one
					DestroyGLSurfaceInternal ();

					eglSurface = egl.EglCreateWindowSurface (eglDisplay, eglConfig, (Java.Lang.Object)this.Holder, null);
					if (eglSurface == null || eglSurface == EGL10.EglNoSurface)
						throw new Exception ("Could not create EGL window surface" + GetErrorAsString ());

					if (!egl.EglMakeCurrent (eglDisplay, eglSurface, eglSurface, eglContext))
						throw new Exception ("Could not make EGL current" + GetErrorAsString ());

					glSurfaceAvailable = true;

				} catch (Exception ex) {
					Log.Error ("AndroidGameView", ex.ToString ());
					glSurfaceAvailable = false;
				}
		}

		protected EGLSurface CreatePBufferSurface (EGLConfig config, int[] attribList)
		{
			IEGL10 egl = EGLContext.EGL.JavaCast<IEGL10> ();
			EGLSurface result = egl.EglCreatePbufferSurface (eglDisplay, config, attribList);
			if (result == null || result == EGL10.EglNoSurface)
				throw new Exception ("EglCreatePBufferSurface");
			return result;
		}

		protected void ContextSetInternal ()
		{
			if (lostglContext)
			{
				if (_game.GraphicsDevice != null)
				{
					_game.GraphicsDevice.Initialize();

					IsResuming = true;
					if (_gameWindow.Resumer != null)
					{
						_gameWindow.Resumer.LoadContent();
					}

					// Reload textures on a different thread so the resumer can be drawn
					System.Threading.Thread bgThread = new System.Threading.Thread(
						o =>
						{
							Android.Util.Log.Debug("MonoGame", "Begin reloading graphics content");
							Microsoft.Xna.Framework.Content.ContentManager.ReloadGraphicsContent();
							Android.Util.Log.Debug("MonoGame", "End reloading graphics content");

							// DeviceReset events
							_game.graphicsDeviceManager.OnDeviceReset(EventArgs.Empty);
							_game.GraphicsDevice.OnDeviceReset();

							IsResuming = false;
						});

					bgThread.Start();
				}
			}
			OnContextSet (EventArgs.Empty);
		}

		protected void ContextLostInternal ()
		{
			OnContextLost (EventArgs.Empty);
			_game.graphicsDeviceManager.OnDeviceResetting(EventArgs.Empty);
			if (_game.GraphicsDevice != null)
				_game.GraphicsDevice.OnDeviceResetting();
		}

		protected virtual void OnContextLost (EventArgs eventArgs)
		{

		}

		protected bool IsGLSurfaceAvailable ()
		{
			lock (lockObject) {
				// we want to wait until we have a valid surface
				// this is not called from the UI thread but on
				// the background rendering thread
				while (!cts.IsCancellationRequested) {
					//Log.Verbose ("AndroidGameView", "IsGLSurfaceAvailable {0} IsPaused {1} lostcontext {2} surfaceAvailable {3} contextAvailable {4} ThreadID {5}",
					//	glSurfaceAvailable, isPaused, lostglContext, surfaceAvailable, glContextAvailable,Thread.CurrentThread.ManagedThreadId);
					if (glSurfaceAvailable && (isPaused || !surfaceAvailable)) {
						// Surface we are using needs to go away
						DestroyGLSurface ();
						if (loaded)
							OnUnload (EventArgs.Empty);

					} else if ((!glSurfaceAvailable && !isPaused && surfaceAvailable) || lostglContext) {
						// We can (re)create the EGL surface (not paused, surface available)
						if (glContextAvailable && !lostglContext) {
							try {
								CreateGLSurface ();
							} catch (Exception ex) {
								// We failed to create the surface for some reason
								Log.Verbose ("AndroidGameView", ex.ToString ());
							}
						}

						if (!glSurfaceAvailable || lostglContext) { // Start or Restart due to context loss
							bool contextLost = false;
							if (lostglContext || glContextAvailable) {
								// we actually lost the context
								// so we need to free up our existing 
								// objects and re-create one.
								DestroyGLContext ();
								contextLost = true;
								Log.Verbose ("AndroidGameView", "ContentLostInternal");
								ContextLostInternal ();
							}

							CreateGLContext ();
							CreateGLSurface ();

							if (!loaded && glContextAvailable)
								OnLoad (EventArgs.Empty);

							if (contextLost && glContextAvailable) {
								Log.Verbose ("AndroidGameView", "ContentSetInternal");
								// we lost the gl context, we need to let the programmer
								// know so they can re-create textures etc.
								ContextSetInternal ();
							}
						}
					}

					// If we have a GL surface we can continue 
					// rednering
					if (glSurfaceAvailable) {
						return true;
					} else {
						// if we dont we need to wait until we get
						// a surfaceCreated event or some other 
						// event from the ISurfaceHolderCallback
						// so we can create a new GL surface.
						if (cts.IsCancellationRequested)
							break;
						if (Game.Activity.IsFinishing)
							return false;
						Log.Verbose ("AndroidGameView", "IsGLSurfaceAvailable entering wait state");
						Monitor.Wait (lockObject);
						Log.Verbose ("AndroidGameView", "IsGLSurfaceAvailable exiting wait state");
						continue;
					}
				}
				Log.Verbose ("AndroidGameView", "IsGLSurfaceAvailable exited!!!!!");
				return false;
			}
		}

		protected virtual void OnUnload (EventArgs eventArgs)
		{

		}

		protected virtual void OnContextSet (EventArgs eventArgs)
		{

		}

		protected virtual void OnLoad (EventArgs eventArgs)
		{

		}

		protected virtual void OnStopped (EventArgs eventArgs)
		{

		}

		#region Key and Motion

		public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
		{
			if (GamePad.OnKeyDown(keyCode, e))
				return true;

			Keyboard.KeyDown(keyCode);
			// we need to handle the Back key here because it doesnt work any other way
			if (keyCode == Keycode.Back)
				GamePad.Back = true;

			if (keyCode == Keycode.VolumeUp)
			{
				AudioManager audioManager = (AudioManager)Context.GetSystemService(Context.AudioService);
				audioManager.AdjustStreamVolume(Stream.Music, Adjust.Raise, VolumeNotificationFlags.ShowUi);
				return true;
			}

			if (keyCode == Keycode.VolumeDown)
			{
				AudioManager audioManager = (AudioManager)Context.GetSystemService(Context.AudioService);
				audioManager.AdjustStreamVolume(Stream.Music, Adjust.Lower, VolumeNotificationFlags.ShowUi);
				return true;
			}

			return true;
		}

		public override bool OnKeyUp(Keycode keyCode, KeyEvent e)
		{
			if (GamePad.OnKeyUp(keyCode, e))
				return true;
			Keyboard.KeyUp(keyCode);
			return true;
		}

		public override bool OnGenericMotionEvent(MotionEvent e)
		{
			if (GamePad.OnGenericMotionEvent(e))
				return true;

			return base.OnGenericMotionEvent(e);
		}

		#endregion

		#region Properties

		private  IEGL10 egl;
		private  EGLDisplay eglDisplay;
		private  EGLConfig eglConfig;
		private  EGLContext eglContext;
		private  EGLSurface eglSurface;

		/// <summary>The visibility of the window. Always returns true.</summary>
		/// <value></value>
		/// <exception cref="T:System.ObjectDisposed">The instance has been disposed</exception>
		public virtual bool Visible {
			get {
				EnsureUndisposed ();
				return true;
			}
			set {
				EnsureUndisposed ();
			}
		}

		/// <summary>The size of the current view.</summary>
		/// <value>A <see cref="T:System.Drawing.Size" /> which is the size of the current view.</value>
		/// <exception cref="T:System.ObjectDisposed">The instance has been disposed</exception>
		public virtual Size Size {
			get {
				EnsureUndisposed ();
				return size;
			}
			set {
				EnsureUndisposed ();
				if (size != value) {
					size = value;
					OnResize (EventArgs.Empty);
				}
			}
		}

		private void OnResize (EventArgs eventArgs)
		{

		}

		#endregion

		public event FrameEvent RenderFrame;
		public event FrameEvent UpdateFrame;

		public delegate void FrameEvent (object sender,FrameEventArgs e);

		public class FrameEventArgs : EventArgs
		{
			double elapsed;

			/// <summary>
			/// Constructs a new FrameEventArgs instance.
			/// </summary>
			public FrameEventArgs ()
			{
			}

			/// <summary>
			/// Constructs a new FrameEventArgs instance.
			/// </summary>
			/// <param name="elapsed">The amount of time that has elapsed since the previous event, in seconds.</param>
			public FrameEventArgs (double elapsed)
			{
				Time = elapsed;
			}

			/// <summary>
			/// Gets a <see cref="System.Double"/> that indicates how many seconds of time elapsed since the previous event.
			/// </summary>
			public double Time {
				get { return elapsed; }
				internal set {
					if (value < 0)
						throw new ArgumentOutOfRangeException ();
					elapsed = value;
				}
			}
		}

		public BackgroundContext CreateBackgroundContext()
		{
			return new BackgroundContext (this);
		}

		public class BackgroundContext {

			EGLContext eglContext;
			AndroidGameView view;
			EGLSurface surface;

			public BackgroundContext (AndroidGameView view)
			{
				this.view = view;
				int[] contextAttribs = new int[] { EglContextClientVersion, 2, EGL10.EglNone };
				eglContext = view.egl.EglCreateContext (view.eglDisplay, view.eglConfig, view.eglContext, contextAttribs);
				if (eglContext == null || eglContext == EGL10.EglNoContext) {
					eglContext = null;
					throw new Exception ("Could not create EGL context" + view.GetErrorAsString ());
				}
				int[] pbufferAttribList = new int [] { EGL10.EglWidth, 64, EGL10.EglHeight, 64, EGL10.EglNone };
				surface = view.CreatePBufferSurface (view.eglConfig, pbufferAttribList);
				if (surface == EGL10.EglNoSurface)
					throw new Exception ("Could not create Pbuffer Surface" + view.GetErrorAsString ());
			}

			public void MakeCurrent() {
				view.ClearCurrent ();
				view.egl.EglMakeCurrent (view.eglDisplay, surface, surface, eglContext);
			}
		}
	}
}