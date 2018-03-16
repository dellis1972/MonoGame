

# What Is a Depth Buffer?

A depth buffer contains per-pixel floating-point data for the z depth of each pixel rendered. A depth buffer may also contain stencil data which can be used to do more complex rendering such as simple shadows or outlines.

When a pixel is rendered, color data as well as depth data can be stored. If a pixel is rendered a second time - such as when two objects overlap - depth testing determines which pixel is closer to the camera. The depth function determines what to do with the test result. For example, if [CompareFunction.LessEqual](T.md#CompareFunction_Microsoft_Xna_Framework_Graphics_CompareFunction.LessEqual) is the current depth function, if the current pixel depth is less than or equal to the previous pixel depth, the current pixel depth is written to the depth buffer. Values that fail the depth test are discarded.

The depth of a pixel, which ranges between 0.0 and 1.0, is determined based on the view and projection matrices. A pixel that touches the near plane has depth 0, a pixel that touches the far plane has depth 1. As each object in the scene is rendered, normally the pixels that are closest to the camera are kept, as those objects block the view of the objects behind them.

A depth buffer may also contain stencil bits - for this reason it's often called a _depth-stencil buffer_. The depth format describes the data format of the depth buffer. The depth buffer is always 32 bits, but those bits can be arranged in different ways, similar to how texture formats can vary. A common depth format is [DepthFormat.Depth24Stencil8](T.md#DepthFormat_Microsoft_Xna_Framework_Graphics_DepthFormat.Depth24Stencil8), where 24 bits are used for the depth data and 8 bits are used for the stencil data. [DepthFormat.Depth24Stencil8Single](T.md#DepthFormat_Microsoft_Xna_Framework_Graphics_DepthFormat.Depth24Stencil8Single) is a more unusual format where the 24 bits for the depth buffer are arranged as a floating point value. Use [DepthFormat.None](T.md#DepthFormat_Microsoft_Xna_Framework_Graphics_DepthFormat.None) if you don't want to create a depth buffer.

Use [DepthStencilState.DepthBufferEnable](P_Microsoft_Xna_Framework_Graphics_DepthStencilState_DepthBufferEnable.md) to enable or disable depth buffering. Use the [DepthStencilState.DepthBufferFunction](P_Microsoft_Xna_Framework_Graphics_DepthStencilState_DepthBufferFunction.md) to change the comparison function used for the depth test. Clear the depth buffer by passing [ClearOptions.DepthBuffer](T.md#ClearOptions_Microsoft_Xna_Framework_Graphics_ClearOptions.DepthBuffer) to [GraphicsDevice.Clear](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_Clear.md).

In previous versions of XNA Game Studio, **DepthStencilBuffer** is a class; you create one and set it on the device when you want depth buffering. You cannot read data from it or render to it. A default depth buffer is created when the back buffer is created. A common problem occurs when drawing into a render target without creating a matching depth buffer (matching means size and data format must be the same). This was true even if you disable depth buffering. And you needed to set the depth buffer to null if it does not match the render target.

In Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0, there is no **DepthStencilBuffer** type. The runtime automatically creates a depth buffer when a render target is created, and you specify the format for the depth buffer in a render target's constructor along with the surface format. This prevents a render target from being created without a matching depth buffer.

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0