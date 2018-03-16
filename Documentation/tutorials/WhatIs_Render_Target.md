

# What Is a Render Target?

A render target is a memory buffer for rendering pixels. One common use for a render target is offscreen rendering.

The graphics pipeline has a default render target called the back buffer, which is a portion of video memory that contains the next frame to be drawn. If your program does not create a render target, and you render to the screen, you are using the back buffer by default. Use the [RenderTarget2D](T_Microsoft_Xna_Framework_Graphics_RenderTarget2D.md) class to create additional render targets. A common scenario is to render to one or more offscreen render targets and assemble them in the back buffer to produce the final frame.

The [RenderTarget2D](T_Microsoft_Xna_Framework_Graphics_RenderTarget2D.md) class derives from the [Texture2D](T_Microsoft_Xna_Framework_Graphics_Texture2D.md) class because a render target contains a 2D texture. This is a change from previous versions of XNA Game Studio. To draw a render target like you would draw a texture, use the render target object directly. That is, pass a [RenderTarget2D](T_Microsoft_Xna_Framework_Graphics_RenderTarget2D.md) object to any method that takes a [Texture2D](T_Microsoft_Xna_Framework_Graphics_Texture2D.md) object. To read render target data back to the CPU, call the [Texture2D.GetData Method](O_M_Microsoft_Xna_Framework_Graphics_Texture2D_GetData.md). For more information about using a render target, see [Creating a Render Target](RenderTarget1.md).

To use a render target, create a [RenderTarget2D](T_Microsoft_Xna_Framework_Graphics_RenderTarget2D.md) object with the width, height, and other options you prefer. Call [GraphicsDevice.SetRenderTarget](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_SetRenderTarget.md) to make your render target the current render target. Any subsequent **Draw** calls you make will draw into your render target rather than the back buffer. When you are finished rendering, call [GraphicsDevice.SetRenderTarget](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_SetRenderTarget.md) and pass it null to set the device to render to the back buffer again.

There are other considerations for choosing width and height when you create a render target. For example, you should choose the width and height of the back buffer to match the size of your game window (although Windows Phone scales the final result to match the user's screen). This prevents any resizing when the back buffer is copied to the screen. However, an offscreen render target does not need to have the same width and height as the back buffer. The final image can be rendered in several small render targets and then reassembled in a larger render target.

A render target has a surface format, which describes how many bits are allocated to each pixel, and how they are divided between red, green, blue, and alpha. For example, [SurfaceFormat.Bgr565](T.md#SurfaceFormat_Microsoft_Xna_Framework_Graphics_SurfaceFormat.Bgr565) allocates 16 bits per pixel; 5 bits for blue and red and 6 bits for green.

A render target works in cooperation with a depth-stencil buffer. When creating a render target, the depth format for the depth-stencil buffer is specified as one of the parameters to the render target constructor. Anytime you set a new render target to the device, the matching depth buffer is also set to the device.

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0