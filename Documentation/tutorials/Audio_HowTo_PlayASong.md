

# Playing a Song

Demonstrates how to play a song from a user's media library.

# Complete Sample

The code in this tutorial illustrates the technique for playing a song from a library. A complete code sample for this tutorial is available for you to download, including full source code and any additional supporting files required by the sample.

[Download PlaySong_Sample.zip](http://go.microsoft.com/fwlink/?LinkId=258717)

The [Albums](P_MXF_Media_MediaLibrary_Albums.md) property provides access to the media library, and the [Play](O_M_MXFM_MediaPlayer_Play.md) method plays a song. Consider any current audio playback when using the [Play](O_M_MXFM_MediaPlayer_Play.md) method. If the user currently is playing a different song, the [Stop](M_MXFM_MediaPlayer_Stop.md) method can be used to stop the current song.

### To play a song from a random album in a user's media library

The following demonstrates how to play a song from a randomly picked album (shuffle).

1.  Declare [MediaLibrary](T_MXF_Media_MediaLibrary.md) and [Random](http://msdn.microsoft.com/en-us/library/system.random.aspx).
    
    ```
    MediaLibrary sampleMediaLibrary;
    Random rand;
    ```
    
2.  Initialize [MediaLibrary](T_MXF_Media_MediaLibrary.md) and the random number generator in the game's constructor.
    
    ```
    sampleMediaLibrary = new MediaLibrary();
    rand = new Random();
    ```
    
3.  Stop the current audio playback by calling [MediaPlayer.Stop](M_MXFM_MediaPlayer_Stop.md), and then generate a random number that will serve as a valid album index. Next, play the first track in the random album.
    
    ```
    MediaPlayer.Stop(); // stop current audio playback 
    
    // generate a random valid index into Albums
    int i = rand.Next(0, sampleMediaLibrary.Albums.Count - 1);
    
    // play the first track from the album
    MediaPlayer.Play(sampleMediaLibrary.Albums[i].Songs[0]);
    ```
    

# Concepts

[Playing a Song from a URI](Media_HowTo_PlaySongfromURI.md)

Demonstrates how to use the [MediaPlayer](T_MXFM_MediaPlayer.md) to play a song from a Uniform Resource Identifier (URI).

[Media Overview](Media_XNA.md)

Provides a high-level overview about the capabilities—such as playing music and video and accessing pictures—of the Media API in MonoGame.

# Reference

[MediaPlayer Class](T_MXFM_MediaPlayer.md)

Provides methods and properties to play, pause, resume, and stop songs. **MediaPlayer** also exposes shuffle, repeat, volume, play position, and visualization capabilities.

[MediaLibrary Class](T_MXF_Media_MediaLibrary.md)

Provides access to songs, playlists, and pictures in the device's media library.

[MediaPlayer.Play Method](O_M_MXFM_MediaPlayer_Play.md)

Plays a song or collection of songs.

[MediaLibrary.Albums Property](P_MXF_Media_MediaLibrary_Albums.md)

Gets the [AlbumCollection](T_MXF_Media_AlbumCollection.md) that contains all albums in the media library.

© 2012 Microsoft Corporation. All rights reserved.

© The MonoGame Team.
