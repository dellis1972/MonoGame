

# Adding a Custom Importer

XNA Game Studio provides standard importers and processors for a number of common file formats used to store such basic game assets as models, materials effects, sprites, textures, and so on. For a list of file formats supported by these standard importers and processors, see [Standard Content Importers and Content Processors](CP_StdImpsProcs.md).

![](security.gif)Security Note

Before you open an existing project or component, determine the trustworthiness of the code outside of the Visual Studio designer. Opening projects or components in the Visual Studio designer automatically executes that code on your local machine in the trusted process of VCSExpress.exe or DevEnv.exe.

### To add a custom importer or processor to a game project

This procedure assumes you have copied the new importer or processor to a local folder in the game project.

1.  Open XNA Game Studio.
2.  Load the solution associated with your game.
3.  In **Solution Explorer**, right-click Content, and then click **Add Reference**.
4.  Navigate to the directory containing the assembly with the custom importer or processor, and then add it to the solution.
5.  Save the solution.

The new importer or processor now appears as a choice in [using dialog properties](UsingXNA_Dlg_Properties.md) for importing or processing a newly added game asset.

# See Also

[Adding New Content Types](CP_Content_Advanced.md)  
[Standard Content Importers and Content Processors](CP_StdImpsProcs.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0