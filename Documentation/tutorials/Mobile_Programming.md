

# Exploring Windows Phone Programming

This topic provides links to a variety of other topics that provide important information about programming games for Windows Phone.

![](caution.gif)Caution

Windows Phone applications and games use the [Silverlight Application Security Model](http://msdn.microsoft.com/library/dd470128(VS.95).aspx). Windows Phone code—called [Transparent](http://msdn.microsoft.com/library/dd233102.aspx) code—is unable to call security critical libraries, types, and methods. A majority of security critical methods and types are in the System.IO namespace. In the Silverlight or .NET Framework libraries supported in the XNA Framework, security critical methods can be identified by the \[SecurityCriticalAttribute\] attribute. Although these methods will compile, they throw a [MethodAccessException](http://msdn2.microsoft.com/library/system.methodaccessexception.aspx) at runtime.

# Key Programming Topics in the Windows Phone Documentation

[The Silverlight and XNA Frameworks for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=255063)

Describes what is supported when using the XNA Framework from a Windows Phone application, and which Silverlight namespaces are not supported in a Windows Phone Game.

[Execution Model Overview for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=255064)

Under the Windows Phone execution model, applications are activated and deactivated dynamically when a user navigates away from the application. This document describes how to persist data correctly for a Windows Phone application when the application is activated or deactivated.

[Execution Model Best Practices for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=255065)

Windows Phone applications are terminated when the user navigates away from them. This topic highlights some best practices for handling execution model events.

# Windows Phone Programming with XNA Game Studio

[Working with Multitouch and Accelerometer Input](Input_TouchandAccel.md)

Topics that provide guidance on writing code to support multitouch and accelerometer input for XNA Game Studio.

[Writing Data](Storage_HowTo_SaveDataMobile.md)

Describes how to save game data on Windows Phone.

[Adding a Windows Phone Background Agent to an XNA Game](UsingBackgroundAgentsOnPhone.md)

Describes the steps required to add a Windows Phone background agent to an XNA Framework game.

[Handling Interruptions on Windows Phone](RespondingtoShutdownEvents.md)

Provides detail about how to handle interruptions, such as incoming calls, on Windows Phone.

[Enable XNA Framework Events in Windows Phone Applications](UsingXNAFrameworkInSilverlight.md)

Describes the the process of manually initializing and updating a [FrameworkDispatcher](T_MXF_FrameworkDispatcher.md) to support XNA Framework functionality from other application models that use the Silverlight application model.

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team