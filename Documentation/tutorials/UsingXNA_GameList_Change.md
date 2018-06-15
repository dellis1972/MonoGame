

# Configuring the Genre of Your Windows Phone Game

Depending on its genre setting, once your game package is installed on the Windows Phone device, it will be listed in one of two possible menus:

*   **Application List** — The menu where all Windows Phone/Silverlight applications are listed and can be launched.
    
*   **Games Hub** — The menu where all applications that have been marked as games are listed and can be launched.
    

The genre setting does not have to match the technology used to create the application or game. While the expected experience is to create games with XNA Game Studio and applications with Silverlight, you may designate projects created with XNA Game Studio as applications, or designate projects created with Silverlight as games if you choose.

![](note.gif)Important

The dimension requirements of the thumbnail are different for display in each genre list. For more information on these requirements, see the topic [Changing your Game Thumbnail on Windows Phone](UsingXNA_GameIcon_Change.md).

This topic explains how to configure your game package to designate its genre as either an application or a game in each deployment scenario.

*   [For Deployment to a Windows Phone Device](#ID4EWC)
*   [For Deployment to the Windows Phone Emulator](#ID4EAF)
*   [For Submission to Windows Phone Marketplace](#ID4E5F)

# For Deployment to a Windows Phone Device

The genre designation is controlled through the Genre attribute of the <App> tag in the file WMAppManifest.xml. This file is found in the **Properties** folder of the Windows Phone project.

### To configure your project as an application

1.  In <App> tag within the file WMAppManifest.xml, set the Genre attribute to “Apps.Normal”.
2.  Applications will deploy to the **Application List** on the Windows Phone device.

### To configure your project as a game

1.  In <App> tag within the file WMAppManifest.xml, set the Genre attribute to “Apps.Games”.
2.  Applications will deploy to the **Games Hub** on the Windows Phone device.

# For Deployment to the Windows Phone Emulator

The Windows Phone Emulator does not support the **Games Hub**. For this reason, the default setting of an XNA Game Studio project for Windows Phone is as an application, not a game.

If the Genre attribute is set to "Apps.Games" when deployed to the Windows Phone Emulator, the project will successfully deploy and run, however it will not be displayed in the **Application List**.

# For Submission to Windows Phone Marketplace

When you submit your application or game to the Windows Phone Marketplace, you will be asked in a web form whether it is an application or a game. The ingestion process will override the setting within the submitted package, so no direct changes to the project files are needed.

# See Also

[Getting Started with XNA Game Studio Development](Getting_Started.md)  
[Changing your Game Thumbnail on Windows Phone](UsingXNA_GameIcon_Change.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team