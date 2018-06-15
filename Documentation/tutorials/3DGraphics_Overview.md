

# 3D Pipeline Basics

The 3D graphics pipeline uses a graphics device to load resources and render a 3D scene using an effect.

In general, the 3D pipeline requires the following state for initialization:

*   World, view, and projection transforms to transform 3D vertices into a 2D space.
*   A vertex buffer which contains the geometry to render.
*   An effect that sets the render state necessary for drawing the geometry.

As you become comfortable with these ideas, you may want to learn more about the following: manipulating vertices, creating your own effects, applying textures, or improving performance by using index buffers.

The MonoGame Framework uses a shader-driven programmable pipeline. It requires a graphics card capable of at least Shader Model 2.0, but requirements depend on the platform being targeted. The MonoGame Framework provides a class called [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md) that encapsulates most of these common operations.

# The Graphics Device

When you create a game with MonoGame, the framework initializes a graphics device for you.

The [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) initializes the [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md) before you call [Game.Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md). Before you call [Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md), there are three ways to change the [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md) settings:

1.  Set the appropriate properties (e.g. [PreferredBackBufferHeight](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferHeight.md), [PreferredBackBufferWidth](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferredBackBufferWidth.md)) on the [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) in your game's constructor.
    
2.  Handle the [PreparingDeviceSettings](E_Microsoft_Xna_Framework_GraphicsDeviceManager_PreparingDeviceSettings.md) event on the [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md), and change the [PreparingDeviceSettingsEventArgs.GraphicsDeviceInformation.PresentationParameters](T_Microsoft_Xna_Framework_Graphics_PresentationParameters.md) member properties.
    
    Any changes made to the [PreparingDeviceSettingsEventArgs](T_Microsoft_Xna_Framework_PreparingDeviceSettingsEventArgs.md) will override the [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) preferred settings.
    
3.  Handle the [DeviceCreated](E_Microsoft_Xna_Framework_GraphicsDeviceManager_DeviceCreated.md) event on the [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md), and change the [PresentationParameters](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_PresentationParameters.md) of the [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md) directly.
    

When you call [Game.Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md), [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) creates and configures [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md). You can safely access [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md) settings such as the backbuffer, depth/stencil buffer, viewport, and render states in [Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md).

After you call [Game.Initialize](M_Microsoft_Xna_Framework_Game_Initialize.md), changes to the [PresentationParameters](P_Microsoft_Xna_Framework_Graphics_GraphicsDevice_PresentationParameters.md) of the [GraphicsDevice](T_Microsoft_Xna_Framework_Graphics_GraphicsDevice.md) will not take effect until you call [GraphicsDeviceManager.ApplyChanges](M_Microsoft_Xna_Framework_GraphicsDeviceManager_ApplyChanges.md). Other changes, such as render states, will take effect immediately.

# Resources

A resource is a collection of data stored in memory that can be accessed by the CPU or GPU. Types of resources that an application might use include render targets, vertex buffers, index buffers, and textures.

Based on the resource management mode that was used when a resource is created, it should be reloaded when the device is reset. For more information, see [Loading Resources](AppModel_HowTo_LoadResources.md).

## Vertex and Index Buffers

A vertex buffer contains a list of 3D vertices to be streamed to the graphics device. Each vertex in a vertex buffer may contain data about not only the 3D coordinate of the vertex, but also other information describing the vertex, such as the vertex normal, color, or texture coordinate. The MonoGame Framework contains several classes to describe common vertex declaration types, such as [VertexPositionColor](T_Microsoft_Xna_Framework_Graphics_VertexPositionColor.md), [VertexPositionColorTexture](T_Microsoft_Xna_Framework_Graphics_VertexPositionColorTexture.md), [VertexPositionNormalTexture](T_Microsoft_Xna_Framework_Graphics_VertexPositionNormalTexture.md), and [VertexPositionTexture](T_Microsoft_Xna_Framework_Graphics_VertexPositionTexture.md). Use the [VertexElement](T_Microsoft_Xna_Framework_Graphics_VertexElement.md) class to compose custom vertex types.

Vertex buffers contain indexed or non-indexed vertex data.

If a vertex buffer is not indexed, all of the vertices are placed in the vertex buffer in the order they are to be rendered. Because 3D line lists or triangle lists often reference the same vertices multiple times, this can result in a large amount of redundant data.

Index buffers allow you to list each vertex only once in the vertex buffer. An index buffer is a list of indices into the vertex buffer, given in the order that you want the vertices to render.

To render a non-indexed vertex buffer, call the [GraphicsDevice.DrawPrimitives Method](M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawPrimitives.md) or [GraphicsDevice.DrawUserPrimitives Method](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserPrimitives.md). To render an indexed buffer, call the [GraphicsDevice.DrawIndexedPrimitives Method](M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawIndexedPrimitives.md) or [GraphicsDevice.DrawUserIndexedPrimitives Method](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserIndexedPrimitives.md).

## Textures

A texture resource is a structured collection of texture data. The data in a texture resource is made up of one or more subresources that are organized into arrays and mipmap chains. Textures are filtered by a texture sampler as they are read. The type of texture influences how the texture is filtered.

You can apply textures by using the [Texture](P_Microsoft_Xna_Framework_Graphics_BasicEffect_Texture.md) property of the [BasicEffect](T_Microsoft_Xna_Framework_Graphics_BasicEffect.md) class, or choose to write your own effect methods to apply textures.

# See Also

#### Concepts

[Getting Started with 3D Games at App Hub Online](http://go.microsoft.com/fwlink/?LinkId=128882)  
[Shader Content Catalog at App Hub Online](http://go.microsoft.com/fwlink/?LinkId=128870)  

© 2012 Microsoft Corporation. All rights reserved. 

© The MonoGame Team.
