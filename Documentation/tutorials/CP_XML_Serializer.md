

# Creating an XML File

For complex game data, using a software tool to create and maintain these assets may be useful. Level tables, for example, might be easier to develop through a custom-level editor tool.

The [IntermediateSerializer](T_Microsoft_Xna_Framework_Content_Pipeline_Serialization_Intermediate_IntermediateSerializer.md) class of the XNA Framework can be employed by custom tools running under Windows to directly serialize game data to an XML file.

An XML file generated in this way then can be included in the game project and imported by [XmlImporter Class](T_Microsoft_Xna_Framework_Content_Pipeline_XmlImporter.md) as part of the Content Pipeline. Because [XmlImporter](T_Microsoft_Xna_Framework_Content_Pipeline_XmlImporter.md) is actually a wrapper for [IntermediateSerializer](T_Microsoft_Xna_Framework_Content_Pipeline_Serialization_Intermediate_IntermediateSerializer.md), it is certain that the XML file will be in the [correct format](CP_XML_Elements.md) to be deserialized by the same facility.

The [IntermediateSerializer](T_Microsoft_Xna_Framework_Content_Pipeline_Serialization_Intermediate_IntermediateSerializer.md) class is controlled through the **XmlWriter** class of the .NET Framework Class Library defined in **System.Xml**. The properties of the **XmlWriterSettings** class can be used to specify its output properties.

The serializer produces its output according to these rules:

*   All public fields and properties are serialized; a separate XML element is used for each.
*   Protected, private, or internal data is skipped.
*   Get-only or set-only properties are skipped.
*   Properties come before fields.
*   If there is more than one field or property, these are serialized in the order they are declared.
*   Nested types are serialized using nested XML elements.
*   When the class derives from another, members of the base class are serialized before data from the derived type.

# XML Serialization Example

The following steps create a simple program that demonstrates how a program can use the [IntermediateSerializer](T_Microsoft_Xna_Framework_Content_Pipeline_Serialization_Intermediate_IntermediateSerializer.md) method [IntermediateSerializer.Serialize Generic Method](M_Microsoft_Xna_Framework_Content_Pipeline_Serialization_Intermediate_IntermediateSerializer_Serialize``1.md) to serialize program data to an XML file.

## Step 1: Create a New Project

You will create a new project in Visual Studio.

1.  On the **File** menu, click **New**, and then click **Project**.
    
2.  In the **New Project** dialog box, ensure **Windows** is selected in the **Project types** pane, and then click **Console Application** in the **Templates** pane.
    
3.  In **Solution Explorer**, right-click the **References** folder, and then click **Add Reference**.
    
4.  In the **Add Reference** dialog box, select the **Microsoft.Xna.Framework.Content.Pipeline** assembly, and then click **OK**.
    

## Step 2: Add XML Serialization

1.  In **Solution Explorer**, double-click **Program.cs** to edit it.
    
2.  Add `using` declarations for the System.Xml and Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate namespaces.
    
                        `using System.Xml;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;`
                      
    
3.  Define a class to serialize to XML.
    
                        `namespace XMLSerializerTest
    {
        class MyData
        {
            public int elf = 23;
            public string hello = "Hello World";
        }
    }`
                      
    
4.  Within the function `Main`, add the following code. This code employs [IntermediateSerializer.Serialize Generic Method](M_Microsoft_Xna_Framework_Content_Pipeline_Serialization_Intermediate_IntermediateSerializer_Serialize``1.md) to serialize the **MyData** class as an XML file.
    
                        `MyData ExampleData = new MyData();
    
    XmlWriterSettings settings = new XmlWriterSettings();
    settings.Indent = true;
    
    using (XmlWriter writer = XmlWriter.Create("example.xml", settings))
    {
        IntermediateSerializer.Serialize(writer, ExampleData, null);
    }`
                      
    

## Step 3: Generate XML

1.  Press F5 to build and execute the program.
2.  Examine the example.xml file in the project's bin\\Debug folder.
    

# See Also

#### Concepts

[Using an XML File to Specify Content](CP_XML_Overview.md)  
[Adding Content to a Game](CP_TopLevel.md)  

#### Reference

[IntermediateSerializer Class](T_Microsoft_Xna_Framework_Content_Pipeline_Serialization_Intermediate_IntermediateSerializer.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team