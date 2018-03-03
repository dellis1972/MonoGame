// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;

#if ANGLE
using OpenTK.Graphics;
#else
using MonoGame.OpenGL;
#endif

namespace Microsoft.Xna.Framework.Graphics
{
    public partial class GraphicsDevice
    {
        private void PlatformSetup ()
        {
        }

        private void PlatformInitialize ()
        {
        }

        private void PlatformReset()
        {
            
        }

        private void PlatformPresent()
        {
            
        }

        private void OnPresentationChanged()
        {
            
        }

        private void PlatformSetViewport (ref Viewport value)
        {
            
        }

        private void PlatformResolveRenderTargets ()
        {
            
        }

        private void PlatformApplyDefaultRenderTarget ()
        {
            
        }

        private IRenderTarget PlatformApplyRenderTargets ()
        {
            
        }

        private void PlatformDrawIndexedPrimitives (PrimitiveType primitiveType, int baseVertex, int startIndex, int primitiveCount)
        {
        }

        private void PlatformDrawUserPrimitives<T> (PrimitiveType primitiveType, T [] vertexData, int vertexOffset, VertexDeclaration vertexDeclaration, int vertexCount) where T : struct
        {
        }

        private void PlatformDrawPrimitives (PrimitiveType primitiveType, int vertexStart, int vertexCount)
        {
        }

        private void PlatformDrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T [] vertexData, int vertexOffset, int numVertices, short [] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct
        {
        }

        private void PlatformDrawUserIndexedPrimitives<T> (PrimitiveType primitiveType, T [] vertexData, int vertexOffset, int numVertices, int [] indexData, int indexOffset, int primitiveCount, VertexDeclaration vertexDeclaration) where T : struct
        {
        }

        private void PlatformDrawInstancedPrimitives (PrimitiveType primitiveType, int baseVertex, int startIndex,
            int primitiveCount, int instanceCount)
        {
        }

        private void PlatformGetBackBufferData<T> (Rectangle? rect, T [] data, int startIndex, int count) where T : struct
        {
        }
    }
}
