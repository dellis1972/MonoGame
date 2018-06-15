

# Tiling a Sprite

Demonstrates how to draw a sprite repeatedly in the x and y directions in one [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) call.

![](graphics_sprite_tiled.jpg)

This sample uses a texture addressing mode to duplicate a texture across the area defined by [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw). Other address modes, such as mirroring, can create interesting results.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download TiledSprites_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258736).

# Tiling a Sprite

### To tile a sprite

1.  Follow the procedures of [Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md).
2.  In the [Draw](xref:Microsoft.Xna.Framework.Game.Draw) method, create a [Rectangle](xref:Microsoft.Xna.Framework.Rectangle) to define the area to fill.
    
    The destination [Rectangle](xref:Microsoft.Xna.Framework.Rectangle) can be any size. In this example, the width and height of the destination rectangle are integer multiples of the source sprite. This will cause the sprite texture to be tiled, or drawn several times, to fill the destination area.
    
    ```
    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.LinearWrap,
        DepthStencilState.Default, RasterizerState.CullNone);
    ```
    
3.  Call [SpriteBatch.Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin) to set the sprite state.
    
4.  Set the [TextureAddressMode](xref:Microsoft.Xna.Framework.Graphics.TextureAddressMode) in the [SamplerState](xref:Microsoft.Xna.Framework.Graphics.SamplerState) to [TextureAddressMode.LinearWrap](T.md#TextureAddressMode_Microsoft_Xna_Framework_Graphics_TextureAddressMode.LinearWrap).
    
    ```
    spriteBatch.Draw(spriteTexture, Vector2.Zero, destRect, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
    ```
    
5.  Call [SpriteBatch.Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) with the sprite, the destination rectangle, and other relevant parameters.
6.  Call [End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End) on your [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object.
    
    ```
    spriteBatch.End();
    ```
    

# See Also

#### Tasks

[Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md)  

#### Concepts

[What Is a Sprite?](Sprite_Overview.md)  

#### Reference

[SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch)  
[Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw)  
[SpriteSortMode](xref:Microsoft.Xna.Framework.Graphics.SpriteSortMode)  
[Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
