

# Creating a Full-Screen Game

Demonstrates how to start a game in full-screen mode.

# Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download FullScreen.zip](http://go.microsoft.com/fwlink/?LinkId=258700).

# Creating a Full-Screen Game

### To create a full-screen game

1.  Derive a class from [Game](T_Microsoft_Xna_Framework_Game.md).
    
2.  After creating the [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md), set its [PreferredBackBufferWidth](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferWidth.md) and [PreferredBackBufferHeight](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferHeight.md) to the desired screen height and width.
    
3.  Set [IsFullScreen](P_Microsoft_Xna_Framework_GraphicsDeviceManager_IsFullScreen.md) to **true**.
    
    ```
    this.graphics.PreferredBackBufferWidth = 480;
    this.graphics.PreferredBackBufferHeight = 800;
    
    this.graphics.IsFullScreen = true;
    ```
    

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
