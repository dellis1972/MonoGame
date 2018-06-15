

# Transforming a Point

This example demonstrates how to use the [Vector3](xref:Microsoft.Xna.Framework.Vector3) and [Matrix](xref:Microsoft.Xna.Framework.Matrix) classes to transform a point. A matrix transform can include scaling, rotating, and translating information.

# The Complete Sample

The code in the topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download TransformPoint_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258738).

# Transforming a Point with a Matrix

### To transform a point

1.  Create a [Matrix](xref:Microsoft.Xna.Framework.Matrix) by using [CreateRotationY](xref:Microsoft.Xna.Framework.Matrix.CreateRotationY) or one of the other **Create** methods.
2.  Pass the point and the [Matrix](xref:Microsoft.Xna.Framework.Matrix) to the [Vector3.Transform](xref:Microsoft.Xna.Framework.Vector3.Transform) method.

```
static Vector3 RotatePointOnYAxis(Vector3 point, float angle)
{
    // Create a rotation matrix that represents a rotation of angle radians.
    Matrix rotationMatrix = Matrix.CreateRotationY(angle);

    // Apply the rotation matrix to the point.
    Vector3 rotatedPoint = Vector3.Transform(point, rotationMatrix);

    return rotatedPoint;
}
```

# See Also

#### Matrix Creation Methods

[CreateRotationX](xref:Microsoft.Xna.Framework.Matrix.CreateRotationX)  
[CreateRotationY](xref:Microsoft.Xna.Framework.Matrix.CreateRotationY)  
[CreateRotationZ](xref:Microsoft.Xna.Framework.Matrix.CreateRotationZ)  
[CreateScale](xref:Microsoft.Xna.Framework.Matrix.CreateScale)  
[CreateTranslation](xref:Microsoft.Xna.Framework.Matrix.CreateTranslation)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team