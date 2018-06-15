

# Accessing Pictures from a Picture Album

This section demonstrates how to access pictures in a picture album by using the Media API on Windows Phone.

# Complete Sample

The code in the topic shows you the technique for using Media API to access pictures. You can download a complete code sample for this topic, including full source code and any additional supporting files required by the sample.

[Download ShowPictures.zip](http://go.microsoft.com/fwlink/?LinkId=258727)

# Accessing Pictures

This example retrieves and displays pictures from a device's media library.

### To retrieve pictures

1.  Declare variables.
    
                          `ICollection<MediaSource> mediaSources;
    MediaLibrary mediaLib;
    PictureCollection picCollection;        
    Texture2D pic;`
                        
    
2.  Call [MediaSource.GetAvailableMediaSources](M_MXFM_MediaSource_GetAvailableMediaSources.md) to retrieve the available media on the device.
    
                          `// Retrieve all available media libraries on the device
    mediaSources = MediaSource.GetAvailableMediaSources();`
                        
    
3.  Call [MediaLibrary](O_M_MXFM_MediaLibrary_ctor.md) to get the media libraries from the device.
    
                          `mediaLib = new MediaLibrary();`
                        
    
4.  Obtain the picture collection from the media library.
    
                          `picCollection = mediaLib.Pictures;`
                        
    
5.  Get a picture's texture from the collection.
    
                          `pic = Texture2D.FromStream(GraphicsDevice, picCollection[currentPic].GetImage());`
                        
    
6.  Display the picture on-screen.
    
                          `protected override void Draw(GameTime gameTime)
    {           
        GraphicsDevice.Clear(Color.CornflowerBlue);
        Vector2 pos = Vector2.Zero;
        spriteBatch.Begin();
        spriteBatch.Draw(pic, pos, Color.White);
        spriteBatch.End();
    
        base.Draw(gameTime);
    }`
                        
    

# Concepts

[Playing a Song from a URI](Media_HowTo_PlaySongfromURI.md)

Demonstrates how to use the [MediaPlayer](T_MXFM_MediaPlayer.md) to play a song from a Uniform Resource Identifier (URI).

[Displaying Pictures and Playing Video](Media.md)

Provides overviews about how to use the Media API to retrieve system media, including pictures, songs, and video.

# Reference

[MediaLibrary Class](T_MXF_Media_MediaLibrary.md)

Provides access to songs, playlists, and pictures in the device's media library.

[PictureCollection Class](T_MXF_Media_PictureCollection.md)

A collection of pictures in the media library.

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team