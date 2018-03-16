

# Drawing Text with a Sprite

Demonstrates how to import a [SpriteFont](T_Microsoft_Xna_Framework_Graphics_SpriteFont.md) into a project and to draw text using [DrawString](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_DrawString.md).

# Complete Sample

The code in this topic shows you the draw technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download FontDemo_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258699).

# Adding a Sprite Font and Drawing Text

### To add a sprite font

1.  Right-click your Content project in Solution Explorer, click **Add**, and then click **New Item**.
    
2.  In the **Add New Item** dialog box, click **Sprite Font**.
    
    You may find it convenient at this point to change the name of the new file from "SpriteFont1" to the friendly name of the font you intend to load (keeping the .spritefont file extension). The friendly name identifies the font once it is installed on your computer, for example, "Courier New" or "Times New Roman." When you reference the font in your code, you must use the friendly name you have assigned it.
    
    XNA Game Studio creates a new .spritefont file for your font and opens it.
    
3.  If you did not name the new file with the font's friendly name, type the friendly name of the font to load into the FontName element.
    
    Again, this is not the name of a font file, but rather the name that identifies the font once it is installed on your computer. You can use the Fonts folder in the **Control Panel** to see the names of fonts installed on your system, and to install new ones. The content pipeline supports the same fonts as the [System.Drawing.Font](http://msdn.microsoft.com/en-us/library/system.drawing.font.aspx) class, including TrueType fonts, but not bitmap (.fon) fonts. You may find it convenient to save the new .spritefont file using this friendly name. When you reference the font in your code, you must use the friendly name you have assigned it.
    
4.  If necessary, change the **Size** entry to the point size you desire for your font.
    
5.  If necessary, change the **Style** entry to the style of font to import.
    
    You can specify **Regular**, **Bold**, **Italic**, or **Bold, Italic**. The **Style** entry is case sensitive.
    
6.  Specify the character regions to import for this font.
    
    Character regions specify which characters in the font are rendered by the [SpriteFont](T_Microsoft_Xna_Framework_Graphics_SpriteFont.md). You can specify the start and end of the region by using the characters themselves, or by using their decimal values with an &# prefix. The default character region includes all the characters between the space and tilde characters, inclusive.
    

### To draw text on the screen

1.  Add a Sprite Font to your project as described above.
    
2.  Create a [SpriteFont](T_Microsoft_Xna_Framework_Graphics_SpriteFont.md) object to encapsulate the imported font.
    
3.  Create a [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object for drawing the font on the screen.
    
4.  In your [LoadContent](M_MXF_Game_LoadContent.md) method, call [ContentManager.Load](M_Microsoft_Xna_Framework_Content_ContentManager_Load``1.md), specifying the [SpriteFont](T_Microsoft_Xna_Framework_Graphics_SpriteFont.md) class and the asset name of the imported font.
    
5.  Create your [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object, passing the current [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md).
    
                          `SpriteFont Font1;
    Vector2 FontPos;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        Font1 = Content.Load<SpriteFont>("Courier New");
    
        // TODO: Load your game content here            
        FontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
            graphics.GraphicsDevice.Viewport.Height / 2);
    }`
                        
    
6.  In your [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method, call [Begin](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Begin.md) on the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object.
    
7.  If necessary, determine the origin of your text.
    
    If you want to draw your text centered on a point, you can find the center of the text by calling [MeasureString](O_M_Microsoft_Xna_Framework_Graphicsx_SpriteFont_MeasureString.md) and dividing the returned vector by 2.
    
8.  Call [DrawString](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_DrawString.md) to draw your output text, specifying the [SpriteFont](T_Microsoft_Xna_Framework_Graphics_SpriteFont.md) object for the font you want to use.
    
    All other parameters of [DrawString](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_DrawString.md) produce the same effects as a call to [SpriteBatch.Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md).
    
9.  Call [SpriteBatch.End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md) after all text is drawn.
    
                          `protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        spriteBatch.Begin();
    
        // Draw Hello World
        string output = "Hello World";
    
        // Find the center of the string
        Vector2 FontOrigin = Font1.MeasureString(output) / 2;
        // Draw the string
        spriteBatch.DrawString(Font1, output, FontPos, Color.LightGreen,
            0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
    
        spriteBatch.End();
        base.Draw(gameTime);
    }`
                        
    

# See Also

#### Tasks

[Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md)  

#### Concepts

[What Is a Sprite?](Sprite_Overview.md)  

#### Reference

[SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md)  
[DrawString](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_DrawString.md)  
[SpriteFont](T_Microsoft_Xna_Framework_Graphics_SpriteFont.md)  
[ContentManager.Load](M_Microsoft_Xna_Framework_Content_ContentManager_Load``1.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0