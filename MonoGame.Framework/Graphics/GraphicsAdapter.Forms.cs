// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MonoGame.OpenGL;

namespace Microsoft.Xna.Framework.Graphics
{
    partial class GraphicsAdapter
    {
        
        private static void PlatformInitializeAdapters(out ReadOnlyCollection<GraphicsAdapter> adapters)
        {
            GL.LoadEntryPoints();
            var adapter = new GraphicsAdapter() {
                _currentDisplayMode = new DisplayMode(0,0, SurfaceFormat.Color),
            };
            adapters = new ReadOnlyCollection<GraphicsAdapter>(new GraphicsAdapter[] { adapter });
        }



        private bool PlatformIsProfileSupported(GraphicsProfile graphicsProfile)
        {
            if(UseReferenceDevice)
                return true;
            switch(graphicsProfile)
            {
                case GraphicsProfile.Reach:
                    return true;
                case GraphicsProfile.HiDef:
                    return true;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
