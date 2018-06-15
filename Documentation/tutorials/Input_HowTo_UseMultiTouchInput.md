

# Working with Touch Input

This topic demonstrates how to detect and use multitouch input in a MonoGame game.

# Complete Sample

The code in this topic shows you the technique for detecting and using multitouch input. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download InputToyWP7.zip](http://go.microsoft.com/fwlink/?LinkId=258710)

Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0 supports multitouch input on Windows Phone 8.0. The primary class that provides this support is [TouchPanel](xref:MXFIxref:TouchPanel), which can:

*   Determine the touch capabilities of the current device.
*   Get the current state of the touch panel.
*   Detect touch gestures such as flicks, pinches, and drags. (For more information, see [Detecting Gestures on a Multitouch Screen](Input_GestureSupport.md).)

# Determining the Capabilities of the Touch Input Device

By using [TouchPanel.GetCapabilities](xref:MXFIT.TouchPanel.GetCapabilities) you can determine if the touch panel is available. You also can determine the maximum touch count (the number of touches that can be detected simultaneously).

### To determine the capabilities of the touch device

1.  Call [TouchPanel.GetCapabilities](xref:MXFIT.TouchPanel.GetCapabilities), which returns a [TouchPanelCapabilities](xref:MXFIxref:TouchPanelCapabilities) structure.
    
2.  Ensure [TouchPanelCapabilities.IsConnected](xref:MXFIT.TouchPanelCapabilities.IsConnected) is **true**, indicating that the touch panel is available for reading.
    
3.  You then can use the [TouchPanelCapabilities.MaximumTouchCount](xref:MXFIT.TouchPanelCapabilities.MaximumTouchCount) property to determine how many touch points are supported by the touch panel.
    

![](note.gif)Note

All touch panels for Windows Phone return a [MaximumTouchCount](xref:MXFIT.TouchPanelCapabilities.MaximumTouchCount) value of 4 on Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0.

The following code demonstrates how to determine if the touch panel is connected, and then reads the maximum touch count.

              `TouchPanelCapabilities tc = TouchPanel.GetCapabilities();
if(tc.IsConnected)
{
    return tc.MaximumTouchCount;
}`
            

# Getting Multitouch Data from the Touch Input Device

You can use [TouchPanel.GetState](xref:MXFIT.TouchPanel.GetState) to get the current state of the touch input device. It returns a [TouchCollection](xref:MXFIxref:TouchCollection) structure that contains a set of [TouchLocation](xref:MXFIxref:TouchLocation) structures, each containing information about position and state for a single touchpoint on the screen.

### To read multitouch data from the touch input device

1.  Call [TouchPanel.GetState](xref:MXFIT.TouchPanel.GetState) to get a [TouchCollection](xref:MXFIxref:TouchCollection) representing the current state of the device.
    
2.  For each [TouchLocation](xref:MXFIxref:TouchLocation) in the [TouchCollection](xref:MXFIxref:TouchCollection), read the location and state data provided for each touchpoint.
    

The following code demonstrates how to get the current state of the touch input device and read touch data from each [TouchLocation](xref:MXFIxref:TouchLocation). It checks to see if a touch location has been pressed or has moved since the last frame, and if so, draws a sprite at the touch location.

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

[Microsoft.Xna.Framework.Input.Touch](xref:Microsoft.Xna.Framework.Input.Touch)  
[TouchPanel](xref:MXFIxref:TouchPanel)  
[TouchPanelCapabilities](xref:MXFIxref:TouchPanelCapabilities)  
[TouchLocation](xref:MXFIxref:TouchLocation)  
[TouchLocationState](xref:MXFIxref:TouchLocationState)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team