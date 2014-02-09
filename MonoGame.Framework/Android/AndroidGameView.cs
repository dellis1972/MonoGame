//#if DEBUG
//#define TIMING
//#endif
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

namespace Microsoft.Xna.Framework
{

	[CLSCompliant (false)]
	public class AndroidGameView : SurfaceView, ISurfaceHolderCallback
	{

		bool disposed = false;
		ISurfaceHolder mHolder;
		Size size;
		object lockObject = new object ();

		bool surfaceAvailable;
		bool surfaceChanged;

		int surfaceWidth;
		int surfaceHeight;

		bool glSurfaceAvailable;
		bool glContextAvailable;
		bool lostglContext;
		private bool isPaused;
		ManualResetEvent pauseSignal;
		System.Diagnostics.Stopwatch stopWatch;
		double tick = 0;

		bool loaded = false;

		Task renderTask;
		CancellationTokenSource cts = null;

		public AndroidGameView (Context context)
			: base (context)
		{
			Init ();
		}

		public AndroidGameView (Context context, IAttributeSet attrs)
			: base (context, attrs)
		{
			Init ();
		}

		public AndroidGameView (IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
			: base (handle, transfer)
		{
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
				surfaceWidth = Width;
				surfaceHeight = Height;
				surfaceChanged = true;
			}
		}

		public void SurfaceCreated (ISurfaceHolder holder)
		{
			lock (lockObject) {
				surfaceWidth = Width;
				surfaceHeight = Height;
				surfaceAvailable = true;
				Monitor.PulseAll (lockObject);
			}
		}

		public void SurfaceDestroyed (ISurfaceHolder holder)
		{
			lock (lockObject) {
				surfaceAvailable = false;
				Monitor.PulseAll (lockObject);
				while (glSurfaceAvailable) {
					Monitor.Wait (lockObject);
				}
			}
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

		double updates;


		public virtual void Run ()
		{
			cts = new CancellationTokenSource ();
#if TIMING
			targetFps = currentFps = 0;
			avgFps = 1;
#endif
			updates = 0;
			renderTask = Task.Factory.StartNew (() => { RenderLoop (cts.Token); }, cts.Token);
		}

		public virtual void Run (double updatesPerSecond)
		{
			cts = new CancellationTokenSource ();
#if TIMING
			avgFps = targetFps = currentFps = updatesPerSecond;
#endif
			updates = 1000 / updatesPerSecond;
			renderTask = Task.Factory.StartNew (() => { RenderLoop (cts.Token); }, cts.Token);
		}

		public virtual void Pause ()
		{
			EnsureUndisposed ();
			isPaused = true;
		}

		public virtual void Resume ()
		{
			EnsureUndisposed ();
			if (isPaused) {
				isPaused = false;
				lock (lockObject) {
					Monitor.PulseAll (lockObject);
				}
			}
			RequestFocus ();
		}

		public void Stop ()
		{
			EnsureUndisposed ();
			if (cts != null)
				cts.Cancel ();
		}

		FrameEventArgs renderEventArgs = new FrameEventArgs ();

		protected void RenderLoop (CancellationToken token)
		{
			try {
				stopWatch = System.Diagnostics.Stopwatch.StartNew ();
				tick = 0;
				while (!cts.IsCancellationRequested) {
					Threading.ResetThread (Thread.CurrentThread.ManagedThreadId);

					if (!IsGLSurfaceAvailable ()) {
						return;
					}

					RunIteration (token);

					if (updates > 0) {
						var t = updates - (stopWatch.Elapsed.TotalMilliseconds - tick);
						if (t > 0) {
#if TIMING
							Log.Verbose ("AndroidGameView", "took {0:F2}ms, should take {1:F2}ms, sleeping for {2:F2}", stopWatch.Elapsed.TotalMilliseconds - tick, updates, t);
#endif
							if (token.IsCancellationRequested)
								return;

							pauseSignal.Reset ();
							pauseSignal.WaitOne ((int)t);
							if (!isPaused)
								pauseSignal.Set ();
						}
					}
				}
			} catch (Exception ex) {
				Log.Error ("AndroidGameView", ex.ToString ());
			} finally {
				lock (lockObject) {
					cts = null;
					if (glSurfaceAvailable)
						DestroyGLSurface ();
					if (glContextAvailable) {
						DestroyGLContext ();
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
					var t = (curUpdateTime - prevUpdateTime).TotalSeconds;
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
					var t = (curRenderTime - prevRenderTime).TotalSeconds;
					renderEventArgs.Time = t < 0 ? 0 : t;
				}

				RenderFrameInternal (renderEventArgs);
				prevRenderTime = curRenderTime;
			
		}

		void RenderFrameInternal (FrameEventArgs e)
		{
#if TIMING
			Mark();
#endif
			OnRenderFrame (e);
		}

		protected virtual void OnRenderFrame (FrameEventArgs e)
		{
			
		}

#if TIMING
		int frames = 0;
		double prev = 0;
		double avgFps = 0;
		double currentFps = 0;
		double targetFps = 0;
		void Mark()
		{
			double cur = stopWatch.Elapsed.TotalMilliseconds;
			if (cur < 2000) {
				return;
			}
			frames++;

			if (cur - prev >= 995) {
				avgFps = 0.8 * avgFps + 0.2 * frames;

				Log.Verbose("AndroidGameView", "frames {0} elapsed {1}ms {2:F2} fps",
					frames,
					cur - prev,
					avgFps);

				frames = 0;
				prev = cur;
			}
		}
#endif

		protected void EnsureUndisposed ()
		{
			if (disposed)
				throw new ObjectDisposedException ("");
		}

		protected void DestroyGLContext ()
		{
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
							EGL10.EglNoSurface, EGL10.EglNoContext))
					throw new Exception ("Could not unbind EGL surface" + GetErrorAsString ());

				if (!egl.EglDestroySurface (eglDisplay, eglSurface))
					throw new Exception ("Could not destroy EGL surface" + GetErrorAsString ());
			}
		}

		protected virtual void DestroyGLSurface ()
		{
			DestroyGLSurfaceInternal ();
			glSurfaceAvailable = false;
			Monitor.PulseAll (lockObject);
		}

		protected void CreateGLContext ()
		{
			lostglContext = false;

			egl = EGLContext.EGL.JavaCast<IEGL10> ();

			eglDisplay = egl.EglGetDisplay (EGL10.EglDefaultDisplay);
			if (eglDisplay == EGL10.EglNoDisplay)
				throw new Exception ("Could not get EGL display" + GetErrorAsString ());

			int[] version = new int[2];
			if (!egl.EglInitialize (eglDisplay, version))
				throw new Exception ("Could not initialize EGL display" + GetErrorAsString ());

			// TODO: allow GraphicsDeviceManager to specify a frame buffer configuration
			// TODO: test this configuration works on many devices:
			int[] configAttribs = new int[] {
					EGL11.EglRedSize, 8,
					EGL11.EglGreenSize, 8,
					EGL11.EglBlueSize, 8,
					EGL11.EglAlphaSize, 8,
					EGL11.EglDepthSize, 16,
					EGL11.EglStencilSize, 0,
					EGL11.EglRenderableType, 4,
					EGL11.EglNone };
			EGLConfig[] configs = new EGLConfig[1];
			int[] numConfigs = new int[1];
			if (!egl.EglChooseConfig (eglDisplay, configAttribs, configs, 1, numConfigs)) {
				configAttribs = new int[] {
					EGL11.EglRedSize, 4,
					EGL11.EglGreenSize, 4,
					EGL11.EglBlueSize, 4,
					EGL11.EglRenderableType, 4,
					EGL11.EglNone };
				if (!egl.EglChooseConfig (eglDisplay, configAttribs, configs, 1, numConfigs)) {
					throw new Exception ("Could not get EGL config" + GetErrorAsString ());
				}
			}
			if (numConfigs[0] == 0)
				throw new Exception ("No valid EGL configs found" + GetErrorAsString ());
			eglConfig = configs[0];

			const int EglContextClientVersion = 0x3098;
			int[] contextAttribs = new int[] { EglContextClientVersion, 2, EGL10.EglNone };
			eglContext = egl.EglCreateContext (eglDisplay, eglConfig, EGL10.EglNoContext, contextAttribs);
			if (eglContext == null || eglContext == EGL10.EglNoContext) {
				eglContext = null;
				throw new Exception ("Could not create EGL context" + GetErrorAsString());
			}

			glContextAvailable = true;
		}

		private string GetErrorAsString ()
		{
			switch (egl.EglGetError ()) {
				case EGL10.EglSuccess: return "Success";

				case EGL10.EglNotInitialized: return "Not Initialized";

				case EGL10.EglBadAccess: return "Bad Access";
				case EGL10.EglBadAlloc: return "Bad Allocation";
				case EGL10.EglBadAttribute: return "Bad Attribute";
				case EGL10.EglBadConfig: return "Bad Config";
				case EGL10.EglBadContext: return "Bad Context";
				case EGL10.EglBadCurrentSurface: return "Bad Current Surface";
				case EGL10.EglBadDisplay: return "Bad Display";
				case EGL10.EglBadMatch: return "Bad Match";
				case EGL10.EglBadNativePixmap: return "Bad Native Pixmap";
				case EGL10.EglBadNativeWindow: return "Bad Native Window";
				case EGL10.EglBadParameter: return "Bad Parameter";
				case EGL10.EglBadSurface: return "Bad Surface";

				default: return "Unknown Error";
			}
		}


		protected void CreateGLSurface ()
		{
			if (!glSurfaceAvailable)
				try {
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

		protected void ContextSetInternal ()
		{
			OnContextSet (EventArgs.Empty);
		}

		protected void ContextLostInternal ()
		{
			OnContextLost (EventArgs.Empty);
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
				while (true) {
					//Log.Verbose ("AndroidGameView", "IsGLSurfaceAvailable {0} IsPaused {1} ThreadID {2}", glSurfaceAvailable, isPaused, Thread.CurrentThread.ManagedThreadId);
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
							} catch (Exception) {
								// We failed to create the surface for some reason
							}
						}

						if (!glSurfaceAvailable || lostglContext) // Start or Restart due to context loss
						{
							bool contextLost = false;
							if (lostglContext || glContextAvailable) {
								// we actually lost the context
								// so we need to free up our existing 
								// objects and re-create one.
								DestroyGLContext ();
								contextLost = true;
								ContextLostInternal ();
							}

							CreateGLContext ();
							CreateGLSurface ();

							if (!loaded && glContextAvailable)
								OnLoad (EventArgs.Empty);

							if (contextLost) {
								// we lost the gl context, we need to let the programmer
								// know so they can re-create textures etc.
								ContextSetInternal ();
							}
						}
					}

					// If we have a GL surface we can continue 
					// rednering
					if (glSurfaceAvailable && !isPaused) {
						return true;
					} else {
						// if we dont we need to wait until we get
						// a surfaceCreated event or some other 
						// event from the ISurfaceHolderCallback
						// so we can create a new GL surface.
						Log.Verbose ("AndroidGameView", "IsGLSurfaceAvailable entering wait state");
						Monitor.Wait (lockObject);
						continue;
					}
				}
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

		#region Properties

		private  IEGL10 egl;
		private  EGLDisplay eglDisplay;
		private  EGLConfig eglConfig;
		private  EGLContext eglContext;
		private  EGLSurface eglSurface;

		/// <summary>The visibility of the window. Always returns true.</summary>
		/// <value></value>
		/// <exception cref="T:System.ObjectDisposed">The instance has been disposed</exception>
		public new virtual bool Visible
		{
			get
			{
				EnsureUndisposed ();
				return true;
			}
			set
			{
				EnsureUndisposed ();
			}
		}

		/// <summary>The size of the current view.</summary>
		/// <value>A <see cref="T:System.Drawing.Size" /> which is the size of the current view.</value>
		/// <exception cref="T:System.ObjectDisposed">The instance has been disposed</exception>
		public virtual Size Size
		{
			get
			{
				EnsureUndisposed ();
				return size;
			}
			set
			{
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

		public class FrameEventArgs : EventArgs
		{
			double elapsed;

			/// <summary>
			/// Constructs a new FrameEventArgs instance.
			/// </summary>
			public FrameEventArgs ()
			{ }

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
			public double Time
			{
				get { return elapsed; }
				internal set
				{
					if (value <= 0)
						throw new ArgumentOutOfRangeException ();
					elapsed = value;
				}
			}
		}
	}
}