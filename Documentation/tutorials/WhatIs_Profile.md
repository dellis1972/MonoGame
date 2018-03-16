

# What Is a Profile?

A profile is a feature set that is implemented in hardware. The Reach profile implements High Level Shader Language (HLSL) Shader Model 2.0 and the HiDef profile implements HLSL Shader Model 3.0.

![](note.gif)Note

The Reach profile is the only profile available for Windows Phone game development. Nevertheless, this topic provides background information about the purpose of XNA Game Studio profiles and the differences between the Reach and HiDef profiles.

Prior to XNA Game Studio 4.0, the XNA Framework exposed feature sets and behaviors that varied for each platform (Windows and Xbox 360). These feature sets and behaviors were implemented at runtime as a common core of functionality on all platforms, as well as platform-specific functionality. This required platform-dependent code to be developed and tested for each platform.

To simplify multiplatform development, XNA Game Studio creates a profile. A profile is platform independent so you do not need to query for capability bits. The APIs for accessing the features implemented in hardware are consistent across platforms so that game code written for one platform will compile and run on another platform with little or no changes. A game will not run if a hardware device does not meet the profile requirements.

You can set the profile at design time by using the XNA Game Studio property page in Microsoft Visual Studio 2012 Express for Windows Phone or you can set a profile at runtime by using the [GraphicsProfile](P_Microsoft_Xna_Framework_GraphicsDeviceManager_GraphicsProfile.md) property. For more information, see [Selecting Reach vs. HiDef](http://blogs.msdn.com/b/shawnhar/archive/2010/07/19/selecting-reach-vs-hidef.aspx).

There are two profiles, HiDef and Reach: the former for fully featured, high-powered hardware, and the latter for less featured, available-everywhere hardware. Reach is designed to broadly cover all platforms and has a limited set of graphic features and capabilities implemented in hardware. This profile is designed to support the widest variety of devices, more specifically Windows-based computers, an Xbox 360, and Windows Phone. Reach dramatically speeds up writing multiplatform games because you can design and debug game code on one platform knowing that it will run on other platforms. Performance is dependent on the hardware available for each platform.

The HiDef profile is designed for the highest performance and largest available set of graphic features. Use the HiDef profile to target hardware with more enhanced graphic capabilities such as an Xbox 360 and a Windows-based computer with at least a DirectX 10 GPU. More specifically, the HiDef profile requires a GPU with Xbox 360-level capabilities such as multiple render targets (MRT), floating-point surface formats, and per-vertex texture fetching. These are optional capabilities in DirectX 9 hardware, but all are required to support HiDef. Since DirectX 9 graphic cards are not required to support these features, it is easier to say that the HiDef profile requires at least a DirectX 10-capable GPU. A HiDef game will run on a DirectX 9 card if it implements the named DirectX 10 features.

If you try to run a HiDef game on a device such as a Windows Phone device) that does not support HiDef, an exception is thrown at runtime. Additionally, if you try to access HiDef features from a game built for a Reach profile, the runtime throws an exception. To find out which profile your target hardware supports, call [GraphicsAdapter.IsProfileSupported Method](M_Microsoft_Xna_Framework_Graphics_GraphicsAdapter_IsProfileSupported.md).

# Reach vs. HiDef Comparison

Differences between the Reach and HiDef profiles are presented next. For more detail, see the sections following the table.

Profiles

Reach

HiDef

Platforms

Windows Phone, Xbox 360, and any computer running Windows with a DirectX 9 GPU that supports at least Shader Model 2.0.

Xbox 360, and any Windows-based PC with at least a DirectX 10 (or equivalent) GPU. See the paragraph above for more detail.

Shader Model

2.0 (Windows Phone, however, does not support custom shaders.)

3.0 (Xbox 360 supports custom shader extensions such as vfetch that are unavailable on Windows.)

Maximum texture size

2,048

4,096

Maximum cube map size

512

4,096

Maximum volume texture size

Volume textures are not supported.

256

Nonpower of two textures

Yes with limitations: no wrap addressing mode, no mipmaps, no DXT compression on nonpower of two textures.

Yes

Nonpower of two cube maps

No

Yes

Nonpower of two volume textures

Volume textures are not supported.

Yes

Maximum number of primitives per draw call

65,535

1,048,575

Maximum number of vertex streams

16

16

Maximum vertex stream stride

25

255

Index buffer formats

16 bit

16 and 32 bit

Vertex element formats

Color, Byte4, Single, Vector2, Vector3, Vector4, Short2, Short4, NormalizedShort2, NormalizedShort4

All of the Reach vertex element formats, as well as HalfVector2 and HalfVector4.

Texture formats

Color, Bgr565, Bgra5551, Bgra4444, NormalizedByte2, NormalizedByte4, Dxt1, Dxt3, Dxt5

All of the Reach texture formats, as well as Alpha8, Rg32, Rgba64, Rgba1010102, Single, Vector2, Vector4, HalfSingle, HalfVector2, HalfVector4. Floating point texture formats do not support filtering.

Vertex texture formats

Vertex texturing is not supported.

Single, Vector2, Vector4, HalfSingle, HalfVector2, HalfVector4

Render target formats

Call **QueryRenderTargetFormat** to find out what is supported.

Call **QueryRenderTargetFormat** to find out what is supported.

Multiple render targets

No

Up to four. All must have the same bit depth. Alpha blending and independent write masks per render target are supported.

Occlusion queries

No

Yes

Separate alpha blend

No

Yes

# Shader Model

The Reach profile supports HLSL Shader Model 2.0 and the configurable effects for all platforms. Windows Phone does not support custom shaders.

The HiDef profile supports HLSL Shader Model 3.0. Xbox 360 also supports custom shader extensions such as vertex fetching (vfetch).

# Textures

Texture size limitations are listed in the table comparing the two profiles. These numbers are the maximum width or height of a texture that can be consumed by the profile. HiDef supports larger textures and volume textures.

HiDef supports the following features without limitations; Reach supports the following features only for power-of-two textures:

*   The **wrap** texture addressing mode
*   Mipmaps
*   DXT compression

# Formats

Many older formats from previous versions of XNA Game Studio were removed from Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0.

*   These vertex element formats no longer are supported:
    
    *   Rg32
    *   Rgba32
    *   Rgba64
    *   UInt101010
    *   Normalized101010
*   These texture formats no longer are supported:
    
    *   Dxt2
    *   Dxt4
    *   Bgr555
    *   Bgr444
    *   Bgra2338
    *   Bgr233
    *   Bgr24
    *   Bgr32
    *   Bgra1010102
    *   Rgba32
    *   Rgb32
    *   NormalizedShort2
    *   NormalizedShort4
    *   Luminance8
    *   Luminance16
    *   LuminanceAlpha8
    *   LuminanceAlpha16
    *   Palette8
    *   PaletteAlpha16
    *   NormalizedLuminance16
    *   NormalizedLuminance32
    *   NormalizedAlpha1010102
    *   NormalizedByte2Computed
    *   VideoYuYv
    *   Video UyVy
    *   VideoGrGb
    *   VideoRgBg
    *   Multi2Bgra32

There are a wide variety of render target formats. Call [GraphicsAdapter.QueryRenderTargetFormat Method](M_Microsoft_Xna_Framework_Graphics_GraphicsAdapter_QueryRenderTargetFormat.md) to find out what is supported for your hardware. The runtime also has a built-in fallback mechanism if the format you request is unavailable. The format parameters used when creating render targets and back buffers are now named "preferredFormat" instead of "format." The runtime will try to create a resource with the exact format passed in, and will fallback to the closest possible match (based on similar bit depth, number of channels, and so on) if that format is unavailable. For example, if you run a Reach game on a Windows Phone device using a 16-bit render target format and then run the game on an Xbox 360 that does not support 16-bit render targets, the runtime will switch the render-target format to a [Color](T_MXF_Color.md) format.

Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0 changes the [Color](T_MXF_Color.md) structure from BGRA to RGBA byte ordering. Most games will never notice the change because both the [Color](T_MXF_Color.md) structure and the texture and vertex declaration creation code were changed. If you have code that creates [Color](T_MXF_Color.md) format textures directly, setting their contents from a byte array rather than a strongly typed **Color\[\]**, you will need to swap the order of your red and blue channels.

# Conclusion

XNA Game Studio has introduced the Reach profile for Xbox 360, Windows, and Windows Phone, and the HiDef profile for Xbox 360 and Windows devices. The HiDef profile includes a strict superset of the functionality in the Reach profile, which means that HiDef implements all Reach functionality and more. If you run a Reach game on a HiDef platform, the framework will enforce Reach rules. Use this to your advantage when you develop a multiplatform game. You can design and debug on the most powerful hardware with the knowledge that the game will compile and run on the other less powerful platforms when you are ready to test your game. The runtime will throw an exception if you try to set the profile to HiDef on hardware that does not support HiDef, or if you are running a Reach profile game that tries to access HiDef features.

# Cited Works

"Selecting Reach vs. HiDef." Shawn Hargreaves Blog. July 2010. [http://blogs.msdn.com/b/shawnhar/archive/2010/07/19/selecting-reach-vs-hidef.aspx](http://blogs.msdn.com/b/shawnhar/archive/2010/07/19/selecting-reach-vs-hidef.aspx) (Last accessed August 2, 2010)

# See Also

#### Reference

[XNA Game Studio Properties Page](UsingXNA_Dlg_GameStudioProperties.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0