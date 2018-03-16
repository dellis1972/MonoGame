

# Getting Gamer Profiles

Demonstrates how to retrieve gamertags and pictures using a technique that also can be applied to retrieve other gamer profile information.

# Retrieving Gamertags and Gamer Pictures

### To retrieve a player's gamertag and profile picture

*   Use [Gamer.GetProfile](M_Microsoft_Xna_Framework_GamerServices_Gamer_GetProfile.md) to retrieve the profile information for any player in the game.
    
    ```
    foreach (NetworkGamer gamer in session.AllGamers)
    {
        string text = gamer.Gamertag;
        GamerProfile gamerProfile = gamer.GetProfile();
        Texture2D picture = Texture2D.FromStream(this.GraphicsDevice, gamerProfile.GetGamerPicture());
    }
    ```
    

![](note.gif)Note

This method completes quickly when you use it with a locally signed-in profile. It can, however, take some time if you call it on a remote gamer instance. For a remote case, you might use the non-blocking alternative [Gamer.BeginGetProfile](M_Microsoft_Xna_Framework_GamerServices_Gamer_BeginGetProfile.md).

The [GamerProfile](T_Microsoft_Xna_Framework_GamerServices_GamerProfile.md) returned by [Gamer.GetProfile](M_Microsoft_Xna_Framework_GamerServices_Gamer_GetProfile.md) also contains such information as the motto, gamerscore, and more.

If the gamer is a [SignedInGamer](T_Microsoft_Xna_Framework_GamerServices_SignedInGamer.md), you can retrieve the local player's preferred settings for gameplay by using the [SignedInGamer.GameDefaults](P_Microsoft_Xna_Framework_Graphics_SignedInGamer_GameDefaults.md) property. You also can retrieve the local player's privileges by using [SignedInGamer.Privileges](P_Microsoft_Xna_Framework_Graphics_SignedInGamer_Privileges.md). Unlike [Gamer.GetProfile](M_Microsoft_Xna_Framework_GamerServices_Gamer_GetProfile.md), which can be slow for remote players, the properties of a [SignedInGamer](T_Microsoft_Xna_Framework_GamerServices_SignedInGamer.md) return instantly.

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0