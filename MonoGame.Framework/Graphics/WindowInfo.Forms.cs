using System;

namespace MonoGame.OpenGL
{
    public class WindowInfo : IWindowInfo
    {
        public IntPtr Handle { get; private set; }

        public WindowInfo(IntPtr handle)
        {
            Handle = handle;
        }
    }
}
