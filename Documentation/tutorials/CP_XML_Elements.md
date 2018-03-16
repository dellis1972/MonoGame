

# XML Elements for XMLImporter

# XML Elements

The following base elements are recognized by [XmlImporter Class](T_Microsoft_Xna_Framework_Content_Pipeline_XmlImporter.md):

Element

Parent

Children

Description

<XnaContent>

—

<Asset>

Top-level tag for XNA Content.

<Asset>

<XnaContent>

<Item>

Marks the asset. The _Type_ attribute specifies the corresponding namespace and class of the matching data type.

<Item>

<Asset>

—

When the asset contains multiple objects (as in an array), marks a single object within the group. The child elements correspond to the properties of the data type's class definition.

# Examples

## Example 1: Single Object

This example demonstrates an XML file that defines an asset that consists of a single item (not an array).

Assume that the XML file is to define a single object of data for the class that is defined as:

                `namespace XMLTest
{
    class MyTest
    {
        public int elf;
        public string hello;
    }
}` 
              

The XML file that specifies the data that the Content Loader will read into the object would appear as:

## Example 2: Multiple Objects

This example demonstrates an XML file that defines an asset that is an array of objects.

Assume that the XML file is to define an array of data for the class that is defined as:

                `
namespace MyDataTypes
{
    public class CatData
    {
        public string Name;
        public float Weight;
        public int Lives;
    }
}`
              

The XML file that specifies the data that the Content Loader will read into the object array would appear as:

# See Also

[Using an XML File to Specify Content](CP_XML_Overview.md)  
[Adding Content to a Game](CP_TopLevel.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0