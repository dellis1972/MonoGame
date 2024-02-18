

# Selecting an Object in 3D Space

Demonstrates how to check whether the mouse (or touch location) is positioned over a 3D object by creating a ray starting at the camera's near clipping plane and ending at its far clipping plane.

![](note.gif)Note

This example applies most directly to Windows development where mice are ubiquitous.

Because the [Mouse](xref:Microsoft.Xna.Framework.Input.Mouse) class and [MouseState](xref:Microsoft.Xna.Framework.Input.MouseState) structure do provide limited functionality on Windows Phone, this sample should work, as is, on that platform as well. However, a better solution on Windows Phone would be to substitute a call to the [TouchPanel.GetState](xref:MXFIT.TouchPanel.GetState) method for the call to the [Mouse.GetState](xref:Microsoft.Xna.Framework.Input.Mouse.GetState) method in order to retrieve alternative X and Y coordinates from a Windows Phone touch screen. In general, games and other applications designed for Windows Phone should use the [TouchPanel](xref:MXFIxref:TouchPanel) class to retrieve user input.

As written, this sample is least applicable to Xbox 360, although the technique of creating a ray and then keeping track of the closest object that intersects the ray remains potentially useful.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download Click3DObject.zip](http://go.microsoft.com/fwlink/?LinkId=258689)

# Detecting Whether a User Clicked a 3D Object

### To check whether the mouse is positioned over a 3D object

1.  Get the current state of the mouse by using [GetState](xref:Microsoft.Xna.Framework.Input.Mouse.GetState).
    
    ```
    MouseState mouseState = Mouse.GetState();
    ```
    
2.  Get the current screen coordinates of the mouse from [X](xref:Microsoft.Xna.Framework.Input.MouseState.X) and [Y](xref:Microsoft.Xna.Framework.Input.MouseState.Y).
    
    ```
    int mouseX = mouseState.X;
    int mouseY = mouseState.Y;
    ```
    
3.  Using [Viewport.Unproject](xref:Microsoft.Xna.Framework.Graphics.Viewport.Unproject), determine points in world space on the near and far clipping planes. For the point on the near plane, pass a source vector with x and y set to the mouse position, and z set to 0.
    
4.  For the point on the far plane, pass a source vector with x and y set to the mouse position, and z set to 1.
    
5.  Create a translation matrix for a point that is the origin, (0,0,0).
    
6.  For both points, pass [Unproject](xref:Microsoft.Xna.Framework.Graphics.Viewport.Unproject) the current projection matrix, the view matrix.
    
    ```
    Vector3 nearsource = new Vector3((float)mouseX, (float)mouseY, 0f);
    Vector3 farsource = new Vector3((float)mouseX, (float)mouseY, 1f);
    
    Matrix world = Matrix.CreateTranslation(0, 0, 0);
    
    Vector3 nearPoint = GraphicsDevice.Viewport.Unproject(nearsource,
        proj, view, world);
    
    Vector3 farPoint = GraphicsDevice.Viewport.Unproject(farsource,
        proj, view, world);
    ```
    
7.  Create a [Ray](xref:Microsoft.Xna.Framework.Ray) whose origin is at the near point and whose direction points to the far point.
    
    ```
    // Create a ray from the near clip plane to the far clip plane.
    Vector3 direction = farPoint - nearPoint;
    direction.Normalize();
    Ray pickRay = new Ray(nearPoint, direction);
    ```
    
8.  Loop throught each object in the scene using the [Intersects](xref:Microsoft.Xna.Framework.Ray.Intersects) method to check whether the [Ray](xref:Microsoft.Xna.Framework.Ray) intersects each object.
    
9.  If the [Ray](xref:Microsoft.Xna.Framework.Ray) intersects an object, check whether it is the closest object intersected so far. If it is, store the object and the distance at which it was intersected, replacing any previously stored object.
    
10.  When you completely loop through the objects, the last object stored will be the closest object underneath the area the user clicked.
    

# See Also

[Rotating and Moving the Camera](Math_HowTo_RotateMoveCamera.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team