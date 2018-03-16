

# Your First Game - XNA Game Studio in 2D

Follow these steps to create a simple sprite-based game for Windows Phone using Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0.

![](note.gif)Note

A sprite is a simple 2D graphic (such as a bitmap) that is displayed on the screen using a call to [SpriteBatch.Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md).

*   [Step 1: Install Your Software](#ID4EVC)
*   [Step 2: Create a New Project](#ID4EUD)
*   [Step 3: View the Code](#ID4ECG)
*   [Step 4: Add a Sprite](#ID4ETH)
*   [Step 5: Make the Sprite Move and Bounce](#ID4EAFAC)
*   [Step 6: Explore!](#ID4ECGAC)

# The Complete Sample

The code in this tutorial illustrates the technique described in the text. A complete code sample for this tutorial is available for you to download, including full source code and any additional supporting files required by the sample.

[Download MyFirstGame\_Tutorial\_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258715).

# Step 1: Install Your Software

Before you begin, make sure that you have installed the Microsoft Windows Phone SDK 8.0, which includes Microsoft Visual Studio 2012 Express and Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0.

# Step 2: Create a New Project

1.  From the **Start** menu, click **All Programs**, click the **XNA Game Studio 4.0** folder, and then click your supported version of Microsoft Visual Studio tools.
    
2.  When the Start Page appears, click the **File** menu, and then click **New Project**.
    
    A dialog box appears with a tree list on the left pane, marked Project Types.
    
3.  Select the **XNA Game Studio 4.0** node within the **Visual C#** node.
    
    A set of available projects appears in the right pane.
    
4.  In the right pane of the dialog box that appears, click **Windows Phone Game (4.0)** , and then type a title for your project (such as "MyFirstGame") in the **Name** box.
    
5.  Type a path where you'd like to save your project in the **Location** box, and then click **OK**.
    
    After creating a new project, you'll be presented with the code view of your game.
    

# Step 3: View the Code

Some of the hard work has already been done for you. If you build and run your game now, the [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) will set up your screen size and render a blank screen. Your game will run and update all by itself. It's up to you to insert your own code to make the game more interesting.

Much of the code to start and run your game has already been written for you. You can insert your own code now.

*   The [Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md) method is where you can initialize any assets that do not require a [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md) to be initialized.
*   The [LoadContent](M_MXF_Game_LoadContent.md) method is where you load any necessary game assets such as models and textures.
*   The [UnloadContent](M_MXF_Game_UnloadContent.md) method is where any game assets can be released. Generally, no extra code is required here, as assets will be released automatically when they are no longer needed.
*   The overridden [Update](M_Microsoft_Xna_Framework_Game_Update.md) method is the best place to update your game logic: move objects around, take player input, decide the outcome of collisions between objects, and so on.
*   The overridden [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method is the best place to render all of your objects and backgrounds on the screen.

# Step 4: Add a Sprite

The next step is to add a graphic that can be drawn on the screen. Use a small graphics file, such as a small .bmp or .jpg file. Be creative—you can even make your own. You can even skip ahead a bit and make a sprite that "hides" parts that should not be seen (such as edges or corners) so that it looks even better.

Once you have a graphic picked out on your computer, follow these steps.

1.  Make sure you can see the Solution Explorer for your project on the right side of the window. If you cannot see it, click the **View** menu, and then click **Solution Explorer**.
    
    When it appears, you will see files associated with your project in a tree structure. Inside the tree, you will see a node named **Content**.
    
2.  Right-click the **Content** node, click **Add**, click **Existing Item**, and then browse to your graphic.
    
    If you can't see any files, make sure you change the **Files of type** selection box to read **Texture Files**.
    
3.  Click the graphic file, and then click **Add**.
    
    An entry for the graphic file will appear in Solution Explorer.
    
4.  Click the entry for the graphic in the Solution Explorer. If you do not see the entry, ensure the **Content** node is expanded by clicking the small plus sign (+) to the left of the node, and then click on the entry that appears underneath the **Content** node.
    
    When you add a graphic file, it is added automatically to the XNA Framework Content Pipeline. This allows you to quickly and easily load the graphic into your game.
    
    In the **Properties** window below Solution Explorer, look for the "Asset Name" property. Note the name; you'll use it in your code to load the graphic so it can be displayed in your game.
    
5.  If the **Properties** window is not visible, press F4, or click the **View** menu, and then click **Properties Window**.
    
    Now, you must write code that loads and displays the sprite on the screen.
    
6.  Back in the code view of your game, find the [LoadContent](M_MXF_Game_LoadContent.md) method, and add the following lines in and above the method so it looks similar to this:
    
                      `// This is a texture we can render.
    Texture2D myTexture;
    
    // Set the coordinates to draw the sprite at.
    Vector2 spritePosition = Vector2.Zero;
    
    // Store some information about the sprite's motion.
    Vector2 spriteSpeed = new Vector2(50.0f, 50.0f);
    
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
        myTexture = Content.Load<Texture2D>("mytexture");
    }`
                    
    
    The [Content](P_MXF_Game_Content.md) property of the parent [Game](T_Microsoft_Xna_Framework_Game.md) class offers the [ContentManager](T_Microsoft_Xna_Framework_Content_ContentManager.md) class through which your game assets can be loaded.
    
    Make sure the call to [Content.Load](M_Microsoft_Xna_Framework_Content_ContentManager_Load``1.md) is using the "Asset Name" you saw in the Properties window in the previous step. This code will load and prepare your graphic to be drawn, and will reload your graphic if the graphics device is reset (such as in the case of the game window being reoriented).
    
7.  Now, add code to the [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method so it looks like this:
    
                      `protected override void Draw(GameTime gameTime)
    {
        graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
    
        // Draw the sprite.
        spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
        spriteBatch.Draw(myTexture, spritePosition, Color.White);
        spriteBatch.End();
    
        base.Draw(gameTime);
    }`
                    
    
    This code draws the sprite on the screen each frame.
    
    Notice the parameter passed by the [Begin](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_1850401A_Begin.md) method, [BlendState.AlphaBlend](T_Microsoft_Xna_Framework_Graphics_BlendState.md). This parameter tells the [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method to use the alpha channel of the source color to create a transparency effect so that the destination color appears through the source color.
    
8.  Build and run your game.
    
    The sprite appears.
    

Now, it's time to give it some motion.

# Step 5: Make the Sprite Move and Bounce

*   Change the lines of code in the [Update](M_Microsoft_Xna_Framework_Game_Update.md) method, and add the entirely new method `UpdateSprite`, to the following (the conditional compilation directives for WINDOWS, and the enclosed code, is not required). This code provides logic that will move the sprite around each frame and cause the sprite to change direction if it hits the edges of the game window.
    
                      `        protected override void Update(GameTime gameTime)
            {
                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();
    
    #if WINDOWS
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    this.Exit();
    #endif
    
                // Move the sprite around.
                UpdateSprite(gameTime);
    
                base.Update(gameTime);
            }
    
            void UpdateSprite(GameTime gameTime)
            {
                // Move the sprite by speed, scaled by elapsed time.
                spritePosition +=
                    spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    
                int MaxX =
                    graphics.GraphicsDevice.Viewport.Width - myTexture.Width;
                int MinX = 0;
                int MaxY =
                    graphics.GraphicsDevice.Viewport.Height - myTexture.Height;
                int MinY = 0;
    
                // Check for bounce.
                if (spritePosition.X > MaxX)
                {
                    spriteSpeed.X *= -1;
                    spritePosition.X = MaxX;
                }
    
                else if (spritePosition.X < MinX)
                {
                    spriteSpeed.X *= -1;
                    spritePosition.X = MinX;
                }
    
                if (spritePosition.Y > MaxY)
                {
                    spriteSpeed.Y *= -1;
                    spritePosition.Y = MaxY;
                }
    
                else if (spritePosition.Y < MinY)
                {
                    spriteSpeed.Y *= -1;
                    spritePosition.Y = MinY;
                }
            }`
                    
    
*   Build and run your game.
    

The sprite moves across the screen and changes direction when it encounters the edges of the game window.

# Step 6: Explore!

From here, you can do just about anything.

Here are some more ideas to extend this sample:

*   Add a second sprite, and use [BoundingBox](T_Microsoft_Xna_Framework_BoundingBox.md) objects to detect collisions.
*   Use one or more of the following classes to make the sprite respond to movements of an input device:
    
    *   [TouchPanel](T_MXFIT_TouchPanel.md) (Windows Phone)
    *   [Keyboard](T_Microsoft_Xna_Framework_Input_Keyboard.md) (Windows, Xbox 360, Windows Phone)
    
    For more information, see [Overview of User Input and Input Devices](Input_XNA.md).
    
*   Get more ideas and resources at [App Hub](http://go.microsoft.com/fwlink/?LinkID=215642).

# See Also

[Software Install Requirements](Required_SW.md)  
[Getting Started with XNA Game Studio Development](Getting_Started.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0