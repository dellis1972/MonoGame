

# Working with Touch Input

This topic demonstrates how to detect and use multitouch input in an XNA Game Studio game.

# Complete Sample

The code in this topic shows you the technique for detecting and using multitouch input. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download InputToyWP7.zip](http://go.microsoft.com/fwlink/?LinkId=258710)

Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0 supports multitouch input on Windows Phone 8.0. The primary class that provides this support is [TouchPanel](T_MXFIT_TouchPanel.md), which can:

*   Determine the touch capabilities of the current device.
*   Get the current state of the touch panel.
*   Detect touch gestures such as flicks, pinches, and drags. (For more information, see [Detecting Gestures on a Multitouch Screen](Input_GestureSupport.md).)

# Determining the Capabilities of the Touch Input Device

By using [TouchPanel.GetCapabilities](M_MXFIT_TouchPanel_GetCapabilities.md) you can determine if the touch panel is available. You also can determine the maximum touch count (the number of touches that can be detected simultaneously).

### To determine the capabilities of the touch device

1.  Call [TouchPanel.GetCapabilities](M_MXFIT_TouchPanel_GetCapabilities.md), which returns a [TouchPanelCapabilities](T_MXFIT_TouchPanelCapabilities.md) structure.
    
2.  Ensure [TouchPanelCapabilities.IsConnected](P_MXFIT_TouchPanelCapabilities_IsConnected.md) is **true**, indicating that the touch panel is available for reading.
    
3.  You then can use the [TouchPanelCapabilities.MaximumTouchCount](P_MXFIT_TouchPanelCapabilities_MaximumTouchCount.md) property to determine how many touch points are supported by the touch panel.
    

![](note.gif)Note

All touch panels for Windows Phone return a [MaximumTouchCount](P_MXFIT_TouchPanelCapabilities_MaximumTouchCount.md) value of 4 on Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0.

The following code demonstrates how to determine if the touch panel is connected, and then reads the maximum touch count.

              `TouchPanelCapabilities tc = TouchPanel.GetCapabilities();
if(tc.IsConnected)
{
    return tc.MaximumTouchCount;
}`
            

# Getting Multitouch Data from the Touch Input Device

You can use [TouchPanel.GetState](M_MXFIT_TouchPanel_GetState.md) to get the current state of the touch input device. It returns a [TouchCollection](T_MXFIT_TouchCollection.md) structure that contains a set of [TouchLocation](T_MXFIT_TouchLocation.md) structures, each containing information about position and state for a single touchpoint on the screen.

### To read multitouch data from the touch input device

1.  Call [TouchPanel.GetState](M_MXFIT_TouchPanel_GetState.md) to get a [TouchCollection](T_MXFIT_TouchCollection.md) representing the current state of the device.
    
2.  For each [TouchLocation](T_MXFIT_TouchLocation.md) in the [TouchCollection](T_MXFIT_TouchCollection.md), read the location and state data provided for each touchpoint.
    

The following code demonstrates how to get the current state of the touch input device and read touch data from each [TouchLocation](T_MXFIT_TouchLocation.md). It checks to see if a touch location has been pressed or has moved since the last frame, and if so, draws a sprite at the touch location.

              `// Process touch events
TouchCollection touchCollection = TouchPanel.GetState();
foreach (TouchLocation tl in touchCollection)
{
    if ((tl.State == TouchLocationState.Pressed)
            || (tl.State == TouchLocationState.Moved))
    {

        // add sparkles based on the touch location
        sparkles.Add(new Sparkle(tl.Position.X,
                 tl.Position.Y, ttms));

    }
}`
            

# See Also

#### Reference

[Microsoft.Xna.Framework.Input.Touch](N_Microsoft_Xna_Framework_Input_Touch.md)  
[TouchPanel](T_MXFIT_TouchPanel.md)  
[TouchPanelCapabilities](T_MXFIT_TouchPanelCapabilities.md)  
[TouchLocation](T_MXFIT_TouchLocation.md)  
[TouchLocationState](T_MXFIT_TouchLocationState.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team