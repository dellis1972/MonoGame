

# What are Vectors, Matrices, and Quaternions?

The XNA Framework Math Libraries are in the [Microsoft.Xna.Framework](N_Microsoft_Xna_Framework.md) namespace alongside a number of additional types that deal with the XNA Framework Application model.

*   [Coordinate system](#ID4EZB)
*   [Mathematical Constants and Scalar Manipulation](#ID4EAC)
*   [Basic Geometric Types](#ID4EXC)
*   [Precision and Performance](#ID4ECH)

# Coordinate system

The XNA Framework uses a right-handed coordinate system, with the positive z-axis pointing toward the observer when the positive x-axis is pointing to the right, and the positive y-axis is pointing up.

# Mathematical Constants and Scalar Manipulation

The XNA Framework provides the [MathHelper Members](T_Microsoft_Xna_Framework_MathHelper.md) class for [manipulating scalar values](Methods_T_Microsoft_Xna_Framework_MathHelper.md) and retrieving some [common mathematical constants](Fields_T_Microsoft_Xna_Framework_MathHelper.md). This includes methods such as the [ToDegrees](M_Microsoft_Xna_Framework_MathHelper_ToDegrees.md) and [ToRadians](M_Microsoft_Xna_Framework_MathHelper_ToRadians.md) utility methods for converting between degrees and radians.

# Basic Geometric Types

The XNA Framework Math library has multiple basic geometric types for manipulating objects in 2D or 3D space. Each geometric type has a number of mathematical operations that are supported for the type.

## Vectors

The XNA Framework provides the [Vector2](T_Microsoft_Xna_Framework_Vector2.md), [Vector3](T_Microsoft_Xna_Framework_Vector3.md), and [Vector4](T_Microsoft_Xna_Framework_Vector4.md) classes for representing and manipulating vectors. A vector typically is used to represent a direction and magnitude. In the XNA Framework, however, it also could be used to store a coordinate or other data type with the same storage requirements.

Each vector class has methods for performing standard vector operations such as:

*   [Dot product](O_M_Microsoft_Xna_Framework_Vector3_Dot.md)
*   [Cross product](O_M_Microsoft_Xna_Framework_Vector3_Cross.md)
*   [Normalization](O_M_Microsoft_Xna_Framework_Vector3_Normalize.md)
*   [Transformation](O_M_Microsoft_Xna_Framework_Vector3_Transform.md)
*   [Linear](O_M_Microsoft_Xna_Framework_Vector3_Lerp.md), [Cubic](O_M_Microsoft_Xna_Framework_Vector3_SmoothStep.md), [Catmull-Rom](O_M_Microsoft_Xna_Framework_Vector3_CatmullRom.md), or [Hermite spline](O_M_Microsoft_Xna_Framework_Vector3_Hermite.md) interpolation.

## Matrices

The XNA Framework provides the [Matrix](T_Microsoft_Xna_Framework_Matrix.md) class for transformation of geometry. The [Matrix](T_Microsoft_Xna_Framework_Matrix.md) class uses row major order to address matrices, which means that the row is specified before the column when describing an element of a two-dimensional matrix. The [Matrix](T_Microsoft_Xna_Framework_Matrix.md) class provides methods for performing standard matrix operations such as calculating the [determinate](M_Microsoft_Xna_Framework_Matrix_Determinant.md) or [inverse](O_M_Microsoft_Xna_Framework_Matrix_Invert.md) of a matrix. There also are helper methods for creating scale, rotation, and translation matrices.

## Quaternions

The XNA Framework provides the [Quaternion](T_Microsoft_Xna_Framework_Quaternion.md) structure to calculate the efficient rotation of a vector by a specified angle.

## Curves

The [Curve](T_Microsoft_Xna_Framework_Curve.md) class represents a hermite curve for interpolating varying positions at different times without having to explicitly define each position. The curve is defined by a collection of [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md) points representing each varying position at different times. This class can be used not only for spatial motion, but also to represent any response that changes over time.

## Bounding Volumes

The XNA Framework provides the [BoundingBox](T_Microsoft_Xna_Framework_BoundingBox.md), [BoundingFrustum](T_Microsoft_Xna_Framework_BoundingFrustum.md), [BoundingSphere](T_Microsoft_Xna_Framework_BoundingSphere.md), [Plane](T_Microsoft_Xna_Framework_Plane.md), and [Ray](T_Microsoft_Xna_Framework_Ray.md) classes for representing simplified versions of geometry for the purpose of efficient collision and hit testing. These classes have methods for checking for intersection and containment with each other.

# Precision and Performance

The XNA Framework Math libraries are single-precision. This means that the primitives and operations contained in this library use 32-bit floating-point numbers to achieve a balance between precision and efficiency when performing large numbers of calculations.

A 32-bit floating-point number ranges from –3.402823e38> to +3.402823e38. The 32 bits store the sign, mantissa, and exponent of the number that yields seven digits of floating-point precision. Some numbers—for example π, 1/3, or the square root of two—can be approximated only with seven digits of precision, so be aware of rounding errors when using a binary representation of a floating-point number. For more information about single-precision numbers, see the documentation for the [Single](http://msdn.microsoft.com/en-us/library/system.single.aspx) data type.

# See Also

#### Concepts

[Math Content Catalog at App Hub Online](http://go.microsoft.com/fwlink/?LinkId=128874)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team