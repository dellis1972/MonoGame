

# Transforming a Point

This example demonstrates how to use the [Vector3](T_Microsoft_Xna_Framework_Vector3.md) and [Matrix](T_Microsoft_Xna_Framework_Matrix.md) classes to transform a point. A matrix transform can include scaling, rotating, and translating information.

# The Complete Sample

The code in the topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download TransformPoint_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258738).

# Transforming a Point with a Matrix

### To transform a point

1.  Create a [Matrix](T_Microsoft_Xna_Framework_Matrix.md) by using [CreateRotationY](O_M_Microsoft_Xna_Framework_Matrix_CreateRotationY.md) or one of the other **Create** methods.
2.  Pass the point and the [Matrix](T_Microsoft_Xna_Framework_Matrix.md) to the [Vector3.Transform](O_M_Microsoft_Xna_Framework_Vector3_Transform.md) method.

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

[CreateRotationX](O_M_Microsoft_Xna_Framework_Matrix_CreateRotationX.md)  
[CreateRotationY](O_M_Microsoft_Xna_Framework_Matrix_CreateRotationY.md)  
[CreateRotationZ](O_M_Microsoft_Xna_Framework_Matrix_CreateRotationZ.md)  
[CreateScale](O_M_Microsoft_Xna_Framework_Matrix_CreateScale.md)  
[CreateTranslation](O_M_Microsoft_Xna_Framework_Matrix_CreateTranslation.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0