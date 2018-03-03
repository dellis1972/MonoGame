// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.


namespace Microsoft.Xna.Framework.Graphics
{
    internal partial class ConstantBuffer : GraphicsResource
    {
        private void PlatformInitialize()
        {
        }

        private void PlatformClear()
        {
        }

        internal void PlatformApply(GraphicsDevice device, ShaderStage stage, int slot)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
