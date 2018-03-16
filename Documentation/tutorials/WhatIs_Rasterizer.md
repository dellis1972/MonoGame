

# What Is Rasterizer State?

Rasterizer state determines how to render 3D data such as position, color, and texture onto a 2D surface.

Rasterization takes a 3D scene containing polygons (which are represented by triangles and vertices) and renders the scene onto a 2D surface. This requires mapping or transforming the 3D vertices into 2D vertices using the world, view, and projection transforms to calculate the final vertex positions in the viewing frustum. To reduce the amount of geometry that needs to be rasterized, the rasterizer clips geometry so that only the parts of a polygon that are visible get processed. The resulting list of transformed vertices is then scan-converted to determine how to fill pixel positions between vertices. Scissor testing takes a list of user-supplied rectangles to further limit the areas you may want rasterized.

Create a rasterizer state object using the [RasterizerState](T_Microsoft_Xna_Framework_Graphics_RasterizerState.md) class. Set the rasterizer state to the graphics device using the [RasterizerState](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_RasterizerState.md) property.

This is the default state for rasterization:

*   Render triangles with clockwise winding order.
    
*   Fill primitives so they are solid.
    
*   Turn off scissor testing.
    
*   Enable multisampling.
    
*   Avoid using either depth bias or sloped scaled depth bias.
    

These are the corresponding API states:

*   Set [CullMode](P_Microsoft_Xna_Framework_Graphics_RasterizerState_CullMode.md) to **CullMode.CullCounterClockwiseFace**.
    
*   Set [FillMode](P_Microsoft_Xna_Framework_Graphics_RasterizerState_FillMode.md) to **FillMode.Solid**.
    
*   Set [ScissorTestEnable](P_Microsoft_Xna_Framework_Graphics_RasterizerState_ScissorTestEnable.md) to **false**.
    
*   Set [MultiSampleAntiAlias](P_Microsoft_Xna_Framework_Graphics_RasterizerState_MultiSampleAntiAlias.md) to **true**.
    
*   Set [DepthBias](P_Microsoft_Xna_Framework_Graphics_RasterizerState_DepthBias.md) and [SlopeScaleDepthBias](P_Microsoft_Xna_Framework_Graphics_RasterizerState_SlopeScaleDepthBias.md) to 0.
    

Built-in state objects make it easy to create objects with the most common rasterizer state settings. **CullNone**, **CullClockwise**, and **CullCounterClockwise** are the most common settings. For an example of creating a state object, see [Creating a State Object](StateObject1.md).

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0