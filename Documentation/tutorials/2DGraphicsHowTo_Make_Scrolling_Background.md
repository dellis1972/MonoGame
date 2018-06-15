

# Making a Scrolling Background

Demonstrates how to draw a scrolling background sprite using the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) class.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download ScrollingBackground_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258726).

# Drawing a Scrolling Background Sprite

### To draw a scrolling background sprite

1.  Create the game class.
    
2.  Load resources as described in the procedures of [Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md).
    
3.  Load the background texture.
    
    ```
    private ScrollingBackground myBackground;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        myBackground = new ScrollingBackground();
        Texture2D background = Content.Load<Texture2D>("starfield");
        myBackground.Load(GraphicsDevice, background);
    }
    ```
    
4.  Determine the size of the background texture and the size of the screen.
    
    The texture size is determined using the [Height](P_Microsoft_Xna_Framework_Graphics_Texture2D_Height.md) and [Width](P_Microsoft_Xna_Framework_Graphics_Texture2D_Width.md) properties, and the screen size is determined using the [Viewport](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_Viewport.md) property on the graphics device.
    
5.  Using the texture and screen information, set the origin of the texture to the center of the top edge of the texture, and the initial screen position to the center of the screen.
    
    ```
    // class ScrollingBackground
    private Vector2 screenpos, origin, texturesize;
    private Texture2D mytexture;
    private int screenheight;
    public void Load( GraphicsDevice device, Texture2D backgroundTexture )
    {
        mytexture = backgroundTexture;
        screenheight = device.Viewport.Height;
        int screenwidth = device.Viewport.Width;
        // Set the origin so that we're drawing from the 
        // center of the top edge.
        origin = new Vector2( mytexture.Width / 2, 0 );
        // Set the screen position to the center of the screen.
        screenpos = new Vector2( screenwidth / 2, screenheight / 2 );
        // Offset to draw the second texture, when necessary.
        texturesize = new Vector2( 0, mytexture.Height );
    }
    ```
    
6.  To scroll the background, change the screen position of the background texture in your [Update](M_Microsoft_Xna_Framework_Game_Update.md) method.
    
    This example moves the background down 100 pixels per second by increasing the screen position's Y value.
    
    ```
    protected override void Update(GameTime gameTime)
    {
        ...
        // The time since Update was called last.
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
        // TODO: Add your game logic here.
        myBackground.Update(elapsed * 100);
    
        base.Update(gameTime);
    }
    ```
    
    The Y value is kept no larger than the texture height, making the background scroll from the bottom of the screen back to the top.
    
    ```
    public void Update( float deltaY )
    {
        screenpos.Y += deltaY;
        screenpos.Y = screenpos.Y % mytexture.Height;
    }
    // ScrollingBackground.Draw
    ```
    
7.  Draw the background using the origin and screen position calculated in [LoadContent](M_MXF_Game_LoadContent.md) and [Update](M_Microsoft_Xna_Framework_Game_Update.md).
    
    ```
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        spriteBatch.Begin();
        myBackground.Draw(spriteBatch);
        spriteBatch.End();
    
        base.Draw(gameTime);
    }
    ```
    
    In case the texture doesn't cover the screen, another texture is drawn. This subtracts the texture height from the screen position using the **texturesize** vector created at load time. This creates the illusion of a loop.
    
    ```
    public void Draw( SpriteBatch batch )
    {
        // Draw the texture, if it is still onscreen.
        if (screenpos.Y < screenheight)
        {
            batch.Draw( mytexture, screenpos, null,
                 Color.White, 0, origin, 1, SpriteEffects.None, 0f );
        }
        // Draw the texture a second time, behind the first,
        // to create the scrolling illusion.
        batch.Draw( mytexture, screenpos - texturesize, null,
             Color.White, 0, origin, 1, SpriteEffects.None, 0f );
    }
    ```
    

# See Also

#### Tasks

[Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md)  
[Drawing a Masked Sprite over a Background](2DGraphicsHowTo_Draw_Sprite_Background.md)  

#### Concepts

[What Is a Sprite?](Sprite_Overview.md)  

#### Reference

[SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md)  
[Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md)  
[Texture2D](T_Microsoft_Xna_Framework_Graphics_Texture2D.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
