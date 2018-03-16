

# Pushing Notifications from a Server

Before a push notification can be sent to a client phone, the phone must have created a push notification channel with the server as described in [Setting up Push Notifications](PushNotifications_SettingUp.md). Once the push notification channel has been set up and the server has received the Uniform Resource Identified (URI) that identifies the phone, the server can send an Hypertext Transport Protocol (HTTP) request to the URI to send a notification to the phone.

The precise format of the request sent to the phone depends on the type of notification being sent, but the general procedure for creating a notification request is similar for all notification types.

### To send a push notification to a client phone

1.  Create an HTTP web request with the X-MessageID, X-WindowsPhoneTarget, and X-NotificationClass elements in the HTTP header. Each of these elements is described later in this topic.
2.  Send the HTTP request with a payload appropriate for the notification type. Raw notifications do not have a particular payload format.

An example of creating and sending such a request is provided here:

## C#

                `// create a payload for a toast notification
    string msg =
        "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
        "<wp:Notification xmlns:wp=\"WPNotification\">" +
           "<wp:Toast>" +
           "<wp:Text1><_string_></ltwp:Text1>" +
           "<wp:Text2><_string_></wp:Text2>" +
           "</ltwp:Toast>" +
        "</wp:Notification>";
    
    byte[] msgBytes = new UTF8Encoding().GetBytes(msg);
    
    // create a web request that identifies the payload as a toast notification
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(deviceUri);
    request.Method = WebRequestMethods.Http.Post;
    request.ContentType = "text/xml";
    request.ContentLength = msg.Length;
    request.Headers["X-MessageID"] = Guid.NewGuid().ToString();
    request.Headers["X-WindowsPhone-Target"] = "toast";
    request.Headers["X-NotificationClass"] = "2";
    
    // post the payload
    Stream requestStream = request.GetRequestStream();
    requestStream.Write(msgBytes, 0, msgBytes.Length);
    requestStream.Close();` 

When the device receives the notification, it can handle it as described in [Responding to Push Notifications on the Client](PushNotifications_Client.md).

# HTTP Header Elements

There are three HTTP header elements that are important when sending a push notification to a phone:

Element

Description

X-MessageID

An optional GUID that can be used by the sender and recipient to uniquely identify each message. If an X-MessageID value is present in the request, it will be copied to the response.

X-WindowsPhone-Target

Specifies the type of push notification. It can be either "token" (for tile notifications) or "toast" (for toast notifications). If the X-WindowsPhone-Target header is not present, the notification type is raw.

X-NotificationClass

"X-NotificationClass" determines the priority of the notification message by specifying one of three batching intervals:

*   Realtime – The notification is sent as soon as possible.
*   Priority – The notification is delivered within 450 seconds.
*   Regular – The notification is delivered within 900 seconds.

Each of these batching intervals uses a different set of values.

The values that correspond to batching intervals for each type of notification are provided here:

Batching Interval

Tile

Toast

Raw

Realtime

1

2

3–10

Priority

11

12

13–20

Regular

21

22

23–31

# HTTP Body Elements

The body (also referred to as _payload_) of a push notification HTTP request is highly variable, and depends on the type of notification that is being sent. For raw notifications, the body of the request is not defined, and can contain whatever data you like. For toast and tile notifications, the contents must be specified in a particular format.

## HTTP Body for Toast Notifications

Toast Notifications can send two strings that will be displayed in the popup on the phone. These are enclosed in the body of the HTTP request as shown:

    <?xml version=\\"1.0\\" encoding=\\"utf-8\\"?>
    <wp:Notification xmlns:wp=\\"WPNotification\\">
       <wp:Toast>
          <wp:Text1>_string_</wp:Text1>
          <wp:Text2>_string_</wp:Text2>
       </wp:Toast>
    </wp:Notification>
            

## HTTP Body for Tile Notifications

A tile notification contains a title string, a local or remote URI to a background image, and an optional count element. The use of the count element is up to the game, but displays on the game's tile image on the gamer's phone.

Background image, count, and title are all strings in the message payload. If the background image URI is a remote resource, the maximum allowed size of the tile image is 80 KB, with a maximum download time of one minute. If the tile image size is too large, or if the download time is greater than one minute, the default tile of the application is displayed instead.

These elements are enclosed in the body of the HTTP request as shown:

    <?xml version=\\"1.0\\" encoding=\\"utf-8\\"?>
    <wp:Notification xmlns:wp=\\"WPNotification\\">
       <wp:Tile>
          <wp:BackgroundImage>_background image path_</wp:BackgroundImage>
          <wp:Count>_count_</wp:Count>
          <wp:Title>_count_</wp:Title>
       </wp:Tile>
    </wp:Notification>
            

# See Also

[Push Notifications](PushNotificationsPhone.md)  

[Setting up Push Notifications](PushNotifications_SettingUp.md)  

[Responding to Push Notifications on the Client](PushNotifications_Client.md)  

© 2012 Microsoft Corporation. All rights reserved.  
Version: 2.0.61024.0