

# Adding an XML Content File to a Visual Studio Project

Describes how to add custom game data as an XML file through the Content Pipeline.

# Adding XML files to game content

Custom game data that is expressed in an XML format can be easily integrated into your game through the XNA Game Studio Content Pipeline.

This example demonstrates the procedure for integrating custom XML data into the content project of a simple XNA Game Studio game for the Windows platform.

### To define game data

Within the XNA Game Studio solution, you create a new Windows Game Library project.

1.  Right-click the solution node, point to **Add**, click **New Project**, and then select the Windows Game Library template.
    
    ![](note.gif)Note
    
    A Windows Game Library project is created instead of a Content Pipeline Extension Library project so that the class we will define can be used by both the Content Importer that runs at build-time and the Content Loader at game runtime.
    
2.  In the **Name** box, type **MyDataTypes**, and then click **OK**.
    
3.  In **Solution Explorer**, double-click Class1.cs to edit the new library project file.
    
4.  Replace the default template with the following code to define the class.
    
    ```
    namespace MyDataTypes
    {
        public class PetData
        {
            public string Name;
            public string Species;
            public float Weight;
            public int Age;
        }
    }
    ```
    

### To add an XML file to game content

In this procedure, the "MyDataTypes" library is added as a reference in the content project.

1.  In **Solution Explorer**, right-click the game content project, point to **Add Reference**, click the **Projects** tab, select **MyDataTypes**, and then click **OK**.
    
    A new content item is created next.
    
2.  In **Solution Explorer**, right-click the game content folder, point to **Add**, and then click **New Item**.
    
3.  In the **Add New Item** dialog box, select **XML file (.xml)** in the Templates pane.
    
4.  In the **Name** box, type **pets** as the file name, and then click **OK**.
    
    The new "pets.xml" file automatically opens for editing.
    
5.  Replace the contents of the template file with the following XML code:
    

When you press F6 to build the solution, it should build successfully, including the custom game content imported from the XML file.

To load the data at runtime, see the tutorial [Loading XML Content at Runtime](CP_XML_HowTo_Load.md).

# See Also

[Using an XML File to Specify Content](CP_XML_Overview.md)  
[Adding Content to a Game](CP_TopLevel.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team