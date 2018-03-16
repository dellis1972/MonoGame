

# What Is Antialiasing?

Antialiasing is a technique for softening or blurring sharp edges so they appear less jagged when rendered.

Antialiasing is accomplished by multisampling each pixel at multiple pixel locations and combining the samples to generate a final pixel color. Increasing the number of samples per pixel increases the amount of antialiasing which generates a smoother edge. 4x multisampling requires four samples per pixel and 2x multisampling requires two sampler per pixel. Use the [MultiSampleCount](P_Microsoft_Xna_Framework_Graphics_PresentationParameters_MultiSampleCount.md) property of the [PresentationParameters](T_Microsoft_Xna_Framework_Graphics_PresentationParameters.md) class to set the number of samples for the back buffer.

Set the [PreferMultiSampling](P_Microsoft_Xna_Framework_GraphicsDeviceManager_PreferMultiSampling.md) property on the [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) class to **true** to enable multisampling for the back buffer. This will be ignored if the hardware does not support multisampling.

# See Also

[Enabling Antialiasing (Multisampling)](Enable_Anti_Aliasing.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0