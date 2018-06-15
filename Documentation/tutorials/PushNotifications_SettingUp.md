

# Setting up Push Notifications

# Modifying the Application Manifest File

To use push notifications, the game project's Properties\\WMAppManifest.xml file must contain an <App> element with a Publisher attribute. For example:

              `<App Publisher="Contoso" ... >` 
            

In the same file, the push notification capability also must be enabled:

              `<Capabilities>
        <Capability Name="ID_CAP_PUSH_NOTIFICATION" />
    <Capabilities>` 
            

# Setting Up a Notification Channel

Push notifications are sent to devices via a notification channel. Once created, a channel persists even after the application closes. When a game initializes, it should query for existence of its channel before attempting to create a new one.

### To set up a notification channel

1.  Call **HttpNotificationChannel.Find** with the channel's name to check to see if the channel has already been created for this device.
    
2.  If the channel already exists, use the **HttpNotificationChannel.Uri** property to get the Uniform Resource Identifier (URI) for the device.
    
3.  If the channel doesn't exist, create a new **HttpNotificationChannel**, passing it the name of the channel, and the URI for the web service that will be sending the notification requests.
    
    ![](bp.gif)Best Practice
    
    When creating an **HttpNotificationChannel**, titles should use the fully qualified domain name (FQDN) as the service name. For authenticated notifications, this FQDN must match the registered certificate's subject name (the CN attribute), for example, www.contoso.com. For more information about authenticated notifications, see: [Push Notifications](PushNotificationsPhone.md).
    

This procedure is shown in the following code:

              `// Try to find an existing channel
    HttpNotificationChannel httpChannel = HttpNotificationChannel.Find("MyChannel");

    if (null == httpChannel)
    {
        httpChannel = new HttpNotificationChannel("MyChannel", "www.contoso.com");
        
        // handle Uri notification events
        httpChannel.ChannelUriUpdated += 
            new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);

        httpChannel.Open();
    }
    else
    {
        // the channel already exists.  httpChannel.ChannelUri contains the device’s
        // unique locator	
    }

    // handle error events
    httpChannel.ErrorOccurred += 
        new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ErrorOccurred);` 
            

![](bp.gif)Best Practice

You should detect and respond to events about the channel status or incoming push notifications, and catch any errors that occur. The **HttpChannel.ChannelUriUpdated** event is particularly important, since the **NotificationChannelEventArgs** will contain the URI that uniquely identifies the mobile device when a new channel is opened. Any notification intended for a user's phone must be sent using the URI for that device.

# See Also

[Push Notifications](PushNotificationsPhone.md)  

[Responding to Push Notifications on the Client](PushNotifications_Client.md)  

[Pushing Notifications from a Server](PushNotifications_Server.md)  

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team