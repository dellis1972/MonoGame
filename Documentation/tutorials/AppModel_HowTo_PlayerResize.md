

# Resizing a Game

Demonstrates how to resize an active game window.

# Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download GameLoop_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258702).

# Adding Window Resizing Functionality

### To add player window resizing to a game

1.  Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).
    
2.  Set [Game.GameWindow.AllowUserResizing](xref:Microsoft.Xna.Framework.GameWindow.AllowUserResizing) to **true**.
    
3.  Add an event handler for the [ClientSizeChanged](E_Microsoft_Xna_Framework_GameWindow_ClientSizeChanged.md) event of [Game.Window](xref:Microsoft.Xna.Framework.Game.Window).
    
    ```
    this.Window.AllowUserResizing = true;
    this.Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
    ```
    
4.  Implement a method to handle the [ClientSizeChanged](E_Microsoft_Xna_Framework_GameWindow_ClientSizeChanged.md) event of [Game.Window](xref:Microsoft.Xna.Framework.Game.Window).
    
    ```
    void Window_ClientSizeChanged(object sender, EventArgs e)
    {
        // Make changes to handle the new window size.            
    }
    ```
    

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
