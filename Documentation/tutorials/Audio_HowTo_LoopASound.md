

# Looping a Sound

This section demonstrates how to loop a sound.

# Complete Sample

The code in this topic shows you the technique for looping a sound. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download LoopSoundWithoutXACT_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258713)

# Simple Sound Looping

Not much extra code is needed to continiously loop a sound file in your game. Since the [SoundEffect](T_MXFA_SoundEffect.md) class does not provide looping support, you'll need to allocate a [SoundEffectInstance](T_MXFA_SoundEffectInstance.md) object. The following procedure builds on the sample code provided in the [Playing a Sound](Audio_HowTo_PlayASound.md) topic.

### To loop a sound

1.  Follow the instructions show in [Playing a Sound](Audio_HowTo_PlayASound.md) topic.
    
2.  To be able to loop a sound you will need to declare a [SoundEffectInstance](T_MXFA_SoundEffectInstance.md) object, and set it to the return value of [SoundEffect.CreateInstance](M_MXFA_SoundEffect_CreateInstance.md).
    
                          `SoundEffectInstance instance = soundEffect.CreateInstance();`
                        
    
3.  Set [SoundEffectInstance.IsLooped](P_MXFA_SoundEffectInstance_IsLooped.md) to **true** and then play the sound.
    
                          `instance.IsLooped = true;`
                        
    

# Concepts

[Playing a Sound](Audio_HowTo_PlayASound.md)

Demonstrates how to play a simple sound by using [SoundEffect](T_MXFA_SoundEffect.md).

# Reference

[SoundEffect Class](T_MXFA_SoundEffect.md)

Provides a loaded sound resource.

[SoundEffectInstance Class](T_MXFA_SoundEffectInstance.md)

Provides a single playing, paused, or stopped instance of a [SoundEffect](T_MXFA_SoundEffect.md) sound.

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0