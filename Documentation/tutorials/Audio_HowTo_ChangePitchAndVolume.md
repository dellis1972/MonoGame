

# Adjusting Pitch and Volume

The [SoundEffect.Play](xref:MXFA.SoundEffect.Play) method allows you to specify the pitch and volume of a sound to play. However, after you call [Play](xref:MXFA.SoundEffect.Play), you cannot modify the sound. Using [SoundEffectInstance](xref:MXFA.SoundEffectInstance) for a given [SoundEffect](xref:MXFA.SoundEffect) allows you to change the pitch and volume of a sound at any time during playback.

# Complete Sample

The code in this topic shows you the technique for changing a sound's pitch or volume. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download ChangePitchAndVolumeWithoutXACT_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258688)

# Change Pitch and Volume of Sound

### To adjust the pitch and volume of a sound

1.  Declare [SoundEffect](xref:MXFA.SoundEffect) and [Stream](http://msdn.microsoft.com/en-us/library/system.io.stream.aspx) by using the method shown in [Playing a Sound](Audio_HowTo_PlayASound.md). In addition to the method described in [Playing a Sound](Audio_HowTo_PlayASound.md), declare [SoundEffectInstance](xref:MXFA.SoundEffectInstance).
    
    ```
    SoundEffectInstance soundInstance;
    ```
                        
    
2.  In the [Game.LoadContent](xref:MXF.Game.LoadContent) method, set the SoundEffectInstance object to the return value of [SoundEffect.CreateInstance](xref:MXFA.SoundEffect.CreateInstance).
    
    ```
    soundfile = TitleContainer.OpenStream(@"Content\tx0_fire1.wav");
    soundEffect = SoundEffect.FromStream(soundfile);
    soundInstance = soundEffect.CreateInstance();
    ```
                        
    
3.  Adjust the sound to the desired level using the [SoundEffectInstance.Pitch](xref:MXFA.SoundEffectInstance.Pitch) and [SoundEffectInstance.Volume](xref:MXFA.SoundEffectInstance.Volume) properties.
    
    ```
    // Play Sound
    soundInstance.Play();
    ```
                        
    
4.  Play the sound using [SoundEffectInstance.Play](xref:MXFA.SoundEffectInstance.Play).
    
    ```
    // Pitch takes values from -1 to 1
    soundInstance.Pitch = pitch;
    
    // Volume only takes values from 0 to 1
    soundInstance.Volume = volume;
    ```
                        
    

# Concepts

[Playing a Sound](Audio_HowTo_PlayASound.md)

Demonstrates how to play a simple sound by using [SoundEffect](xref:MXFA.SoundEffect).

[Looping a Sound](Audio_HowTo_LoopASound.md)

Demonstrates how to loop a sound.

[Creating and Playing Sounds](Audio.md)

Provides overviews about audio technology, and presents predefined scenarios to demonstrate how to use audio.

# Reference

[SoundEffect Class](xref:MXFA.SoundEffect)

Provides a loaded sound resource.

[SoundEffectInstance Class](xref:MXFA.SoundEffectInstance)

Provides a single playing, paused, or stopped instance of a [SoundEffect](xref:MXFA.SoundEffect) sound.

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
