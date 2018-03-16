

# Drawing a Masked Sprite over a Background

Demonstrates how to draw a foreground and background sprite using the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) class, where only part of the foreground sprite masks the background.

The foreground sprite in this example must include masking information.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download BackgroundSprite_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258686).

# Drawing a Foreground and Background Sprite

### To draw a foreground and background sprite

1.  Create the game class, and load your content as described in the procedures of [Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md).
    
    ```
    private Vector2 ViperPos;  // Position of foreground sprite on screen
    private int ScrollHeight; // Height of background sprite
    private Viewport viewport;
    Texture2D ShipTexture;
    Texture2D StarTexture;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
        StarTexture = Content.Load<Texture2D>("starfield");
        ShipTexture = Content.Load<Texture2D>("ship");
        viewport = graphics.GraphicsDevice.Viewport;
    
        ViperPos.X = viewport.Width / 2;
        ViperPos.Y = viewport.Height - 100;
        ScrollHeight = StarTexture.Height;
    }
    ```
    
2.  In [Draw](M_Microsoft_Xna_Framework_Game_Draw.md), call [Begin](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin.md) for the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md).
    
3.  Specify [BlendState.None](T_Microsoft_Xna_Framework_Graphics_BlendState.md).
    
    This will tell the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) to ignore alpha color values when drawing sprites. By default, the z-order of sprites is the order in which they are drawn.
    
4.  Draw the background sprites, and then call [End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md).
    
    ```
    spriteBatch.Begin();
    DrawBackground(spriteBatch);
    spriteBatch.End();
    ```
    
5.  Call [Begin](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin.md) for the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) again.
    
6.  This time, specify [BlendState.AlphaBlend](T_Microsoft_Xna_Framework_Graphics_BlendState.md).
    
    This will cause pixels on the sprite with an alpha value less than 255 to become progressively transparent based on the magnitude of the alpha value. An alpha of 0 will make the pixel completely transparent. Calling [Begin](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin.md) with no parameters causes [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) to default to [BlendState.AlphaBlend](T_Microsoft_Xna_Framework_Graphics_BlendState.md).
    
7.  Draw the foreground sprites, then call [End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md).
    
    ```
    spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
    DrawForeground(spriteBatch);
    spriteBatch.End();
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

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0