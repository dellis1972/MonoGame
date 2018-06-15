

# Creating a Windows Phone Game or Library Project

This topic describes how to use the C# project templates that are included with XNA Game Studio to begin developing games for Windows Phone using the XNA Framework.

![](note.gif)Note

The Windows Phone project templates for Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0 are available only after the Microsoft Windows Phone SDK has been installed.

![](note.gif)Note

In Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0, you can only build games targeting Windows Phone 7.5, although due to backwards compatability support on Windows Phone, your game will also run on Windows Phone 8.0.

*   [Starting a New Project](#ID4ELF)
*   [Windows Phone Game (4.0)](#ID4EECAC)
*   [Windows Phone Game Library (4.0)](#ID4E5PAC)

![](note.gif)Note

In Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0, you can only build games targeting Windows Phone 7.5, although due to backwards compatability support on Windows Phone, your game will also run on Windows Phone 8.0.

# Starting a New Project

### To start a new project

*   To begin a new Windows Phone project, click **File**, and then click **New Project**.
    
    You’ll be presented with the **New Project** dialog box, listing several C# project templates.
    

![](VS11ChooseNewCSPhoneProject.png)

XNA Game Studio provides template types for XNA Framework game development in the **XNA Game Studio 4.0/>** section of the **Visual C#** project types. The templates offered for developing XNA Framework games for Windows Phone are:

Project type

Description

Windows Phone Game (4.0)

A project for creating an XNA Framework 4.0 game application for Windows Phone.

Windows Phone Game Library (4.0)

A project for creating an XNA Framework 4.0 game library for Windows Phone.

Content Pipeline Extension Library (4.0)

A project for creating an XNA Framework 4.0 Content Pipeline Extension Library.

# Windows Phone Game (4.0)

XNA Game Studio provides a Windows Phone Game template that creates and loads a set of starter files. This new project contains basic code that renders a colored background.

### To create a Windows Phone Game (4.0) project

1.  From the **File** menu, click **New Project**.
    
2.  Select the **Windows Phone Game (4.0)** project type.
    
3.  Type the name for the game project in the **Name** text box.
    
    You can also modify the default values for the **Location** and **Solution Name** controls. In the example below, the project name and the solution name are both "WindowsPhoneGame1".
    
4.  Click **OK**.
    
    Your new game project will be created and loaded in Microsoft Visual Studio.
    

## Results

After creating a new Windows Phone game, your solution in Microsoft Visual Studio 2012 will look like the following:

![](VS11CSWindowsPhoneGameInSolutionExplorer.png)

In this case, your Windows Phone game solution actually contains two projects, each with their own distinct features, as described in the following table.

Solution Explorer Item

Description

Project: WindowsPhoneGame1

This project contains the items that constitute the source code and image representation portion of your Windows Phone game. Note that the project name might be different based on the string in the **Name** text box in the **New Project** dialog box.

    Properties

The Properties node provides access to various properties that control many aspects of your current project. Some examples include general application settings, debug settings, and additional project resources. You can use the Project Designer to modify the values for these properties.

This node also allows direct access to three configuration file that you can edit manually: AppManifest.xml, AssemblyInfo.cs, and WMAppManifest.xml. For example, the game title is stored in the AssemblyInfo.cs file, which you can modify using the **Assembly Information** dialog box or by manually editing the AssemblyInfo.cs file.

    References

Upon solution creation, the References node shows the assemblies that are automatically added to a new Windows Phone game project. These include references to several XNA assemblies and a larger set of system assemblies.

In addition to these standard assemblies, you can add references to other assemblies as needed for your Windows Phone game project.

    Content References

The Content References node references the second project that is automatically added to your Windows Phone game solution. This second project is a game content project named, in this case, "WindowsPhoneGame1Content", and which is used to contain the assets used by your Windows Phone game. For more information about game content projects, see [Game Content Project](UsingXNA_GameContentProjects.md).

    Background.png

The image file Background.png is the image that is displayed when your game is pinned to the Start screen of a Windows Phone. Optimally, this image is 62 × 62 pixels in size (to avoid scaling) and should graphically represent your game.

    Game.ico

The icon file Game.ico is not used by Windows Phone games and should be ignored. This file is included in your project in case you decide to port your game to Xbox 360 and Windows in the future.

    Game1.cs

The C# source file Game1.cs is a good starting point for adding simple game logic and basic features. It implements a single class derived from [Game](T_Microsoft_Xna_Framework_Game.md) and called `Game1`, and it overrides five methods: [LoadContent](M_MXF_Game_LoadContent.md), [UnloadContent](M_MXF_Game_UnloadContent.md), [Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md), [Draw](M_Microsoft_Xna_Framework_Game_Draw.md), and [Update](M_Microsoft_Xna_Framework_Game_Update.md). In addition, the `Game1` constructor is defined. Use these methods to initialize your game components, to load and render your game content, and to handle any input from the user or changes to the game environment.

    PhoneGameThumb.png

The image file PhoneGameThumb.png is the default icon shown when the game is displayed in the Games Hub of a Windows Phone. Optimally, this image is 173 × 173 pixels in size (to avoid scaling) and should graphically represent your game.

    Program.cs

The C# source file Program.cs implements a single class (called `Program`) that provides an entry point to game execution. Usually, little code is added to this file unless the game is fairly advanced.

Project: WindowsPhoneGame1Content (Content)

This game content project occupies a parallel position with the game project in the solution, and it stores and builds art assets for your Windows Phone game. For more information, see [Game Content Project](UsingXNA_GameContentProjects.md).

Note that the project name might be different based on the string in the **Name** text box in the **New Project** dialog box.

    References

The References node provides references to the default Content Importer assemblies provided with Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0. If you create one or more custom Content Importers, you would add references to their assemblies here.

# Windows Phone Game Library (4.0)

XNA Game Studio provides a Windows Phone Game Library (4.0) template that creates and loads a set of starter files. Typically, projects of this type contain managed classes that implement basic or advanced features used by a game engine. Once completed, these class libraries can be referenced by other Windows Phone projects. Also, they provide common functionality without having that code reside within the game project.

The new project contains basic code that implements an empty library, usable by other Windows Phone Game projects or Windows Phone Game libraries.

### To create a Windows Phone Game Library (4.0) project

1.  From the **File** menu, click **New Project**.
    
2.  Select the **Windows Phone Game Library (4.0)** project type.
    
3.  Type the name for the library project in the **Name** text box.
    
    You can also modify the default values for the **Location** and **Solution Name** controls. In the example below, the project name and the solution name are both "WindowsPhoneGameLibrary1".
    
4.  Click **OK** to create and load the new project.
    

## Results

After creating a new Windows Phone game library, your solution in Microsoft Visual Studio 2012 will look like the following:

![](VS11CSWindowsPhoneGameLibraryInSolutionExplorer.png)

In this case, your Windows Phone game library solution contains a single project, with its own distinct features, as described in the following table.

Solution Explorer Item

Description

Project: WindowsPhoneGameLibrary1

This project contains the items that constitute the source code portion of your Windows Phone game library. Note that the project name might be different based on the string in the **Name** text box in the **New Project** dialog box.

    Properties

The Properties node allows access to the various properties that control many aspects of your current project. Some examples include general application settings, debug settings, and additional project resources. You can use the Project Designer to modify the values for these properties.

This node also allows direct access to the configuration file AssemblyInfo.cs, which you can edit manually. For example, the library title is stored in the AssemblyInfo.cs file, which you can modify using the **Assembly Information** dialog box or by manually editing the AssemblyInfo.cs file.

    References

Upon solution creation, the References node shows the assemblies that are automatically added to a new Windows Phone game library project. These include references to several XNA assemblies and a larger set of system assemblies.

In addition to these standard assemblies, you can add references to other assemblies as needed for your Windows Phone game library project.

    Class1.cs

This C# source file Class1.cs begins as an empty C# class within a namespace. It provides a starting point for adding functionality to your Windows Phone game library.

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team