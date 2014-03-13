using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


using Microsoft.Xna.Framework.Input.Touch;
using Android.Views.InputMethods;


namespace Microsoft.Xna.Framework
{
	[CLSCompliant (false)]
	public class AndroidGameActivity : Activity
	{
		static Game game;
		public static Game Game
		{
			get { return game; }
			set
			{
				game = value;
				if (game != null)
					TitleContainer.InitActivity ();
			}
		}


		private OrientationListener o;
		private ScreenReceiver screenReceiver;
		private  bool keyboardVisible;


		private bool _AutoPauseAndResumeMediaPlayer = true;
		public bool AutoPauseAndResumeMediaPlayer
		{
			get { return _AutoPauseAndResumeMediaPlayer; }
			set { _AutoPauseAndResumeMediaPlayer = value; }
		}


		/// <summary>
		/// OnCreate called when the activity is launched from cold or after the app
		/// has been killed due to a higher priority app needing the memory
		/// </summary>
		/// <param name='savedInstanceState'>
		/// Saved instance state.
		/// </param>
		protected override void OnCreate (Bundle savedInstanceState)
		{
			System.Diagnostics.Debug.WriteLine ("MonoGame OnCreate !!!!!!!!!");
			base.OnCreate (savedInstanceState);
			o = new OrientationListener (this);
			if (o.CanDetectOrientation ()) {
				o.Enable ();
			}


			IntentFilter filter = new IntentFilter ();
			filter.AddAction (Intent.ActionScreenOff);
			filter.AddAction (Intent.ActionScreenOn);
			filter.AddAction (Intent.ActionUserPresent);


			screenReceiver = new ScreenReceiver ();
			RegisterReceiver (screenReceiver, filter);


			RequestWindowFeature (WindowFeatures.NoTitle);
		}

		public override void Finish ()
		{
			TitleContainer.Close ();
			base.Finish ();

		}

		public static event EventHandler Paused;


		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			// we need to refresh the viewport here.			
			base.OnConfigurationChanged (newConfig);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			#if !OUYA
			GamerServices.GameHelper.Instance.OnActivityResult (requestCode, resultCode, data);
			#endif
			base.OnActivityResult (requestCode, resultCode, data);
		}

		protected override void OnPause ()
		{
			System.Diagnostics.Debug.WriteLine ("MonoGame OnPause !!!!!!!!!");
			base.OnPause ();
			if (Paused != null)
				Paused (this, EventArgs.Empty);


		}

		protected override void OnStart ()
		{
			#if !OUYA
			GamerServices.GameHelper.Instance.Start ();
			#endif
			base.OnStart ();
		}

		protected override void OnStop ()
		{
			#if !OUYA
			GamerServices.GameHelper.Instance.Stop ();
			#endif
			base.OnStop ();
		}


		public static event EventHandler Resumed;

		protected override void OnResume ()
		{
			System.Diagnostics.Debug.WriteLine ("MonoGame OnResume !!!!!!!!!");
			base.OnResume ();
			if (Resumed != null)
				Resumed (this, EventArgs.Empty);


			var deviceManager = (IGraphicsDeviceManager)Game.Services.GetService (typeof (IGraphicsDeviceManager));
			if (deviceManager == null)
				return;
			(deviceManager as GraphicsDeviceManager).ForceSetFullScreen ();
			Game.Window.RequestFocus ();
		}


		protected override void OnDestroy ()
		{
			System.Diagnostics.Debug.WriteLine ("MonoGame OnDestroy !!!!!!!!!");
			UnregisterReceiver (screenReceiver);
			if (keyboardVisible)
				HideKeyboard ();
			if (Game != null) {
				Game.DoExiting ();
				Game.Window.Stop ();
				Game.Dispose ();
				Graphics.GraphicsAdapter.Reset ();
				Content.ContentManager.ClearGraphicsContent ();
				Microsoft.Xna.Framework.Graphics.GraphicsDevice.ClearDisposeActions ();
			}
			Game = null;
			base.OnDestroy ();
		}

		bool wasalreadyvisible = false;

		public void ShowKeyboard ()
		{
			if (!keyboardVisible) {
				InputMethodManager manager = (InputMethodManager)Game.Activity.GetSystemService (Context.InputMethodService);
				keyboardVisible = manager.ShowSoftInput (Game.Window, ShowFlags.Implicit, null);
			} else
				wasalreadyvisible = true;
		}

		public void HideKeyboard ()
		{
			if (keyboardVisible && !wasalreadyvisible) {
				InputMethodManager manager = (InputMethodManager)Game.Activity.GetSystemService (Context.InputMethodService);
				manager.HideSoftInputFromWindow (Game.Window.WindowToken, HideSoftInputFlags.ImplicitOnly);
				keyboardVisible = false;
			}
			if (wasalreadyvisible) wasalreadyvisible = false;
		}
	}


	[CLSCompliant (false)]
	public static class ActivityExtensions
	{
		public static ActivityAttribute GetActivityAttribute (this AndroidGameActivity obj)
		{
			var attr = obj.GetType ().GetCustomAttributes (typeof (ActivityAttribute), true);
			if (attr != null) {
				return ((ActivityAttribute)attr[0]);
			}
			return null;
		}
	}


}

