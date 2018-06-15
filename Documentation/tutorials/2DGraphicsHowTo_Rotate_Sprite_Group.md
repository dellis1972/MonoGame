

# Rotating a Group of Sprites

Demonstrates how to rotate a group of sprites around a single point.

# The Complete Sample

The code in the topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download RotateSpriteGroup_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258721).

# Drawing a Rotated Group of Sprites

### To draw a rotated group of sprites on screen

1.  Follow the steps of [Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md).
    
2.  Create one set of [Vector2](T_Microsoft_Xna_Framework_Vector2.md) objects that represents the unrotated positions of the sprites and one set to hold the rotated values.
    
    ```
    private Vector2[] myVectors;
    private Vector2[] drawVectors;
    protected override void Initialize()
    {
        myVectors = new Vector2[9];
        drawVectors = new Vector2[9];
    
        base.Initialize();
    }
    ```
                        
    
3.  After loading the sprite, calculate the positions of the unrotated group of sprites based on the sprite's size.
    
    ```
    private Texture2D SpriteTexture;
    private Vector2 origin;
    private Vector2 screenpos;
    protected override void LoadContent()
    {
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
        SpriteTexture = Content.Load<Texture2D>("ship");
        origin.X = SpriteTexture.Width / 2;
        origin.Y = SpriteTexture.Height / 2;
        Viewport viewport = graphics.GraphicsDevice.Viewport;
        screenpos.X = viewport.Width / 2;
        screenpos.Y = viewport.Height / 2;
    }
    ```
                        
    
4.  In your [Update](M_Microsoft_Xna_Framework_Game_Update.md) method, copy the unrotated vectors and determine the screen position around which all the sprites will rotate.
    
    ```
    private float RotationAngle = 0f;
    private Matrix rotationMatrix = Matrix.Identity;
    protected override void Update(GameTime gameTime)
    {
        ...
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
    
        RotationAngle += elapsed;
        float circle = MathHelper.Pi * 2;
        RotationAngle = RotationAngle % circle;
    
        // Copy and rotate the sprite positions.
        drawVectors = (Vector2[])myVectors.Clone();
    
        RotatePoints(ref screenpos, RotationAngle, ref drawVectors);
    
        base.Update(gameTime);
    }
    ```
                        
    
    Transform each vector using a rotation matrix created for the rotation angle.
    
5.  To rotate around the origin, transform each vector relative to the origin by subtracting the origin vector.
    
6.  Add the origin vector to the transformed vector to create the final rotated vector.
    
    ```
    private static void RotatePoints(ref Vector2 origin, float radians, ref Vector2[] Vectors)
    {
        Matrix myRotationMatrix = Matrix.CreateRotationZ(radians);
    
        for (int i = 0; i < 9; i++)
        {
            // Rotate relative to origin.
            Vector2 rotatedVector =
                Vector2.Transform(Vectors[i] - origin, myRotationMatrix);
    
            // Add origin to get final location.
            Vectors[i] = rotatedVector + origin;
        }
    }
    ```
                        
    
7.  Draw each sprite using the rotated vectors as screen locations.
    
    ```
    private void DrawPoints()
    {
        // Draw using manually rotated vectors
        spriteBatch.Begin();
        for (int i = 0; i < drawVectors.Length; i++)
            spriteBatch.Draw(SpriteTexture, drawVectors[i], null,
                Color.White, RotationAngle, origin, 1.0f,
                SpriteEffects.None, 0f);
        spriteBatch.End();
    }
    ```
                        
    
8.  When all the sprites have been drawn, call [End](M_Microsoft_Xna_Framework_Graphics_SpriteBatch_End.md).
    

# See Also

#### Tasks

[Drawing a Sprite](2DGraphicsHowTo_Draw_Sprite.md)  

#### Concepts

[What Is a Sprite?](Sprite_Overview.md)  

#### Reference

[SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md)  
[Draw](O_M_Microsoft_Xna_Framework_Graphics_SpriteBatch_Draw.md)  
[Texture2D](T_Microsoft_Xna_Framework_Graphics_Texture2D.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
