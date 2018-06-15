

# Playing a Sound

This topic demonstrates how to play a simple sound by using [SoundEffect](xref:MXFA.SoundEffect).

# Complete Sample

The code in this topic shows you the technique for playing sound. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download PlaySoundWithoutXACT.zip](http://go.microsoft.com/fwlink/?LinkId=258718)

### To add a sound file to your project

1.  Load your XNA Game Studio game in Visual Studio.
    
2.  In **Solution Explorer**, right-click the solution's **Content** project.
    
3.  Click **Add**, and then click **Existing Item**.
    
4.  Navigate to the .wav file you want to add, and then select it.
    
    ![](note.gif)Note
    
    The selected .wav file is inserted into your project. By default, it is processed by the Content Pipeline, and built wave files are accessed automatically when the game is built.
    

### To play a simple sound

1.  Declare a [SoundEffect](xref:MXFA.SoundEffect) object to hold the sound file.
    
    ```
    // Audio objects
    SoundEffect soundEffect;
    ```
                      
    
2.  Load the sound file using [Content.Load](xref:Microsoft.Xna.Framework.Content.ContentManager.Load``1).
    
    ```
    soundEffect = Content.Load<SoundEffect>("kaboom");
    ```
                      
    
3.  Play the sound.
    
    ```
    // Play the sound
    soundEffect.Play();
    ```               
    

# Concepts

[Looping a Sound](Audio_HowTo_LoopASound.md)

Demonstrates how to loop a sound.

# Reference

[SoundEffect Class](xref:MXFA.SoundEffect)

Provides a loaded sound resource.

[SoundEffectInstance Class](xref:MXFA.SoundEffectInstance)

Provides a single playing, paused, or stopped instance of a [SoundEffect](xref:MXFA.SoundEffect) sound.

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.
