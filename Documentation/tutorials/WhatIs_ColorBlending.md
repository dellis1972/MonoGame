

# What Is Color Blending?

Color blending mixes two colors together to produce a third color.

The first color is called the source color which is the new color being added. The second color is called the destination color which is the color that already exists (in a render target, for example). Each color has a separate blend factor that determines how much of each color is combined into the final product. Once the source and destination colors have been multiplied by their blend factors, the results are combined according to the specified blend function. The normal blend function is simple addition.

The blend formula looks like this:

(source * sourceBlendFactor) blendFunction (destination*destinationBlendFactor)

The source blend factor is specified by the [ColorSourceBlend](P_Microsoft_Xna_Framework_Graphics_BlendState_ColorSourceBlend.md) property, and the destination blend factor is specified by the [ColorDestinationBlend](P_Microsoft_Xna_Framework_Graphics_BlendState_ColorDestinationBlend.md) property. The [ColorBlendFunction](P_Microsoft_Xna_Framework_Graphics_BlendState_ColorBlendFunction.md) property specifies the blend function to use, normally [BlendFunction.Add](T.md#BlendFunction_Microsoft_Xna_Framework_Graphics_BlendFunction.Add). In that case the formula reduces to this:

(source * sourceBlendFactor) + (destination * destinationBlendFactor)  

When no blending is done, a source pixel overwrites a destination pixel. When blending, you can create a lot of special effects using the blending properties:

Blend type

Blend settings

Alpha Blending

(_source_ × [Blend.SourceAlpha](T.md#Blend_Microsoft_Xna_Framework_Graphics_Blend.SourceAlpha)) \+ (_destination_ × [Blend.InvSourceAlpha](T.md#Blend_Microsoft_Xna_Framework_Graphics_Blend.InvSourceAlpha))

Additive Blending

(_source_ × [Blend.One](T.md#Blend_Microsoft_Xna_Framework_Graphics_Blend.One)) \+ (_destination_ × [Blend.One](T.md#Blend_Microsoft_Xna_Framework_Graphics_Blend.One))

Multiplicative Blending

(_source_ × [Blend.Zero](T.md#Blend_Microsoft_Xna_Framework_Graphics_Blend.Zero)) \+ (_destination_ × [Blend.SourceColor](T.md#Blend_Microsoft_Xna_Framework_Graphics_Blend.SourceColor))

2X Multiplicative Blending

(_source_ × [Blend.DestinationColor](T.md#Blend_Microsoft_Xna_Framework_Graphics_Blend.DestinationColor)) \+ (_destination_ × [Blend.SourceColor](T.md#Blend_Microsoft_Xna_Framework_Graphics_Blend.SourceColor))

**Figure 1.  This picture illustrates four common blend modes. From left to right: Alpha blending, Additive blending, Multiplicative blending, and 2X Multiplicative blending. The top image in each column is the source image and below, it's effect when added to the destination.**

![](blends.jpg)

Alpha blending uses the alpha channel of the source color to create a transparency effect so that the destination color appears through the source color. For example, if you clear your backbuffer to [Color.Gray](T.md#Color_MXF_Color.Gray), it will be colored (0.5,0.5,0.5,1). If you then take a white color with a partial alpha value (1,1,1,0.4), the result will be 60 percent of the destination color and 40 percent of the source: (0.5 x 0.6) + (1 x 0.4). The resulting color will be (0.7,0.7,0.7, 1). The alpha values are multiplied as well - (.6 x 1) + .4 gives us an alpha value of 1.

When drawing a sprite using the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) class, choose [BlendState.AlphaBlend](T.md#BlendState_Microsoft_Xna_Framework_Graphics_BlendState.AlphaBlend) to configure alpha blending.

By default, the alpha channel is blended along with the red, green, and blue channels using the [ColorSourceBlend](P_Microsoft_Xna_Framework_Graphics_BlendState_ColorSourceBlend.md) and [ColorDestinationBlend](P_Microsoft_Xna_Framework_Graphics_BlendState_ColorDestinationBlend.md) properties. You can choose to customize the blending for just the alpha channel by using the [AlphaSourceBlend](P_Microsoft_Xna_Framework_Graphics_BlendState_AlphaSourceBlend.md) and [AlphaDestinationBlend](P_Microsoft_Xna_Framework_Graphics_BlendState_AlphaDestinationBlend.md) properties.

# See Also

#### Concepts

[What Is a Sprite?](Sprite_Overview.md)  
[3D Pipeline Basics](3DGraphics_Overview.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team