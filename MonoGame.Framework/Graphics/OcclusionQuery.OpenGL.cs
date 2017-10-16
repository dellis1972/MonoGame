// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.
using System;
using MonoGame.OpenGL;

namespace Microsoft.Xna.Framework.Graphics
{
    partial class OcclusionQuery
    {
        private int glQueryId = -1;

        private void PlatformConstruct()
        {
            if (GL.GenQueries == null)
                throw new NotSupportedException("OcclusionQuery is not supported on this device.");
            GL.GenQueries(1, out glQueryId);
            GraphicsExtensions.CheckGLError();
        }

        private void PlatformBegin()
        {
            GL.BeginQuery(GL.BoundQueryTarget, glQueryId);
            GraphicsExtensions.CheckGLError();
        }

        private void PlatformEnd()
        {
            GL.EndQuery(GL.BoundQueryTarget);
            GraphicsExtensions.CheckGLError();
        }

        private bool PlatformGetResult(out int pixelCount)
        {
            int resultReady = 0;
            GL.GetQueryObject(glQueryId, GetQueryObjectParam.QueryResultAvailable, out resultReady);
            GraphicsExtensions.CheckGLError();

            if (resultReady == 0)
            {
                pixelCount = 0;
                return false;
            }

            GL.GetQueryObject(glQueryId, GetQueryObjectParam.QueryResult, out pixelCount);
            GraphicsExtensions.CheckGLError();

            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (glQueryId > -1)
                {
                    GraphicsDevice.DisposeQuery(glQueryId);
                    glQueryId = -1;
                }
            }

            base.Dispose(disposing);
        }
    }
}

