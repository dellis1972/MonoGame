

# Working with Hardware Keyboards

Some, but not all, Windows Phone devices feature hardware keyboards. These keyboards can be accessed using XNA Game Studio. There is one major caveat, however: XNA Game Studio provides no way to determine whether the device has a hardware keyboard, or whether it is currently enabled (open in the case of a slide-style keyboard).

For Windows Phone, the [Keys](T_Microsoft_Xna_Framework_Input_Keys.md) enumeration maps the following members to special keys:

Windows Phone key

Keys enumeration member

Accent key

**Keys.F1**

Emoticon key

**Keys.F2**

Symbol key

**Keys.F3**

Vib key

**Keys.F4**

Fn key

**Keys.LeftShift**

All other keys are assigned as described in [Keys](T_Microsoft_Xna_Framework_Input_Keys.md).

For information about detecting hardware key presses, and for code examples see [Detecting a Key Press](Input_HowTo_DetectKeyPress.md).

![](note.gif)Note

If you are interested in retrieving characters typed—such as when you want text input from the user— instead of the key state, you should use [Guide.BeginShowKeyboardInput](O_M_Microsoft_Xna_Framework_Graphicsx_Guide_BeginShowKeyboardInput.md) as described in [Displaying a Software Input Panel](ShowSWKeyboard.md). This process displays a software keyboard that can be used on devices with or without a hardware keyboard, and returns the characters typed.

# See Also

[Input Overviews](Input.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team