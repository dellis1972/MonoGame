

# Adding XNA Rendering to a Silverlight Application on Windows Phone

Describes how to add XNA rendering to a Windows Phone game.

This how to illustrates how to create a Windows Phone game that runs on the Silverlight model and adds XNA rendering. This is broken into several smaller procedures to make it easier to follow. The first three procedures are prerequisites for implementing XNA rendering in a Silverlight application. The last two procedures show two different techniques: the first uses XNA to render 3D geometry and the second uses Silverlight to render to a texture and then uses XNA to render the texture. Don't forget that all three prerequisite procedures are required for either type of rendering.

*   [Prerequisites](#ID4E3)
*   [Rendering Techniques](#RenderingTechniques)

# Prerequisites

This prerequisites section illustrates the steps required to enable a Windows Phone game (created with the Windows Phone Application template) to make use of the 3D rendering capabilities of XNA.

![](note.gif)Note

Game studio now provides a template that creates a project with these changes already made: [Windows Phone Silverlight and XNA Application](UsingXNA_CreatingMobileProject.md). When this template is used, the steps in this section can be ignored and you can proceed directly to the [Rendering Techniques](#RenderingTechniques) section.

*   [Creating a Windows Phone Page for Adding XNA Game Code](#ID4EXB)
*   [Adding Update Methods to Call XNA Rendering Code](#ID4EBE)
*   [Creating a Shared Graphics Device](#ID4EJG)

## Creating a Windows Phone Page for Adding XNA Game Code

This procedure creates a Silverlight application using the Windows Phone Page template in Visual Studio.

### Creating a Windows Phone Page

1.  Right click on the C# project in the Solution Explorer. Choose **Add**, **New Item**, **Windows Phone Portrait Page**. Name it "GamePage".
    
2.  Add two new methods to the GamePage class: OnNavigatedTo and OnNavigatedFrom.
    
    public partial class GamePage : PhoneApplicationPage
    {
        ...
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
        ...
    }
    
3.  Add a new `using` statement to define the event args.
    
    using System.Windows.Navigation;
    
4.  Add a button to MainPage.xaml.
    
5.  Double click the button and modify the handler to navigate to this new page.
    
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
    }
    

## Adding Update Methods to Call XNA Rendering Code

This procedure adds update methods for XNA rendering code; the Silverlight model calls these update methods when control has been given to the shared graphics device enabling XNA code to update or draw a frame.

### Adding OnUpdate and OnDraw Methods to the GamePage Class

1.  Add the prototypes for OnUpdate and OnDraw.
    
    public partial class GamePage : PhoneApplicationPage
    {
        ...
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
        }
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
        }
        ...
    }
    
2.  Add new `using` statements to define the game timer event arguments.
    
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    
3.  Declare a Timer variable.
    
    public partial class GamePage : PhoneApplicationPage
    {
        ...
        GameTimer timer;
    }
    
4.  Initialize the timer in the GamePage class constructor.
    
    public GamePage()
    {
        ...
        // Create a timer for this page
        timer = new GameTimer();
        timer.UpdateInterval = TimeSpan.FromTicks(333333);
        timer.Update += OnUpdate;
        timer.Draw += OnDraw;
    }
    
5.  Add a call to start the timer when the user navigates to the page.
    
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        timer.Start();
    }
    
6.  Add a call to stop the timer when the user navigates away from the page.
    
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        // Stop the timer
        timer.Stop();
    }
    

## Creating a Shared Graphics Device

This procedure creates a shared graphics device manager; the Silverlight model uses a shared graphics device to call either Silverlight APIs or XNA APIs.

### Creating a Shared Graphics Device

1.  Add the xna namespace to the Application tag in App.xaml.
    
    xmlns:xna="clr-namespace:Microsoft.Xna.Framework;assembly=Microsoft.Xna.Framework.Interop"
    
2.  Add [SharedGraphicsDeviceManager](T_Microsoft_Xna_Framework_SharedGraphicsDeviceManager.md) to the ApplicationLifetimeObjects collection in App.xaml.
    
    <xna:SharedGraphicsDeviceManager/>
    
    When you are finished, the App.xaml file should look like this:
    
    <Application
      x:Class="MyLittleTeapot.App"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:phone="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone"
      xmlns:shell="clr-namespace:Microsoft.Phone.Shell;assembly=Microsoft.Phone"
      xmlns:xna="clr-namespace:Microsoft.Xna.Framework;assembly=Microsoft.Xna.Framework.Interop">
    
      <!--Application Resources-->
        <Application.Resources>
      </Application.Resources>
    
      <Application.ApplicationLifetimeObjects>
        <!--Required object that handles lifetime events for the application-->
        <shell:PhoneApplicationService
          Launching="Application\_Launching" Closing="Application\_Closing"
          Activated="Application\_Activated" Deactivated="Application\_Deactivated"/>
        <!--The SharedGraphicsDeviceManager is used to render with the XNA Graphics APIs-->
        <xna:SharedGraphicsDeviceManager />
        </Application.ApplicationLifetimeObjects>
    
    </Application>
    

# Rendering Techniques

*   [Rendering a 3D Object in XNA](#ID4ETAAC)
*   [Rendering Silverlight Controls and Text in XNA](#ID4E5CAC)

## Rendering a 3D Object in XNA

This procedure renders 3D geometry using XNA.

### Rendering a 3D Object in XNA

This procedure assumes you have already completed the first three procedures shown above.

1.  Declare a teapot object and a couple of teapot properties. This example copies the code for the teapot primitive from the [Primitives3D Sample](http://create.msdn.com/en-US/education/catalog/sample/primitives_3d).
    
    public partial class GamePage : PhoneApplicationPage
    {
        ...
        TeapotPrimitive teapot;
        Microsoft.Xna.Framework.Color teapotColor = Microsoft.Xna.Framework.Color.Black;
        float teapotYaw, teapotPitch;
        ...
    }
    
2.  Enable device sharing between Silverlight and XNA when navigating to the page.
    
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        // Set the sharing mode of the graphics device to turn on XNA rendering
        SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);
        ...
    }
    
3.  Add a corresponding disable call when navigating away from the page.
    
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        // Set the sharing mode of the graphics device to turn off XNA rendering
        SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);
    }
    
4.  Add code to create an instance of the teapot primitive when navigating to the page.
    
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ...
        teapot = new TeapotPrimitive(SharedGraphicsDeviceManager.Current.GraphicsDevice);
        ...
    }
    
5.  Add code to the OnDraw method to draw the 3D object.
    
    private void OnDraw(object sender, GameTimerEventArgs e)
    {
        SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
        //XNA Rendering
        DrawTeapot(e);
    }
    
6.  Add the following helper function.
    
    public partial class GamePage : PhoneApplicationPage
    {
        ...
        private void DrawTeapot(GameTimerEventArgs e)
        {
            float aspectRatio = SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.AspectRatio;
    
            // Construct the world, view, and projection matrices
            Matrix world = Matrix.CreateFromYawPitchRoll(teapotYaw, teapotPitch, 0f);
            Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 2.5f), Vector3.Zero, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, aspectRatio, 1, 10);
    
            // Draw the teapot
            teapot.Draw(world, view, projection, teapotColor);
        }
        ...
    }
    

## Rendering Silverlight Controls and Text in XNA

This procedure uses Silverlight to render Silverlight controls and Silverlight text to a texture which is passed to XNA for rendering.

### Rendering Silverlight Controls and Text in XNA

This procedure assumes you have already completed the first three procedures shown above.

1.  Declare [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) and [UIElementRenderer](T_Microsoft_Xna_Framework_Graphics_UIElementRenderer.md) objects in the GamePage class.
    
    public partial class GamePage : PhoneApplicationPage
    {
        ...
        SpriteBatch spriteBatch;
        UIElementRenderer elementRenderer;
        ...
    }
    
2.  Declare a new LayoutUpdated event handler.
    
    public GamePage()
    {
        ...
        // Use the LayoutUpdate event to know when the page layout has completed so we can
        // create the UIElementRenderer
        LayoutUpdated += new EventHandler(XNARendering_LayoutUpdated);
        ...
    }
    
3.  Implement the event handler to create a [UIElementRenderer](T_Microsoft_Xna_Framework_Graphics_UIElementRenderer.md) object.
    
    public partial class GamePage : PhoneApplicationPage
    {
        ...
        void XNARendering_LayoutUpdated(object sender, EventArgs e)
        {
            // make sure page size is valid
            if (ActualWidth == 0 || ActualHeight == 0)
                return;
    
            // see if we already have the right sized renderer
            if (elementRenderer != null &&
                elementRenderer.Texture != null &&
                elementRenderer.Texture.Width == (int)ActualWidth &&
                elementRenderer.Texture.Height == (int)ActualHeight)
            {
                return;
            }
    
            // dispose the current renderer
            if (elementRenderer != null)
                elementRenderer.Dispose();
    
            // create the renderer
            elementRenderer = new UIElementRenderer(this, (int)ActualWidth, (int)ActualHeight);
        }
        ...
    }
    
4.  Enable device sharing between Silverlight and XNA when navigating to the page.
    
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        // Set the sharing mode of the graphics device to turn on XNA rendering
        SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);
        ...
    }
    
5.  Add a corresponding disable call when navigating away from the page.
    
    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        // Set the sharing mode of the graphics device to turn off XNA rendering
        SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);
    }
    
6.  Create a [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object.
    
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        ...
        // Create a new SpriteBatch, which can be used to draw textures.
        spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
        ...
    }
    
7.  Add code to OnDraw to render the Silverlight content into the [UIElementRenderer](T_Microsoft_Xna_Framework_Graphics_UIElementRenderer.md) object, and then render its texture using the [SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md) object.
    
    private void OnDraw(object sender, GameTimerEventArgs e)
    {
        SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
        // Render the Silverlight content in the UIElementRenderer
        elementRenderer.Render();
    
        // Using the texture from the UIElementRenderer, draw the SL content to the screen
        spriteBatch.Begin();
        spriteBatch.Draw(elementRenderer.Texture, Vector2.Zero, Color.White);
        spriteBatch.End();
    }
    

# Remarks

If you want to enable your game to run at 60 hz instead of 30 hz (which is the default rate), see [Running a Windows Phone Game at 60 HZ](VariableSpeedTiming.md).

# See Also

#### Reference

[SetSharingMode](M_Microsoft_Xna_Framework_Graphics_GraphicsDeviceExtensions_SetSharingMode.md)  
[SpriteBatch](T_Microsoft_Xna_Framework_Graphics_SpriteBatch.md)  
[UIElementRenderer](T_Microsoft_Xna_Framework_Graphics_UIElementRenderer.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0