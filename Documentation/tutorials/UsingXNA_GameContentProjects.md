

# Game Content Project

A game content project contains the game assets (models, textures, sounds, and so on) used by the game projects in your solution. A game solution, which may contain multiple content projects, should contain at least one content project to specify and build game content. Game assets are added as project items to the content project by using either the standard **Add New Item** or **Add Existing Item**. Each item's properties (accessible in the [Properties](UsingXNA_Dlg_Properties.md) window) define how the content item is imported and processed by the Content Pipeline when building the game projects that use the assets.

Game content projects are assigned to game projects in the Content **References** node of each game project. In this way, content projects can be used to apply different collections of game assets to game projects intended for different platforms. The **References** node in the content project identifies the assemblies and projects containing the Content Pipeline importers and processors assigned to each game asset.

![](caution.gif)Caution

The **Build Action** property of any item moved into the content project is set automatically to Compile. This setting persists even if the same item later is moved out of the content project.

# Content in Game Libraries

Classes and methods in game libraries may rely on specific content, such as textures and shaders, to perform game-related functions. Game libraries provide their own content subfolders that can contain those assets to ensure their availability.

When a game project includes a reference to a game library, a copy of the output of that game library is included in the output directory of the game project. In this way, both the game library's referenced assembly and its associated content are available at run time.

All content in the game content subproject of a successfully built game library is compiled through the Content Pipeline, just as the code of a game library is stored in its compiled form. This permits game projects that reference these reusable elements of the game library to build more quickly.

Content within a game library may be referenced directly by the game project. There is no requirement that the game library's classes or methods must reference the library's content.

# Project Designer Properties

Project design properties are unavailable to game content projects. All changeable properties are in the Content Pipeline properties.

# Content Pipeline Properties

The content project has additional properties (other than those available in the Project Designer tabs) that specify operating parameters for the XNA Framework Content Pipeline.

### To access Content Pipeline properties

1.  In **Solution Explorer**, select the **Content** node.
    
2.  On the **View** menu, click **Properties Window** or press F4.
    
    The Properties window opens.
    

## Content Root Directory

Specifies the name of the subdirectory that will hold the final output files of pipeline content generated from the project folder. This may be useful if your game project has multiple content projects and you wish to keep the output of each in different subdirectories.

# Code-Only Builds

By manually editing a .csproj file, you can build your game project without also building the content project. If you are using XNA Game Studio, open the related .csproj file, and add the following property:

      <SkipNestedContentBuild>true</SkipNestedContentBuild>
    

If you are building from the command line, use the following command to skip the content project:

Msbuild /p:SkipNestedContentBuild=true WindowsGame1.csproj

For more information about using MSBuild with project files, see [How to: Edit Project Files](http://msdn.microsoft.com/en-us/library/ms171487.aspx) and [Visual Studio Integration (MSBuild)](http://msdn.microsoft.com/en-us/library/ms171468.aspx).

In addition, you can build _only_ the content project from the command line by invoking MSBuild directly in the .contentproj file of your content project.

# See Also

[How to: Add a Game Content Project](UsingXNA_GameContent_Add.md)  
[Adding Art, Music, and Other Game Assets](UsingXNA_GameContent_Overviews.md)  
[Getting Started with XNA Game Studio Development](Getting_Started.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team