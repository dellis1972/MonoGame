

# Getting Gamer Profiles

Demonstrates how to retrieve gamertags and pictures using a technique that also can be applied to retrieve other gamer profile information.

# Retrieving Gamertags and Gamer Pictures

### To retrieve a player's gamertag and profile picture

*   Use [Gamer.GetProfile](xref:Microsoft.Xna.Framework.GamerServices.Gamer.GetProfile) to retrieve the profile information for any player in the game.
    
    ```
    foreach (NetworkGamer gamer in session.AllGamers)
    {
        string text = gamer.Gamertag;
        GamerProfile gamerProfile = gamer.GetProfile();
        Texture2D picture = Texture2D.FromStream(this.GraphicsDevice, gamerProfile.GetGamerPicture());
    }
    ```
    

![](note.gif)Note

This method completes quickly when you use it with a locally signed-in profile. It can, however, take some time if you call it on a remote gamer instance. For a remote case, you might use the non-blocking alternative [Gamer.BeginGetProfile](xref:Microsoft.Xna.Framework.GamerServices.Gamer.BeginGetProfile).

The [GamerProfile](xref:Microsoft.Xna.Framework.GamerServices.GamerProfile) returned by [Gamer.GetProfile](xref:Microsoft.Xna.Framework.GamerServices.Gamer.GetProfile) also contains such information as the motto, gamerscore, and more.

If the gamer is a [SignedInGamer](xref:Microsoft.Xna.Framework.GamerServices.SignedInGamer), you can retrieve the local player's preferred settings for gameplay by using the [SignedInGamer.GameDefaults](xref:Microsoft.Xna.Framework.Graphics.SignedInGamer.GameDefaults) property. You also can retrieve the local player's privileges by using [SignedInGamer.Privileges](xref:Microsoft.Xna.Framework.Graphics.SignedInGamer.Privileges). Unlike [Gamer.GetProfile](xref:Microsoft.Xna.Framework.GamerServices.Gamer.GetProfile), which can be slow for remote players, the properties of a [SignedInGamer](xref:Microsoft.Xna.Framework.GamerServices.SignedInGamer) return instantly.

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team