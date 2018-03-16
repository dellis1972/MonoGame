

# Simulating Trial Mode for Marketplace Content

Implementing a trial mode in your game allows players download and try your game for free to make a better decision about whether or not to purchase the full version of your game. This can help advertise your game (by providing a good experience of what your gameplay has to offer players) and also increases the chance that a player will download your game to try it (since the download itself is free).

Because a conversion from trial mode to full mode requires only a license change, players don't need to re-download the entire game again, and you can retain the players saved game from trial to full mode, providing your players with a pleasant, seamless experience when purchasing your game.

![](note.gif)Note

It is unnecessary to write special code to ensure that a Marketplace offer is available to players of Xbox LIVE Indie Games. By default, released trial mode games present a Marketplace offer to the player when the gameplay time limit expires. You might, however, want to write special code in your game to present a Marketplace offer, or to control other areas of gameplay when a game is in trial mode.

# Detecting and Simulating Trial Mode

### To detect trial mode

1.  Call [Guide.IsTrialMode](P_Microsoft_Xna_Framework_GamerServices_Guide_IsTrialMode.md) to determine if your game is currently in trial mode.
2.  If [Guide.IsTrialMode](P_Microsoft_Xna_Framework_GamerServices_Guide_IsTrialMode.md) is **true**, call [Guide.ShowMarketplace](M_Microsoft_Xna_Framework_Graphics_Guide_ShowMarketplace.md) to present an offer to purchase the full version of the game.

              `if (Guide.IsTrialMode)
{
    Guide.ShowMarketplace(signedInGamer.PlayerIndex);
}`
            

![](bp.gif)Best Practice

Getting the value of [IsTrialMode](P_Microsoft_Xna_Framework_GamerServices_Guide_IsTrialMode.md) on Windows Phone is a synchronous (blocking) operation that can take 60ms or more to complete when your game is in trial mode.

To prevent significant delays in your game's framerate, don't place this call in your game's [Draw](M_Microsoft_Xna_Framework_Game_Draw.md) or [Update](M_Microsoft_Xna_Framework_Game_Update.md) methods. It's best to check this property in the [Game.OnActivated Method](M_Microsoft_Xna_Framework_Game_OnActivated.md) method when your game is initializing or returning from an interruption in gameplay.

Testing trial mode in your game requires that you simulate trial mode before your game is on the Marketplace.

### To simulate trial mode

*   Set [Guide.SimulateTrialMode](P_Microsoft_Xna_Framework_GamerServices_Guide_SimulateTrialMode.md) to **true** to test presentation of a Marketplace offer if the game is in trial mode.
    
    This property is typically set in the game's [constructor](M_Microsoft_Xna_Framework_Game_ctor.md), and must be set before the first call to [Game.Update](M_Microsoft_Xna_Framework_Game_Update.md).
    

              `#if DEBUG
            Guide.SimulateTrialMode = true;
#endif`
            

![](bp.gif)Best Practice

Enclose any calls to [SimulateTrialMode](P_Microsoft_Xna_Framework_GamerServices_Guide_SimulateTrialMode.md) in your code with an #if DEBUG block, so that this code does not appear in your released game.

# See Also

#### Windows Phone Development

[Creating Trial Applications for Windows Phone](http://go.microsoft.com/fwlink/?LinkId=254837)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0