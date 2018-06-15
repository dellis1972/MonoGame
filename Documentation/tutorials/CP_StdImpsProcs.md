

# Standard Content Importers and Content Processors

Describes the standard Content Pipeline Content Importers and Content Processors of MonoGame that support various common art asset file formats.

Content Importers and Content Processors are implemented as assemblies. In addition to the standard ones provided by MonoGame and listed below, you can also use custom Content Importers and Content Processors developed by you or third parties. Use the Properties window to assign an appropriate Content Importer and Content Processor to each game asset added to your game project (see [Using dialog properties](UsingMonoGame_Dlg_Properties.md) for more information).

# Standard Content Importers

The following is a description of the standard Content Importers shipped with MonoGame and the type of game asset each supports.

All standard Content Importers are declared as part of the Microsoft.Xna.Framework.Content.Pipeline namespace.

| Name | Type name | Output type | Description
| ------------- |:-------------:| :----- | :---- |
| Autodesk FBX|FbxImporter|[NodeContent](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_NodeContent.md)|Imports game assets specified with the Autodesk FBX file format (.fbx). This Content Importer is designed to work with assets exported with the 2013 version of the FBX exporter. |
| Effect|EffectImporter|[EffectContent](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_EffectContent.md)|Imports a game asset specified with the DirectX Effect file format (.fx). |
| Sprite Font Description|FontDescriptionImporter|[FontDescription](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_FontDescription.md)|Imports a font description specified in a .spritefont file.|
| Texture|TextureImporter|[TextureContent](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_TextureContent.md)|Imports a texture. These file types are supported: .bmp, .dds, .dib, .hdr, .jpg, .pfm, .png, .ppm, and .tga.|
| X File|XImporter|[NodeContent](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_NodeContent.md)|Imports game assets specified with the DirectX X file format (.x). This Content Importer expects the coordinate system to be left-sided.|
| XML Content|XmlImporter|object|Imports XML content used for editing the values of a custom object at run time. For instance, you could pass XML code to this Content Importer that looks for the specified property of a custom type and changes it to the specified value. You could then process the custom object with a Content Processor or pass it to your game untouched using the No Processing Required Content Processor.This Content Importer is designed for scenarios like importing an XML file that describes game data at run time (similar to the Sprite Font Description Content Importer) or importing terrain data in an XML file that then is passed to a Content Processor that generates a random terrain grid using that data.|
| Other 3D Content|OpenAssetImporter|[NodeContent](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_NodeContent.md)|Imports game assets specified with one of the formats supported by assimp. A sample of supported files types are: .dae, .3ds, .blend, .obj, .fbx (v2013). More are available see [Assimp Supported File Formats](https://github.com/assimp/assimp#supported-file-formats) for more details. Note some formats might not behave correctly with the standard [ModelProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_ModelProcessor.md).|

# Standard Content Processors

MonoGame ships with a variety of Content Processors that support several common game asset types. Many of the standard Content Processors, such as the [TextureProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_TextureProcessor.md), support parameters for modifying the default behavior of the Content Processor. For more information, see [Parameterized Content Processors](CP_StdParamProcs.md).

The following describes the standard Content Processors and the type of game asset each supports.

| Name| Type name| Input type| Output type| Description|
| ----------------- |:-------------:| :----- | :---- | :---- |
| Model|[ModelProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_ModelProcessor.md)|[NodeContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_NodeContent.md)|[ModelContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_ModelContent.md)|A parameterized Content Processor that outputs models as a [ModelContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_ModelContent.md) object.<br>Available parameters:<br>*   Color Key Color–Any valid [Color](T_MXF_Color.md). [Magenta](T_MXF_Color.md) is the default value.<br>*   Color Key Enabled–A Boolean value indicating if color keying is enabled. The default value is **true**.<br>*   Generate Mipmaps–A Boolean value indicating if mipmaps are generated. The default value is **false**.<br>*   Generate Tangent Frames–A Boolean value indicating if tangent frames are generated. The default value is **false**.<br>*   Resize Textures to Power of Two–A Boolean value indicating if a texture is resized to the next largest power of 2. The default value is **false**.<br>*   Scale–Any valid [float](http://msdn.microsoft.com/en-us/library/system.single.aspx) value. The default value is 1.0.<br>*   Swap Winding Order–A Boolean value indicating if the winding order is swapped. This is useful for models that appear to be drawn inside out. The default value is **false**.<br>*   Texture Format–Any valid [SurfaceFormat](T_Microsoft_Xna_Framework_Graphics_SurfaceFormat.md) value. Textures are either unchanged, converted to the Color format, or DXT Compressed. For more information, see [TextureProcessorOutputFormat](T_MXFCPP_TextureProcessorOutputFormat.md).<br>*   X Axis Rotation–Number, in degrees of rotation. The default value is 0.<br>*   Y Axis Rotation–Number, in degrees of rotation. The default value is 0.<br>*   Z Axis Rotation–Number, in degrees of rotation. The default value is 0.|
|No Processing Required|[PassThroughProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_PassThroughProcessor.md)|Object|Object|Performs no processing on the file. Select this Content Processor if your content is already in a game-ready format (for example, an externally prepared DDS file) or a specialized XML format (.xml) designed for use with XNA Game Studio.|
|Sprite Font Description|[FontDescriptionProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_FontDescriptionProcessor.md)|[FontDescription](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_FontDescription.md)|[SpriteFontContent](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_SpriteFontContent.md)|Converts a .spritefont file specifying a font description into a font.|
|Sprite Font Texture|[FontTextureProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_FontTextureProcessor.md)|[TextureContent](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_TextureContent.md)|[SpriteFontContent](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_SpriteFontContent.md)|A parameterized Content Processor that outputs a sprite font texture as a [SpriteFontContent](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_SpriteFontContent.md) object.<br>Available parameters:<br>*   First Character–Any valid character. The space character is the default value.|
| Sprite Font Texture|[FontTextureProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_FontTextureProcessor.md)|[Texture2DContent](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_Texture2DContent.md)|[SpriteFontContent](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_SpriteFontContent.md)|Converts a specially marked 2D bitmap file (.bmp) into a font. Pixels of **Color.Magenta** are converted to **Color.Transparent**.|
| Texture|[TextureProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_TextureProcessor.md)|[TextureContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_TextureContent.md)|[TextureContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_TextureContent.md)|A parameterized Content Processor that outputs textures as a [TextureContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_TextureContent.md) object.<br>Available parameters:<br>*   Color Key Color–Any valid [Color](T_MXF_Color.md). [Magenta](T_MXF_Color.md) is the default value.<br>*   Color Key Enabled–A Boolean value indicating if color keying is enabled. The default value is **true**.<br>*   Generate Mipmaps–A Boolean value indicating if mipmaps are generated. The default value is **false**.<br>*   Resize to Power of Two–A Boolean value indicating if a texture is resized to the next largest power of 2. The default value is **false**.<br>*   Texture Format–Any valid [SurfaceFormat](T_Microsoft_Xna_Framework_Graphics_SurfaceFormat.md) value. Textures are either unchanged, converted to the Color format, or DXT Compressed. For more information, see [TextureProcessorOutputFormat](T_MXFCPP_TextureProcessorOutputFormat.md).|
|Localized Sprite Font Texture|[LocalizedFontProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_LocalizedFontDescriptionProcessor.md)|[FontDescription](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_FontDescription.md)|[SpriteFontContent](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_SpriteFontContent.md)|Converts a .spritefont file specifying a font description into a font.|

# See Also

[Adding Content to a Game](CP_TopLevel.md)  
[What Is Content?](CP_Overview.md)  
[Adding a Custom Importer](CP_AddCustomProcImp.md)  

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.
