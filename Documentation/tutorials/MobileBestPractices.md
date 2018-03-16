

# Best Practices for Windows Phone Games

The practices discussed here will help you have the most success possible with your Windows Phone game.

This overview covers the following topics.

*   [Polish Your Game](#ID4EZ)
*   [Build Intuitive and Fun Controls](#ID4EKB)
*   [Support Changing Screen Orientation](#ID4E4C)
*   [Audio Tips](#ID4EAE)
*   [Respond Correctly to Back Button Use](#ID4EYE)
*   [Diligently Save Game State](#ID4EIG)

# Polish Your Game

It is difficult to over emphasize how important polish is for a successful game. The best game ideas and the most stable code do not compare to a game with an extra level of polish. Polish can be defined as putting in the extra effort to make your game look and feel its best. It also is:

*   The difference between a basic menu with buttons that just work and the same menu with polish. It may take time to add a small flourish of animation for each button press, sound effect, and styled buttons to a menu, but doing so makes a big difference.
*   Smooth menu operation and intuitive controls.
*   Smooth transitions among screens, modes, and levels.

# Build Intuitive and Fun Controls

Avoid simulating traditional controls such as thumbsticks in Windows Phone games. That type of control takes away useful space from the gameplay area and is not platform friendly. Use gestures for user input. Games that are engaging and naturally fun have controls that are natural to the platform.

Control

Description

Touch

Touch control systems will feel natural to Windows Phone users. Design games from the beginning to take full advantage of the touch screen. The touch screen is the primary way users interact with their phone, and users expect to interact with games in the same way.

Back Button

Although there are other buttons on the device, only the **Back** button is available to the game. Use the **Back** button for pausing and exiting the game.

Gestures

Design your gameplay to use touch gestures in natural ways. For example: allowing players to draw paths on the screen to direct gameplay, or allowing for group selection by stretching an on-screen rectangle around play pieces. Consider allowing navigation by dragging the landscape, and allowing users to rotate by touching and rotating two fingers.

# Support Changing Screen Orientation

Windows Phone supports three screen orientation views: portrait, landscape left, and landscape right. Portrait view is the default view for applications. The **Start** button is always presented in portrait view. In portrait view, the page is oriented vertically with the steering buttons displayed at the bottom of the phone.

In both landscape views, the **Status Bar** and **Application Bar** remain on the side of the screen that has the **Power** and **Start** buttons. Landscape left has the **Status Bar** on the left, and landscape right has the **Status Bar** on the right.

Routinely check current screen orientation and enable gameplay regardless of how the phone is held.

For more detailed information on screen rotation see [Automatic Rotation and Scaling](AutomaticRotation.md).

# Audio Tips

Audio can enrich an application and add needed polish. Playing audio should also dependent on user preference. Consider these tips when building audio into your game:

*   Play sound effects at an average volume to avoid forcing the player to adjust the phone's volume during the game.
*   Allow sound effects and background music to be turned on and off by users.
*   Play directional sounds that reflect a location of the originating element on the screen.

For more detailed information on audio see [Creating and Playing Sounds](Audio.md).

# Respond Correctly to Back Button Use

All Windows Phone games must respond to use of the **Back** button. Windows Phone consistently uses **Back** to move backward through the UI. Games must implement this behavior as follows:

*   During gameplay, the game can do one of the following:

*   Present a contextual pause menu (dialog). Pressing the **Back** button again while in the pause menu closes the menu and resumes the game.
*   Navigate the user to the prior menu screen. Pressing the **Back** button again should continue to return the user to the previous menu or page.

*   Outside of gameplay, such as within the game's menu system, pressing **Back** must return to the previous menu or page.
*   At the game’s initial (start) screen, pressing **Back** must exit the game.

It is a good practice in case the player exits the game to automatically save the game state while the pause menu is shown.

# Diligently Save Game State

On Windows Phone, a game might be exited at any time. An incoming call may interrupt gameplay, or the user might quit the game by using the **Home** or **Search** buttons to use other applications. We recommend saving game state whenever possible to protect the user's time investment in the game. We also recommend that you make a distinction between the automatically saved game state and the user's explicitly saved games. Automatically saved games should be viewed as a backup in case the game ends unexpectedly, but should not replace the user's ability to save the game at a chosen time or place.

If you implement automatic game saving, check for an automatically saved state when the game launches. If found, let the user choose to resume the game from the automatically saved state or from a specific manually saved game, if present. During the save process, we also recommend that you display a visual cue warning users not to press the **Search** or **Home** button because the action could cause the game to exit before the save is complete.

# See Also

[Creating a Windows Phone Game or Library Project](UsingXNA_CreatingMobileProject.md)  
[Debugging a Windows Phone Game](UsingXNA_DebuggingMobile.md)  
[Exploring Windows Phone Programming](Mobile_Programming.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0