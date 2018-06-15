

# Overview of User Input and Input Devices

Input is a general term referring to the process of receiving actions from the user. In XNA Game Studio, the [Microsoft.Xna.Framework.Input](N_Microsoft_Xna_Framework_Input.md) namespace provides support for most input devices.

![](note.gif)Note

Methods related to input devices unavailable on the platform your game is running on are always available to your code. For example, you can access all [GamePad](T_Microsoft_Xna_Framework_Input_GamePad.md) methods on Windows Phone, but they will not return valid information. Although using these methods will not cause exceptions or build errors in your code, they will silently fail when run.

Physical keyboards may or may not be present on Windows Phone devices; you should not rely on the presence of a physical keyboard. For text input, you should use a software input panel (SIP), which will work on all devices, including those with physical keyboards. For more information, see [Displaying a Software Input Panel](ShowSWKeyboard.md).

If the Windows Phone device does have a physical keyboard, the same methods used for keyboards on Windows or Xbox 360 can be used, given a few caveats. For more information, see [Working with Hardware Keyboards](Input_HWKeysOnWP.md).

For Windows Phone's multitouch device, you can use the raw touch data provided by the [TouchPanel](T_MXFIT_TouchPanel.md) class, but you can also use XNA Game Studio's support for predefined gestures if your input fits one of the supported gesture types. For information about working with raw multitouch input, see [Working with Touch Input](Input_HowTo_UseMultiTouchInput.md). For information about gesture support, see [Detecting Gestures on a Multitouch Screen](Input_GestureSupport.md).

An Accelerometer API is not provided by XNA Game Studio. For more information about how to use Silverlight's **Accelerometer** class with XNA Game Studio, see [Retrieving Accelerometer Input](Input_HowTo_UseAccelerometerInput.md).

The microphone on Windows Phone can be used to capture audio that can be used in your game. For more information, see [Recording Sounds with Microphones](Microphone.md).

# See Also

#### Concepts

[Input Content Catalog at App Hub Online](http://go.microsoft.com/fwlink/?LinkId=128875)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team