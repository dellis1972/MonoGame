

# What Is a Sprite?

Sprites are 2D bitmaps that are drawn directly to a render target without using the pipeline for transformations, lighting or effects. Sprites are commonly used to display information such as health bars, number of lives, or text such as scores. Some games, especially older games, are composed entirely of sprites.

*   [Overview](#Overview)
*   [Sprite Origin](#Origin)
*   [Sprite Depth](#Depth)
*   [Sampling Textures](#SourceRect)
*   [Sprite Scaling](#Scaling)
*   [Sprite Transformation Matrices](#Transformation)
*   [Sprite Fonts](#Fonts)
*   [Sprite Batching](#Batching)

# Overview

Sprites are positioned on the screen by coordinates. The width and height of the screen is the same as the back buffer. The x-axis represents the screen width and the y-axis represents the screen height. The y-axis is measured from the top of the screen and increases as you move _down_ the screen, and the x-axis is measured from left to right. For example, when the graphics back buffer is 800×600, 0,0 is the upper left of the screen, and 800,600 is the lower right of the screen.

To draw a sprite, create a [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object, initialize it by calling [Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin), and then call [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) for each sprite. The bitmap data for a sprite is taken from a [Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D) object. The texture may contain alpha channel information to make part of the texture transparent or semi-transparent. You can tint, rotate, or scale sprites by using [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw). This method also gives you the option of drawing only part of the texture on the screen. After you draw a sprite, call [End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End) before calling [Present](xref:Microsoft.Xna.Framework.Graphics.GraphicsDevice.Present).

# Sprite Origin

When you draw a sprite, the sprite _origin_ is an important concept. The origin is a specific point on the sprite, which is by default the upper-left corner of the sprite, or (0,0). [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) draws the origin of the sprite at the screen location you specify. For example, if you draw a 50×50 pixel sprite at location (400,200) without specifying an origin, the upper left of the sprite will be on pixel (400,200). If you use the center of the 50×50 sprite as the origin (25,25), to draw the sprite in the same position you must add the origin coordinates to the position. In this case, the position is (425,225) and the origin is (25,25).

When rotating a sprite, the method uses the origin as the center of the rotation. In these cases, it is common to use the center of the sprite as the origin when calculating where to draw the sprite on the screen.

# Sprite Depth

Sprites also have a concept of _depth_ which is a floating-point number between 0 and 1. Sprites drawn at a depth of 0 are drawn in front of sprites which have a depth of greater than 0; sprites drawn at a depth of 1 are covered by sprites drawn at a depth less than 1.

# Sampling Textures

A sprite is based on a [Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D) object—in other words, a bitmap. Use [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) to draw the entire texture or a portion of the texture. To draw a portion of the texture, use the **sourceRectangle** parameter to specify which _texels_, or texture pixel, to draw. A 32×32 texture has 1024 texels, specified as x and y values similar to how screen coordinates are specified. Specifying a **sourceRectangle** of (0, 0, 16, 16) would select the upper-left quadrant of a 32×32 texture.

# Sprite Scaling

[Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) provides three options for scaling a sprite: using a uniform scaling parameter, a nonuniform scaling parameter, or a source and destination rectangle. The uniform scaling parameter is a floating-point number that multiplies the sprite size through both the x- and y-axes. This will shrink or expand the sprite along each axis equally, maintaining the original ratio between the sprite width and height.

To scale the x- and y-axes independently, [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) accepts a [Vector2](xref:Microsoft.Xna.Framework.Vector2) value as a scalar. This [Vector2](xref:Microsoft.Xna.Framework.Vector2) specifies nonuniform scaling: x- and y-axes are scaled independently according to the [X](F_Microsoft_Xna_Framework_Vector2_X.md) and [Y](F_Microsoft_Xna_Framework_Vector2_Y.md) fields of the [Vector2](xref:Microsoft.Xna.Framework.Vector2).

[Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) also accepts a source and destination rectangle. The destination rectangle is specified in screen coordinates, while the source rectangle is specified in texels. [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) takes the pixels on the texture specified in **sourceRectangle** and scales them independently along the x- and y-axes until they fit the screen coordinates specified by **destinationRectangle**.

# Sprite Transformation Matrices

You can also specify a transformation matrix that the batch can apply to each sprite before drawing. The transformation matrix can be any combination of translation, rotation, or scaling matrices multiplied together into a single matrix. This matrix is combined with the sprite position, rotation, scaling, and depth parameters supplied to [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw). Because the matrix also applies to depth, any z-coordinate transformation that makes the sprite depth greater than 1.0 or less than 0.0 will cause the sprite to disappear.

See [Rotating a Group of Sprites](2DGraphicsHowTo_Rotate_Sprite_Group.md) for an example of matrix rotation and [Scaling Sprites Based On Screen Size](2DGraphicsHowTo_Scale_Sprites_Matrix.md) for an example of matrix scaling.

# Sprite Fonts

Use a [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) to draw text. The [DrawString](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString) method will draw text on screen with position, color, rotation, origin, and scaling. [DrawString](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.DrawString) also requires a special type of texture encapsulated by the [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) class. A [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) is created by the content pipeline when you add a Sprite Font file to your project. The sprite font file has information such as the name and point size of the font, and which Unicode characters to include in the [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) texture. At run time, a [SpriteFont](xref:Microsoft.Xna.Framework.Graphics.SpriteFont) is loaded with [ContentManager.Load](xref:Microsoft.Xna.Framework.Content.ContentManager.Load``1) just like a [Texture2D](xref:Microsoft.Xna.Framework.Graphics.Texture2D) object.

See [Sprite Font XML Schema Reference](CP_SpriteFontSchema.md) for a list of Sprite Font tags. You can use the content pipeline to determine your character regions automatically. For more information, see [How to: Extend the Font Description Processor to Support Additional Characters](CP_HowTo_ExtendFontProcessor.md).

The following redistributable fonts are installed by XNA Game Studio. For information about redistribution rights, see the text in the end user license agreement.

*   Andyb.ttf
*   JingJing.ttf
*   Kooten.ttf
*   Linds.ttf
*   Miramo.ttf
*   Miramob.ttf
*   Moire-Bold.ttf
*   Moire-ExtraBold.ttf
*   Moire-Light.ttf
*   Moire-Regular.ttf
*   MotorwerkOblique.ttf
*   NGO_____.ttf
*   NGOB____.ttf
*   OcraExt.ttf
*   Peric.ttf
*   Pericl.ttf
*   Pesca.ttf
*   Pescab.ttf
*   QuartMS.ttf
*   SegoeKeycaps.ttf
*   Segoepr.ttf
*   Segoeprb.ttf
*   SegoeUIMono-Bold.ttf
*   SegoeUIMono-Regular.ttf
*   Wscsnbd.ttf
*   Wscsnbdit.ttf
*   Wscsnit.ttf
*   Wscsnrg.ttf

# Sprite Batching

In normal drawing, the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) object does not change any render states or draw any sprites until you call [End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End). This is known as _Deferred_ mode. In Deferred mode, [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) saves the information from each [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) call until you call [End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End).

If you call [Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin), specifying [SpriteSortMode.Immediate](T.md#SpriteSortMode_Microsoft_Xna_Framework_Graphics_SpriteSortMode.Immediate), it triggers _Immediate_ mode. In Immediate mode, the [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) immediately changes the graphics device render states to begin drawing sprites. Thereafter, each call to [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) immediately draws the sprite using the current device settings.

In Immediate mode, once you call [Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin) on one [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) instance, do not call it on any other [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) instance until you call [End](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.End) for the first [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch).

Deferred mode is slower than Immediate mode, but it allows multiple instances of [SpriteBatch](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch) to accept [Begin](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Begin) and [Draw](xref:Microsoft.Xna.Framework.Graphics.SpriteBatch.Draw) calls without interfering with each other.

# See Also

[Getting Started with 2D Games at App Hub Online](http://go.microsoft.com/fwlink/?LinkId=128880)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team