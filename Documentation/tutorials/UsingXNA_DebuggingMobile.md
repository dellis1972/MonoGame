

# Debugging a Windows Phone Game

In general, debugging a Windows Phone game is no different than debugging any other Windows Phone application. For more information about debugging applications for Windows Phone, see [Debugging Windows Phone Applications](http://go.microsoft.com/fwlink/?LinkId=254755) in the Windows Phone documentation.

However, there are several Microsoft Visual Studio debugging features that are unavailable when debugging a Windows Phone game created with XNA Game Studio:

Unavailable feature

Description

Interop Debugging

You cannot debug both managed and native code simultaneously.

Assembly Debugging

While you can debug your own assemblies, you cannot debug system assemblies.

Edit and Continue

You cannot edit the binary content of your game without interrupting your debugging session.

Exception Interrupting

You cannot stop unhandled exceptions before they unwind in order to make changes and retry the operation.

Debugger Visualizers

You cannot display more informative views of some data types, such as XML data.

Exception Assistant Support

The additional features provided by [Exception Assistant](http://msdn.microsoft.com/en-us/library/197c1fsc.aspx) are unavailable when debugging Windows Phone games. Information on exceptions is provided by the standard **Exceptions** dialog box within XNA Game Studio.

Debugging a Running Process

You cannot attach the debugger to a process that is already executing.

# See Also

[Getting Started with XNA Game Studio Development](Getting_Started.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team