

# Audio API Developers Guide

This topic provides developers with information about the audio buffer format used by the MonoGame Audio API.

# Audio Buffer Format

The byte\[\] buffer format used as a parameter for the [SoundEffect](O_M_Microsoft_Xna_Framework_Audio_SoundEffect_ctor.md) constructor, [Microphone.GetData](O_M_MXFA_Microphone_GetData.md) method, and [DynamicSoundEffectInstance.SubmitBuffer](O_M_MXFA_DynamicSoundEffectInstance_SubmitBuffer.md) method is PCM wave data. Additionally, the PCM format is interleaved and in little-endian.

The audio format has the following constraints:

*   The audio channels can be mono (1) or stereo (2).
*   The PCM wave file must have 16-bits per sample.
*   The sample rate must be between 8,000 Hz and 48,000 Hz.
*   The interleaving for stereo data is left channel to right channel.

# Concepts

[Sounds Overview](Audio_XNA.md)

Provides a high-level overview about the capabilities of the Audio API in MonoGame in addition to general audio terminology.

[Playing a Sound](Audio_HowTo_PlayASound.md)

Demonstrates how to play a simple sound by using [SoundEffect](T_MXFA_SoundEffect.md).

[Streaming Data from a WAV File](Audio_HowTo_StreamDataFromWav.md)

Demonstrates how to stream audio from a wave (.wav) file.

# Reference

[SoundEffect Class](T_MXFA_SoundEffect.md)

Provides a loaded sound resource.

[SoundEffectInstance Class](T_MXFA_SoundEffectInstance.md)

Provides a single playing, paused, or stopped instance of a [SoundEffect](T_MXFA_SoundEffect.md) sound.

[DynamicSoundEffectInstance Class](T_MXFA_DynamicSoundEffectInstance.md)

Provides properties, methods, and events for play back of the audio buffer.

# Online Resources

[Audio Content Catalog at App Hub Online](http://go.microsoft.com/fwlink/?LinkId=128877)

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.
