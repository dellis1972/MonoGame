

# XNA Game Studio Properties Page

Enables the user to change profile and other properties for the current XNA Game Studio project.

You open the **XNA Game Studio Properties** page from Project Designer. Open Project Designer by double-clicking the **Properties** item in Solution Explorer or by clicking **\[project name\] Properties** on the **Project** menu. Then click the **XNA Game Studio** tab.

# Selecting the Game Profile

The **Game Profile** selection specifies the hardware feature set for which the game will be built.

Games for Windows Phone support only the Reach profile.

For further information on game profiles, see the topic [What Is a Profile?](WhatIs_Profile.md).

# Setting a Game Thumbnail

The **Game Thumbnail** box enables you set the thumbnail image that appears beside your game on the Windows Phone device.

There are several things to keep in mind when you specify a thumbnail for your game.

## File Requirements

*   The image must be saved as a .png file, and should be 173 pixels by 173 pixels for a Windows Phone game. For more information, see [Changing your Game Thumbnail on Windows Phone](UsingXNA_GameIcon_Change.md).
*   If the largest dimension of the image you provide is not exactly 173 pixels, the image will be scaled so that its aspect ratio remains the same and its largest dimension becomes exactly 173 pixels. Such scaling can produce results you might not expect. If you provide an image whose largest dimension is already exactly 173 pixels, you can avoid scaling.
*   If you do specify an image that needs to be scaled, it must be smaller than 2,048 pixels by 2,048 pixels in size, and the .png file that contains it must be smaller than 16 KB in size.

## File Location

*   The thumbnail file must be specified in the game project. Files specified in the game content subproject will not be recognized as thumbnails.
*   The **Game thumbnail** drop-down menu lists all files with a .png extension that have been added to your project, regardless of each file's size or resolution.
*   You can also browse for any other .png files accessible from your machine by clicking the **...** button beside the **Game thumbnail** drop-down menu. When you select a file from the browse dialog box, that file is added to your project and is set as the game thumbnail.

# Setting Tile Properties

The tile properties define what is displayed when the Windows Phone application appears in the **Start** menu of the Windows Phone device. These are different from what it is displayed in the Windows Phone **Game** menu.

## Tile Title

Specifies the text that is displayed for the game when it appears in the Windows Phone **Start** menu.

The default string is the project name.

## Tile Image

Specifies the image that is displayed for the game when it appears in the Windows Phone **Start** menu.

The image must be .png file, containing a graphic that is 173 pixels x 173 pixels.

The default image is "Background.png".

# Setting the Game Startup Type

The **Game Startup Type** box specifies the initial type (object) for the game program that will be called when the game starts.

The default startup type is the **Game1** object of the game's primary namespace ([Microsoft.XNA.Framework.Game](http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.game.aspx)), as defined in the standard templates for an XNA Game Studio game.

If your game requires the use of a different startup type other than the default, you may select it in this property. For the startup type to appear in the drop-down list, it must be a public type derived from [Microsoft.XNA.Framework.Game](http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.game.aspx), and contain no private default constructor.

If no valid startup type exists, or if you select "Not Set" in this property when multiple valid startup types exist, you will get an error when you build the game.

# See Also

[Advanced Windows Phone Development](UsingXNA_GameStudio_Overviews.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team