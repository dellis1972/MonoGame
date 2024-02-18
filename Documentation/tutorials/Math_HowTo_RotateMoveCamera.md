

# Rotating and Moving the Camera

Demonstrates how to rotate and move a camera in a 3D environment. You can rotate the camera about its y-axis, and move it forward and backward. You control the camera's position and orientation by using the directional keys on your keyboard or by using the D-pad of your Xbox 360 gamepad.

# The Complete Sample

The code in the topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download FirstPersonCamera_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258697).

# Overview

This sample is based on several assumptions.

*   The camera will move frequently, so the camera view [Matrix](xref:Microsoft.Xna.Framework.Matrix) is created and set every time [Game.Update](xref:Microsoft.Xna.Framework.Game.Update) is called.
*   The projection [Matrix](xref:Microsoft.Xna.Framework.Matrix) may also change frequently for effects such as zooming.
*   You have added a model to the project.

For the sake of simplicity, the sample limits the camera object to rotation about the y axis (vertical spin) and movement along the z axis (forward and backward). The following steps show you how to render the sample scene.

### To render the sample scene

1.  Determine the location and orientation of the camera.
    
    ```
    static Vector3 avatarPosition = new Vector3(0, 0, -50);
    static Vector3 cameraPosition = avatarPosition;
    ```
                        
    
2.  Create a view matrix using the camera position, the camera orientation (also called the look at point), and the up vector using [CreateLookAt](xref:Microsoft.Xna.Framework.Matrix.CreateLookAt).
    
    ```
    // Calculate the camera's current position.
    
    Matrix rotationMatrix = Matrix.CreateRotationY(avatarYaw);
    
    // Create a vector pointing the direction the camera is facing.
    Vector3 transformedReference = Vector3.Transform(cameraReference, rotationMatrix);
    
    // Calculate the position the camera is looking at.
    Vector3 cameraLookat = cameraPosition + transformedReference;
    
    // Set up the view matrix and projection matrix.
    view = Matrix.CreateLookAt(cameraPosition, cameraLookat, new Vector3(0.0f, 1.0f, 0.0f));
    ```
                        
    
3.  Create a perspective matrix using the near and far clipping planes and the aspect ratio using [CreatePerspectiveFieldOfView](xref:Microsoft.Xna.Framework.Matrix.CreatePerspectiveFieldOfView).
    
    ```
    proj = Matrix.CreatePerspectiveFieldOfView(viewAngle, graphics.GraphicsDevice.Viewport.AspectRatio, nearClip, farClip);
    ```
                        
    
4.  In the [Draw](xref:Microsoft.Xna.Framework.Game.Update) method of your game, initialize a [BasicEffect](xref:Microsoft.Xna.Framework.Graphics.BasicEffect) object with the world, view, and projection matrices and render all of the 3D objects in the scene.
    
    ```
    void DrawModel(Model model, Matrix world, Texture2D texture)
    {
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect be in mesh.Effects)
            {
                be.Projection = proj;
                be.View = view;
                be.World = world;
                be.Texture = texture;
                be.TextureEnabled = true;
            }
            mesh.Draw();
        }
    }
    ```

# Rotating and Moving a Camera

### To rotate and move the camera

1.  Determine the location and orientation of the camera.
    
    ```
    static Vector3 avatarPosition = new Vector3(0, 0, -50);
    static Vector3 cameraPosition = avatarPosition;
    ```
                        
    
2.  Determine the reference [Vector3](xref:Microsoft.Xna.Framework.Vector3) to which the rotation of the camera is relative.
    
    The direction should not change during the game, and usually it will be (0, 0, 1) or (0, 0, −1).
    
    ```
    // Set the direction the camera points without rotation.
    Vector3 cameraReference = new Vector3(0, 0, 1);
    ```
                        
    
3.  Create a rotation [Matrix](xref:Microsoft.Xna.Framework.Matrix) for the amount of rotation for the camera.
    
    Because the camera is limited to one axis of rotation, this matrix represents the rotation of the camera around its own y-axis. Use [CreateRotationY](xref:Microsoft.Xna.Framework.Matrix.CreateRotationY) to create a rotation [Matrix](xref:Microsoft.Xna.Framework.Matrix) representing the rotation around the y-axis.
    
    ```
    Matrix rotationMatrix = Matrix.CreateRotationY(avatarYaw);
    ```
                        
    
4.  Use [Transform](xref:Microsoft.Xna.Framework.Vector3.Transform) and the rotation [Matrix](xref:Microsoft.Xna.Framework.Matrix) to transform the reference [vector](xref:Microsoft.Xna.Framework.Vector3).
    
    This represents the direction the camera is pointing in transformed (or view) space.
    
    ```
    // Create a vector pointing the direction the camera is facing.
    Vector3 transformedReference = Vector3.Transform(cameraReference, rotationMatrix);
    ```
                        
    
5.  Add the camera's current position to the transformed direction [vector](xref:Microsoft.Xna.Framework.Vector3).
    
    The result is the position to which the camera is pointing.
    
    ```
    // Calculate the position the camera is looking at.
    Vector3 cameraLookat = cameraPosition + transformedReference;
    ```
    
6.  Create a new view [Matrix](xref:Microsoft.Xna.Framework.Matrix) using [CreateLookAt](xref:Microsoft.Xna.Framework.Matrix.CreateLookAt).
    
7.  Use [CreateLookAt](xref:Microsoft.Xna.Framework.Matrix.CreateLookAt) to pass the camera's current position and the transformed direction vector.
    
    The third parameter of [CreateLookAt](xref:Microsoft.Xna.Framework.Matrix.CreateLookAt) is the up direction of the camera. Typically, it is [Vector3.Up](xref:Microsoft.Xna.Framework.Vector3.Up) (0, 1, 0). This matrix [Matrix](xref:Microsoft.Xna.Framework.Matrix) controls how world coordinates are transformed to camera coordinates.
    
    ```
    // Set up the view matrix and projection matrix.
    view = Matrix.CreateLookAt(cameraPosition, cameraLookat, new Vector3(0.0f, 1.0f, 0.0f));
    ```
    
8.  Use [CreatePerspectiveFieldOfView](xref:Microsoft.Xna.Framework.Matrix.CreatePerspectiveFieldOfView) to create a new projection [Matrix](xref:Microsoft.Xna.Framework.Matrix).
    
    This [Matrix](xref:Microsoft.Xna.Framework.Matrix) controls how camera coordinate values are transformed to screen coordinates.
    
    The first parameter is the field of view of the projection [Matrix](xref:Microsoft.Xna.Framework.Matrix) expressed in radians. A typical field of view of 45 degrees would be expressed as π/4 radians. The second parameter is the aspect ratio of the projection [Matrix](xref:Microsoft.Xna.Framework.Matrix); it corrects for the difference in width and height of a viewspace. The third and fourth parameters specify the near and far distances at which the objects will be visible.
    
    ```
    // Set distance from the camera of the near and far clipping planes.
    static float nearClip = 1.0f;
    static float farClip = 2000.0f;
    ```
    
9.  Loop through each 3D model to be rendered using the projection matrix and view matrix created above.
    
    An identity matrix simplifies the code for the world matrix.
    
    ```
    void DrawModel(Model model, Matrix world, Texture2D texture)
    {
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect be in mesh.Effects)
            {
                be.Projection = proj;
                be.View = view;
                be.World = world;
                be.Texture = texture;
                be.TextureEnabled = true;
            }
            mesh.Draw();
        }
    }
    ```

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team