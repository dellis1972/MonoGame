

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

![](note.gif)Windows Phone Specific Information

To deploy resource files, Windows Phone developers must add values to the **<SupportedCultures>** element in the Visual Studio Project file (.csproj) for the Windows Phone game. For more information about how to use **<SupportedCultures>**, please see [How to: Build a Localized Application for Windows Phone](http://msdn.microsoft.com/library/ff637520(VS.92).aspx).

1.  Close the Visual Studio Project file (.csproj) for the Windows Phone game project, then open the project file in a text editor.
2.  Locate the **<SupportedCultures>** element and set the value to a semicolon delimited list of all supported cultures for the game. The default UI culture does not need to be included in this tag. For example:
    
    <SupportedCultures>ja-jp</SupportedCultures>

# Associating Localization Data and Processors With Fonts

Now that project data is divided into resource files, and has a custom content processor to consume it, the parts need to be bound together to help them work. This is accomplished by providing extended information in a .spritefont file, and binding the file to the custom content processor.

### To extend the .spritefont file

Create a new .spritefont file.

1.  In **Solution Explorer**, right-click the content project node, select **Add**, and then click **New Item**.
    
2.  In the **Add New Item** dialog box, select "Sprite Font," and then in the **Name** box, enter a name (for example, Font.spritefont) for the new file.
    
3.  Click **Add** to create the new file.
    
4.  Double-click the newly created file to open it for editing.
    
5.  Change the asset type declaration to reference the previously created extended FontDescriptor class.
    
    ```
    <Asset Type="LocalizationPipeline.LocalizedFontDescription">
    ```
                      
    
6.  Add a block within <ResourceFiles> tags that lists each resource file using the <Resx> elements.
    
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

### To assign the cusom content processor to the .spritefont file

1.  Compile the solution to build the Content Pipeline Extension Library project.
    
2.  In **Solution Explorer**, right-click the project, and then click **Add Reference**.
    
3.  In the **Add Reference** dialog box, click the **Projects** tab, and then select the previously created Content Pipeline Extension Library project (for example, LocalizationPipeline).
    
4.  Click **OK**.
    
5.  In the content project, right-click the .spritefont file, and then select **Properties**.
    
6.  In the resulting **Properties** pane, choose your custom processor (for example, LocalizedFontProcessor) from the drop-down list associated with the **ContentProcessor** field.
    

# Using Localized Strings in the Game

The localized strings will be available in your game as a class with the same name as the base file name of the resource file for the default language (for example, Strings).

Setting the **Culture** property of this class from the [CultureInfo.CurrentCulture](http://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.currentculture.aspx) property will cause the correct language version of the localized strings to be loaded according to the platform's current culture setting.


    Strings.Culture = CultureInfo.CurrentCulture;

# See Also

[Extending a Standard Content Processor](CP_Extend_Processor.md)  
[Adding New Content Types](CP_Content_Advanced.md)  
[Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0](XNA_Overview.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0