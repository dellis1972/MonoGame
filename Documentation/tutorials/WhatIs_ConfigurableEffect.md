

# What Is a Configurable Effect?

A configurable effect is an optimized rendering effect designed for Windows Phone. A configurable effect is created using a built-in object with options for user configuration.

An effect initializes the graphics pipeline for performing transforms, lighting, applying textures, and adding per-pixel visual effects such as a glow or a lens flare. Under the covers, an effect implements at least one shader for processing vertices and at least one shader for processing pixels.

During rendering, the graphics pipeline transforms 3D geometry to a 2D surface, giving you the option of adding lighting, texturing and many other per-vertex or per-pixel visual effects. An effect initializes the pipeline to render 3D geometry using vertex and pixel shaders, although you can also render a 2D sprite with an effect.

Although more advanced progammable effects are available on Windows and Xbox 360, use configurable effects on Windows Phone. There are several built-in configurable effects which have been designed to run efficiently on mobile GPU hardware, and which are appropriate to the Reach [profile](WhatIs_Profile.md) used for Windows Phone games.

Use one of the following configurable effects to implement these rendering effects:

*   [Basic Lighting and Fog](#ID4EAC)
*   [Character Animation](#ID4EOC)
*   [More Sophisticated Lighting with a Light Map](#ID4E2C)
*   [Billboards and Imposters](#ID4EKD)
*   [Lighting Highlights Using an Environment Map](#ID4EZD)

# Basic Lighting and Fog

Use the [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md) configurable effect to implement general purpose functionality, including the following: transformations; lighting with three directional lights; material colors using ambient, diffuse, and specular properties; a single texture; and fog. To improve speed, fog calculations are based on depth instead of distance from the camera. When you choose a basic effect, you can improve the performance of your game if you don't use any fog or if you only use one of the three available directional lights. For an example, see [Creating a Basic Effect](Use_BasicEffect.md).

# Character Animation

Use the [SkinnedEffect](T_Microsoft_Xna_Framework_Graphics_SkinnedEffect.md) configurable effect to animate a character. This effect uses bones and weights to transform a mesh (an object is made up of several meshes). Simply set up a set of bones for a model when you create content, and then transform the bones during the render loop. You can also use this class for hardware instancing by setting **WeightsPerVertex** to one, and replicating the geometry data with an additional bone index channel. This is similar to the way the shader instancing technique works in the instancing sample.

# More Sophisticated Lighting with a Light Map

Use the [DualTextureEffect](T_Microsoft_Xna_Framework_Graphics_DualTextureEffect.md) configurable effect with a prebaked radiosity lightmap to add more sophisticated lighting to a scene. This effect uses two textures, the base texture with the texture detail and an overlay texture with the prebaked lighting.

The two textures are combined using a fixed modulate2X blend formula as shown here:

      result.rgb = x.rgb * y.rgb * 2;
      result.a = x.a * y.a;
    

# Billboards and Imposters

Use the [AlphaTestEffect](T_Microsoft_Xna_Framework_Graphics_AlphaTestEffect.md) configurable effect to use alpha data to test whether to draw a pixel. The effect uses a **CompareFunction** to compare the alpha value for a pixel against the **ReferenceAlpha** value to determine whether to draw the pixel. This functionality is used for drawing a billboard (a 2D sprite that faces the camera) and an imposter (a 2D sprite that is integrated into a larger scene).

# Lighting Highlights Using an Environment Map

Use the [EnvironmentMapEffect](T_Microsoft_Xna_Framework_Graphics_EnvironmentMapEffect.md) configurable effect to generate fast, specular highlights that add shininess to an object. The effect uses two textures, a base texture with the texture detail and a cubemap whose six sides reflect the environment onto the object. Use **EnvironmentMapAmount** to control the amount of the environment map to add to the object. Also, use **FresnelFactor** to control how much the edge of an object reflects specular lighting.

Pseudo code for the lighting calculations looks similar to this:

      viewAngle = dot(eyeToVertexVector, vertexNormal);
      fresnel = saturate(pow(1 – abs(viewAngle), FresnelFactor);
      amount = fresnel * EnvironmentMapAmount;
      result.rgb = lerp(diffuseTexture.rgb, cubeTexture.rgb, amount);
      result.rgb += cubeTexture.a * EnvironmentMapSpecular
    

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team