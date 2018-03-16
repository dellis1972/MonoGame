

# Troubleshooting XNA Game Studio Projects

Provides some helpful troubleshooting tips for XNA Game Studio projects.

# Project Fails to Deploy/Debug

If your XNA Game Studio project fails to deploy and execute (for example, from an F5 command), it may because the startup project has been mistakenly set to the solution's content project. Since the content project is not executable, the deployment will fail.

### To view and change the startup project

1.  In Solution Explorer, right-click the project you want to set as the startup project.
2.  In the context menu, click **Set as StartUp Project**.

Also, you can either use Solution Explorer to select the project or, on the **Project** menu, click **Set as StartUp Project** to select the project.

This condition can occur after Visual Studio encounters errors in a .csproj file. These errors may have been introduced by a faulty manual edit of that file. In this circumstance, Visual Studio may reset the startup project to the content project. This setting will persist even after the error in the .csproj file has been corrected.

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0