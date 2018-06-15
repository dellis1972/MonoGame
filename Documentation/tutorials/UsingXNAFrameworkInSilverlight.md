

# Enable XNA Framework Events in Windows Phone Applications

Event-based systems place messages in a queue where they wait to be dispatched. XNA Framework event messages are placed in a queue that is processed by the XNA [FrameworkDispatcher](T_MXF_FrameworkDispatcher.md).

In an application that implements the [Game](T_Microsoft_Xna_Framework_Game.md) class, the [FrameworkDispatcher.Update](M_MXF_FrameworkDispatcher_Update.md) method is called automatically whenever [Game.Update](M_Microsoft_Xna_Framework_Game_Update.md) is processed. This [FrameworkDispatcher.Update](M_MXF_FrameworkDispatcher_Update.md) method triggers event processing in the XNA Framework.

If you use the XNA Framework from an application that does not implement the [Game](T_Microsoft_Xna_Framework_Game.md) class—for example, a Windows Phone application that uses the Silverlight application model—you must call the [FrameworkDispatcher.Update](M_MXF_FrameworkDispatcher_Update.md) method yourself to dispatch messages that are in the XNA Framework message queue. You can do this once per frame in a ticking timer loop, or you can implement the [IApplicationService](http://msdn.microsoft.com/en-us/library/system.windows.iapplicationservice.aspx) interface with a [DispatcherTimer](http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatchertimer.aspx) and a [Tick](http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatchertimer.tick.aspx) event handler.

To do this, you can create an application service, and then register the service with the application.

### To create an XNA Framework dispatcher service

1.  Add a reference to the XNA Framework.
    
    *   On the **Project** menu, click **Add Reference**, click the **.NET** tab, select **Microsoft.Xna.Framework**, and then click **OK**.
        
2.  Create **using** directives to use the types in the **Microsoft.Xna.Framework** and **System.Windows.Threading** namespaces without having to specify the namespace.
    
    ```
    using System.Windows.Threading;
    using Microsoft.Xna.Framework;
    ```
    
3.  Create a class that implements [IApplicationService](http://msdn.microsoft.com/en-us/library/system.windows.iapplicationservice.aspx).
    
    ```
    public class XNAFrameworkDispatcherService : IApplicationService
    {
    }
    ```
    
4.  Add a [DispatcherTimer](http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatchertimer.aspx) member.
    
    ```
    private DispatcherTimer frameworkDispatcherTimer;
    ```
    
5.  In the constructor for the XNA Framework dispatcher service, do the following:
    
    *   Create a [DispatcherTimer](http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatchertimer.aspx) object and configure it to tick at 30fps, which is the standard rendering rate.
    *   Subscribe to the [Tick](http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatchertimer.tick.aspx) event handler of the [DispatcherTimer](http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatchertimer.aspx)
    *   Call [FrameworkDispatcher.Update](M_MXF_FrameworkDispatcher_Update.md) once. This is important and must be done in the constructor to ensure proper behavior from the XNA Framework.
    
                        `public XNAFrameworkDispatcherService()
    {
        this.frameworkDispatcherTimer = new DispatcherTimer();
        this.frameworkDispatcherTimer.Interval = TimeSpan.FromTicks(333333);
        this.frameworkDispatcherTimer.Tick += frameworkDispatcherTimer_Tick;
        FrameworkDispatcher.Update();
    }`
                      
    
6.  Call [FrameworkDispatcher.Update](M_MXF_FrameworkDispatcher_Update.md) in the [Tick](http://msdn.microsoft.com/en-us/library/system.windows.threading.dispatchertimer.tick.aspx) event handler.
    
    ```
    void frameworkDispatcherTimer_Tick(object sender, EventArgs e) { FrameworkDispatcher.Update(); }
    ```
    
7.  Call [StartService](http://msdn.microsoft.com/en-us/library/system.windows.iapplicationservice.startservice.aspx) to start the dispatcher timer.
    
    ```
    void IApplicationService.StartService(ApplicationServiceContext context) { this.frameworkDispatcherTimer.Start(); }
    ```
    
8.  Call [StopService](http://msdn.microsoft.com/en-us/library/system.windows.iapplicationservice.stopservice.aspx) to stop the dispatcher timer.
    
    ```
    void IApplicationService.StopService() { this.frameworkDispatcherTimer.Stop(); }
    ```
    

### To register the service with the application

Your implementation of the [IApplicationService](http://msdn.microsoft.com/en-us/library/system.windows.iapplicationservice.aspx) class must to be added to the **ApplicationLifeTimeObjects** collection. This can be done by editing **App.xaml**.

1.  In the **Application** element of **App.xaml**, add an attribute called **xmlsn:s** and set the value to `clr-namespace:WindowsPhoneApplication;assembly=WindowsPhoneApplication`. This will allow you to reference the class in the `<ApplicationLifetimeObjects>` element. Note that **WindowsPhoneApplication** is the name of the namespace for the sample in this example, your application namespace should be used instead.
2.  Add to or create an `<ApplicationLifetimeObjects/>` element as a child to the `<Application/>` element.
3.  In the `<ApplicationLifetimeObjects/>` element, add an element which specifies the name of your implementation of the application service. In this example, the class that was created is named **XNAFrameworkDispatcherService**, so the element is added as `<s:XNAFrameworkDispatcherService/>`.

          <Application
          x:Class="WindowsPhoneApplication.App"
          xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
          xmlns:System="clr-namespace:System;assembly=mscorlib"
          xmlns:navigation="clr-namespace:Microsoft.Phone.Controls;assembly=Microsoft.Phone.Controls.Navigation"
          xmlns:s="clr-namespace:WindowsPhoneApplication;assembly=WindowsPhoneApplication">

          <Application.ApplicationLifetimeObjects>
          <s:XNAFrameworkDispatcherService />
          </Application.ApplicationLifetimeObjects>
          .
          .
          .
          </Application>
          </code>
          </li>
        

# See Also

#### Concepts

[Initializing, Updating, and Exiting a Game](ApplicationModel.md)  

#### Reference

[SoundEffect](T_MXFA_SoundEffect.md)  
[DynamicSoundEffectInstance](T_MXFA_DynamicSoundEffectInstance.md)  
[Microphone](T_MXFA_Microphone.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team