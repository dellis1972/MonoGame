

# Automatic Rotation and Scaling

This topic describes automatic rotation and scaling in the MonoGame Framework. Rotation and scaling are done in hardware at no performance cost to the game.

If your game supports more than one display orientation, as specified by [SupportedOrientations](P_Microsoft_Xna_Framework_GraphicsDeviceManager_SupportedOrientations.md) and described with [DisplayOrientation](T_MXF_DisplayOrientation.md), the MonoGame Framework automatically rotates and scales the game when the [OrientationChanged](E_MXF_GameWindow_OrientationChanged.md) event is raised.

The current back buffer resolution is scaled, and can be queried by using [PreferredBackBufferWidth](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferWidth.md) and [PreferredBackBufferHeight](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferHeight.md). These values will not be the same as the nonscaled screen resolution, which can be queried by using [DisplayMode](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DisplayMode.md) or [ClientBounds](P_Microsoft_Xna_Framework_GameWindow_ClientBounds.md).

If you leave [SupportedOrientations](P_Microsoft_Xna_Framework_GraphicsDeviceManager_SupportedOrientations.md) set to **DisplayOrientation.Default**, orientation is automatically determined from your [PreferredBackBufferWidth](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferWidth.md) and [PreferredBackBufferHeight](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferHeight.md). If the [PreferredBackBufferWidth](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferWidth.md) is greater than the [PreferredBackBufferHeight](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferHeight.md), the game will run in the landscape orientation and automatically switch between **LandscapeLeft** and **LandscapeRight** depending on the position which the user holds the phone. To run a game in the portrait orientation, set the [PreferredBackBufferWidth](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferWidth.md) to a value smaller than the [PreferredBackBufferHeight](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferHeight.md).

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.
