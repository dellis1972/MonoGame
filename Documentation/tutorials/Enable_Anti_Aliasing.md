

# Enabling Antialiasing (Multisampling)

Demonstrates how to enable antialiasing for your game.

**Figure 1.  Antialiasing the edges of a cube: multisampling is disabled on the left, and enabled on the right.**

![](graphics_aa.jpg)

Antialiasing is a technique for minimizing distortion artifacts caused by aliasing when rendering a high-resolution signal (such as a sharp edge) at a low resolution (such as in a render target with a fixed number of pixel locations). Antialiasing smoothes sharp edges by partially rendering to neighboring pixels. This technique is also called multisampling because each pixel value can be the result of multiple samples.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download AntiAliasing_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258681).

# Enabling Antialiasing

### To enable antialiasing in your game

*   Render 3D geometry. One way to do this is by creating a BasicEffect using the [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md) class. For more detail, see [Creating a Basic Effect](Use_BasicEffect.md).
    
*   Set [PreferMultiSampling](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferMultiSampling.md) to **true** in your [Game](T_Microsoft_Xna_Framework_Game.md) class constructor.
    
    ```
    graphics.PreferMultiSampling = true;
    ```
    
*   Set the view matrix to place the camera close to the object so you can more clearly see the smoothed, antialiased edges.
    
    ```
    worldMatrix = Matrix.CreateRotationX(tilt) * Matrix.CreateRotationY(tilt);
    
    viewMatrix = Matrix.CreateLookAt(new Vector3(1.75f, 1.75f, 1.75f), Vector3.Zero, Vector3.Up);
    
    projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
        MathHelper.ToRadians(45),  // 45 degree angle
        (float)GraphicsDevice.Viewport.Width /
        (float)GraphicsDevice.Viewport.Height,
        1.0f, 100.0f);
    ```
    
*   Draw the geometry by calling [GraphicsDevice.DrawPrimitives](M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawPrimitives.md).
    
    ```
    RasterizerState rasterizerState1 = new RasterizerState();
    rasterizerState1.CullMode = CullMode.None;
    graphics.GraphicsDevice.RasterizerState = rasterizerState1;
    foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
    {
        pass.Apply();
    
        graphics.GraphicsDevice.DrawPrimitives(
            PrimitiveType.TriangleList,
            0,
            12
        );
    }
    ```
    

# See Also

#### Concepts

[3D Pipeline Basics](3DGraphics_Overview.md)  
[What Is Antialiasing?](WhatIs_Antialiasing.md)  

#### Reference

[GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md)  
[PreparingDeviceSettings](E_Microsoft_Xna_Framework_GraphicsDeviceManager_PreparingDeviceSettings.md)  
[PresentationParameters](T_Microsoft_Xna_Framework_Graphics_PresentationParameters.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team