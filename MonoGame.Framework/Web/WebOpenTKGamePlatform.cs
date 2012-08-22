using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using OpenTK.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Security;

namespace Microsoft.Xna.Framework
{
    [SecuritySafeCritical]    
    class WebOpenTKGamePlatform : GamePlatform
    {
        private OpenALSoundController soundControllerInstance = null;
        private IGraphicsContext GraphicsContext = null;
        bool IsRunning = true;
        private List<Microsoft.Xna.Framework.Input.Keys> keys;

        private void Keyboard_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            Keys xnaKey = KeyboardUtil.ToXna(e.Key);
            if (keys.Contains(xnaKey)) keys.Remove(xnaKey);
        }

        private void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            Keys xnaKey = KeyboardUtil.ToXna(e.Key);
            if (!keys.Contains(xnaKey)) keys.Add(xnaKey);
        }

        public override bool VSyncEnabled
        {
            [SecuritySafeCritical]    
            get
            {
                return GraphicsContext != null ? GraphicsContext.VSync : false; 
            }

            [SecuritySafeCritical]
            set
            {
                if (GraphicsContext != null) GraphicsContext.VSync = value;
            }
        }
        
		public WebOpenTKGamePlatform(Game game)
            : base(game)
        {
            
            this.GraphicsContext = Game.GraphicsContext;
            this.Window = new WebOpenTKGameWindow(this.GraphicsContext);
            // Setup our OpenALSoundController to handle our SoundBuffer pools
			//soundControllerInstance = OpenALSoundController.GetInstance;

            keys = new List<Keys>();

        }

        public override GameRunBehavior DefaultRunBehavior
        {
            [SecuritySafeCritical]
            get { return GameRunBehavior.Synchronous; }
        }

        private void HandleInput()
        {
            // mouse doesn't need to be treated here, Mouse class does it alone

            // keyboard
            Keyboard.State = new KeyboardState(keys.ToArray());
        }

        [SecuritySafeCritical]
        public override void RunLoop()
        {
            while (IsRunning)
            {
                if (GraphicsContext == null || GraphicsContext.IsDisposed)
                    return;

                if (Game != null)
                {
                    HandleInput();
                    Game.Tick();
                }                
            }
            
        }

        [SecuritySafeCritical]
        public override void StartRunLoop()
        {
            throw new NotImplementedException();
        }

        [SecuritySafeCritical]
        public override void Exit()
        {
            IsRunning = false;
            base.Exit();            
        }

        [SecuritySafeCritical]
        public override bool BeforeUpdate(GameTime gameTime)
        {
			// Update our OpenAL sound buffer pools
			//soundControllerInstance.Update();

            // Let the touch panel update states.
            //TouchPanel.UpdateState();

            return true;
        }

        [SecuritySafeCritical]
        public override bool BeforeDraw(GameTime gameTime)
        {
            return true;
        }

        [SecuritySafeCritical]
        public override void EnterFullScreen()
        {
            ResetWindowBounds(false);
        }

        [SecuritySafeCritical]
        public override void ExitFullScreen()
        {
            ResetWindowBounds(false);
        }

        internal void ResetWindowBounds(bool toggleFullScreen)
        {
            Rectangle bounds;

            bounds = Window.ClientBounds;

            //Changing window style forces a redraw. Some games
            //have fail-logic and toggle fullscreen in their draw function,
            //so temporarily become inactive so it won't execute.

            bool wasActive = IsActive;
            IsActive = false;

            var graphicsDeviceManager = (GraphicsDeviceManager)
                Game.Services.GetService(typeof(IGraphicsDeviceManager));

            if (graphicsDeviceManager.IsFullScreen)
            {
                bounds = new Rectangle(0, 0,
                                       OpenTK.DisplayDevice.Default.Width,
                                       OpenTK.DisplayDevice.Default.Height);
            }
            else
            {
                bounds.Width = graphicsDeviceManager.PreferredBackBufferWidth;
                bounds.Height = graphicsDeviceManager.PreferredBackBufferHeight;
            }

            // Now we set our Presentation Parameters
            var device = (GraphicsDevice)graphicsDeviceManager.GraphicsDevice;
            // FIXME: Eliminate the need for null checks by only calling
            //        ResetWindowBounds after the device is ready.  Or,
            //        possibly break this method into smaller methods.
            if (device != null)
            {
                PresentationParameters parms = device.PresentationParameters;
                parms.BackBufferHeight = (int)bounds.Height;
                parms.BackBufferWidth = (int)bounds.Width;
            }

            //if (toggleFullScreen)
            //    _view.ToggleFullScreen();

            // we only change window bounds if we are not fullscreen
            //if (!graphicsDeviceManager.IsFullScreen)
            //    _view.ChangeClientBounds(bounds);

            IsActive = wasActive;
        }

        [SecuritySafeCritical]
        public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
        {
            
        }

        [SecuritySafeCritical]
        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            
        }

        [SecuritySafeCritical]
        public override void Log(string Message)
        {
            Console.WriteLine(Message);
        }

        [SecuritySafeCritical]
        public override void Present()
        {
            base.Present();

            GraphicsContext.SwapBuffers();
        }

        [SecuritySafeCritical]
        protected override void Dispose(bool disposing)
        {
            GraphicsContext.Dispose ();
			
			base.Dispose(disposing);
        }
			
    }
}
