

# Retrieving Gyroscope Input

This topic demonstrates how to detect and use gyroscope input in an XNA Game Studio game.

Each sensor may not be available on every device, specifically the gyroscope sensor is an optional component. Applications need a way to determine whether a particular sensor’s hardware is present on a device before using it. The **IsSupported** property on each sensor class is used to determine whether the sensor is available on the current hardware platform. This is a static property that can be queried without creating an instance of the respective sensor class.

Games have the option of polling for new data from the sensor using the familiar mechanism of using the **Update** and **Draw** methods. In the following code the latest reading from the gyroscope sensor is read in the **Update** method. This data is then used during the **Draw** method.

# Adding Microsoft.Devices.Sensors to Your Application

Gyroscope input on Windows Phone 8.0 is not directly supported by Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0, but it is easy to add to an existing project. Gyroscope input is handled by the Microsoft.Devices.Sensors assembly, which must be referenced by your assembly before it can be used in your application.

### To add Microsoft.Devices.Sensors to your game

1.  In **Solution Explorer**, right-click your game project's **References** node, and then select **Add Reference**.
2.  Select Microsoft.Devices.Sensors from the list, and then click **OK**.
3.  Add `using Microsoft.Devices.Sensors;` to the top of any source file that will use the gyroscope classes and methods.

# Use Gyroscope Data in Your Game

After adding a reference to Microsoft.Devices.Sensors you can use Gyroscope APIs.

### To Get Gyroscope Values

1.  Add a data member to your game to hold gyroscope data. Declaring an instance of [Vector3](T_Microsoft_Xna_Framework_Vector3.md)) will hold the data returned from the gyroscope. The other members show here are used to draw the data later in the example.
    
                          `SpriteFont Font1;
    Vector2 FontPos;
    
    Vector3 gyroReading;`
                        
    
2.  Get the latest gyroscope data within your game's **Update** method. Note the use of the **IsSupported** property to check if gyroscope data is available.
    
                          `if (Gyroscope.IsSupported)
    {
        //get current rotation rate, display happens in Draw()
        GyroscopeReading gr = new GyroscopeReading();
        gyroReading = gr.RotationRate;
    }`
                        
    
3.  Do something with the gyroscope readings.
    
    Now you can use the gyroscope readings with your game's **Draw** method.
    
                          `if (Gyroscope.IsSupported)
    {
        //get current rotation rate, display happens in Draw()
        output += gyroReading.X.ToString("0.00") + " Y = "
        + gyroReading.Y.ToString("0.00") + " Z = "
        + gyroReading.Z.ToString("0.00");
    }
    
    // draw 
    spriteBatch.Begin();
    
    // Find the center of the string and draw
    Vector2 FontOrigin = Font1.MeasureString(output) / 2;
    spriteBatch.DrawString(Font1, output, FontPos, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
    spriteBatch.End();`
                        
    

# See Also

[Working with Touch Input](Input_HowTo_UseMultiTouchInput.md)  
[Retrieving Accelerometer Input](Input_HowTo_UseAccelerometerInput.md)  
[Input Overviews](Input.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team