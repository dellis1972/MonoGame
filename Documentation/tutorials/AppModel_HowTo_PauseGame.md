

# Pausing a Game

Demonstrates how to add pause functionality to a game.

# Pausing a Game

Typically, there are two circumstances when you want to pause your game: to respond to a user request such as a launch menu or the user entering a specific keystroke, or when the [Guide](xref:Microsoft.Xna.Framework.GamerServices.Guide) appears on the screen and obscures the playing field. In addition, there are different scenarios for ending a pause. A user requested pause should end when the user takes an action, however, a [Guide](xref:Microsoft.Xna.Framework.GamerServices.Guide) pause should end when the [Guide](xref:Microsoft.Xna.Framework.GamerServices.Guide) is dismissed.

Query the [Guide.IsVisible](xref:Microsoft.Xna.Framework.GamerServices.Guide.IsVisible) property to check for the presence of the [Guide](xref:Microsoft.Xna.Framework.GamerServices.Guide).

Pausing your game often means more than just halting your simulation. It could also require pausing or muting sounds, halting controller vibrations, sending a network message, and so on. To handle those tasks, you need to declare a **BeginPause** and **EndPause** method. **EndPause** resumes anything that was halted by **BeginPause**.

### Pausing a game

1.  Add a variable to track the pause state.
    
2.  Add a variable to track the state of the pause key.
    
3.  Add a variable to track if the pause is due to user action or the [Guide](xref:Microsoft.Xna.Framework.GamerServices.Guide).
    
    ```
    private bool paused = false;
    private bool pauseKeyDown = false;
    private bool pausedForGuide = false;
    ```
                        
    
4.  Add a **BeginPause** method to initiate a pause, setting the variables appropriately.
    
    ```
    private void BeginPause(bool UserInitiated)
    {
        paused = true;
        pausedForGuide = !UserInitiated;
        //TODO: Pause audio playback
        //TODO: Pause controller vibration
    }
    ```
                        
    
5.  Add an **EndPause** method to resume from a paused state, resetting variables appropriately.
    
    ```
    private void EndPause()
    {
        //TODO: Resume audio
        //TODO: Resume controller vibration
        pausedForGuide = false;
        paused = false;
    }
    ```
                        
    
6.  Add a function to poll the state of the pause key with [Keyboard.GetState](xref:MXFI.Keyboard.GetState) and [KeyboardState.IsKeyDown](xref:Microsoft.Xna.Framework.Input.KeyboardState.IsKeyDown).
    
    If the key has changed from down to up, toggle the pause state using **BeginPause** or **EndPause**.
    
    ```
    private void checkPauseKey(KeyboardState keyboardState,
        GamePadState gamePadState)
    {
        bool pauseKeyDownThisFrame = (keyboardState.IsKeyDown(Keys.P) ||
            (gamePadState.Buttons.Y == ButtonState.Pressed));
        // If key was not down before, but is down now, we toggle the
        // pause setting
        if (!pauseKeyDown && pauseKeyDownThisFrame)
        {
            if (!paused)
                BeginPause(true);
            else
                EndPause();
        }
        pauseKeyDown = pauseKeyDownThisFrame;
    }
    ```
                        
    
7.  During [Update](xref:Microsoft.Xna.Framework.Game.Update), check to see if the user paused, or if the [Guide](xref:Microsoft.Xna.Framework.GamerServices.Guide) is active.
    
    Add a conditional around any update code so it is called only if the game is not paused. Be sure to call **base.Update** even if the simulation is paused.
    
    ```
    // Check to see if the user has paused or unpaused
    checkPauseKey(keyboardState, gamePadState);
    
    checkPauseGuide();
    
    // If the user hasn't paused, Update normally
    if (!paused)
    {
        Simulate(gameTime);
    }
    ```
                        
    
8.  Add a function to poll the state of the [Guide](xref:Microsoft.Xna.Framework.GamerServices.Guide).
    
    If the [Guide](xref:Microsoft.Xna.Framework.GamerServices.Guide) is newly visible, call **BeginPause**.
    
    If the [Guide](xref:Microsoft.Xna.Framework.GamerServices.Guide) is not visible, but the game was paused for the guide, call **EndPause**.
    
    ```
    private void checkPauseGuide()
    {
        // Pause if the Guide is up
        if (!paused && Guide.IsVisible)
            BeginPause(false);
        // If we paused for the guide, unpause if the guide
        // went away
        else if (paused && pausedForGuide && !Guide.IsVisible)
            EndPause();
    }
    ```
                        
    

# Remarks

![](bp.gif)Best Practice

You could implement a menu of options to display when a game is paused, such as those listed below.

*   [Guide.IsVisible](xref:Microsoft.Xna.Framework.GamerServices.Guide.IsVisible) is **true**, as the guide blocks user inputs from reaching the game.
*   [Game.IsActive](xref:Microsoft.Xna.Framework.Game.IsActive) is **false**, indicating that trial mode may have ended for the game.
*   [GamePadState.IsConnected](xref:Microsoft.Xna.Framework.Input.GamePadState.IsConnected) is **false**, indicating that the game controller is disconnected.

For a multiplayer game, a pause usually disables local input without pausing the game for remote players.

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
