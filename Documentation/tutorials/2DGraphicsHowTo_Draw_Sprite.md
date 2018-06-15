

# Drawing a Sprite

Demonstrates how to draw a sprite by using the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) class.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download SpriteDemo_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258730).

# Drawing a Sprite

### To draw a sprite on screen

1.  Derive a class from [Game](T_Microsoft_Xna_Framework_Game.md).
    
2.  Define a [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object as a field on your game class.
    
3.  In your [LoadContent](M_MXF_Game_LoadContent.md) method, construct the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object, passing the current graphics device.
    
4.  Load the textures that will be used for drawing sprites in [LoadContent](M_MXF_Game_LoadContent.md).
    
    In this case, the example uses the **Content** member to load a texture from the MonoGame Framework Content Pipeline. The texture must be in the project, with the same name passed to [Load](M_Microsoft_Xna_Framework_Content_ContentManager_Load``1.md).
    
    ```
    private Texture2D SpriteTexture;
    private Rectangle TitleSafe;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        SpriteTexture = Content.Load<Texture2D>("ship");
        TitleSafe = GetTitleSafeArea(.8f);
    }
    ```
    
5.  In the overridden [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method, call [Clear](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_Clear.md).
    
6.  After [Clear](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_Clear.md), call [Begin](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin.md) on your [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object.
    
7.  Create a [Vector2](T_Microsoft_Xna_Framework_Vector2.md) to represent the screen position of the sprite.
    
8.  Call [Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md) on your [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object, passing the texture to draw, the screen position, and the color to apply.
    
9.  Use [Color.White](T_MXF_Color.md) to draw the texture without any color effects.
    
10.  When all the sprites have been drawn, call [End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md) on your [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object.
    
    ```
    protected override void Draw(GameTime gameTime)
    {
        graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();
        Vector2 pos = new Vector2(TitleSafe.Left, TitleSafe.Top);
        spriteBatch.Draw(SpriteTexture, pos, Color.White);
        spriteBatch.End();
        base.Draw(gameTime);
    }
    ```
    

# See Also

#### Concepts

[What Is a Sprite?](Sprite_Overview.md)  

#### Reference

[SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md)  
[Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md)  
[Texture2D](T_Microsoft_Xna_Framework_Graphics_Texture2D.md)  

© 2012 Microsoft Corporation. All rights reserved. 

© The MonoGame Team.
