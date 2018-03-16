

# Sharing Your Windows Phone Game with Others

In the course of developing your Windows Phone game, you might want to share an early version of your game with one or more colleagues, either with or without source code. This topic describes how both of these levels of sharing are accomplished.

# Sharing the Binary Version of Your Game

As with all Windows Phone applications, when you build your Windows Phone game using Microsoft Visual Studio, one of the files produced is an application package file with a .xap extension. This file contains the various assemblies and other files that constitute the binary version of your game, appropriately compressed.

In order to share the binary version of your Windows Phone game, simply send the .xap file produced when you built your game. To deploy a Windows Phone game in a received .xap file, either to the Windows Phone Emulator or to a developer-registered Windows Phone device, use the Windows Phone Application Deployment tool that is included with the Microsoft Windows Phone SDK 8.0. For detailed instructions about using this tool, see [How to deploy an app using the Application Deployment tool for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=254838) in the Windows Phone documentation.

# Sharing the Source Code for Your Game

You can share your game's source code and other assets by providing a copy of the entire Microsoft Visual Studio solution folder structure, which includes your solution and project files, your source code, your assets, and so on. Before doing so, to reduce the number of files to be copied, consider using the Microsoft Visual Studio command **Clean Solution** to delete all intermediate and executable files from the solution folder structure.

![](note.gif)Note

Depending on the total size of the files comprising this structure, and the manner in which you intend to send the files, it might make sense to compress the entire structure into a single file. One easy way to accomplish this is to use Windows Explorer: right-click the top level folder, choose **Send to**, then choose **Compressed (zipped) folder**, and then send the resulting Compressed (zipped) Folder. At the other end, in Windows Explorer, right-click the compressed folder and choose **Extract All...**.

Upon reception (and potential extraction) of your Microsoft Visual Studio solution folder structure, use Microsoft Visual Studio 2012 with Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0 to open your solution file. At this point, your Windows Phone game can be built, deployed, and run normally, and your source code and game assets are visible to the recipient.

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0