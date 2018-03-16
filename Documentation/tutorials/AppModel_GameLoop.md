

# Initializing a Game

The MonoGame Framework [Game](T_Microsoft_Xna_Framework_Game.md) class implements a game loop, which provides not only the window which displays your game, but also provides overloadable methods that your game implements to facilitate communication between your game and the operating system. This topic provides an overview of the basic functionality provided by the game loop.

*   [Making a New Game](#Making-a-New-Game)
*   [Game Loop Timing](#Game-Loop-Timing)
*   [Game Components](#Fixed-Step-Game-Loops)
*   [Game Services](#Game Services)
*   [Game Components Consuming Game Services](#Game-Components-Consuming-Game-Services)

# Making a New Game

The first step in creating a new game is to make a class that derives from [Game](T_Microsoft_Xna_Framework_Game.md). The new class needs to override [Update](M_Microsoft_Xna_Framework_Game_Update.md), [Draw](M_Microsoft_Xna_Framework_Game_Draw.md), and [Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md). The [Update](M_Microsoft_Xna_Framework_Game_Update.md) method is responsible for handling game logic, and the [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method is responsible for drawing each frame. The [Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md) method is responsible for game setup before the first frame of the game.

# Game Loop Timing

A [Game](T_Microsoft_Xna_Framework_Game.md) is either fixed step or variable step, defaulting to fixed step. The type of step determines how often [Update](M_Microsoft_Xna_Framework_Game_Update.md) will be called and affects how you need to represent time-based procedures such as movement and animation.

## Fixed-Step Game Loops

A fixed-step [Game](T_Microsoft_Xna_Framework_Game.md) tries to call its [Update](M_Microsoft_Xna_Framework_Game_Update.md) method on the fixed interval specified in [TargetElapsedTime](P_Microsoft_Xna_Framework_Game_TargetElapsedTime.md). Setting [Game.IsFixedTimeStep](P_Microsoft_Xna_Framework_Game_IsFixedTimeStep.md) to **true** causes a [Game](T_Microsoft_Xna_Framework_Game.md) to use a fixed-step game loop. A new XNA project uses a fixed-step game loop with a default [TargetElapsedTime](P_Microsoft_Xna_Framework_Game_TargetElapsedTime.md) of 1/60th of a second.

In a fixed-step game loop, [Game](T_Microsoft_Xna_Framework_Game.md) calls [Update](M_Microsoft_Xna_Framework_Game_Update.md) once the [TargetElapsedTime](P_Microsoft_Xna_Framework_Game_TargetElapsedTime.md) has elapsed. After [Update](M_Microsoft_Xna_Framework_Game_Update.md) is called, if it is not time to call [Update](M_Microsoft_Xna_Framework_Game_Update.md) again, [Game](T_Microsoft_Xna_Framework_Game.md) calls [Draw](M_Microsoft_Xna_Framework_Game_Draw.md). After [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) is called, if it is not time to call [Update](M_Microsoft_Xna_Framework_Game_Update.md) again, [Game](T_Microsoft_Xna_Framework_Game.md) idles until it is time to call [Update](M_Microsoft_Xna_Framework_Game_Update.md).

If [Update](M_Microsoft_Xna_Framework_Game_Update.md) takes too long to process, [Game](T_Microsoft_Xna_Framework_Game.md) sets [IsRunningSlowly](P_Microsoft_Xna_Framework_GameTime_IsRunningSlowly.md) to **true** and calls [Update](M_Microsoft_Xna_Framework_Game_Update.md) again, without calling [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) in between. When an update runs longer than the [TargetElapsedTime](P_Microsoft_Xna_Framework_Game_TargetElapsedTime.md), [Game](T_Microsoft_Xna_Framework_Game.md) responds by calling [Update](M_Microsoft_Xna_Framework_Game_Update.md) extra times and dropping the frames associated with those updates to catch up. This ensures that [Update](M_Microsoft_Xna_Framework_Game_Update.md) will have been called the expected number of times when the game loop catches up from a slowdown. You can check the value of [IsRunningSlowly](P_Microsoft_Xna_Framework_GameTime_IsRunningSlowly.md) in your [Update](M_Microsoft_Xna_Framework_Game_Update.md) if you want to detect dropped frames and shorten your [Update](M_Microsoft_Xna_Framework_Game_Update.md) processing to compensate. You can reset the elapsed times by calling [ResetElapsedTime](M_MXF_Game_ResetElapsedTime.md).

When your game pauses in the debugger, [Game](T_Microsoft_Xna_Framework_Game.md) will not make extra calls to [Update](M_Microsoft_Xna_Framework_Game_Update.md) when the game resumes.

## Variable-Step Game Loops

A variable-step game calls its [Update](M_Microsoft_Xna_Framework_Game_Update.md) and [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) methods in a continuous loop without regard to the [TargetElapsedTime](P_Microsoft_Xna_Framework_Game_TargetElapsedTime.md). Setting [Game.IsFixedTimeStep](P_Microsoft_Xna_Framework_Game_IsFixedTimeStep.md) to **false** causes a [Game](T_Microsoft_Xna_Framework_Game.md) to use a variable-step game loop.

## Animation and Timing

For operations that require precise timing, such as animation, the type of game loop your game uses (fixed-step or variable-step) is important.

Using a fixed step allows game logic to use the [TargetElapsedTime](P_Microsoft_Xna_Framework_Game_TargetElapsedTime.md) as its basic unit of time and assume that [Update](M_Microsoft_Xna_Framework_Game_Update.md) will be called at that interval. Using a variable step requires the game logic and animation code to be based on [ElapsedGameTime](P_Microsoft_Xna_Framework_GameTime_ElapsedGameTime.md) to ensure smooth gameplay. Because the [Update](M_Microsoft_Xna_Framework_Game_Update.md) method is called immediately after the previous frame is drawn, the time between calls to [Update](M_Microsoft_Xna_Framework_Game_Update.md) can vary. Without taking the time between calls into account, the game would seem to speed up and slow down. The time elapsed between calls to the [Update](M_Microsoft_Xna_Framework_Game_Update.md) method is available in the [Update](M_Microsoft_Xna_Framework_Game_Update.md) method's _gameTime_ parameter. You can reset the elapsed times by calling [ResetElapsedTime](M_MXF_Game_ResetElapsedTime.md).

When using a variable-step game loop, you should express rates—such as the distance a sprite moves—in game units per millisecond (ms). The amount a sprite moves in any given update can then be calculated as the rate of the sprite times the elapsed time. Using this approach to calculate the distance the sprite moved ensures that the sprite will move consistently if the speed of the game or computer varies.

# Game Components

Game components provide a modular way of adding functionality to a game. You create a game component by deriving the new component either from the [GameComponent](T_Microsoft_Xna_Framework_GameComponent.md) class, or, if the component loads and draws graphics content, from the [DrawableGameComponent](T_Microsoft_Xna_Framework_DrawableGameComponent.md) class. You then add game logic and rendering code to the game component by overriding [GameComponent.Update](M_Microsoft_Xna_Framework_GameComponent_Update.md), [DrawableGameComponent.Draw](M_Microsoft_Xna_Framework_DrawableGameComponent_Draw.md) and [GameComponent.Initialize](M_Microsoft_Xna_Framework_GameComponent_Initialize.md). A game component is registered with a game by passing the component to [Game.Components.Add](T_Microsoft_Xna_Framework_GameComponentCollection.md). A registered component will have its initialize, update, and draw methods called from the [Game.Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md), [Game.Update](M_Microsoft_Xna_Framework_Game_Update.md), and [Game.Draw](M_Microsoft_Xna_Framework_Game_Draw.md) methods, respectively.

# Game Services

Game services are a mechanism for maintaining loose coupling between objects that need to interact with each other. Services work through a mediator—in this case, [Game.Services](P_Microsoft_Xna_Framework_Game_Services.md). Service providers register with [Game.Services](P_Microsoft_Xna_Framework_Game_Services.md), and service consumers request services from [Game.Services](P_Microsoft_Xna_Framework_Game_Services.md). This arrangement allows an object that requires a service to request the service without knowing the name of the service provider.

Game services are defined by an interface. A class specifies the services it provides by implementing interfaces and registering the services with [Game.Services](P_Microsoft_Xna_Framework_Game_Services.md). A service is registered by calling [Game.Services.AddService](M_Microsoft_Xna_Framework_GameServiceContainer_AddService.md) specifying the type of service being implemented and a reference to the object providing the service. For example, to register an object that provides a service represented by the interface IMyService, you would use the following code.

Services.AddService( typeof( IMyService ), myobject );

Once a service is registered, the object providing the service can be retrieved by [Game.Services.GetService](M_Microsoft_Xna_Framework_GameServiceContainer_GetService.md) and specifying the desired service. For example, to retrieve [IGraphicsDeviceService](T_Microsoft_Xna_Framework_Graphics_IGraphicsDeviceService.md), you would use the following code.

IGraphicsDeviceService graphicsservice = (IGraphicsDeviceService)Services.GetService( typeof(IGraphicsDeviceService) );

# Game Components Consuming Game Services

The [GameComponent](T_Microsoft_Xna_Framework_GameComponent.md) class provides the [Game](P_Microsoft_Xna_Framework_GameComponent_Game.md) property so a [GameComponent](T_Microsoft_Xna_Framework_GameComponent.md) can determine what [Game](T_Microsoft_Xna_Framework_Game.md) it is attached to. With the [Game](P_Microsoft_Xna_Framework_GameComponent_Game.md) property, a [GameComponent](T_Microsoft_Xna_Framework_GameComponent.md) can call [Game.Services.GetService](M_Microsoft_Xna_Framework_GameServiceContainer_GetService.md) to find a provider of a particular service. For example, a [GameComponent](T_Microsoft_Xna_Framework_GameComponent.md) would find the [IGraphicsDeviceService](T_Microsoft_Xna_Framework_Graphics_IGraphicsDeviceService.md) provider by using the following code.

IGraphicsDeviceService graphicsservice = (IGraphicsDeviceService)Game.Services.GetService( typeof( IGraphicsDeviceService ) );

# In This Section

[Creating a Full-Screen Game](AppModel_HowTo_FullScreen.md)

Demonstrates how to start a game in full-screen mode.

[Resizing a Game](AppModel_HowTo_PlayerResize.md)

Demonstrates how to resize an active game window.

[Restricting Aspect Ratio on a Graphics Device](AppModel_HowTo_AspectRatio.md)

Demonstrates how to create a custom [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) that only selects graphics devices with widescreen aspect ratios in full-screen mode.

[Automatic Rotation and Scaling](AutomaticRotation.md)

Describes automatic rotation and scaling in the XNA Framework. Rotation and scaling are done in hardware at no performance cost to the game.

© 2012 Microsoft Corporation. All rights reserved. 
© 2018 @MonoGameTeam. All rights reserved. 
Version: 2.0.61024.0
