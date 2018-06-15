

# Scaling Sprites Based On Screen Size

Demonstrates how to scale sprites using a matrix that is created based on the viewport width.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download ScaleScreen_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258723).

# Scaling Sprites Based on Screen Size

### To scale sprites based on screen size

1.  Use the [PreferredBackBufferHeight](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferHeight.md) and [PreferredBackBufferWidth](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferWidth.md) properties of [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) during your game's [Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md) to set the default screen size of your game.
    
2.  In your [LoadContent](M_MXF_Game_LoadContent.md) method, use [Matrix.CreateScale](O_M_Microsoft_Xna_Framework_Matrix_CreateScale.md) to create a scaling matrix.
    
    This matrix is recreated any time the resolution of the [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md) changes.
    
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
                        
    
3.  In your [Update](M_Microsoft_Xna_Framework_Game_Update.md) method, determine whether the game needs to change screen resolution.
    
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
                        
    
4.  In your [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method, call [SpriteBatch.Begin](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin.md), passing the scaling matrix created in [LoadContent](M_MXF_Game_LoadContent.md).
    
5.  Draw your scene normally, then call [SpriteBatch.End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md).
    
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

[SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md)  
[Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md)  
[Texture2D](T_Microsoft_Xna_Framework_Graphics_Texture2D.md)  
[Matrix.CreateScale](O_M_Microsoft_Xna_Framework_Matrix_CreateScale.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
