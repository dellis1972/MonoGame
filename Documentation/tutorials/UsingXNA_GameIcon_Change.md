

# Changing your Game Thumbnail on Windows Phone

The thumbnails are the graphic elements that appear alongside the name of your game in the lists where each can be selected to run on the Windows Phone device.

A Windows Phone project requires the specification of two thumbnails, defined in the [XNA Game Studio Properties Page](UsingXNA_Dlg_GameStudioProperties.md) as the Game Thumbnail and the Tile.

The following table summarizes the usage and requirements of each thumbnail used in a Windows Phone project:

Property

Default filename

Dimensions

Use

Game Thumbnail

GameThumbnail.png

62x62 pixels _or_

173x173 pixels

The (smaller) icon displayed when listed in the **Application List** on the Windows Phone device _or_

The (larger) icon displayed when listed in the **Games Hub** on the Windows Phone device.

Tile

Background.png

173x173

The icon used when the game is "pinned" to the Windows Phone **Start** menu.

# Game Thumbnail Dimensions

The game's genre setting determines the list in which your deployed game appears, either the **Application List** or the **Games Hub**. For more information on this, see the topic [Configuring the Genre of Your Windows Phone Game](UsingXNA_GameList_Change.md).

As the table above shows, the optimal dimensions of the thumbnail differ, depending on the list in which your game is configured to appear.

In either list, if the dimensions of the thumbnail do not match the optimal dimensions, it will be scaled so that its largest dimension matches the optimal dimension, with an appropriate aspect ratio. However, this scaling may produce an unattractive result, so it is recommended that your thumbnail's size match the optimal dimensions.

The Game Thumbnail file must be in .png format.

# Tile Dimensions

The Tile thumbnail can also appear in the **Start** menu once the application or game has been "pinned" there. This occurs when the user clicks and holds on the thumbnail in Games Hub or the Application List and selects “pin to start.”

The dimensions of the Tile thumbnail are always 173 x 173 pixels, and the file must be in .png format.

# Changing the Thumbnail Files

The template for a new Windows Phone project automatically creates the files GameThumbnail.png (for the Game Thumbnail) and Background.png (for the Tile thumbnail).

![](note.gif)Note

The project template for Windows Phone defaults to the genre type App.Normal, which will place the deployed project in the **Application List**. The dimensions of the created GameThumbnail.png file will be optimal for that usage (62x62 pixels).

To change the thumbnails, you can overwrite the default files created by the template with files having the same name. Or you can change the [properties](UsingXNA_Dlg_GameStudioProperties.md) to use different thumbnail files.

### To change the thumbnail files

1.  Delete the default GameThumbnail.png and Background.png files from the Windows Phone project.
2.  Add the new thumbnail files you have created to the Windows Phone project.
3.  Double-click the **Properties** item in Solution Explorer and select the **XNA Game Studio Properties** page in the resulting Properties Designer.
4.  From the **Game Thumbnail** drop-down list, select the .png file you wish to use for the thumbnail.
5.  From the **Tile Image** drop-down list, select the .png file you wish to display when the the game appears in the **Start** menu.

![](note.gif)Tip

Since the thumbnails for **Games Hub** and the **Start** menu require the same dimensions, you can assign the same thumbnail file for both when the genre type is set to App.Games. For example, both the **Game Thumbnail** and the **Tile Image** properties can be set to the default Background.png file.

# See Also

[Getting Started with XNA Game Studio Development](Getting_Started.md)  
[Configuring the Genre of Your Windows Phone Game](UsingXNA_GameList_Change.md)  
[XNA Game Studio Properties Page](UsingXNA_Dlg_GameStudioProperties.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team