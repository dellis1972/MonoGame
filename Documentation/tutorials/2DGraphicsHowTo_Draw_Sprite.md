

# Drawing a Sprite

Demonstrates how to draw a sprite by using the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) class.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download SpriteDemo_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258730).

# Drawing a Sprite

### To draw a sprite on screen

1.  Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).
    
2.  Define a [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object as a field on your game class.
    
3.  In your [LoadContent](xref:MXF.Game.LoadContent) method, construct the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object, passing the current graphics device.
    
4.  Load the textures that will be used for drawing sprites in [LoadContent](xref:MXF.Game.LoadContent).
    
    In this case, the example uses the **Content** member to load a texture from the MonoGame Framework Content Pipeline. The texture must be in the project, with the same name passed to [Load](xref:Microsoft.Xna.Framework.Content.ContentManager.Load``1).
    
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
    
5.  In the overridden [Draw](xref:Microsoft.Xna.Framework.Game.Draw) method, call [Clear](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.Clear).
    
6.  After [Clear](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.Clear), call [Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object.
    
7.  Create a [Vector2](xref:Microsoft.Xna.Framework.Vector2) to represent the screen position of the sprite.
    
8.  Call [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object, passing the texture to draw, the screen position, and the color to apply.
    
9.  Use [Color.White](xref:MXF.Color) to draw the texture without any color effects.
    
10.  When all the sprites have been drawn, call [End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object.
    
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

[SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)  
[Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw)  
[Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)  

© 2012 Microsoft Corporation. All rights reserved. 

© The MonoGame Team.
