

# Visual Basic Example

Provides an example of programming XNA Game Studio with Visual Basic.

*   [Prerequisites](#ID4ET)
*   [Common XNA Methods in Visual Basic](#ID4EBB)
*   [Adding and Loading Content](#ID4ESD)
*   [Drawing and Moving a Sprite](#ID4EEF)
*   [Running Your Visual Basic Code](#ID4EJG)

# Prerequisites

Before programming XNA Game Studio in Visual Basic (VB), you must first create an XNA VB project. For more information, see [Creating a Visual Basic Windows Phone Game or Library Project](UsingXNA_VBProject.md).

# Common XNA Methods in Visual Basic

All XNA projects contain a number of placeholder methods (also known as subroutines) to use for your game code. This section includes examples of the methods in Game1.vb in a new XNA Visual Basic project.

## Initialize Method

The **Initialize** method is where you can initialize any assets that do not require a GraphicsDevice to be initialized. Here is an example of **Initialize** in VB:

```

Protected Overrides Sub Initialize()
   ' TODO: Add your initialization logic here

   MyBase.Initialize()
End Sub        
      
```

## LoadContent Method

The **LoadContent** method is where you load any necessary game assets such as models and textures.

```

Protected Overrides Sub LoadContent()
   ' Create a new SpriteBatch, which can be used to draw textures.
   spriteBatch = New SpriteBatch(GraphicsDevice)

   ' TODO: use Me.Content to load your game content here
End Sub  
      
```

## UnloadContent Method

The **UnloadContent** method is where any game assets can be released. Generally, no extra code is required here, since assets will be released automatically when they are no longer needed.

```

Protected Overrides Sub UnloadContent()
   ' TODO: Unload any non ContentManager content here
End Sub         
      
```

## Update Method

The Update method—called each frame—is the place to update your game logic. For example, to move objects around, take player input, decide the outcome of collisions between objects, and so on.

```

Protected Overrides Sub Update(ByVal gameTime As GameTime)
   ' Allows the game to exit
   If GamePad.GetState(PlayerIndex.One).Buttons.Back = ButtonState.Pressed Then
      Me.Exit()
   End If

   ' TODO: Add your update logic here
   MyBase.Update(gameTime)
End Sub         
      
```

## Draw Method

The Draw method—called each frame—is the place to render the background and all other game objects on the screen.

```

Protected Overrides Sub Draw(ByVal gameTime As GameTime)
   GraphicsDevice.Clear(Color.CornflowerBlue)

   ' TODO: Add your drawing code here
   MyBase.Draw(gameTime)
End Sub         
      
```

# Adding and Loading Content

Your game will need various content assets such as textures, meshes, and sprites.

Once you have an image chosen on your computer, add it to your content project by using the steps in [How to: Add Game Assets to a Content Project](UsingXNA_HowTo_AddAResource.md). Add the image as an existing item, and then perform the following steps to name the asset:

### To name a new content asset

1.  Select the entry for the image file in your game's Content project.
2.  If the **Properties** window is not visible below **Solution Explorer**, press F4 or open the **View** menu, and click **Properties**.
3.  In the **Properties** window, look for the "Asset Name" property. Note the name; you'll use it in your code to load the image so it can be displayed in your game. If you wish, you can change the name to something more convenient (for example, "mytexture").

When the game executes, it will need to load the content asset so that it can be drawn to the Windows Phone display. You will need to modify the overridden **LoadContent** method to perform the load.

```

Private myTexture As Texture2D

' Set the coordinates to draw the sprite at.
Private spritePosition As Vector2 = Vector2.Zero

Protected Overrides Sub LoadContent()
   ' Create a new SpriteBatch, which can be used to draw textures.
   spriteBatch = New SpriteBatch(GraphicsDevice)

   myTexture = Content.Load(Of Texture2D)("mytexture")
End Sub         
      
```

# Drawing and Moving a Sprite

Once the game has loaded content, it can draw the sprite to the display from within the **Draw** method.

```

Protected Overrides Sub Draw(ByVal gameTime As GameTime)
   GraphicsDevice.Clear(Color.CornflowerBlue)

   ' Draw the sprite.
   spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend)
   spriteBatch.Draw(myTexture, spritePosition, Color.White)
   spriteBatch.End()

   MyBase.Draw(gameTime)
End Sub
      
```

In the above code, the position where the sprite is drawn is specified in the spritePosition variable. Your game can make the sprite move by changing the value of spritePosition in the **Update** method.

The **UpdateSprite** subroutine, shown below, adjusts the value of spritePosition at a speed proportional to the elapsed game time, and changes direction if the sprite hits the edges of the display. When called from the **Update** method, it will move the sprite position each frame.

```

Private Sub UpdateSprite(ByVal gameTime As GameTime)
    ' Move the sprite by speed, scaled by elapsed time.
    spritePosition += spriteSpeed * CSng(gameTime.ElapsedGameTime.TotalSeconds)

    Dim MaxX = graphics.GraphicsDevice.Viewport.Width - myTexture.Width
    Dim MinX = 0
    Dim MaxY = graphics.GraphicsDevice.Viewport.Height - myTexture.Height
    Dim MinY = 0

    ' Check for bounce.
    If spritePosition.X > MaxX Then
        spriteSpeed.X *= -1
        spritePosition.X = MaxX
    ElseIf spritePosition.X < MinX Then
        spriteSpeed.X *= -1
        spritePosition.X = MinX
    End If

    If spritePosition.Y > MaxY Then
        spriteSpeed.Y *= -1
        spritePosition.Y = MaxY

    ElseIf spritePosition.Y < MinY Then
        spriteSpeed.Y *= -1
        spritePosition.Y = MinY
    End If
End Sub         
      
```

# Running Your Visual Basic Code

To test your Visual Basic code and see it running on the Windows Phone Emulator in Visual Studio, press F5 or right click your project in **Solution Explorer**, select **Debug**, then click **Start new instance**.

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team