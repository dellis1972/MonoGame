// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using System.Security;
using MonoGame.Utilities;

namespace MonoGame.OpenGL
{
    internal partial class GL
    {
		static partial void LoadPlatformEntryPoints()
        {
            
        }

        private static IGraphicsContext PlatformCreateContext (IWindowInfo info)
        {
            return null;
        }
    }

	internal static class EntryPointHelper {

        [DllImport("kernel32")]
        private static extern IntPtr LoadLibrary(string fileName);

        [DllImport("kernel32.dll")]
        private static extern int FreeLibrary(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr handle, string procedureName);

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlopen(String fileName, int flags);

        [DllImport("libdl.dylib")]
        private static extern IntPtr dlsym(IntPtr handle, String symbol);

        [DllImport("libdl.dylib")]
        private static extern int dlclose(IntPtr handle);

        static IntPtr Handle;

        static EntryPointHelper ()
        {
            if (CurrentPlatform.OS == OS.Windows)
            {
                Handle = LoadLibrary("opengl32.dll");
            }
            if (CurrentPlatform.OS == OS.Linux)
            {
                Handle = dlopen("libGL.so", 2);
            }
            if (CurrentPlatform.OS == OS.MacOSX)
            {
                Handle = dlopen("/System/Library/Frameworks/OpenGL.framework/OpenGL", 2);
            }
            if (Handle == IntPtr.Zero)
                throw new Exception("Failed to load OpenGL");
        }

		public static IntPtr GetAddress(String function)
		{
            if (CurrentPlatform.OS == OS.Windows) {
                return GetProcAddress(Handle, function);
            }
            if (CurrentPlatform.OS != OS.Windows)
            {
                return dlsym(Handle, function);
            }
			return IntPtr.Zero;
		}
	}
}

