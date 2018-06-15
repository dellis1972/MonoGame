

# Displaying a Message Box

Describes how to show a message box.

# The Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download GuideUIWP7.zip](http://go.microsoft.com/fwlink/?LinkId=258708)

You can use [Guide.BeginShowMessageBox](xref:Microsoft.Xna.Framework.Graphics.Guide.BeginShowMessageBox) to show a system-supplied message box that displays alerts, warnings, and other information to the gamer. This is an asynchronous process for which you can use both polling and callback-style techniques to retrieve user input.

![](note.gif)Note

For more information about programming asynchronous methods, see [Working with Asynchronous Methods in XNA Game Studio](AsyncProgramming.md).

### To show a message box

1.  Call [Guide.BeginShowMessageBox](xref:Microsoft.Xna.Framework.Graphics.Guide.BeginShowMessageBox) to begin display of the message box.
    
    For example:
    
    ```
    List<string> MBOPTIONS = new List<string>();
    ```
    
    ![](note.gif)Note
    
    [Guide.BeginShowMessageBox](xref:Microsoft.Xna.Framework.Graphics.Guide.BeginShowMessageBox) has two overloads, one of which takes a player index. It doesn't matter which one you choose, but if you call the overload that takes a player index, the player index must always be **PlayerIndex.One**.
    
    The maximum number of buttons you can define for a message box on Windows Phone is currently two.
    
2.  When **IASyncResult.IsCompleted** is **true**, call [Guide.EndShowMessageBox](xref:Microsoft.Xna.Framework.GamerServices.Guide.EndShowMessageBox) to retrieve the zero-based index of the message box button chosen by the user.
    

For example:

```
protected void GetMBResult(IAsyncResult r)
{
    int? b = Guide.EndShowMessageBox(r);
    gameState = 0;
}
```

# See Also

[MessageBoxIcon Enumeration](xref:Microsoft.Xna.Framework.GamerServices.MessageBoxIcon)  
[PlayerIndex Enumeration](xref:Microsoft.Xna.Framework.PlayerIndex)  
[IAsyncResult](http://msdn.microsoft.com/en-us/library/system.iasyncresult.aspx)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team