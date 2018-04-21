

# How to: Create a Localized Game

A localized game is one in which the UI displays alternative sets of text that are appropriate to the language and culture of the gamer.

This tutorial demonstrates methods that allow a game to display differing UI text according to the platform's current culture setting stored in [CultureInfo](http://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.aspx). It also demonstrates how to extend the Content Pipeline so that the minimum number of additionally required font characters are included in the game executable for the localized text.

For more detailed information about both globalization and localization issues for Windows Phone, see [Globalization and Localization for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=254839) in the Windows Phone documentation.

*   [Complete Sample](#ID4EKB)
*   [Creating Resource Files](#ID4ECC)
*   [Extending the Content Pipeline](#ID4EYF)
*   [Associating Localization Data and Processors With Fonts](#ID4EVEAC)
*   [Using Localized Strings in the Game](#ID4ETJAC)

# Complete Sample

The code in the topic shows you the technique for creating a game that can be localized. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download LocalizationSample.zip](http://go.microsoft.com/fwlink/?LinkId=258712)

# Creating Resource Files

A localized game should maintain its resources in resource (.resx) files in the game's code project. The game should maintain at least one resource file for every language/culture it is to support. Each resource file defines identifiers for each localizable UI string in the game, with a localized version of that string for the intended language/culture. In this way, localization can be maintained independently from coding.

### To create resource files

1.  In **Solution Explorer**, right-click the code project node, select **Add**, and then click **New Item**.
    
2.  In the **Add New Item** dialog box, select the resources file, and rename it as appropriate, for example, Strings.resx.
    
    This file will contain the resources for the default language for the application.
    
3.  Identify strings in your application and add them to the resource file.
    
    Each string has a name, a value, and an optional comment. The name must be unique so make it as descriptive as possible. The value is the string to be displayed in the application to the user. Adding a comment is useful, especially in large resource files as a reminder of the purpose of the string and later as information for the translator to understand the correct context.
    
    **Figure 1.  An example of English language strings for the Strings.resx resource file**
    
    ![](CP_HowTo_Localize_Strings.png)
4.  Add a new resource file to your project for each language/culture the application will support.
    
    ![](note.gif)Note
    
    Each resource file must follow the naming convention:
    
    _name of the resource file for the default language_._culture name_
    
    An example of this format is MyStrings.fr.resx, where the culture name—fr in this instance—is derived from the [CultureInfo](http://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.aspx) class.
    
    As shown in the next figure, the strings defined for the resource file must have the same set of identifying names as defined in the default resource file.
    
    **Figure 2.   An example of French language strings for the resource file Strings.fr.resx**
    
    ![](CP_HowTo_Localize_Strings_fr.png)
    
    **Figure 3.  An example of strings in the Japanese language resource file Strings.ja.resx**
    
    ![](CP_HowTo_Localize_Strings_ja.png)

![](note.gif)Universal Windows Applications Specific Information

Universal Windows Applications (UWP) do not use `resx` files. These have been replace with `resw` files, the format is slightly different but the process is the same as `resx`. It does however mean you need to maintain two copies of the lanuage files when targeting UWP and other platforms.

# Associating Localization Data and Processors With Fonts

Now that project data is divided into resource files, and has a custom content processor to consume it, the parts need to be bound together to help them work. This is accomplished by providing extended information in a .spritefont file, and binding the file to the custom content processor.

### To extend the .spritefont file

Create a new .spritefont file.

1.  In **Pipeline Tool**, right-click the content project node, select **Add**, and then click **New Item**.
    
2.  In the **Add New Item** dialog box, select "Localized Sprite Font," and then in the **Name** box, enter a name (for example, Font.spritefont) for the new file.
    
3.  Click **Add** to create the new file. This will create a new Localized Sprite Font which is already setup to work with localized resource files.
    
4.  Right Click on the new file and select Open or Open With to open it for editing.
        
5.  Add a block within <ResourceFiles> tags that lists each resource file using the <Resx> elements.
    
    The example tags below specify resource files for the default language, as well as for Danish (da), French (fr), Japanese (ja), and Korean (ko).
    
    ```
    <ResourceFiles>
        <Resx>..\\Strings.resx</Resx>
        <Resx>..\\Strings.da.resx</Resx>
        <Resx>..\\Strings.fr.resx</Resx>
        <Resx>`..\\Strings.ja.resx</Resx>
        <Resx>`..\\Strings.ko.resx</Resx>
    </ResourceFiles>
    ```
    
7.  Save the file.

# Using Localized Strings in the Game

The localized strings will be available in your game as a class with the same name as the base file name of the resource file for the default language (for example, Strings).

Setting the **Culture** property of this class from the [CultureInfo.CurrentCulture](http://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.currentculture.aspx) property will cause the correct language version of the localized strings to be loaded according to the platform's current culture setting.


    Strings.Culture = CultureInfo.CurrentCulture;

# See Also

[Extending a Standard Content Processor](CP_Extend_Processor.md)  
[Adding New Content Types](CP_Content_Advanced.md)  
[MonoGame Overview](MonoGame_Overview.md)  

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.
