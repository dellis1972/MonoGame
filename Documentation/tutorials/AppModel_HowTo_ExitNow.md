

# Exiting a Game Immediately

Demonstrates how to exit a game in response to user input.

# Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download GameLoop_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258702).

# Exiting a Game Without Finishing the Current Update

### To exit the game loop without running any remaining code in the update handler

1.  Derive a class from [Game](xref:Microsoft.Xna.Framework.Game).
    
2.  Create a method that checks [KeyboardState.IsKeyDown](xref:Microsoft.Xna.Framework.Input.KeyboardState.IsKeyDown) for the state of the ESC key.
    
3.  If the ESC key has been pressed, call [Game.Exit](xref:Microsoft.Xna.Framework.Game.Exit) and return **true**.
    
    ```
    bool checkExitKey(KeyboardState keyboardState, GamePadState gamePadState)
    {
        // Check to see whether ESC was pressed on the keyboard 
        // or BACK was pressed on the controller.
        if (keyboardState.IsKeyDown(Keys.Escape) ||
            gamePadState.Buttons.Back == ButtonState.Pressed)
        {
            Exit();
            return true;
        }
        return false;
    }
    ```
    
4.  Call the method in [Game.Update](xref:Microsoft.Xna.Framework.Game.Update), and return from [Update](xref:Microsoft.Xna.Framework.Game.Update) if the method returned **true**.
    
    ```
    GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
    KeyboardState keyboardState = Keyboard.GetState();
    
    // Check to see if the user has exited
    if (checkExitKey(keyboardState, gamePadState))
    {
        base.Update(gameTime);
        return;
    }
    ```
    
5.  Create a method to handle the [Game.Exiting](E_Microsoft_Xna_Framework_Game_Exiting.md) event.
    
    The [Exiting](E_Microsoft_Xna_Framework_Game_Exiting.md) event is issued at the end of the tick in which [Game.Exit](xref:Microsoft.Xna.Framework.Game.Exit) is called.
    
    ```
    void Game1_Exiting(object sender, EventArgs e)
    {
        // Add any code that must execute before the game ends.
    }
    ```
    

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
