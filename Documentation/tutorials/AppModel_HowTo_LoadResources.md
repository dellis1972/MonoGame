

# Loading Content

# Complete Sample

The code in this topic shows you the technique for loading content. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download GameLoop_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258702)

# Loading Content

You need a content project in your game if you are using Microsoft Visual Studio 2012 Express to compile and build your project. For more information, see [How to: Add Game Assets to a Content Project](UsingXNA_HowTo_AddAResource.md).

### To load content and ensure it is reloaded when necessary

1.  Derive a class from [Game](T_Microsoft_Xna_Framework_Game.md).
    
2.  Override the [LoadContent](M_MXF_Game_LoadContent.md) method of [Game](T_Microsoft_Xna_Framework_Game.md).
    
3.  In the [LoadContent](M_MXF_Game_LoadContent.md) method, load your content using the [ContentManager](T_Microsoft_Xna_Framework_Content_ContentManager.md).
    
    ```
    SpriteBatch spriteBatch;
    // This is a texture we can render.
    Texture2D myTexture;
    
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        myTexture = Content.Load<Texture2D>("mytexture");
    }
    ```
                        
    
4.  Override the [UnloadContent](M_MXF_Game_UnloadContent.md) method of [Game](T_Microsoft_Xna_Framework_Game.md).
    
5.  In the [UnloadContent](M_MXF_Game_UnloadContent.md) method, unload resources that are not managed by the [ContentManager](T_Microsoft_Xna_Framework_Content_ContentManager.md).
    
    ```
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }
    ```
                        
    

# See Also

#### Reference

[Game Class](T_Microsoft_Xna_Framework_Game.md)  
[LoadContent](M_MXF_Game_LoadContent.md)  
[UnloadContent](M_MXF_Game_UnloadContent.md)  
[Game Members](T_Microsoft_Xna_Framework_Game.md)  
[Microsoft.Xna.Framework Namespace](N_Microsoft_Xna_Framework.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
