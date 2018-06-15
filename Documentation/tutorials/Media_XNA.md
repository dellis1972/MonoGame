

# Media Overview

This topic provides a high-level overview about the capabilities—such as playing music and video and accessing pictures—of the Media API in XNA Game Studio. A game can query a media collection for songs, artists, albums, playlist, genres, pictures, and picture albums by using the [Microsoft.Xna.Framework.Media](N_MXF_Media.md) namespace. Items in the media library also can be searched based on metadata such as artist name, album name, and musical genre.

# Songs as Background Music

Access to the media library, combined with the ability to use playlists, allows games to create interesting background scores that can change with gameplay. Songs can be played directly from the media library, or can be imported by using the Content Pipeline. For more information, see [Playing a Song](Audio_HowTo_PlayASong.md).

# Pictures as Textures

Games can obtain and use textures from pictures within a media library. For more information, see [Accessing Pictures from a Picture Album](Media_HowTo_ShowPictures.md).

# Silverlight Constraints

Silverlight applications must ensure that [FrameworkDispatcher.Update](M_MXF_FrameworkDispatcher_Update.md) is called regularly in order for "fire and forget" sounds to work correctly. **Media Overview** throws an InvalidOperationException if [FrameworkDispatcher.Update](M_MXF_FrameworkDispatcher_Update.md) has not been called at least once before making this call. For more information, see [Enable XNA Framework Events in Windows Phone Applications](UsingXNAFrameworkInSilverlight.md).

# Concepts

[Accessing Pictures from a Picture Album](Media_HowTo_ShowPictures.md)

Demonstrates how to access pictures in a picture album using the Media API on Windows Phone.

[Playing a Song](Audio_HowTo_PlayASong.md)

Demonstrates how to play a song from a user's media library.

[Playing a Song from a URI](Media_HowTo_PlaySongfromURI.md)

Demonstrates how to use the [MediaPlayer](T_MXFM_MediaPlayer.md) to play a song from a Uniform Resource Identifier (URI).

# Reference

[Picture Class](T_MXF_Media_Picture.md)

Provides access to a picture in the media library.

[Song Class](T_MXF_Media_Song.md)

Provides access to a song in the song library.

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team