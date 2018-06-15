

# Displaying Multiple Screens with Viewports

Demonstrates how to use viewports to display different scenes simultaneously using two cameras.

![](graphics_split_screen.png)

# Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download SplitScreenWindows.zip](http://go.microsoft.com/fwlink/?LinkId=258729).

# Using Viewports to Display Multiple Screens

### To create multiple screens

1.  In your [LoadContent](M_MXF_Game_LoadContent.md) method, create two new [Viewport](T_Microsoft_Xna_Framework_Graphics_Viewport.md) objects to define the two new "split" regions of the screen.
    
    In this example, the screen is split in half vertically.
    
    ```
    Viewport defaultViewport;
    Viewport leftViewport;
    Viewport rightViewport;
    Matrix projectionMatrix;
    Matrix halfprojectionMatrix;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
        defaultViewport = GraphicsDevice.Viewport;
        leftViewport = defaultViewport;
        rightViewport = defaultViewport;
        leftViewport.Width = leftViewport.Width / 2;
        rightViewport.Width = rightViewport.Width / 2;
        rightViewport.X = leftViewport.Width;
        //            rightViewport.X = leftViewport.Width + 1;
    ```
    
2.  Create a projection matrix to fit each new viewport.
    
    In this case, because the screen is split in half, only one new projection matrix is necessary. It has the same settings as the 4:3 full screen projection matrix, except the aspect ratio is now 2:3.
    
    ```
        projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4, 4.0f / 3.0f, 1.0f, 10000f);
        halfprojectionMatrix = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4, 2.0f / 3.0f, 1.0f, 10000f);
    }
    ```
    
3.  In your [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method, assign one of the viewports to draw as the [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md)[Viewport](T_Microsoft_Xna_Framework_Graphics_Viewport.md).
    
4.  Draw your scene as normal, using the camera (or view matrix) associated with this perspective along with the proper projection matrix.
    
    ```
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Viewport = defaultViewport;
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        GraphicsDevice.Viewport = leftViewport;
        DrawScene(gameTime, Camera1.ViewMatrix, halfprojectionMatrix);
    ```
    
5.  After drawing the first scene, assign the other viewport to the [Viewport](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_Viewport.md) property.
    
6.  Draw your scene again with the associated camera or view matrix, and the proper projection matrix.
    
    ```
        GraphicsDevice.Viewport = rightViewport;
        DrawScene(gameTime, Camera2.ViewMatrix, halfprojectionMatrix);
    
        base.Draw(gameTime);
    
    }
    ```
    

# See Also

#### Concepts

[What Is a Viewport?](WhatIs_Viewport.md)  

#### Reference

[GraphicsDevice.Viewport](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_Viewport.md)  
[Viewport Structure](T_Microsoft_Xna_Framework_Graphics_Viewport.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team