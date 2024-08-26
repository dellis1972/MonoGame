// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;

namespace Microsoft.Xna.Framework
{
    public class AndroidGameActivity : AppCompatActivity
    {
        internal Game Game { private get; set; }

        private ScreenReceiver screenReceiver;
        private OrientationListener _orientationListener;

        public bool AutoPauseAndResumeMediaPlayer = true;
        public bool RenderOnUIThread = true; 

		/// <summary>
		/// OnCreate called when the activity is launched from cold or after the app
		/// has been killed due to a higher priority app needing the memory
		/// </summary>
		/// <param name='savedInstanceState'>
		/// Saved instance state.
		/// </param>
		protected override void OnCreate (Bundle savedInstanceState)
		{
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            if (OperatingSystem.IsAndroidVersionAtLeast (30)) {
                WindowCompat.GetInsetsController (this.Window, ((AndroidGameWindow)Game.Window).GameView).Hide (WindowInsetsCompat.Type.SystemBars());
            }
            else if (OperatingSystem.IsAndroidVersionAtLeast (19)) {
                View decorView = Window.DecorView;
                var uiVisibility = SystemUiFlags.LayoutStable |
                    SystemUiFlags.LayoutHideNavigation |
                    SystemUiFlags.LayoutFullscreen |
                    SystemUiFlags.HideNavigation |
                    SystemUiFlags.Fullscreen |
                    SystemUiFlags.ImmersiveSticky;
                decorView.SystemUiFlags = uiVisibility;
            } else {
                Window.AddFlags (WindowManagerFlags.Fullscreen);
            }

			IntentFilter filter = new IntentFilter();
		    filter.AddAction(Intent.ActionScreenOff);
		    filter.AddAction(Intent.ActionScreenOn);
		    filter.AddAction(Intent.ActionUserPresent);
		    
		    screenReceiver = new ScreenReceiver();
		    RegisterReceiver(screenReceiver, filter);

            _orientationListener = new OrientationListener(this);

			Game.Activity = this;
		}

        public static event EventHandler Paused;

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			// we need to refresh the viewport here.
			base.OnConfigurationChanged (newConfig);
		}

        public override void OnWindowFocusChanged (bool hasFocus)
        {
            base.OnWindowFocusChanged (hasFocus);
            if (hasFocus)
            {
                if (OperatingSystem.IsAndroidVersionAtLeast (30)) {
                    WindowCompat.GetInsetsController (this.Window, ((AndroidGameWindow)Game.Window).GameView).Hide (WindowInsetsCompat.Type.SystemBars());
                }
                else if (OperatingSystem.IsAndroidVersionAtLeast (19)) {
                    View decorView = Window.DecorView;
                    var uiVisibility = SystemUiFlags.LayoutStable |
                        SystemUiFlags.LayoutHideNavigation |
                        SystemUiFlags.LayoutFullscreen |
                        SystemUiFlags.HideNavigation |
                        SystemUiFlags.Fullscreen |
                        SystemUiFlags.ImmersiveSticky;
                    decorView.SystemUiFlags = uiVisibility;
                } else {
                    Window.AddFlags (WindowManagerFlags.Fullscreen);
                }
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            EventHelpers.Raise(this, Paused, EventArgs.Empty);

            if (_orientationListener.CanDetectOrientation())
                _orientationListener.Disable();
        }

        public static event EventHandler Resumed;
        protected override void OnResume()
        {
            base.OnResume();
            EventHelpers.Raise(this, Resumed, EventArgs.Empty);

            if (Game != null)
            {
                var deviceManager = (IGraphicsDeviceManager)Game.Services.GetService(typeof(IGraphicsDeviceManager));
                if (deviceManager == null)
                    return;
                ((GraphicsDeviceManager)deviceManager).ForceSetFullScreen();
                ((AndroidGameWindow)Game.Window).GameView.RequestFocus();
                if (_orientationListener.CanDetectOrientation())
                    _orientationListener.Enable();
            }
        }

		protected override void OnDestroy ()
		{
            UnregisterReceiver(screenReceiver);
            ScreenReceiver.ScreenLocked = false;
            _orientationListener = null;
            if (Game != null)
                Game.Dispose();
            Game = null;
			base.OnDestroy ();
		}
    }

	public static class ActivityExtensions
    {
        public static ActivityAttribute GetActivityAttribute(this AndroidGameActivity obj)
        {			
            var attr = obj.GetType().GetCustomAttributes(typeof(ActivityAttribute), true);
			if (attr != null)
			{
            	return ((ActivityAttribute)attr[0]);
			}
			return null;
        }
    }

}
