

# Scaling a Sprite

Demonstrates how to scale a sprite using a uniform scale.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download ScaleSprite_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258724).

# Drawing a Scaled Sprite

### To draw a scaled sprite with a uniform scale

1.  Follow the procedures of [Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md).
    
2.  In your **Update** method, determine how your sprite will be scaled.
    
    The normal size of the sprite is multiplied by the scale specified. For example, a value of 1.0 draws the sprite full size, where 0.5 will draw it half-sized and 2.0 will draw it at twice its original size.
    
    ```
            protected float scale = 1f;
            protected override void Update(GameTime gameTime)
            {
                ...
    
    #if WINDOWS
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    this.Exit();
    #endif
    
                // The time since Update was called last.
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
                // TODO: Add your game logic here.
                scale += elapsed;
                scale = scale % 6;
    
                base.Update(gameTime);
            }
    ```
    
3.  When drawing the sprite, specify the scale of the sprite as a parameter to draw.
    
    Specifying a floating-point scale parameter scales the sprite evenly in both the x and y directions.
    
    ```
    protected virtual void DrawForeground(SpriteBatch batch)
    {
        Rectangle safeArea = GetTitleSafeArea(.8f);
        Vector2 position = new Vector2(safeArea.X, safeArea.Y);
        batch.Begin();
        batch.Draw(SpriteTexture, position, null,
            Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        batch.End();
    }
    ```
    
4.  When all the sprites have been drawn, call [End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md) on your [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object.

### To draw a scaled sprite using a nonuniform scale

1.  Follow the procedures of [Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md).
2.  In your **Update** method, determine how your sprite will be scaled along each axis and store those values in a [Vector2](T_Microsoft_Xna_Framework_Vector2.md) object.
    
    ```
    protected Vector2 nonuniformscale = Vector2.One;
    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        float basescale = nonuniformscale.Y;
        basescale += (float)gameTime.ElapsedGameTime.TotalSeconds;
        basescale = basescale % 6;
        nonuniformscale.Y = basescale;
        nonuniformscale.X = basescale * .8f;
    }
    ```
    
3.  When drawing the sprite, specify the scale of the sprite using the [Vector2](T_Microsoft_Xna_Framework_Vector2.md) object that you updated earlier.
    
    Specifying a [Vector2](T_Microsoft_Xna_Framework_Vector2.md) scales the sprite independently in both the x and y directions.
    
    ```
    protected override void DrawForeground(SpriteBatch batch)
    {
        Rectangle safeArea = GetTitleSafeArea(.8f);
        Vector2 position = new Vector2(safeArea.X, safeArea.Y);
        batch.Begin();
        batch.Draw(SpriteTexture, position, null, Color.White, 0, Vector2.Zero,
            nonuniformscale, SpriteEffects.None, 0);
        batch.End();
    }
    ```
    
4.  When all of the sprites have been drawn, call [End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md) on your [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object.
    

### To draw a scaled sprite using a destination rectangle

1.  Follow the procedures of [Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md).
    
2.  In your **Update** method, construct a rectangle that defines where on screen the sprite will be drawn.
    
    This rectangle does not need to be the same shape or size as the original sprite. Each dimension of the sprite is scaled independently to fit the destination rectangle.
    
    ```
    protected Rectangle destrect;
    protected override void Update(GameTime gameTime)
    {
        destrect = new Rectangle();
        Rectangle safeArea = GetTitleSafeArea(.8f);
        destrect.X = safeArea.X;
        destrect.Y = safeArea.Y;
        destrect.Width = (int)scale * 100;
        destrect.Height = (int)scale * 80;
        base.Update(gameTime);
    }
    ```
    
3.  When drawing the sprite, specify the destination rectangle as a parameter to [Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md).
    
    The sprite will be drawn, filling the destination rectangle.
    
    ```
    protected override void DrawForeground(SpriteBatch batch)
    {
        batch.Begin();
        batch.Draw(SpriteTexture, destrect, Color.White);
        batch.End();
    }
    ```
    
4.  When all of the sprites have been drawn, call [End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md) on your [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object.
    

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

© The MonoGame Team.
