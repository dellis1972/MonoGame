

# Creating a Render Target

Demonstrates how to create a render target using the [RenderTarget2D](T_Microsoft_Xna_Framework_Graphics_RenderTarget2D.md) class.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download RenderTarget1.zip](http://go.microsoft.com/fwlink/?LinkId=258720).

![](Graphics_RenderTarget1.jpg)

# Creating a RenderTarget

### To create a render target

1.  Declare variables for a render target using the [RenderTarget2D](T_Microsoft_Xna_Framework_Graphics_RenderTarget2D.md) class and the render target texture using a [Texture2D](T_Microsoft_Xna_Framework_Graphics_Texture2D.md) class.
    
    ```
    SpriteBatch spriteBatch;
    Texture2D grid;
    RenderTarget2D renderTarget;
    ```
    
2.  Create a render target, giving it the same size as the back buffer.
    
    ```
    renderTarget = new RenderTarget2D(
        GraphicsDevice,
        GraphicsDevice.PresentationParameters.BackBufferWidth,
        GraphicsDevice.PresentationParameters.BackBufferHeight);
    ```
    
3.  Load a grid for a texture, which contains two vertical and two horizontal lines.
    
    ```
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
        grid = Content.Load<Texture2D>("grid");
    }
    ```
    
4.  Render the texture to the render target.
    
    This function sets the render target on the device, draws the texture (to the render target) using a [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md), and sets the device render target to null (which resets the device to the back buffer).
    
    ```
    private void DrawRenderTarget()
    {
        // Set the device to the render target
        graphicsDeviceManager.GraphicsDevice.SetRenderTarget(renderTarget);
    
        graphicsDeviceManager.GraphicsDevice.Clear(Color.Black);
    
        spriteBatch.Begin();
        Vector2 pos = Vector2.Zero;
        spriteBatch.Draw(grid, pos, Color.White);
        spriteBatch.End();
    
        // Reset the device to the back buffer
        graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
    }
    ```
    
5.  Draw the render target texture to the back buffer.
    
    ```
    graphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);
    
    spriteBatch.Begin();
    spriteBatch.Draw((Texture2D)renderTarget,
        new Vector2(200, 50),          // x,y position
        new Rectangle(0, 0, 32, 32),   // just one grid
        Color.White                    // no color scaling
        );
    spriteBatch.End();
    ```
    

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0