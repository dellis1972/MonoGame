

# What are Vectors, Matrices, and Quaternions?

The XNA Framework Math Libraries are in the [Microsoft.Xna.Framework](xref:Microsoft.Xna.Framework) namespace alongside a number of additional types that deal with the XNA Framework Application model.

*   [Coordinate system](#ID4EZB)
*   [Mathematical Constants and Scalar Manipulation](#ID4EAC)
*   [Basic Geometric Types](#ID4EXC)
*   [Precision and Performance](#ID4ECH)

# Coordinate system

The XNA Framework uses a right-handed coordinate system, with the positive z-axis pointing toward the observer when the positive x-axis is pointing to the right, and the positive y-axis is pointing up.

# Mathematical Constants and Scalar Manipulation

The XNA Framework provides the [MathHelper Members](xref:Microsoft.Xna.Framework.MathHelper) class for [manipulating scalar values](Methods_T_Microsoft_Xna_Framework_MathHelper.md) and retrieving some [common mathematical constants](Fields_T_Microsoft_Xna_Framework_MathHelper.md). This includes methods such as the [ToDegrees](xref:Microsoft.Xna.Framework.MathHelper.ToDegrees) and [ToRadians](xref:Microsoft.Xna.Framework.MathHelper.ToRadians) utility methods for converting between degrees and radians.

# Basic Geometric Types

The XNA Framework Math library has multiple basic geometric types for manipulating objects in 2D or 3D space. Each geometric type has a number of mathematical operations that are supported for the type.

## Vectors

The XNA Framework provides the [Vector2](xref:Microsoft.Xna.Framework.Vector2), [Vector3](xref:Microsoft.Xna.Framework.Vector3), and [Vector4](xref:Microsoft.Xna.Framework.Vector4) classes for representing and manipulating vectors. A vector typically is used to represent a direction and magnitude. In the XNA Framework, however, it also could be used to store a coordinate or other data type with the same storage requirements.

Each vector class has methods for performing standard vector operations such as:

*   [Dot product](xref:Microsoft.Xna.Framework.Vector3.Dot)
*   [Cross product](xref:Microsoft.Xna.Framework.Vector3.Cross)
*   [Normalization](xref:Microsoft.Xna.Framework.Vector3.Normalize)
*   [Transformation](xref:Microsoft.Xna.Framework.Vector3.Transform)
*   [Linear](xref:Microsoft.Xna.Framework.Vector3.Lerp), [Cubic](xref:Microsoft.Xna.Framework.Vector3.SmoothStep), [Catmull-Rom](xref:Microsoft.Xna.Framework.Vector3.CatmullRom), or [Hermite spline](xref:Microsoft.Xna.Framework.Vector3.Hermite) interpolation.

## Matrices

The XNA Framework provides the [Matrix](xref:Microsoft.Xna.Framework.Matrix) class for transformation of geometry. The [Matrix](xref:Microsoft.Xna.Framework.Matrix) class uses row major order to address matrices, which means that the row is specified before the column when describing an element of a two-dimensional matrix. The [Matrix](xref:Microsoft.Xna.Framework.Matrix) class provides methods for performing standard matrix operations such as calculating the [determinate](xref:Microsoft.Xna.Framework.Matrix.Determinant) or [inverse](xref:Microsoft.Xna.Framework.Matrix.Invert) of a matrix. There also are helper methods for creating scale, rotation, and translation matrices.

## Quaternions

The XNA Framework provides the [Quaternion](xref:Microsoft.Xna.Framework.Quaternion) structure to calculate the efficient rotation of a vector by a specified angle.

## Curves

The [Curve](xref:Microsoft.Xna.Framework.Curve) class represents a hermite curve for interpolating varying positions at different times without having to explicitly define each position. The curve is defined by a collection of [CurveKey](xref:Microsoft.Xna.Framework.CurveKey) points representing each varying position at different times. This class can be used not only for spatial motion, but also to represent any response that changes over time.

## Bounding Volumes

The XNA Framework provides the [BoundingBox](xref:Microsoft.Xna.Framework.BoundingBox), [BoundingFrustum](xref:Microsoft.Xna.Framework.BoundingFrustum), [BoundingSphere](xref:Microsoft.Xna.Framework.BoundingSphere), [Plane](xref:Microsoft.Xna.Framework.Plane), and [Ray](xref:Microsoft.Xna.Framework.Ray) classes for representing simplified versions of geometry for the purpose of efficient collision and hit testing. These classes have methods for checking for intersection and containment with each other.

# Precision and Performance

The XNA Framework Math libraries are single-precision. This means that the primitives and operations contained in this library use 32-bit floating-point numbers to achieve a balance between precision and efficiency when performing large numbers of calculations.

A 32-bit floating-point number ranges from –3.402823e38> to +3.402823e38. The 32 bits store the sign, mantissa, and exponent of the number that yields seven digits of floating-point precision. Some numbers—for example π, 1/3, or the square root of two—can be approximated only with seven digits of precision, so be aware of rounding errors when using a binary representation of a floating-point number. For more information about single-precision numbers, see the documentation for the [Single](http://msdn.microsoft.com/en-us/library/system.single.aspx) data type.

# See Also

#### Concepts

[Math Content Catalog at App Hub Online](http://go.microsoft.com/fwlink/?LinkId=128874)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team