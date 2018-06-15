

# Responding to Push Notifications on the Client

When a raw or toast notification is received by a running game on Windows Phone, the notification is sent to the game and can be handled by an event handler. Tile notifications cannot be intercepted by game code.

![](note.gif)Note

For more information about push notification types, see [Push Notifications](PushNotificationsPhone.md#push notification types).

# Handling Toast Notifications

### To handle a toast notification

1.  Instantiate a HttpChannel object for the notification channel. For more information, see [Setting up Push Notifications](PushNotifications_SettingUp.md).
2.  Create an event handler for the toast notification that takes a **NotificationEventArgs** parameter.
3.  Add the handler to the HttpChannel's **ShellToastNotificationReceived** property.
4.  Call **BindToShellToast** on the HttpChannel.

The following code demonstrates this process:

              `// set up event handler for toast notifications
    httpChannel.ShellToastNotificationReceived += 
        new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);
    
    // add a toast message subscription to the channel.
    httpChannel.BindToShellToast();` 
            

# Handling Raw Notifications

### To handle a raw notification

1.  Instantiate a HttpChannel object for the notification channel. For more information, see [Setting up Push Notifications](PushNotifications_SettingUp.md).
2.  Create an event handler for the toast notification that takes a **HttpNotificationEventArgs** parameter.
3.  Add the handler to the HttpChannel's **HttpNotificationReceived** property.

The following code demonstrates this process:

              `// get notification of raw messages when my game is running
    httpChannel.HttpNotificationReceived += 
        new EventHandler<HttpNotificationEventArgs>(httpChannel_HttpNotificationReceived);` 
            

# See Also

[Push Notifications](PushNotificationsPhone.md)  

[Setting up Push Notifications](PushNotifications_SettingUp.md)  

[Pushing Notifications from a Server](PushNotifications_Server.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team