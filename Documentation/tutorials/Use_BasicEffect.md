

# Creating a Basic Effect

Demonstrates how to create and initialize an instance of the [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md) class and use it to draw simple geometry.

![](note.gif)Note

The steps described here apply to effects created with the [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md) class. Use the [Effect](T_Microsoft_Xna_Framework_Graphics_Effect.md) class to write a custom effect. The example draws aliased geometry. To see an example that draws smoother edges because it also applies antialiasing, see [Enabling Antialiasing (Multisampling)](Enable_Anti_Aliasing.md).

![](graphics_use_basic_effect.jpg)

# The Complete Sample

You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download UseBasicEffect.zip](http://go.microsoft.com/fwlink/?LinkId=258739).

# Using BasicEffect

### To use BasicEffect

Using the basic effect class requires a set of world, view, and projection matrices, a vertex buffer, a vertex declaration, and an instance of the [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md) class.

1.  Declare these objects at the beginning of the game.
    
    ```
    Matrix worldMatrix;
    Matrix viewMatrix;
    Matrix projectionMatrix;
    VertexPositionNormalTexture[] cubeVertices;
    VertexDeclaration vertexDeclaration;
    VertexBuffer vertexBuffer;
    BasicEffect basicEffect;
    ```
    
2.  Initialize the world, view, and projection matrices.
    
    In this example, you create a world matrix that rotates the geometry 22.5 degrees along the x and y axes. The view matrix is a look-at matrix with a camera position at (0, 0, 5), pointing at the origin. The projection matrix is a perspective projection matrix based on a a 45-degree field of view, an aspect ratio equal to the client window, and a set of near and far planes.
    
    ```
    float tilt = MathHelper.ToRadians(0);  // 0 degree angle
    // Use the world matrix to tilt the cube along x and y axes.
    worldMatrix = Matrix.CreateRotationX(tilt) * Matrix.CreateRotationY(tilt);
    viewMatrix = Matrix.CreateLookAt(new Vector3(5, 5, 5), Vector3.Zero, Vector3.Up);
    
    projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
        MathHelper.ToRadians(45),  // 45 degree angle
        (float)GraphicsDevice.Viewport.Width /
        (float)GraphicsDevice.Viewport.Height,
        1.0f, 100.0f);
    ```
    
3.  Initialize [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md) with transformation and light values.
    
    ```
    basicEffect = new BasicEffect(graphics.GraphicsDevice);
    
    basicEffect.World = worldMatrix;
    basicEffect.View = viewMatrix;
    basicEffect.Projection = projectionMatrix;
    
    // primitive color
    basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
    basicEffect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
    basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
    basicEffect.SpecularPower = 5.0f;
    basicEffect.Alpha = 1.0f;
    
    basicEffect.LightingEnabled = true;
    if (basicEffect.LightingEnabled)
    {
        basicEffect.DirectionalLight0.Enabled = true; // enable each light individually
        if (basicEffect.DirectionalLight0.Enabled)
        {
            // x direction
            basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1, 0, 0); // range is 0 to 1
            basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1, 0, 0));
            // points from the light to the origin of the scene
            basicEffect.DirectionalLight0.SpecularColor = Vector3.One;
        }
    
        basicEffect.DirectionalLight1.Enabled = true;
        if (basicEffect.DirectionalLight1.Enabled)
        {
            // y direction
            basicEffect.DirectionalLight1.DiffuseColor = new Vector3(0, 0.75f, 0);
            basicEffect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(0, -1, 0));
            basicEffect.DirectionalLight1.SpecularColor = Vector3.One;
        }
    
        basicEffect.DirectionalLight2.Enabled = true;
        if (basicEffect.DirectionalLight2.Enabled)
        {
            // z direction
            basicEffect.DirectionalLight2.DiffuseColor = new Vector3(0, 0, 0.5f);
            basicEffect.DirectionalLight2.Direction = Vector3.Normalize(new Vector3(0, 0, -1));
            basicEffect.DirectionalLight2.SpecularColor = Vector3.One;
        }
    }
    ```
    
4.  Create a vertex declaration for the type [VertexPositionNormalTexture](T_Microsoft_Xna_Framework_Graphics_VertexPositionNormalTexture.md).
    
    *   If lighting is enabled, the vertex must have a normal type.
        
    *   If vertex colors are enabled, the vertex must have colors.
        
    *   If texturing is enabled, the vertex must have a texture coordinate.
        
    
    ```
    vertexDeclaration = new VertexDeclaration(new VertexElement[]
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        }
    );
    ```
    
5.  Create the per vertex data. This example shows the data for one face of the cube.
    
    ```
    Vector3 topLeftFront = new Vector3(-1.0f, 1.0f, 1.0f);
    Vector3 bottomLeftFront = new Vector3(-1.0f, -1.0f, 1.0f);
    Vector3 topRightFront = new Vector3(1.0f, 1.0f, 1.0f);
    Vector3 bottomRightFront = new Vector3(1.0f, -1.0f, 1.0f);
    Vector3 topLeftBack = new Vector3(-1.0f, 1.0f, -1.0f);
    Vector3 topRightBack = new Vector3(1.0f, 1.0f, -1.0f);
    Vector3 bottomLeftBack = new Vector3(-1.0f, -1.0f, -1.0f);
    Vector3 bottomRightBack = new Vector3(1.0f, -1.0f, -1.0f);
    
    Vector2 textureTopLeft = new Vector2(0.0f, 0.0f);
    Vector2 textureTopRight = new Vector2(1.0f, 0.0f);
    Vector2 textureBottomLeft = new Vector2(0.0f, 1.0f);
    Vector2 textureBottomRight = new Vector2(1.0f, 1.0f);
    
    Vector3 frontNormal = new Vector3(0.0f, 0.0f, 1.0f);
    Vector3 backNormal = new Vector3(0.0f, 0.0f, -1.0f);
    Vector3 topNormal = new Vector3(0.0f, 1.0f, 0.0f);
    Vector3 bottomNormal = new Vector3(0.0f, -1.0f, 0.0f);
    Vector3 leftNormal = new Vector3(-1.0f, 0.0f, 0.0f);
    Vector3 rightNormal = new Vector3(1.0f, 0.0f, 0.0f);
    
    // Front face.
    cubeVertices[0] =
        new VertexPositionNormalTexture(
        topLeftFront, frontNormal, textureTopLeft);
    cubeVertices[1] =
        new VertexPositionNormalTexture(
        bottomLeftFront, frontNormal, textureBottomLeft);
    cubeVertices[2] =
        new VertexPositionNormalTexture(
        topRightFront, frontNormal, textureTopRight);
    cubeVertices[3] =
        new VertexPositionNormalTexture(
        bottomLeftFront, frontNormal, textureBottomLeft);
    cubeVertices[4] =
        new VertexPositionNormalTexture(
        bottomRightFront, frontNormal, textureBottomRight);
    cubeVertices[5] =
        new VertexPositionNormalTexture(
        topRightFront, frontNormal, textureTopRight);
    ```
    
6.  Call [GraphicsDevice.Clear](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_Clear.md) to clear the render target.
    
7.  Set the rasterizer state to turn off culling using the [RasterizerState](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_RasterizerState.md) property.
    
8.  Call [EffectPass.Apply](M_Microsoft_Xna_Framework_Graphics_EffectPass_Apply.md) to set the effect state in preparation for rendering.
    
9.  Draw the geometry by calling [GraphicsDevice.DrawPrimitives](M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawPrimitives.md).
    
    ```
    graphics.GraphicsDevice.Clear(Color.SteelBlue);
    
    RasterizerState rasterizerState1 = new RasterizerState();
    rasterizerState1.CullMode = CullMode.None;
    graphics.GraphicsDevice.RasterizerState = rasterizerState1;
    foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
    {
        pass.Apply();
    
        graphics.GraphicsDevice.DrawPrimitives(
            PrimitiveType.TriangleList,
            0,
            12
        );
    }
    
    
    base.Draw(gameTime);
    ```
    

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team