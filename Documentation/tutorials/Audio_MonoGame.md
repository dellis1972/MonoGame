

# Sounds Overview

The MonoGame Framework provides audio playback through several core audio classes.

# Introduction

If your game is to use a few sound files, then the [SoundEffect](xref:MXFA.SoundEffect), [SoundEffectInstance](xref:MXFA.SoundEffectInstance), and [DynamicSoundEffectInstance](xref:MXFA.DynamicSoundEffectInstance) classes will provide everything you need to play and stream audio during gameplay.

# Simple Audio Playback

The simplest way to play sounds for background music or sound effects is to use [SoundEffect](xref:MXFA.SoundEffect) and [SoundEffectInstance](xref:MXFA.SoundEffectInstance). Source audio files are added like any other game asset to the project. For example code, see [Playing a Sound](Audio_HowTo_PlayASound.md), [Looping a Sound](Audio_HowTo_LoopASound.md), and [Adjusting Pitch and Volume](Audio_HowTo_ChangePitchAndVolume.md). For background music, see [Playing a Song](Audio_HowTo_PlayASong.md).

# Accessing the Audio Buffer

Developers can use [DynamicSoundEffectInstance](xref:MXFA.DynamicSoundEffectInstance) for direct access to an audio buffer. By accessing the audio buffer, developers can manipulate sound, break up large sound files into smaller data chunks, and stream sound. For example code, see [Streaming Data from a WAV File](Audio_HowTo_StreamDataFromWav.md).

# 3D Audio

The [SoundEffect](xref:MXFA.SoundEffect) class provides the ability to place audio in a 3D space. By creating [AudioEmitter](xref:Microsoft.Xna.Framework.Audio.AudioEmitter) and [AudioListener](xref:Microsoft.Xna.Framework.Audio.AudioListener) objects, the API can position a sound in 3D, and can change the 3D position of a sound during playback. Once you create and initialize [AudioEmitter](xref:Microsoft.Xna.Framework.Audio.AudioEmitter) and [AudioListener](xref:Microsoft.Xna.Framework.Audio.AudioListener), call [SoundEffectInstance.Apply3D](xref:MXFA.SoundEffectInstance.Apply3D).

# Audio Constraints

Mobile platformns have a maximum of 32 sounds playing simultaneously.
Dekstop platforms have a maximum of 256 sounds playing simultaneously.
Consoles and other platforms have their own constraints, please look at the console sdk
documentation for more information,
An [InstancePlayLimitException](xref:MXFA.InstancePlayLimitException) exception is thrown if this limit is exceeded.

# Concepts

[Playing a Sound](Audio_HowTo_PlayASound.md)

Demonstrates how to play a simple sound by using [SoundEffect](xref:MXFA.SoundEffect).

[Streaming Data from a WAV File](Audio_HowTo_StreamDataFromWav.md)

Demonstrates how to stream audio from a wave (.wav) file.

# Reference

[SoundEffect Class](xref:MXFA.SoundEffect)

Provides a loaded sound resource.

[SoundEffectInstance Class](xref:MXFA.SoundEffectInstance)

Provides a single playing, paused, or stopped instance of a [SoundEffect](xref:MXFA.SoundEffect) sound.

[DynamicSoundEffectInstance Class](xref:MXFA.DynamicSoundEffectInstance)

Provides properties, methods, and events for play back of the audio buffer.

# Online Resources

[Audio Content Catalog at App Hub Online](http://go.microsoft.com/fwlink/?LinkId=128877)

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.
