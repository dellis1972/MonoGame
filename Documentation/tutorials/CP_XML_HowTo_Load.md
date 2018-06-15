

# Loading XML Content at Runtime

Describes how to load custom game data at game runtime through the Content Pipeline.

# Loading custom game content

This example concludes the procedure begun in the tutorial [Adding an XML Content File to a Visual Studio Project](CP_XML_HowTo_Add.md).

Once custom game data is integrated as game content from an XML file through the Content Pipeline, it exists within your game runtime package in binary format. The data can be [loaded at runtime](AppModel_HowTo_LoadResources.md) through the [ContentManager](T_Microsoft_Xna_Framework_Content_ContentManager.md).

### To load the custom data in the game

Add the "MyDataTypes" library as a reference in the game project.

1.  In **Solution Explorer**, right-click the game project, click **Add Reference**, click the **Projects** tab, select **MyDataTypes**, and then click **OK**.
    
2.  In **Solution Explorer**, double-click Game1.cs to edit it.
    
3.  Add the `using` declaration for the MyDataTypes namespace.
    
    ```
    using MyDataTypes;
    ```
    
4.  Add a data declaration for an array of type PetData, the class defined in the "MyDataTypes" library.
    
    ```
    PetData[] pets;
    ```
    
5.  In the [LoadContent](M_MXF_Game_LoadContent.md) override function, load the custom content.
    
    ```
    protected override void LoadContent()
    {
        // Load the pet data table
        pets = Content.Load<PetData[]>("pets");
    }
    ```
    
    The custom game data now resides in the array of PetData objects.
    

# See Also

[Using an XML File to Specify Content](CP_XML_Overview.md)  
[Adding Content to a Game](CP_TopLevel.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team