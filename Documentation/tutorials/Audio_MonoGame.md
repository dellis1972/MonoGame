

# Sounds Overview

The MonoGame Framework provides audio playback through several core audio classes.

# Introduction

If your game is to use a few sound files, then the [SoundEffect](T_MXFA_SoundEffect.md), [SoundEffectInstance](T_MXFA_SoundEffectInstance.md), and [DynamicSoundEffectInstance](T_MXFA_DynamicSoundEffectInstance.md) classes will provide everything you need to play and stream audio during gameplay.

# Simple Audio Playback

The simplest way to play sounds for background music or sound effects is to use [SoundEffect](T_MXFA_SoundEffect.md) and [SoundEffectInstance](T_MXFA_SoundEffectInstance.md). Source audio files are added like any other game asset to the project. For example code, see [Playing a Sound](Audio_HowTo_PlayASound.md), [Looping a Sound](Audio_HowTo_LoopASound.md), and [Adjusting Pitch and Volume](Audio_HowTo_ChangePitchAndVolume.md). For background music, see [Playing a Song](Audio_HowTo_PlayASong.md).

# Accessing the Audio Buffer

Developers can use [DynamicSoundEffectInstance](T_MXFA_DynamicSoundEffectInstance.md) for direct access to an audio buffer. By accessing the audio buffer, developers can manipulate sound, break up large sound files into smaller data chunks, and stream sound. For example code, see [Streaming Data from a WAV File](Audio_HowTo_StreamDataFromWav.md).

# 3D Audio

The [SoundEffect](T_MXFA_SoundEffect.md) class provides the ability to place audio in a 3D space. By creating [AudioEmitter](T_Microsoft_Xna_Framework_Audio_AudioEmitter.md) and [AudioListener](T_Microsoft_Xna_Framework_Audio_AudioListener.md) objects, the API can position a sound in 3D, and can change the 3D position of a sound during playback. Once you create and initialize [AudioEmitter](T_Microsoft_Xna_Framework_Audio_AudioEmitter.md) and [AudioListener](T_Microsoft_Xna_Framework_Audio_AudioListener.md), call [SoundEffectInstance.Apply3D](O_M_MXFA_SoundEffectInstance_Apply3D.md).

# Audio Constraints

Mobile platformns have a maximum of 32 sounds playing simultaneously.
Dekstop platforms have a maximum of 256 sounds playing simultaneously.
Consoles and other platforms have their own constraints, please look at the console sdk
documentation for more information,
An [InstancePlayLimitException](T_MXFA_InstancePlayLimitException.md) exception is thrown if this limit is exceeded.

# Concepts

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
