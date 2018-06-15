

# Testing for Collisions

Demonstrates how to use the [BoundingSphere](T_Microsoft_Xna_Framework_BoundingSphere.md) class to check whether two models are colliding.

# The Complete Sample

The code in the topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download CollisionBetweenSpheres.zip](http://go.microsoft.com/fwlink/?LinkId=258690).

# Detecting Whether Two Models Collide

### To check whether two objects are colliding

1.  Track the position of a model as it moves about the game world.
    
    ```
    struct WorldObject
    {
        public Vector3 position;
        public Vector3 velocity;
        public Model model;
        public Texture2D texture2D;
        public Vector3 lastPosition;
        public void MoveForward()
        {
            lastPosition = position;
            position += velocity;
        }
        public void Backup()
        {
            position -= velocity;
        }
        public void ReverseVelocity()
        {
            velocity.X = -velocity.X;
        }
    }
    ```
    
2.  Make a nested loop with the first model's meshes as the outer loop and the second model's meshes as the inner loop.
3.  Inside the loop, follow these steps.
    
    1.  Get the bounding sphere for the current mesh of the first model and the current mesh of the second model.
        
    2.  Offset the bounding spheres by the current positions of the models.
        
    3.  Call the [BoundingSphere.Intersects](O_M_Microsoft_Xna_Framework_BoundingSphere_Intersects.md) method to check the pairs of bounding spheres for collision.
        
        If the method returns **true**, the objects are colliding.
        
    4.  If the models are colliding, break out of the loop.
        
        ```
        static void CheckForCollisions(ref WorldObject c1, ref WorldObject c2)
        {
            for (int i = 0; i < c1.model.Meshes.Count; i++)
            {
                // Check whether the bounding boxes of the two cubes intersect.
                BoundingSphere c1BoundingSphere = c1.model.Meshes[i].BoundingSphere;
                c1BoundingSphere.Center += c1.position;
        
                for (int j = 0; j < c2.model.Meshes.Count; j++)
                {
                    BoundingSphere c2BoundingSphere = c2.model.Meshes[j].BoundingSphere;
                    c2BoundingSphere.Center += c2.position;
        
                    if (c1BoundingSphere.Intersects(c2BoundingSphere))
                    {
                        c2.ReverseVelocity();
                        c1.Backup();
                        c1.ReverseVelocity();
                        return;
                    }
                }
            }
        }
        ```
        

![](note.gif)Note

For an example of determining a particle's path after it hits a surface, see [Vector3.Reflect](O_M_Microsoft_Xna_Framework_Vector3_Reflect.md).

# See Also

#### Tasks

[Rotating and Moving the Camera](Math_HowTo_RotateMoveCamera.md)  

#### Concepts

[Bounding Volumes and Collisions](Math_CollisionDetectionOverview.md)  
[Collision Content Catalog at App Hub Online](http://go.microsoft.com/fwlink/?LinkId=128869)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team