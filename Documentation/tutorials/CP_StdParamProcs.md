

# Parameterized Content Processors

Describes how parameterized Content Processors work in MonoGame. Many of the standard Content Pipeline Content Processors shipped with XNA Game Studio support parameter usage. Parameterization makes any standard or custom Content Processor more flexible and better able to meet the needs of your XNA Framework application. In addition to specifying values for standard parameters, you can easily implement parameter support for a new or an existing custom Content Processor. For more information, see [Developing with Parameterized Processors](CP_CustomParamProcs.md).

When you select a game asset, the Properties window displays the parameters for the related Content Processor. Use the Properties window at any time to modify these parameter values.

![](note.gif)Note

If you change the Content Processor for a game asset to a different Content Processor, all parameter values are reset to their default values. This means that if you modify the **Generate Mipmaps** parameter value for the [TextureProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_TextureProcessor.md) and then switch to a different Content Processor (for example, [FontTextureProcessor Class](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_FontTextureProcessor.md)), the parameters switch to the default values for that Content Processor. If you then switch back again, the modified values are reset to the default values of the original Content Processor. _The values do not revert to the modified values you set originally_.

# Standard Parameterized Content Processors

The following describes only standard Content Processors that accept parameters, the parameter types, and their default value. For more information about all standard Content Processors, see [Standard Content Importers and Content Processors](CP_StdImpsProcs.md).

| Friendly name| Type name | Input type | Output type | Description |
|--------------|-----------|------------|-------------|-------------|
| Model | [ModelProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_ModelProcessor.md)| [NodeContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_NodeContent.md)| [ModelContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_ModelContent.md) |A parameterized Content Processor that outputs models as a [ModelContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_ModelContent.md) object.<br>Available parameters:<br>*   Color Key Color–Any valid [Color](T_MXF_Color.md). [Magenta](T_MXF_Color.md) is the default value.<br>*   Color Key Enabled–A Boolean value indicating if color keying is enabled. The default value is **true**.<br>*   Generate Mipmaps–A Boolean value indicating if mipmaps are generated. The default value is **false**.<br>*   Generate Tangent Frames–A Boolean value indicating if tangent frames are generated. The default value is **false**.<br>*   Resize Textures to Power of Two–A Boolean value indicating if a texture is resized to the next largest power of 2. The default value is **false**.<br>*   Scale–Any valid [float](http://msdn.microsoft.com/en-us/library/system.single.aspx) value. The default value is 1.0.<br>*   Swap Winding Order–A Boolean value indicating if the winding order is swapped. This is useful for models that appear to be drawn inside out. The default value is **false**.<br>*   Texture Format–Any valid value from [TextureProcessorOutputFormat](T_MXFCPP_TextureProcessorOutputFormat.md). Textures are either unchanged, converted to the Color format, or Compressed using the specified Compression algorithm.<br>*   X Axis Rotation–Number, in degrees of rotation. The default value is 0.<br>*   Y Axis Rotation–Number, in degrees of rotation. The default value is 0.<br>*   Z Axis Rotation–Number, in degrees of rotation. The default value is 0.
| Sprite Font Texture|[FontTextureProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_FontTextureProcessor.md)|[TextureContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_TextureContent.md)|[SpriteFontContent](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_SpriteFontContent.md)|A parameterized Content Processor that outputs a sprite font texture as a [SpriteFontContent](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_SpriteFontContent.md) object.<br>Available parameters:<br><br>*   First Character–Any valid character. The space character is the default value.
| Texture|[TextureProcessor](T_Microsoft_Xna_Framework_Content_Pipeline_Processors_TextureProcessor.md)|[TextureContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_TextureContent.md)|[TextureContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_TextureContent.md)|A parameterized Content Processor that outputs textures as a [TextureContent Class](T_Microsoft_Xna_Framework_Content_Pipeline_Graphics_TextureContent.md) object.<br>Available parameters:<br><br>*   Color Key Color–Any valid [Color](T_MXF_Color.md). [Magenta](T_MXF_Color.md) is the default value.<br>*   Color Key Enabled–A Boolean value indicating if color keying is enabled. The default value is **true**.<br>*   Generate Mipmaps–A Boolean value indicating if mipmaps are generated. The default value is **false**.<br>*   Resize to Power of Two–A Boolean value indicating if a texture is resized to the next largest power of 2. The default value is **false**.<br>*   Texture Format–Any valid value from [TextureProcessorOutputFormat](T_MXFCPP_TextureProcessorOutputFormat.md). Textures are unchanged, converted to the **Color** format, or Compressed using the specified Compression algorithm.

# See Also

[Adding Content to a Game](CP_TopLevel.md)  

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.
