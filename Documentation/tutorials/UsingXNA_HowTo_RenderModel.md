

# Rendering a Model with a Basic Effect

Demonstrates how to load and render a model using the XNA Framework Content Pipeline. It is assumed that an existing Windows game project is loaded in XNA Game Studio. In this example, the project is called "CPModel."

This example has three main parts: importing and processing the model, drawing the resultant managed object as a model with full lighting effect in the game, and enabling movement of the model with the Xbox 360 game pad.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download CPModel.zip](http://go.microsoft.com/fwlink/?LinkId=258692).

# Adding and Rendering a Model

### To add the model to the game

1.  Right-click the **Content** project, click **Add**, and then click **Existing Item**.
2.  Navigate to the correct folder and select the model to be added.
    
    For this example, use the ship.fbx file.
    
3.  Open the **Properties** window and verify that the correct importer and processor are specified.
    
    For this example, the **Content Importer** is AutoDesk FBX - XNA Framework and the **Content Processor** is Model - XNA Framework.
    
4.  Save the solution.

Build the solution.

The remaining parts render the model and add some user control of the model. All code modifications for this part occur within the game1.cs file of the game project.

### To render the model

1.  From Solution Explorer, double-click the game1.cs file.
2.  Modify the **Game1** class by adding the following code at the beginning of the declaration.
    
    ```
    private Model gameShip;
    ```
                        
    
    This member holds the ship model.
    
3.  Modify the **LoadGraphicsContent** method by adding the following code.
    
    ```
    gameShip = Content.Load<Model>("ship");
    ```
                        
    
    This code loads the model into the `gameShip` member (using [Load](M_Microsoft_Xna_Framework_Content_ContentManager_Load``1.md)).
    
4.  Create a new **private** method (called **DrawModel**) in the **Game1** class by adding the following code before the existing [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method.
    
    ```
    private void DrawModel(Model m)
    {
        Matrix[] transforms = new Matrix[m.Bones.Count];
        float aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
        m.CopyAbsoluteBoneTransformsTo(transforms);
        Matrix projection =
            Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
            aspectRatio, 1.0f, 10000.0f);
        Matrix view = Matrix.CreateLookAt(new Vector3(0.0f, 50.0f, Zoom),
            Vector3.Zero, Vector3.Up);
    
        foreach (ModelMesh mesh in m.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
    
                effect.View = view;
                effect.Projection = projection;
                effect.World = gameWorldRotation *
                    transforms[mesh.ParentBone.Index] *
                    Matrix.CreateTranslation(Position);
            }
            mesh.Draw();
        }
    }
    ```
    
    This code sets up the lighting effects for each sub-mesh of the model. The `gameWorldRotation` and `Zoom` variables are used for player control. This functionality is added later.
    
    ![](note.gif)Note
    
    This render code is designed for only those models with a [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md). For custom effects, the inner `for-each` loop should be changed to use the [Effect](T_Microsoft_Xna_Framework_Graphics_Effect.md) class instead of the [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md) class. In addition, you must use [EffectParameter](T_Microsoft_Xna_Framework_Graphics_EffectParameter.md) objects to manually set the world, view, and projection matrices.
    
5.  Modify the **Game1.Draw** method by replacing the following code `**`// TODO: Add your drawing code here` with the following code:
    ```
    DrawModel(gameShip);
    ```
    This initializes the model's effects before the model is rendered.
    
6.  Save the solution.

At this point, the rendering code for the model is complete, but the user control code still needs implementation.

### To move the model:

1.  Modify the **Game1** class by adding the following code after the `gameShip` declaration.
    
    ```
    private Vector3 Position = Vector3.One;
    private float Zoom = 2500;
    private float RotationY = 0.0f;
    private float RotationX = 0.0f;
    private Matrix gameWorldRotation;
    ```
                        
    
    These members store the current position, zoom, and rotation values. In addition, the `gameWorldRotation` simplifies the `UpdateGamePad` code.
    
2.  Add a private method (called **UpdateGamePad**) before the call to [Update](M_Microsoft_Xna_Framework_Game_Update.md).
    
    ```
    private void UpdateGamePad()
    {
        GamePadState state = GamePad.GetState(PlayerIndex.One);
    
        if (state.Buttons.A == ButtonState.Pressed)
        {
            Exit();
        }
    
        Position.X += state.ThumbSticks.Left.X * 10;
        Position.Y += state.ThumbSticks.Left.Y * 10;
        Zoom += state.ThumbSticks.Right.Y * 10;
        RotationY += state.ThumbSticks.Right.X;
        if (state.DPad.Up == ButtonState.Pressed)
        {
            RotationX += 1.0f;
        }
        else if (state.DPad.Down == ButtonState.Pressed)
        {
            RotationX -= 1.0f;
        }
        gameWorldRotation =
            Matrix.CreateRotationX(MathHelper.ToRadians(RotationX)) *
            Matrix.CreateRotationY(MathHelper.ToRadians(RotationY));
    }
    ```
    
    This code implements an exit method for the game (pressing the **A** button), and updates the position members with the current input of the game controller.
    
3.  Modify the **Update** method by adding a call to `UpdateGamePad`, before the call to [Update](M_Microsoft_Xna_Framework_Game_Update.md).
    
    ```
    UpdateGamePad();
    ```
    
    This code updates the state of the position variables with the latest input.
    
4.  Save the solution.

Development is complete so you are ready to build and run the game. Control the ship location with the game pad, and exit by pressing the **A** button.

# See Also

[Adding Content to a Game](CP_TopLevel.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0