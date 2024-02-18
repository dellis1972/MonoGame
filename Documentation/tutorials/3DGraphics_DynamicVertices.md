﻿

# Dynamically Updating Vertex Data

Describes techniques for dynamically updating vertex data in an MonoGame game.

Geometry in a 3D game is defined by vertex data. Sometimes, a game needs to modify vertex data or even generate new vertex data dynamically (at run time). Here are some solutions for dynamically updating vertex data.

# Updating a Set of Primitives Dynamically

The [Primitives Sample](http://go.microsoft.com/fwlink/?LinkId=93003) demonstrates a dynamic vertex buffer that is generated during each rendering frame. The sample renders primitives by first calling `Begin`, adding the necessary vertices, using the `Add` method, and then calling `End`. This forces the buffer to be drawn to the current device. The `Flush` method calls [DrawUserPrimitives](O_M_Microsoft_Xna_Framework_Graphics_GraphicsDevice_DrawUserPrimitives.md) method when `End` is called or when the buffer has no room for new vertices. If there is no room, the buffer is written to the device, it is reset, and the pending vertices are added.

# Dynamically Rendering a Persistent Set of Primitives

The [Particle 3D Sample](http://go.microsoft.com/fwlink/?LinkId=93004), implements a dynamic vertex buffer that contains custom vertices with a limited lifespan. The application adds and removes particles into a fixed length buffer. The custom shader of the sample renders the active subset of vertices dynamically. Because particles have a limited lifespan, the `ParticleSystem` class handles all adding, updating, and deleting of the vertex buffer in real time.

# Generating Geometry Programmatically

Sometimes, your game needs to generate geometry because the geometry is not known at design-time or it changes at run time. For this scenario, create a dynamic vertex and index buffer, and use [VertexBuffer.SetData](O_M_Microsoft_Xna_Framework_Graphics_VertexBuffer_SetData.md) and [IndexBuffer.SetData](O_M_Microsoft_Xna_Framework_Graphics_IndexBuffer_SetData.md) to set or change the data at run time.

# Remarks

Create a dynamic vertex or index buffer using [DynamicVertexBuffer](T_Microsoft_Xna_Framework_Graphics_DynamicVertexBuffer.md) and [DynamicIndexBuffer](T_Microsoft_Xna_Framework_Graphics_DynamicIndexBuffer.md) ; create a static vertex or index buffer using [VertexBuffer](T_Microsoft_Xna_Framework_Graphics_VertexBuffer.md) and [IndexBuffer](T_Microsoft_Xna_Framework_Graphics_IndexBuffer.md). Use a dynamic buffer for vertex data that is updated every render frame, otherwise, use a static buffer.

The samples are located on the App Hub Web site. For a more advanced solution for dynamic vertex updating, download the [Generated Geometry Sample](http://go.microsoft.com/fwlink/?LinkId=93007). This sample uses the [MeshBuilder](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_MeshBuilder.md) helper class and a custom processor to generate a terrain map from a bitmap loaded by the content manager. Specifically, examine the `Process` method, located in TerrainProcessor.cs, which programmatically creates the terrain geometry based on input from the specified bitmap.

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.

Version: 2.0.61024.0
