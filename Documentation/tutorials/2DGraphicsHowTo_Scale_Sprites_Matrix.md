

# Scaling Sprites Based On Screen Size

Demonstrates how to scale sprites using a matrix that is created based on the viewport width.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download ScaleScreen_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258723).

# Scaling Sprites Based on Screen Size

### To scale sprites based on screen size

1.  Use the [PreferredBackBufferHeight](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferHeight) and [PreferredBackBufferWidth](xref:Microsoft.Xna.Framework.GraphicsDeviceManager.PreferredBackBufferWidth) properties of [GraphicsDeviceManager](xref:Microsoft.Xna.Framework.GraphicsDeviceManager) during your game's [Initialize](xref:Microsoft.Xna.Framework.Game.Initialize) to set the default screen size of your game.
    
2.  In your [LoadContent](xref:MXF.Game.LoadContent) method, use [Matrix.CreateScale](xref:Microsoft.Xna.Framework.Matrix.CreateScale) to create a scaling matrix.
    
    This matrix is recreated any time the resolution of the [GraphicsDevice](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice) changes.
    
    Because you are scaling sprites, you should use only the x and y parameters to create the scaling matrix. Scaling the depth of sprites can result in their depth shifting above 1.0. If that happens, they will not render.
    
    ```
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
        ...
    
        // Default resolution is 800x600; scale sprites up or down based on
        // current viewport
        float screenscale =
            (float)graphics.GraphicsDevice.Viewport.Width / 800f;
        // Create the scale transform for Draw. 
        // Do not scale the sprite depth (Z=1).
        SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);
    }
    ```
                        
    
3.  In your [Update](xref:Microsoft.Xna.Framework.Game.Update) method, determine whether the game needs to change screen resolution.
    
    This example uses game pad buttons to switch between two resolutions.
    
    ```
    protected override void Update(GameTime gameTime)
    {
        ...
        // Change the resolution dynamically based on input
        if (GamePad.GetState(PlayerIndex.One).Buttons.A ==
            ButtonState.Pressed)
        {
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();
        }
        if (GamePad.GetState(PlayerIndex.One).Buttons.B ==
            ButtonState.Pressed)
        {
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();
        }
    
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.ApplyChanges();
        }
        if (Keyboard.GetState().IsKeyDown(Keys.B))
        {
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            graphics.ApplyChanges();
        }
    
        base.Update(gameTime);
    }
    ```
                        
    
4.  In your [Draw](xref:Microsoft.Xna.Framework.Game.Draw) method, call [SpriteBatch.Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin), passing the scaling matrix created in [LoadContent](xref:MXF.Game.LoadContent).
    
5.  Draw your scene normally, then call [SpriteBatch.End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End).
    
    All of the sprites you draw will be scaled according to the matrix.
    
    ```
    protected override void Draw(GameTime gameTime)
    {
        ...
        // Initialize the batch with the scaling matrix
        spriteBatch.Begin();
        // Draw a sprite at each corner
        for (int i = 0; i < spritepos.Length; i++)
        {
            spriteBatch.Draw(square, spritepos[i], null, Color.White,
                rotation, origin, scale, SpriteEffects.None, depth);
        }
        spriteBatch.End();
        base.Draw(gameTime);
    }
    ```
                        
    

# See Also

#### Tasks

[Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md)  

#### Concepts

[What Is a Sprite?](Sprite_Overview.md)  

#### Reference

[SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)  
[Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw)  
[Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)  
[Matrix.CreateScale](xref:Microsoft.Xna.Framework.Matrix.CreateScale)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
