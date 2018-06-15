

# Using an XML File to Specify Content

Game assets managed through the Content Pipeline include graphic items such as textures, models and meshes; sound files such as dialogue or music; and custom data that governs the behavior of the game.

Data tables, for example, are custom data that might describe different characters’ attributes or the features of each level in the game. The content and format of this data is specific to the requirements of the game. Custom game data in the form of an XML file also can be loaded into your game through the standard features of the Content Pipeline.

When the Content Pipeline is used, the game does not have to parse the XML format in which the game data is originally stored. Data loaded by the game through [ContentManager](T_Microsoft_Xna_Framework_Content_ContentManager.md) is read in deserialized form directly into a managed code object.

# In This Section

[Creating an XML File](CP_XML_Serializer.md)

Describes how to use [IntermediateSerializer](T_Microsoft_Xna_Framework_Content_Pipeline_Serialization_Intermediate_IntermediateSerializer.md) from a Windows application to generate XML content to add to a XNA Game Studio application.

[Adding an XML Content File to a Visual Studio Project](CP_XML_HowTo_Add.md)

Describes how to add custom game data as an XML file through the Content Pipeline.

[Loading XML Content at Runtime](CP_XML_HowTo_Load.md)

Describes how to load custom game data at game runtime through the Content Pipeline.

[XML Elements for XMLImporter](CP_XML_Elements.md)

Describes the elements of an XML file that can be processed by the [XmlImporter Class](T_Microsoft_Xna_Framework_Content_Pipeline_XmlImporter.md).

[Sprite Font XML Schema Reference](CP_SpriteFontSchema.md)

Describes the valid tags and values for Sprite-Font (.spritefont) XML files used by the Content Pipeline to create [SpriteFont](T_Microsoft_Xna_Framework_Graphics_SpriteFont.md) textures.

# See Also

#### Concepts

[Adding Content to a Game](CP_TopLevel.md)  
[Writing Game Code](ProgrammingGuide.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team