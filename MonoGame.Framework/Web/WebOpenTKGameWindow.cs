using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using System.Security;

namespace Microsoft.Xna.Framework
{
    [SecuritySafeCritical]    
    class WebOpenTKGameWindow : GameWindow
    {
        Rectangle clientBounds;

        public WebOpenTKGameWindow(IGraphicsContext  graphicsContext)
        {
            clientBounds = new Rectangle();
        }

        public override bool AllowUserResizing
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public override void BeginScreenDeviceChange(bool willBeFullScreen)
        {
            
        }

        public override Rectangle ClientBounds
        {
            get { return clientBounds; }
        }

        public override DisplayOrientation CurrentOrientation
        {
            get { return DisplayOrientation.Default; }
        }

        public override void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight)
        {
            
        }

        public override IntPtr Handle
        {
            get { return IntPtr.Zero; }
        }

        public override string ScreenDeviceName
        {
            get { return "Broswer"; }
        }

        protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
        {
            
        }

        protected override void SetTitle(string title)
        {
            
        }
    }
}
