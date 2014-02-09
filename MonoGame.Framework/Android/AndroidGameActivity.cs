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

namespace Microsoft.Xna.Framework
{
	[CLSCompliant (false)]
	public class AndroidGameActivity : Activity
	{
        static Game game = null;

        public static Game Game { 
            get { return game; }
            set
            {
                if (game != null)
                    throw new InvalidOperationException("Game Object has already been set");
                game = value;
                if (game != null)
                    TitleContainer.InitActivity();
            }
        }

		private OrientationListener o;
		private ScreenReceiver screenReceiver;

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

		public static event EventHandler Paused;

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			// we need to refresh the viewport here.			
			base.OnConfigurationChanged (newConfig);
		}

		protected override void OnPause ()
		{
			base.OnPause ();
			Game.Window.Pause ();
			if (Paused != null)
				Paused (this, EventArgs.Empty);

		}

		public static event EventHandler Resumed;
		protected override void OnResume ()
		{
			base.OnResume ();
			if (Resumed != null)
				Resumed (this, EventArgs.Empty);

			var deviceManager = (IGraphicsDeviceManager)Game.Services.GetService (typeof (IGraphicsDeviceManager));
			if (deviceManager == null)
				return;
			(deviceManager as GraphicsDeviceManager).ForceSetFullScreen ();
			Game.Window.Resume ();
		}

		protected override void OnDestroy ()
		{
			UnregisterReceiver (screenReceiver);
			if (Game != null)
				Game.Dispose ();
			Game = null;
			base.OnDestroy ();
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
