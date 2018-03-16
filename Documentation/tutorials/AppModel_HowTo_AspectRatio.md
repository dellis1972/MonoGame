

# Restricting Aspect Ratio on a Graphics Device

Demonstrates how to create a custom [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) that only selects graphics devices with widescreen aspect ratios in full-screen mode.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download AspectRatio_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258685).

# Restricting Graphics Devices

### To restrict graphics devices to widescreen aspect ratios in full-screen mode

1.  Create a class that derives from [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md).
    
    ```
    public class CustomGraphicsDeviceManager : GraphicsDeviceManager
    {
        public CustomGraphicsDeviceManager( Game game )
            : base( game )
        {
        }
    
    }
    ```
    
2.  Add a **WideScreenOnly** property to the class.
    
    The property is used to turn on and off the widescreen-only behavior.
    
    ```
    private bool isWideScreenOnly;
    public bool IsWideScreenOnly
    {
        get { return isWideScreenOnly; }
        set { isWideScreenOnly = value; }
    }
    ```
    
3.  Determine the minimum desired aspect ratio.
    
    ```
    static float WideScreenRatio = 1.6f; //1.77777779f;
    ```
    
4.  Override the [RankDevices](M_Microsoft_Xna_Framework_GraphicsDeviceManager_RankDevices.md) method of [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md).
    
    Note the call to [base.RankDevices](M_Microsoft_Xna_Framework_GraphicsDeviceManager_RankDevices.md). This call ensures that the new version of [RankDevices](M_Microsoft_Xna_Framework_GraphicsDeviceManager_RankDevices.md) has an already ranked list of available devices with which to work.
    
    ```
    protected override void RankDevices( 
        List<GraphicsDeviceInformation> foundDevices )
    {
        base.RankDevices( foundDevices );
    }
    ```
    
5.  Add a check to see if the **WideScreenOnly** property is **true**.
    
    ```
    if (IsWideScreenOnly)
    {
        ...
    }
    ```
    
6.  In the **if** block, loop through all found devices, and check whether the [PresentationParameters](T_Microsoft_Xna_Framework_Graphics_PresentationParameters.md) indicate the device is full-screen.
    
7.  If the device is full-screen, determine the aspect ratio of the device by dividing the [BackBufferWidth](P_Microsoft_Xna_Framework_Graphics_PresentationParameters_BackBufferWidth.md) by the [BackBufferHeight](P_Microsoft_Xna_Framework_Graphics_PresentationParameters_BackBufferHeight.md).
    
8.  If the aspect ratio is less than the desired aspect ratio, remove the device from the list of found devices.
    
    ```
    for (int i = 0; i < foundDevices.Count; )
    {
        PresentationParameters pp = 
            foundDevices[i].PresentationParameters;
        if (pp.IsFullScreen == true)
        {
            float aspectRatio = (float)(pp.BackBufferWidth) / 
                (float)(pp.BackBufferHeight);
    
            // If the device does not have a widescreen aspect 
            // ratio, remove it.
            if (aspectRatio < WideScreenRatio) 
            { 
                foundDevices.RemoveAt( i ); 
            }
            else { i++; }
        }
        else i++;
    }
    ```
    
9.  Replace the default [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md) with the derived [GraphicsDeviceManager](T_Microsoft_Xna_Framework_GraphicsDeviceManager.md).
10.  To test the new component, set the **WideScreenOnly** and [IsFullScreen](P_Microsoft_Xna_Framework_GraphicsDeviceManager_IsFullScreen.md) properties to **true**.
    
    ```
            public Game1()
            {
                graphics = new CustomGraphicsDeviceManager(this);
                Content.RootDirectory = "Content";
    
                this.graphics.PreferMultiSampling = false;
    #if WINDOWS
                this.graphics.PreferredBackBufferWidth = 1280;
                this.graphics.PreferredBackBufferHeight = 720;
    #endif
    #if WINDOWS_PHONE
                this.graphics.PreferredBackBufferWidth = 400;
                this.graphics.PreferredBackBufferHeight = 600;
    #endif
    
                this.graphics.IsFullScreen = true;
                this.graphics.IsWideScreenOnly = true;
                graphics.ApplyChanges();
            }
    ```
    

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0