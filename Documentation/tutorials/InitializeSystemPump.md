

# Initializing and Updating Gamer Services

# Automatically Updating the Gamer Services Dispatcher

The XNA Framework provides a [GamerServicesComponent](T_Microsoft_Xna_Framework_Graphics_GamerServicesComponent.md). This is a game component that automatically takes care of initializing and updating the gamer services dispatcher. To make use of this component, XNA Framework games need to add only one line of code to the [Game](T_Microsoft_Xna_Framework_Game.md) constructor.

### To automatically update the gamer services dispatcher

*   Add this line of code to the [Game](T_Microsoft_Xna_Framework_Game.md) constructor:
    
                          `Components.Add(new GamerServicesComponent(this));`
                        
    

# Manually Updating the Gamer Services System

In some cases, a program may not use the XNA Framework application model or component infrastructure. For this application, it is possible to call the [GamerServicesDispatcher](T_Microsoft_Xna_Framework_GamerServices_GamerServicesDispatcher.md) directly.

### To manually initialize and update the gamer services system

1.  In the startup code for the application, call [GamerServicesDispatcher.Initialize](M_Microsoft_Xna_Framework_Graphicsx_GamerServicesDispatcher_3B5F5930_Initialize.md) once to initialize the gamer services subsystem.
    
                          `protected override void Initialize()
    {
        GamerServicesDispatcher.WindowHandle = Window.Handle;
       
        GamerServicesDispatcher.Initialize(Services);
    
        base.Initialize();
    }`
                        
    
2.  Call [GamerServicesDispatcher.Update](M_Microsoft_Xna_Framework_GamerServices_GamerServicesDispatcher_Update.md) once every frame to update the gamer services system.
    
                          `protected override void Update(GameTime gameTime)
    {
        GamerServicesDispatcher.Update();
        base.Update(gameTime);
    }`
                        
    

# See Also

#### Concepts

[Introduction to Gamer Services](GamerServices_Overview.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0