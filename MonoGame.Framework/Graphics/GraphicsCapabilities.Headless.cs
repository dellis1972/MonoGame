// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

#if OPENGL
using MonoGame.OpenGL;
using GetParamName = MonoGame.OpenGL.GetPName;
#endif

namespace Microsoft.Xna.Framework.Graphics
{

    internal partial class GraphicsCapabilities
    {
        /// <summary>
        /// True, if GL_ARB_framebuffer_object is supported; false otherwise.
        /// </summary>
        internal bool SupportsFramebufferObjectARB { get; private set; }

        /// <summary>
        /// True, if GL_EXT_framebuffer_object is supported; false otherwise.
        /// </summary>
        internal bool SupportsFramebufferObjectEXT { get; private set; }

        /// <summary>
        /// True, if GL_IMG_multisampled_render_to_texture is supported; false otherwise.
        /// </summary>
        internal bool SupportsFramebufferObjectIMG { get; private set; }


        private void PlatformInitialize(GraphicsDevice device)
        {
        }

    }
}
