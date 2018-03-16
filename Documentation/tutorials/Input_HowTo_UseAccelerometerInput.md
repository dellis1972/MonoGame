

# Retrieving Accelerometer Input

This topic demonstrates how to detect and use accelerometer input in an XNA Game Studio game.

# Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download InputToyWP7.zip](http://go.microsoft.com/fwlink/?LinkId=258710)

Accelerometer input on Windows Phone 8.0 is not directly supported by Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0, but it is easy to add to an existing project.

# Adding Microsoft.Devices.Sensors to Your Application

Accelerometer input on Windows Phone 8.0 is handled by the Microsoft.Devices.Sensors assembly, which must be referenced by your assembly before any of its types, events, or methods can be used in your application.

### To add Microsoft.Devices.Sensors to your game

1.  In **Solution Explorer**, right-click your game project's **References** node, and then select **Add Reference**.
2.  Select Microsoft.Devices.Sensors from the list, and then click **OK**.
3.  Add `using Microsoft.Devices.Sensors;` to the top of any source file that will use the accelerometer classes and methods.

# Adding Accelerometer Support to Your Game

Once you have a reference to Microsoft.Devices.Sensors in your project and an associated `using` statement in your source files, you can begin adding code to support accelerometer input.

### To add accelerometer support

1.  Add a data member to your game to hold accelerometer data.
    
    Accelerometer data is returned in an [AccelerometerReadingEventArgs](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometerreadingeventargs.aspx) class. You can declare an instance of this structure in your class, declare an instance of a similar structure (such as [Vector3](T_Microsoft_Xna_Framework_Vector3.md)), or create separate data members to read [AccelerometerReadingEventArgs](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometerreadingeventargs.aspx)'s **X**, **Y**, and **Z** members.
    
                          `Accelerometer accelSensor;
    Vector3 accelReading = new Vector3();`
                        
    
2.  Add an event handler for the [ReadingChanged](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometer.readingchanged.aspx) event.
    
    Add a method to your class that returns void, and has two parameters: an object representing the sender, and an [AccelerometerReadingEventArgs](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometerreadingeventargs.aspx) to get the accelerometer reading.
    
                          `public void AccelerometerReadingChanged(object sender, AccelerometerReadingEventArgs e)
    {
        accelReading.X =  (float)e.X;
        accelReading.Y = (float)e.Y;
        accelReading.Z = (float)e.Z;
    }`
                        
    
3.  Associate your event handler with the [ReadingChanged](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometer.readingchanged.aspx) event.
    
                          `accelSensor = new Accelerometer();
    
    // Add the accelerometer event handler to the accelerometer sensor.
    accelSensor.ReadingChanged +=
        new EventHandler<AccelerometerReadingEventArgs>(AccelerometerReadingChanged);`
                        
    
4.  Start the accelerometer sensor.
    
    The accelerometer must be started before it begins calling your event handler. This may raise an exception, so your code should handle the case where the accelerometer cannot be started.
    
                          `// Start the accelerometer
    try
    {
        accelSensor.Start();
        accelActive = true;
    }
    catch (AccelerometerFailedException e)
    {
        // the accelerometer couldn't be started.  No fun!
        accelActive = false;
    }
    catch (UnauthorizedAccessException e)
    {
        // This exception is thrown in the emulator-which doesn't support an accelerometer.
        accelActive = false;
    }`
                        
    
5.  Get accelerometer readings.
    
    After it is started, the accelerometer calls your event handler when the [ReadingChanged](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometer.readingchanged.aspx) event is raised. Update your stored [AccelerometerReadingEventArgs](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometerreadingeventargs.aspx) class (previously shown in the event handler code), and then use its data in your game's **Update** method.
    
                          `if (accelActive)
    {
        // accelerate the sparkle depending on accelerometer
        // action.
        s.speed.X += accelReading.X * ACCELFACTOR;
        s.speed.Y += -accelReading.Y * ACCELFACTOR;
    }`
                        
    
6.  Stop the accelerometer sensor.
    
    To avoid having your event handler called repeatedly when your game is not actually using the accelerometer data, you can stop the accelerometer when the game is paused, when menus are being shown, or at any other time by calling the [Stop](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometer.stop.aspx) method. Like [Start](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometer.start.aspx), this method can throw an exception, so allow your code to handle the [AccelerometerFailedException](http://msdn.microsoft.com/en-us/library/microsoft.devices.sensors.accelerometerfailedexception.aspx).
    
                          `// Stop the accelerometer if it's active.
    if (accelActive)
    {
        try
        {
            accelSensor.Stop();
        }
        catch (AccelerometerFailedException e)
        {
            // the accelerometer couldn't be stopped now.
        }
    }`
                        
    

# See Also

[Working with Touch Input](Input_HowTo_UseMultiTouchInput.md)  
[Input Overviews](Input.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0