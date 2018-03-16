

# What Is a Stencil Buffer?

A stencil buffer contains per-pixel integer data which is used to add more control over which pixels are rendered. A stencil buffer can also be used in combination with a depth buffer to do more complex rendering such as simple shadows or outlines.

A stencil buffer operates similarly to a [depth buffer](WhatIs_DepthBuffer.md). So similarly, that stencil data is stored in a depth buffer. While depth data determines which pixel is closest to the camera, stencil data can be used as a more general purpose per-pixel mask for saving or discarding pixels. To create the mask, use a stencil function to compare a reference stencil value -- a global value -- to the value in the stencil buffer each time a pixel is rendered.

For example, to remove an object from a scene, fill a stencil buffer with a cut out pattern (using zeros) for each pixel where the object is visible. This is done by setting the reference stencil value to 0, clearing the stencil buffer, and rendering the object. Then set the reference stencil value to 1, set the compare function to [CompareFunction.LessEqual](T_Microsoft_Xna_Framework_Graphics_CompareFunction.md), and render again. The stencil data masks those pixels whose value is non zero but less than 1, resulting in drawing over (or removing) the object.

A stencil buffer can be used in more sophisticated ways such as specifying [StencilOperations](T_Microsoft_Xna_Framework_Graphics_StencilOperation.md) that go beyond replace or discard and increment or decrement the stencil buffer during a stencil test. Then combine this with a [StencilMask](P_Microsoft_Xna_Framework_Graphics_DepthStencilState_StencilMask.md) to mask the portion of the stencil buffer that is updated.

To use a stencil buffer, the [DepthFormat](T_Microsoft_Xna_Framework_Graphics_DepthFormat.md) of the depth buffer must reserve some bits for the stencil data; the [DepthFormat.Depth24Stencil8](T.md#DepthFormat_Microsoft_Xna_Framework_Graphics_DepthFormat.Depth24Stencil8) format uses 8 bits for a stencil buffer as an example. Combining stencil data with an 8 bit [DepthStencilState.StencilMask Property](P_Microsoft_Xna_Framework_Graphics_DepthStencilState_StencilMask.md) provide up to eight different stencil buffers.

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0