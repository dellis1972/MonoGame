

# How to: Add Game Assets to a Content Project

This topic demonstrates how to add an art asset to the game content project, using a texture asset as the example. The same procedure also can be applied to model and sound assets.

It is assumed that an existing Windows Phone game project is loaded in XNA Game Studio.

There are two ways to add a game asset to your game, either by adding the asset file or by adding a link to the asset.

![](note.gif)Tip

Adding an existing asset to your project is quite different from adding an existing item as a link to your project. The first method creats a copy of the asset file and adds the copy to your project. Adding an asset as a link stores only the path to the asset file.

*   [Adding the Texture Asset to the Content Project](#ID4EXB)
*   [Adding the Texture Asset as a Link to the Content Project](#ID4ERD)
*   [Verifying the Content Importer](#ID4EKF)

# Adding the Texture Asset to the Content Project

When you add a game asset (not as a link), XNA Game Studio makes a copy of the asset file and adds the copy to the content project. For this reason, adding a game asset in this manner may be most appropriate when the asset is used by only one developer in one project and is not expected to be changed.

### To add the texture asset to the content project

1.  In **Solution Explorer**, right-click Content, click **Add**, and then click **Existing Item**.
    
2.  Navigate to the location of the texture, and select it.
    
3.  Click the **Add** button.
    
    This creates a copy of the selected asset in your project.
    
4.  Save the solution.
    

# Adding the Texture Asset as a Link to the Content Project

Adding an asset as a link is useful if the referenced asset depends on additional external assets. It ensures that the solution always uses the latest version. For this reason, adding a game asset as a link may be most appropriate when the asset is shared with other people or other game projects, or is likely to be changed.

### To add the texture asset as a link to the content project

1.  In **Solution Explorer**, right-click Content, click **Add**, and then click **Existing Item**.
    
2.  Navigate to the location of the texture, and select it.
    
3.  Click the small arrow to the right of the **Add** button, and then click **Add as Link**.
    
    This creates a reference to the selected asset (and not a copy) in your project.
    
4.  Save the solution.
    

# Verifying the Content Importer

Each time you add a game asset to the solution, you should open the **Properties** window to verify that the asset is correctly recognized and ready to be processed by the Content Pipeline.

### To verify the game asset will use the correct content importer and content processor

1.  In **Solution Explorer**, right-click the asset, and then click **Properties**.
    
2.  In the **Properties** window , verify that the specified **Content Importer** and **Content Processor** are appropriate for the type of the chosen asset.
    
3.  Save the solution.
    

For more information about the **Properties** window of a game asset, see **Game Asset Properties**.

# See Also

[Loading Content](AppModel_HowTo_LoadResources.md)  
[Adding Content to a Game](CP_TopLevel.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team