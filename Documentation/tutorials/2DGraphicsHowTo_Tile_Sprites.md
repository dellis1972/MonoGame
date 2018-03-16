

# Tiling a Sprite

Demonstrates how to draw a sprite repeatedly in the x and y directions in one [Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md) call.

![](graphics_sprite_tiled.jpg)

This sample uses a texture addressing mode to duplicate a texture across the area defined by [SpriteBatch.Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md). Other address modes, such as mirroring, can create interesting results.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download TiledSprites_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258736).

# Tiling a Sprite

### To tile a sprite

1.  Follow the procedures of [Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md).
2.  In the [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method, create a [Rectangle](T_Microsoft_Xna_Framework_Rectangle.md) to define the area to fill.
    
    The destination [Rectangle](T_Microsoft_Xna_Framework_Rectangle.md) can be any size. In this example, the width and height of the destination rectangle are integer multiples of the source sprite. This will cause the sprite texture to be tiled, or drawn several times, to fill the destination area.
    
    ```
    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.LinearWrap,
        DepthStencilState.Default, RasterizerState.CullNone);
    ```
    
3.  Call [SpriteBatch.Begin](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin.md) to set the sprite state.
    
4.  Set the [TextureAddressMode](T_Microsoft_Xna_Framework_Graphics_TextureAddressMode.md) in the [SamplerState](T_Microsoft_Xna_Framework_Graphics_SamplerState.md) to [TextureAddressMode.LinearWrap](T.md#TextureAddressMode_Microsoft_Xna_Framework_Graphics_TextureAddressMode.LinearWrap).
    
    ```
    spriteBatch.Draw(spriteTexture, Vector2.Zero, destRect, color, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
    ```
    
5.  Call [SpriteBatch.Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md) with the sprite, the destination rectangle, and other relevant parameters.
6.  Call [End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md) on your [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object.
    
    ```
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
[SpriteSortMode](T_Microsoft_Xna_Framework_Graphics_SpriteSortMode.md)  
[Texture2D](T_Microsoft_Xna_Framework_Graphics_Texture2D.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0