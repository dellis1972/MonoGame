

# Exiting a Game After a Time Out

Demonstrates how to exit a game after a period of time (such as inactivity) has passed.

# Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download GameLoop_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258702).

# Adding Time-Out Functionality to a Game

### To make a game time out

1.  Create a class that derives from [Game](T_Microsoft_Xna_Framework_Game.md).
    
2.  Determine the desired time-out limit in milliseconds.
    
    ```
    // Time out limit in ms.
    static private int TimeOutLimit = 4000; // 4 seconds
    ```
    
3.  Add a variable for tracking the elapsed time since the most recent user activity.
    
    ```
    // Amount of time that has passed.
    private double timeoutCount = 0;
    ```
    
4.  When user input is checked, set a flag indicating whether any user activity has taken place.
    
    ```
    GamePadState blankGamePadState = new GamePadState(
        new GamePadThumbSticks(), new GamePadTriggers(), new GamePadButtons(),
        new GamePadDPad());
    ```
    
5.  In [Update](M_Microsoft_Xna_Framework_Game_Update.md), if there has not been any user activity, increment the tracking variable by the elapsed time since the last call to [Update](M_Microsoft_Xna_Framework_Game_Update.md).
    
6.  If there has been some user activity, set the tracking variable to zero.
    
    ```
    // Check to see if there has been any activity
    if (checkActivity(keyboardState, gamePadState) == false)
    {
        timeoutCount += gameTime.ElapsedGameTime.Milliseconds;
    }
    else
        timeoutCount = 0;
    ```
    
7.  Check whether the value of the tracking variable is greater than the time-out limit.
    
8.  If the variable is greater than the limit, perform some time-out logic such as playing an idle animation or, in this case, exit the game.
    
    ```
    // Timeout if idle long enough
    if (timeoutCount > TimeOutLimit)
    {
        Exit();
        base.Update(gameTime);
        return;
    }
    ```
    

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
