

# Working with Microphones

This topic provides basic information about microphone usage in games.

# Supported Microphone Devices

The Microphone API is only implelemented on OpenAL based platforms at this time. This includes iOS/Android and Desktop projects using DesktopGL.

# Capabilities of the Microphone API

The MonoGame Microphone API has the following functionality:

*   Captures the audio stream from a microphone.
*   Submits and controls a stream of audio buffers for playback using the [DynamicSoundEffectInstance](T_MXFA_DynamicSoundEffectInstance.md) object.
*   Plays back audio.

# Microphone API Process Workflow

The Microphone API behaves like a simple audio recorder with a configurable capture buffer. It has been designed with the following development process workflow:

1.  Select the microphone connected to the device.
2.  Configure the microphone's capture buffer size.
3.  Control the recording using standard transport controls (Start and Stop).
4.  Retrieve the captured audio using the [GetData](O_M_MXFA_Microphone_GetData.md) method.

Also, you can use the [BufferReady](E_MXFA_Microphone_BufferReady.md) event handler if you want to be notified when the audio capture buffer is ready to be processed.

# Concepts

[Creating and Playing Sounds](Audio.md)

Provides overviews about audio technology, and presents predefined scenarios to demonstrate how to use audio.

# Reference

[DynamicSoundEffectInstance](T_MXFA_DynamicSoundEffectInstance.md)

Provides properties, methods, and events for play back of the audio buffer.

[Microphone](T_MXFA_Microphone.md)

Provides properties, methods, and fields and events for capturing audio data with microphones.

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.
