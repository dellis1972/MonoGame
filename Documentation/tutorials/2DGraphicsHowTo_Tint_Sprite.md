

# Tinting a Sprite

Demonstrates how to tint a sprite using a [Color](xref:MXF.Color) value.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download TintSprite_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258737).

# Drawing a Tinted Sprite

### To draw a tinted sprite

1.  Follow the procedures of [Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md).
2.  In the [Update](xref:Microsoft.Xna.Framework.Game.Update) method, determine how to tint the sprite.
    
    In this example, the value of the game pad thumbsticks determine the Red, Green, Blue, and Alpha values to apply to the sprite.
    
    ```
    protected Color tint;
    protected override void Update(GameTime gameTime)
    {
        ...
        GamePadState input = GamePad.GetState(PlayerIndex.One);
        tint = new Color(GetColor(input.ThumbSticks.Left.X),
            GetColor(input.ThumbSticks.Left.Y),
            GetColor(input.ThumbSticks.Right.X),
            GetColor(input.ThumbSticks.Right.Y));
    
        base.Update(gameTime);
    }
    ```
    
3.  In the [Draw](xref:Microsoft.Xna.Framework.Game.Draw) method, pass the color value created in [Update](xref:Microsoft.Xna.Framework.Game.Update) to [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw).
    
    ```
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        spriteBatch.Begin();
        spriteBatch.Draw(SpriteTexture, position, tint);
        spriteBatch.End();
    
        base.Draw(gameTime);
    }
    ```
    
4.  When all of the sprites have been drawn, call [End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object.
    

# See Also

#### Tasks

[Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md)  

#### Concepts

[What Is a Sprite?](Sprite_Overview.md)  

#### Reference

[SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)  
[Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw)  
[Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)  
[Color](xref:MXF.Color)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
