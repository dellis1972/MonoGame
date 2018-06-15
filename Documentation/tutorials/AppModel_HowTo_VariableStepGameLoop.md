

# Updating with Variable or Fixed Timing

Demonstrates how to set up the runtime to call your Update method using variable or fixed timing.

There are two techniques for setting how often your **Update** method is called. Variable timing means to call **Update** as soon as other work finishes; this implies that it is up to a game developer to ensure that your render loop happens quickly enough so that **Update** will be called often enough to exceed your minimum frame rate. Fixed timing means that **Update** is called each time a fixed interval of time has passed. Fixed timing guarantees that **Update** will be called, however, so you may drop frames if the previous work needs to be interrupted to call **Update**.

# Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download AppModelDemo_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258684).

# Updating with Variable Timing

### To use a variable time step

1.  Create a class that derives from [Game](T_Microsoft_Xna_Framework_Game.md).
    
2.  Set [IsFixedTimeStep](P_Microsoft_Xna_Framework_Game_IsFixedTimeStep.md) to **false**.
    
    This causes the [Update](M_Microsoft_Xna_Framework_Game_Update.md) method to be called as often as possible, instead of being called on a fixed interval.
    
    ```
    this.IsFixedTimeStep = false;
    ```
    

# Updating with Fixed Timing

### To use a fixed time step

1.  Create a class that derives from [Game](T_Microsoft_Xna_Framework_Game.md).
    
2.  Set [IsFixedTimeStep](P_Microsoft_Xna_Framework_Game_IsFixedTimeStep.md) to **true**.
    
    ```
    this.IsFixedTimeStep = true;
    ```
    
    This causes the [Update](M_Microsoft_Xna_Framework_Game_Update.md) method to be called each time the fixed time interval has passed.
    
3.  Set **TargetElapsedTime** to a fixed interval of time.
    
    This example sets the time between calls to 16 milliseconds.
    
    ```
    this.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 16);    // Update() is called every 16 milliseconds
    ```
    

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
