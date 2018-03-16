

# Moving the Camera on a Curve

Demonstrates how to use the [Curve](T_Microsoft_Xna_Framework_Curve.md) and [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md) classes to move a camera along the shape of a curve.

Using [Curve](T_Microsoft_Xna_Framework_Curve.md)s allows a path to be defined by a small number of control points with the [Curve](T_Microsoft_Xna_Framework_Curve.md)s calculating the points on the path between the control points.

# The Complete Sample

The code in the topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download ScriptedCamera_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258725).

# Scripting the Camera to Follow a Curve

### To script camera movement

1.  Create an instance of the [Curve](T_Microsoft_Xna_Framework_Curve.md) class for each component being scripted.
    
    In this case, you need two sets of three curves. One is for each of the x, y, and z components of the camera's position, and the other is for the position at which the camera is looking (the "look-at" position).
    
    ```
    class Curve3D
    {
    
        public Curve curveX = new Curve();
        public Curve curveY = new Curve();
        public Curve curveZ = new Curve();
        ...
    }
    ```
    
2.  Set the [PreLoop](P_Microsoft_Xna_Framework_Curve_PreLoop.md) and [PostLoop](P_Microsoft_Xna_Framework_Curve_PostLoop.md) type of each [Curve](T_Microsoft_Xna_Framework_Curve.md).
    
    The [PreLoop](P_Microsoft_Xna_Framework_Curve_PreLoop.md) and [PostLoop](P_Microsoft_Xna_Framework_Curve_PostLoop.md) types determine how the curve will interpret positions before the first key or after the last key. In this case, the values will be set to [CurveLoopType.Oscillate](T_Microsoft_Xna_Framework_CurveLoopType.md). Values past the ends of the curve will change direction and head toward the opposite side of the curve.
    
    ```
    curveX.PostLoop = CurveLoopType.Oscillate;
    curveY.PostLoop = CurveLoopType.Oscillate;
    curveZ.PostLoop = CurveLoopType.Oscillate;
    
    curveX.PreLoop = CurveLoopType.Oscillate;
    curveY.PreLoop = CurveLoopType.Oscillate;
    curveZ.PreLoop = CurveLoopType.Oscillate;
    ```
    
3.  Add [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md)s to the [Curve](T_Microsoft_Xna_Framework_Curve.md)s.
    
4.  Specify the time each [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md) should be reached and the camera position when the [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md) is reached.
    
    In this case, each point in time will have three [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md)s associated with it – one for each of the x, y, and z coordinates of the point on the [Curve](T_Microsoft_Xna_Framework_Curve.md).
    
    ```
    public void AddPoint(Vector3 point, float time)
    {
        curveX.Keys.Add(new CurveKey(time, point.X));
        curveY.Keys.Add(new CurveKey(time, point.Y));
        curveZ.Keys.Add(new CurveKey(time, point.Z));
    }
    ```
    
5.  Loop through each [Curve](T_Microsoft_Xna_Framework_Curve.md) setting the [TangentIn](P_Microsoft_Xna_Framework_CurveKey_TangentIn.md) and [TangentOut](P_Microsoft_Xna_Framework_CurveKey_TangentOut.md) of each [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md).
    
    The tangents of the [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md)s control the shape of the [Curve](T_Microsoft_Xna_Framework_Curve.md). Setting the tangents of the [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md)s to the slope between the previous and next [CurveKey](T_Microsoft_Xna_Framework_CurveKey.md) will give a curve that moves smoothly through each point on the curve.
    
    ```
    public void SetTangents()
    {
        CurveKey prev;
        CurveKey current;
        CurveKey next;
        int prevIndex;
        int nextIndex;
        for (int i = 0; i < curveX.Keys.Count; i++)
        {
            prevIndex = i - 1;
            if (prevIndex < 0) prevIndex = i;
    
            nextIndex = i + 1;
            if (nextIndex == curveX.Keys.Count) nextIndex = i;
    
            prev = curveX.Keys[prevIndex];
            next = curveX.Keys[nextIndex];
            current = curveX.Keys[i];
            SetCurveKeyTangent(ref prev, ref current, ref next);
            curveX.Keys[i] = current;
            prev = curveY.Keys[prevIndex];
            next = curveY.Keys[nextIndex];
            current = curveY.Keys[i];
            SetCurveKeyTangent(ref prev, ref current, ref next);
            curveY.Keys[i] = current;
    
            prev = curveZ.Keys[prevIndex];
            next = curveZ.Keys[nextIndex];
            current = curveZ.Keys[i];
            SetCurveKeyTangent(ref prev, ref current, ref next);
            curveZ.Keys[i] = current;
        }
    }
    ```
    
6.  Add code to evaluate the x, y, and z coordinates of the [Curve](T_Microsoft_Xna_Framework_Curve.md)s at any given time by passing the elapsed time to the [Evaluate](M_Microsoft_Xna_Framework_Curve_Evaluate.md) method of each of the [Curve](T_Microsoft_Xna_Framework_Curve.md)s.
    
    ```
    public Vector3 GetPointOnCurve(float time)
    {
        Vector3 point = new Vector3();
        point.X = curveX.Evaluate(time);
        point.Y = curveY.Evaluate(time);
        point.Z = curveZ.Evaluate(time);
        return point;
    }
    ```
    
7.  Create a variable to track the amount of time that has passed since the camera started moving.
    
    ```
    double time;
    ```
    
8.  In [Game.Update](M_Microsoft_Xna_Framework_Game_Update.md), set the camera's position and look-at position based on the elapsed time since the camera started moving, and then set the camera's view and projection matrices as in [Rotating and Moving the Camera](Math_HowTo_RotateMoveCamera.md).
    
    ```
    // Calculate the camera's current position.
    Vector3 cameraPosition =
        cameraCurvePosition.GetPointOnCurve((float)time);
    Vector3 cameraLookat =
        cameraCurveLookat.GetPointOnCurve((float)time);
    ```
    
9.  In [Game.Update](M_Microsoft_Xna_Framework_Game_Update.md), use [gameTime.ElapsedGameTime.TotalMilliseconds](P_Microsoft_Xna_Framework_GameTime_ElapsedGameTime.md) to increment the time since the camera started moving.
    
    ```
    time += gameTime.ElapsedGameTime.TotalMilliseconds;
    ```
    

# See Also

[Rotating and Moving the Camera](Math_HowTo_RotateMoveCamera.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0