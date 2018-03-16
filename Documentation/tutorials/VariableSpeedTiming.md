

# Running a Windows Phone Game at 60 HZ

Describes how to enable a Windows Phone game to run at 60 HZ.

This topic describes two techniques for running your Windows Phone Game at 60 hz (the default for a game created using the templates in Visual Studio is 30 hz). Use the first procedure when you create a game that runs on the Silverlight framework and renders with both Silverlight and XNA; see the procedures in [Adding XNA Rendering to a Silverlight Application on Windows Phone](XNARendering.md) for details about rendering with both technologies. Use the second procedure when you create a Windows Phone Game that does all of its rendering using XNA.

![](note.gif)Note

Caution should be used when setting a Windows Phone game to run at greater than 30 hz. Not all devices are able to update the screen at 60 hz. Many are slightly below 60 hz, and some may be significantly less than 60 hz. If the device can not support the specified update frequency the game will constantly fall behind.

*   [Running a Silverlight and XNA Game at 60 HZ](#ID4EEB)
*   [Running an XNA Game at 60 HZ](#ID4EHC)

# Running a Silverlight and XNA Game at 60 HZ

This procedure updates a Windows Phone Game that uses Silverlight and XNA for rendering to run at 60 HZ.

### Running a Silverlight and XNA Game at 60 HZ

1.  Implement the setup procedures in [Adding XNA Rendering to a Silverlight Application on Windows Phone](XNARendering.md) to add OnNavigation methods, and Update and Draw methods, and initialize a GameTimer object.
    
2.  Change the game timer interval to run at 60 hz.
    
    timer.UpdateInterval = TimeSpan.FromTicks(166667);
    

# Running an XNA Game at 60 HZ

This procedure updates a Windows Phone Game that only uses XNA for rendering to run at 60 HZ.

### Running an XNA Game at 60 HZ

1.  Create an event handler that you can use to change the device settings. Add this to your game constructor.
    
    Update the elapsed time to correspond with 60 hz.
    
    public Game1()
    {
      graphics = new GraphicsDeviceManager(this);
    
      graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>   
       (graphics_PreparingDeviceSettings);
      
      // Frame rate is 60 fps
      TargetElapsedTime = TimeSpan.FromTicks(166667);
    }
    
2.  Implement the event handler to set the presentation interval.
    
    void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
    {
      e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;
    }
    

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0