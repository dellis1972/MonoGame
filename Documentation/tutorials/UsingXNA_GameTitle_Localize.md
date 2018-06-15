

# Localizing the Title of a Windows Phone Game

The title of the game appears in the Application List or Game Hub of the Windows Phone device. It will also appear when the game is pinned to the Start screen.

A localized game title must provide translations for the following languages (for cultures):

*   English (United States)
*   English (United Kingdom)
*   French (France)
*   Italian (Italy)
*   German (Germany)
*   Spanish (Spain)

Localized data are provided within resource-only DLLs, which contain the resource strings for each supported language.

![](note.gif)Note

The procedure for localizing a game title on Windows Phone is very similar to the procedure for [localizing a Windows Phone application title](http://msdn.microsoft.com/library/ff967550(v=VS.92).aspx), but with some important differences.

To localize a game title, perform the following steps:

*   [Prerequisites](#ID4EIC)
*   [Create a Language Resource Project and Strings](#ID4EDD)
*   [Create the First Localized Language Resource Strings](#ID4EABAC)
*   [Create the Remaining Language Resource Strings](#ID4ELFAC)
*   [Make the Windows Phone Game Project Dependent on Resource DLL Projects](#ID4ESHAC)
*   [Update the Windows Phone Game to Load Localized Title Strings](#ID4EUIAC)

# Prerequisites

These procedures must be performed on a new or existing Windows Phone project. To create a new project, see [Creating a Windows Phone Game or Library Project](UsingXNA_CreatingMobileProject.md).

![](note.gif)Important

These procedures can only be performed when using Microsoft Visual Studio 2012 Professional, Premium or Ultimate. Solutions that contain both C++ and C# projects, as this feature requires, are not supported in Microsoft Visual Studio Express.

# Create a Language Resource Project and Strings

In this procedure, you create a resource-only DLL named AppResLib.dll that contains a resource string table with the language-neutral title of your game.

![](note.gif)Important

The DLL you create in this procedure must be named AppResLib.dll.

### To create a language resource DLL project

1.  From the **File** menu, choose **New** and then click **Project**.
    
    The **New Project** dialog appears.
    
2.  In the left pane, click **Installed Templates**, expand **Visual C++** and then click **Win32**.
3.  In the list of project types, click **Win32 Project** and name it AppResLib.
4.  In the Win32 Application Wizard, select **DLL** and check the **Empty Project** check box, then click **Finish**.
5.  In **Solution Explorer**, select the new DLL project.
6.  From the **Project** menu, click **Properties**.
    
    The **Property Pages** dialog appears.
    
7.  Change the **Configuration** drop-down to **All Configurations** (from **Active (Debug)**).
8.  In the left pane, expand **Configuration Properties**, expand **Linker**, and then click **Advanced**.
9.  Select the **No EntryPoint** property, set the value to **Yes (/NOENTRY)**, and click **OK**.

### To create the language-neutral resource strings

1.  In **Solution Explorer**, select the new DLL project.
2.  From the **Project** menu, click **Add Resource**.
    
    The **Add Resource** dialog appears.
    
3.  In the **Resource type** list, select **String Table** and click **New**.
    
    The resource string table opens.
    
4.  Create two resource strings with the following properties:
    
    ID
    
    Value
    
    Caption
    
    AppTitle
    
    100
    
    The language-neutral name of your game to be displayed in the Application List or the Games Hub.
    
    AppTileString
    
    200
    
    The language-neutral name of your game to be displayed when the tile is pinned to the Windows Phone Start screen.
    
5.  Save and build the AppResLib project.

# Create the First Localized Language Resource Strings

In this procedure, you create a resource string table that contains game title strings specific to a language. The procedure also specifies a post-build event to rename the built DLL correctly as a localized resource and move it to the needed location.

![](note.gif)Important

The DLLs containing the localized resource strings must be named in a manner conforming to:

AppResLib.dll.\[culture code\].mui

where \[culture code\] is the four-digit culture code of the localized strings.

### To create the first specific language resource strings

1.  Follow the steps in the prior procedure to create a new language resource DLL project named AppRes0409 and set its properties.
2.  In the **Property Pages** dialog, expand **Configuration Options**, expand **Resources**, and click **General**.
3.  Set the **Culture** value to **English (United States) (0x409) (/I 0x0409)**.
4.  Expand **Configuration Options**, expand **Build Events**, and click **Post-Build Event**.
5.  Set the **Command Line** value to the following:
    
    copy "$(OutputPath)$(ProjectName).dll" "$(OutputPath)\\AppResLib.dll.0409.mui"
    
6.  In **Solution Explorer**, select the new DLL project.
7.  From the **Project** menu, click **Add Resource**.
    
    The **Add Resource** dialog appears.
    
8.  In the **Resource type** list, select **String Table** and click **New**.
    
    The resource string table opens.
    
9.  Create two resource strings with the following properties:
    
    ID
    
    Value
    
    Caption
    
    AppTitle
    
    100
    
    The English (United States) name of your game to be displayed in the Application List or the Games Hub.
    
    AppTileString
    
    200
    
    The English (United States) name of your game to be displayed when the tile is pinned to the Windows Phone Start screen.
    
10.  Save and build the AppRes0409 project.

# Create the Remaining Language Resource Strings

This procedure creates the localized resource DLLs for the remaining required languages.

### To create the remaining language resource strings

Repeat the previous procedure, creating the following projects and DLLs to support the remaining languages:

Culture

Project Name

DLL File Name

English (United Kingdom) (0x809) (/I 0x0809)

AppRes0809

AppResLib.dll.0809.mui

French (France) (0x40c) (/I 0x040c)

AppRes040c

AppResLib.dll.040c.mui

German (Germany) (0x407) (/I 0x0407)

AppRes0407

AppResLib.dll.0407.mui

Italian (Italy) (0x410) (/I 0x0410)

AppRes0410

AppResLib.dll.0410.mui

Spanish (Spain) (0xc0a) (/I 0x0c0a)

AppRes0c0a

AppResLib.dll.0c0a.mui

# Make the Windows Phone Game Project Dependent on Resource DLL Projects

1.  In **Solution Explorer**, right-click the solution that contains your Windows Phone game project and your resource DLL projects and choose **Project Dependencies...**
2.  In the **Projects** drop-down list, select your Windows Phone game project.
3.  Check all of the check-boxes next to your resource DLL projects and click **OK**.

# Update the Windows Phone Game to Load Localized Title Strings

### To update the application title

1.  In your Windows Phone game project, in the **Properties** folder, open the file AssemblyInfo.cs
2.  Delete the AssembyTitle entry.
3.  Add an AssemblyTitleAttribute entry:
    
    \[assembly: AssemblyTitleAttribute("@AppResLib.dll,-100")\]
    

### To update the tile title

1.  In **Solution Explorer**, select the Windows Phone game project.
2.  From the **Project** menu, click **Properties**.
    
    The Project Designer appears.
    
3.  Select the [XNA Game Studio Properties Page](UsingXNA_Dlg_GameStudioProperties.md) tab.
4.  Change the **Tile title** value to the following:
    
    @AppResLib.dll,-200
    

# See Also

[Getting Started with XNA Game Studio Development](Getting_Started.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team