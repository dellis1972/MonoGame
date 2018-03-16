

# Drawing a Sprite Over a Model

Demonstrates how to draw a sprite so that it obscures a model. In this example, we are drawing an animated sprite representing an explosion over the current screen position of a 3D model.

# Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download ViewportProjection_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258741).

For this sample, the camera is a standard arc ball camera, implemented by camera.cs. The 3D model file is a simple ring, implemented by ring16b.x. The animated explosion sprite is implemented by explosion.dds. These files can be found in the complete sample. See [Animating a Sprite](2DGraphicsHowTo_Animate_Sprite.md) for an example of the **AnimatedTexture** class.

# Drawing a Sprite Over a Model

### To draw a sprite over a model

1.  In your [Update](M_Microsoft_Xna_Framework_Game_Update.md) method, handle the input to move your camera, then call **UpdateFrame** on the **AnimatedTexture**.
    
     ```
     GamePadState PlayerOne = GamePad.GetState(PlayerIndex.One);
    
    // Move the camera using thumbsticks
    MoveCamera(PlayerOne);
    
    // Start or stop the animated sprite using buttons
    if (PlayerOne.Buttons.A == ButtonState.Pressed)
        explosion.Play();
    if (PlayerOne.Buttons.B == ButtonState.Pressed)
        explosion.Stop();
    
    // Update the animated sprite
    explosion.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
    ```
    
2.  Use [CreateMerged](O_M_Microsoft_Xna_Framework_BoundingSphere_CreateMerged.md) to create a [BoundingSphere](T_Microsoft_Xna_Framework_BoundingSphere.md) that contains all the **BoundingSphere** values for each [ModelMesh](T_Microsoft_Xna_Framework_Graphics_ModelMesh.md) in the [Model](T_Microsoft_Xna_Framework_Graphics_Model.md).
    
3.  Use [Viewport.Project](M_Microsoft_Xna_Framework_Graphics_Viewport_Project.md) to find the center point of that sphere, which is the center of the model in screen coordinates.
    
    ```
    // Create a total bounding sphere for the mesh
    BoundingSphere totalbounds = new BoundingSphere();
    foreach (ModelMesh mesh in Ring.Meshes)
    {
        totalbounds = BoundingSphere.CreateMerged(totalbounds,
            mesh.BoundingSphere);
    }
    
    // Project the center of the 3D object to the screen, and center the
    // sprite there
    Vector3 center = GraphicsDevice.Viewport.Project(totalbounds.Center,
        projectionMatrix, Camera1.ViewMatrix, Matrix.Identity);
    explosionpos.X = center.X;
    explosionpos.Y = center.Y;
    ```
                        
    
4.  Take the **BoundingSphere** for the model and use it to create a [BoundingBox](T_Microsoft_Xna_Framework_BoundingBox.md) with [CreateFromSphere](O_M_Microsoft_Xna_Framework_BoundingBox_CreateFromSphere.md).
    
5.  Use [Project](M_Microsoft_Xna_Framework_Graphics_Viewport_Project.md) to find the corner of the box farthest from the center and use the return value to scale the sprite appropriately.
    
    ```
    // Create a bounding box from the bounding sphere, 
    // and find the corner that is farthest away from 
    // the center using Project
    BoundingBox extents = BoundingBox.CreateFromSphere(totalbounds);
    float maxdistance = 0;
    float distance;
    Vector3 screencorner;
    foreach (Vector3 corner in extents.GetCorners())
    {
        screencorner = GraphicsDevice.Viewport.Project(corner,
        projectionMatrix, Camera1.ViewMatrix, Matrix.Identity);
        distance = Vector3.Distance(screencorner, center);
        if (distance > maxdistance)
            maxdistance = distance;
    }
    
    // Scale the sprite using the two points (the sprite is 
    // 75 pixels square)
    explosion.Scale = maxdistance / 75;
    ```
                        
    
6.  In your [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) method, draw the [Model](T_Microsoft_Xna_Framework_Graphics_Model.md) normally, and then draw the animated sprite using the position calculated in [Update](M_Microsoft_Xna_Framework_Game_Update.md).
    
    ```
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
    
        //Draw the model, a model can have multiple meshes, so loop
        foreach (ModelMesh mesh in Ring.Meshes)
        {
            //This is where the mesh orientation is set, as well as 
            //our camera and projection
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
                effect.World = Matrix.Identity *
                    Matrix.CreateRotationY(RingRotation) *
                    Matrix.CreateTranslation(RingPosition);
                effect.View = Camera1.ViewMatrix;
                effect.Projection = projectionMatrix;
            }
            //Draw the mesh, will use the effects set above.
            mesh.Draw();
        }
    
        // Draw the sprite over the 3D object
        //            spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
        //                SpriteSortMode.Deferred, SaveStateMode.SaveState);
        spriteBatch.Begin();
        explosion.DrawFrame(spriteBatch, explosionpos);
        spriteBatch.End();
    
        base.Draw(gameTime);
    }
    ```
    

# See Also

#### Concepts

[What Is a Sprite?](Sprite_Overview.md)  
[What Is Color Blending?](WhatIs_ColorBlending.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0