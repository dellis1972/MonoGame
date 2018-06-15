

# Making Web Requests

Windows Phone is designed for network connectivity, and this ability can be used in games to make web requests using the Hypertext Transport Protocol (HTTP).

This topic will describe how to add HTTP web requests to your game to retrieve data, using an example that demonstrates retrieving an avatar image from XboxLIVE.com.

### To make a web request on Windows Phone

1.  Add a reference to the [System.Net](http://msdn.microsoft.com/en-us/library/system.net.aspx) namespace in your game project.
    
    1.  Open your project in Visual Studio, right-click **References** in **Solution Explorer**, and click **Add Reference**.
    2.  In the **.Net** tab, select **System.Net** in the list, and click **OK**.
    
    You should see **System.Net** in the list of **References** in your project.
    
2.  Add the [System.Net](http://msdn.microsoft.com/en-us/library/system.net.aspx) namespace to any source files that will use its classes and methods.
    
                        `using System.Net;`
                      
    
3.  Call **HttpWebRequest.Create** with a Uniform Resource Identifier (URI) to use for the request.
    
                        `// retrieve an avatar image from the Web
    string avatarUri = "http://avatar.xboxlive.com/avatar/" + gt +
        "/avatar-body.png";
    HttpWebRequest request =
        (HttpWebRequest)HttpWebRequest.Create(avatarUri);`
                      
    
4.  Call [HttpWebRequest.BeginGetResponse](http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.begingetresponse.aspx) with a callback function that will receive the response from the web server in the URI.
    
                        `request.BeginGetResponse(GetAvatarImageCallback, request);`
                      
    
    ![](note.gif)Note
    
    For more information about working with asynchronous requests and callback functions, see [Working with Asynchronous Methods in XNA Game Studio](AsyncProgramming.md).
    
5.  In the callback, use [HttpWebRequest.EndGetResponse](http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.endgetresponse.aspx) to get a [WebResponse](http://msdn.microsoft.com/en-us/library/system.net.webresponse.aspx) object, which can be used to retrieve the data returned in the response.
    
    ![](bp.gif)Best Practice
    
    Be sure to catch the possible [WebException](http://msdn.microsoft.com/en-us/library/system.net.webexception.aspx) that can be generated from [EndGetResponse](http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.endgetresponse.aspx).
    
                        `void GetAvatarImageCallback(IAsyncResult result)
    {
        HttpWebRequest request = result.AsyncState as HttpWebRequest;
        if (request != null)
        {
            try
            {
                WebResponse response = request.EndGetResponse(result);
                avatarImg = Texture2D.FromStream(
                    graphics.GraphicsDevice,
                    response.GetResponseStream());
            }
            catch (WebException e)
            {
                gamerTag = "Gamertag not found.";
                return;
            }
        }
    }`
                      
    

© 2012 Microsoft Corporation. All rights reserved.  

© The MonoGame Team