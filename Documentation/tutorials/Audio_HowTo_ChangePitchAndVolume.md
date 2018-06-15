

# Adjusting Pitch and Volume

The [SoundEffect.Play](O_M_MXFA_SoundEffect_Play.md) method allows you to specify the pitch and volume of a sound to play. However, after you call [Play](O_M_MXFA_SoundEffect_Play.md), you cannot modify the sound. Using [SoundEffectInstance](T_MXFA_SoundEffectInstance.md) for a given [SoundEffect](T_MXFA_SoundEffect.md) allows you to change the pitch and volume of a sound at any time during playback.

# Complete Sample

The code in this topic shows you the technique for changing a sound's pitch or volume. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download ChangePitchAndVolumeWithoutXACT_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258688)

# Change Pitch and Volume of Sound

### To adjust the pitch and volume of a sound

1.  Declare [SoundEffect](T_MXFA_SoundEffect.md) and [Stream](http://msdn.microsoft.com/en-us/library/system.io.stream.aspx) by using the method shown in [Playing a Sound](Audio_HowTo_PlayASound.md). In addition to the method described in [Playing a Sound](Audio_HowTo_PlayASound.md), declare [SoundEffectInstance](T_MXFA_SoundEffectInstance.md).
    
    ```
    SoundEffectInstance soundInstance;
    ```
                        
    
2.  In the [Game.LoadContent](M_MXF_Game_LoadContent.md) method, set the SoundEffectInstance object to the return value of [SoundEffect.CreateInstance](M_MXFA_SoundEffect_CreateInstance.md).
    
    ```
    soundfile = TitleContainer.OpenStream(@"Content\tx0_fire1.wav");
    soundEffect = SoundEffect.FromStream(soundfile);
    soundInstance = soundEffect.CreateInstance();
    ```
                        
    
3.  Adjust the sound to the desired level using the [SoundEffectInstance.Pitch](P_MXFA_SoundEffectInstance_Pitch.md) and [SoundEffectInstance.Volume](P_MXFA_SoundEffectInstance_Volume.md) properties.
    
    ```
    // Play Sound
    soundInstance.Play();
    ```
                        
    
4.  Play the sound using [SoundEffectInstance.Play](M_MXFA_SoundEffectInstance_Play.md).
    
    ```
    // Pitch takes values from -1 to 1
    soundInstance.Pitch = pitch;
    
    // Volume only takes values from 0 to 1
    soundInstance.Volume = volume;
    ```
                        
    

# Concepts

[Playing a Sound](Audio_HowTo_PlayASound.md)

Demonstrates how to play a simple sound by using [SoundEffect](T_MXFA_SoundEffect.md).

[Looping a Sound](Audio_HowTo_LoopASound.md)

Demonstrates how to loop a sound.

[Creating and Playing Sounds](Audio.md)

Provides overviews about audio technology, and presents predefined scenarios to demonstrate how to use audio.

# Reference

[SoundEffect Class](T_MXFA_SoundEffect.md)

Provides a loaded sound resource.

[SoundEffectInstance Class](T_MXFA_SoundEffectInstance.md)

Provides a single playing, paused, or stopped instance of a [SoundEffect](T_MXFA_SoundEffect.md) sound.

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team.
