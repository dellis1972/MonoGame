

# What Is a Viewport?

A viewport is a 2D rectangle that defines the size of the rendering surface onto which a 3D scene is projected.

A viewport is represented by the [Viewport](T_Microsoft_Xna_Framework_Graphics_Viewport.md) class. The **Width** and **Height** properties of the rectangle are used by methods for a variety of calculations such as creating a projection matrix using [CreatePerspectiveFieldOfView](O_M_Microsoft_Xna_Framework_Matrix_CreatePerspectiveFieldOfView.md). Viewport dimensions default to the dimensions of the back buffer, but they can be a different size. For example, to create a split screen game, you might render multiple viewports to a single render target. The viewport dimensions do not correspond to the display dimensions, which are determined by the [DisplayMode](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DisplayMode.md) property, or to the game window, which is determined by the [ClientBounds](P_Microsoft_Xna_Framework_GameWindow_ClientBounds.md) property.

Use the [Clear](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_Clear.md) method to reset a render-target surface. Overloaded versions accept various parameters such as a set of rectangles that define more specific areas rather than the entire surface. Specify in the [ClearOptions](T_Microsoft_Xna_Framework_Graphics_ClearOptions.md) enumeration which buffer or render target to reset.

Use the [Clear](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_Clear.md) method to clear stencil bits within a depth buffer. Set the options parameter to determine which render-target components are cleared. The color argument sets the color value of the render target after it has been cleared. The depth parameter clears the depth buffer to the specified depth: 0.0 is the closest distance, and 1.0 is the farthest. Finally, the stencil parameter resets the stencil bits to the specified value. Use integers ranging from 0 to 2n−1, where _n_ is the stencil buffer bit depth.

# Desktop Area Versus Display Area

The size of the desktop area in Windows is shown by the [DisplayMode](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DisplayMode.md)**Width** and **Height** properties. The display area is different from the game window, which can be obtained from the [ClientBounds](P_Microsoft_Xna_Framework_GameWindow_ClientBounds.md) dimensions. On Windows, set the display mode width and height to match the back buffer width and height. This prevents the need to resize the back buffer when switching between windowed and full screen mode. On Xbox 360, you need to display the text and menu items within the inner 80 percent of the display area. This prevents text or menus from getting cut off at the edge of a television screen. This inner area is called the title safe area.

The game window size is shown by the [ClientBounds](P_Microsoft_Xna_Framework_GameWindow_ClientBounds.md)**Width** and **Height** properties. The game window size matches the display size when a game is running in full-screen mode. The game window size may be smaller for a game running in windowed mode. Use **ClientBounds.X** and **ClientBounds.Y** for the upper-left coordinates of the game window relative to the display area. On Xbox 360, the [ClientBounds](P_Microsoft_Xna_Framework_GameWindow_ClientBounds.md)**Width** and **Height** properties are always equal to the [DisplayMode](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DisplayMode.md)**Width** and **Height**, and **ClientBounds.X** and **ClientBounds.Y** are always (0, 0).

**Figure 1.  **DisplayMode** and **ClientBounds** areas on Windows and Xbox 360.**

![](Viewport.jpg)

# See Also

[What Is a Back Buffer?](WhatIs_BackBuffer.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0