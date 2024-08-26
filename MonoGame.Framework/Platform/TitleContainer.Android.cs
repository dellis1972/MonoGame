// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;

namespace Microsoft.Xna.Framework
{
    partial class TitleContainer
    {
        private static Stream PlatformOpenStream(string safeName)
        {
            var stream = Android.App.Application.Context.Assets.Open(safeName, Android.Content.Res.Access.Random);
            if (stream == null)
                return null;
            // Read the asset into memory in one go. This results in a ~50% reduction
            // in load times on Android due to slow Android asset streams.
            return new BufferedStream(stream);
        }
    }
}

