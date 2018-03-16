

# Writing Data

Windows Phone SDK 8.0 Extensions for XNA Game Studio 4.0 does not provide access to writeable storage on Windows Phone. To access such storage, you'll need to use classes from the [System.IO.IsolatedStorage](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.aspx) namespace.

For Windows Phone projects, Microsoft Visual Studio automatically adds the assembly containing System.IO.IsolatedStorage to your project. There is no need to add any additional references to your project.

For detailed information about using Isolated Storage in games, see [Local Data Storage for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=254759) in the Windows Phone documentation.

# Complete Sample

The code in this topic shows you the technique. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download MouseInput.zip](http://go.microsoft.com/fwlink/?LinkId=258714)

### To save game data with System.IO.IsolatedStorage

1.  Add a **using System.IO.IsolatedStorage** statement at the beginning of the source file in which you'll be using the namespace.
    
                        `using System.IO.IsolatedStorage;`
                      
    
2.  Use [IsolatedStorageFile.GetUserStoreForApplication](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.getuserstoreforapplication.aspx) to get an [IsolatedStorageFile](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.aspx) object that can be used to create files and directories, or to read and write existing files.
    
    ![](note.gif)Note
    
    When [IsolatedStorageFile.GetUserStoreForApplication](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.getuserstoreforapplication.aspx) is called within an XNA Game Studio game for Windows (but not for Xbox 360 or Windows Phone), an InvalidOperationException will result. To avoid this exception, use the [GetUserStoreForDomain](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.getuserstorefordomain.aspx) method instead.
    
                                `#if WINDOWS
                IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();
    #else
                IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
    #endif`
                              
    
3.  Use [IsolatedStorageFile.OpenFile](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.openfile.aspx) to open a file for writing, and use the returned [IsolatedStorageFileStream](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefilestream.aspx) to write data to the file.
    
                        `        protected override void OnExiting(object sender, System.EventArgs args)
            {
                // Save the game state (in this case, the high score).
    #if WINDOWS
                IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain();
    #else
                IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication();
    #endif
    
                // open isolated storage, and write the savefile.
                IsolatedStorageFileStream fs = null;
                using (fs = savegameStorage.CreateFile(SAVEFILENAME))
                {
                    if (fs != null)
                    {
                        // just overwrite the existing info for this example.
                        byte[] bytes = System.BitConverter.GetBytes(highScore);
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
    
                base.OnExiting(sender, args);
            }`
                      
    

Reading saved data written in this way uses a very similar process. For example:

                  `        protected override void Initialize()
        {
            // open isolated storage, and load data from the savefile if it exists.
#if WINDOWS
            using (IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForDomain())
#else
            using (IsolatedStorageFile savegameStorage = IsolatedStorageFile.GetUserStoreForApplication())
#endif
            {
                if (savegameStorage.FileExists(SAVEFILENAME))
                {
                    using (IsolatedStorageFileStream fs = savegameStorage.OpenFile(SAVEFILENAME, System.IO.FileMode.Open))
                    {
                        if (fs != null)
                        {
                            // Reload the saved high-score data.
                            byte[] saveBytes = new byte[4];
                            int count = fs.Read(saveBytes, 0, 4);
                            if (count > 0)
                            {
                                highScore = System.BitConverter.ToInt32(saveBytes, 0);
                            }
                        }
                    }
                }
            }
            base.Initialize();
        }`
                

![](note.gif)Note

[IsolatedStorageFile](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.isolatedstoragefile.aspx) provides many more methods than are shown here, including support for asynchronous reads and writes. For complete documentation, see [Local Data Storage for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=254759) in the Windows Phone documentation, and the [IsolatedStorage](http://msdn.microsoft.com/en-us/library/system.io.isolatedstorage.aspx) reference on MSDN.

# See Also

#### Tasks

[Handling Interruptions on Windows Phone](RespondingtoShutdownEvents.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0