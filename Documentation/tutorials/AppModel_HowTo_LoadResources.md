

# Loading Content

# Complete Sample

The code in this topic shows you the technique for loading content. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download GameLoop_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258702)

# Loading Content

You need a content project in your game if you are using Microsoft Visual Studio 2012 Express to compile and build your project. For more information, see [How to: Add Game Assets to a Content Project](UsingXNA_HowTo_AddAResource.md).

### To load content and ensure it is reloaded when necessary

1.  Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).
    
2.  Override the [LoadContent](xref:MXF.Game.LoadContent) method of [Game](xref:Microsoft.Xna.Framework.Game).
    
3.  In the [LoadContent](xref:MXF.Game.LoadContent) method, load your content using the [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager).
    
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
                        
    
4.  Override the [UnloadContent](xref:MXF.Game.UnloadContent) method of [Game](xref:Microsoft.Xna.Framework.Game).
    
5.  In the [UnloadContent](xref:MXF.Game.UnloadContent) method, unload resources that are not managed by the [ContentManager](xref:Microsoft.Xna.Framework.Content.ContentManager).
    
    ```
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }
    ```
                        
    

# See Also

#### Reference

[Game Class](xref:Microsoft.Xna.Framework.Game)  
[LoadContent](xref:MXF.Game.LoadContent)  
[UnloadContent](xref:MXF.Game.UnloadContent)  
[Game Members](xref:Microsoft.Xna.Framework.Game)  
[Microsoft.Xna.Framework Namespace](xref:Microsoft.Xna.Framework)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
