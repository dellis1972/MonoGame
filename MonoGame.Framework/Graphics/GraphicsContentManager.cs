using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Security.Permissions;

namespace Microsoft.Xna.Framework.Graphics.Content
{
    public class GraphicsContentManager : ContentManager
    {
        [System.Security.SecurityCritical()]
        public GraphicsContentManager(IServiceProvider serviceProvider): base(serviceProvider)
        {
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
            ContentManager.AddContentManager(this);
        }

        [System.Security.SecuritySafeCritical()]
        protected override T ReadAsset<T>(string assetName, Action<IDisposable> recordDisposableObject)
        {
            object result = null;
            string originalAssetName = assetName;

            GraphicsDeviceManager device = (GraphicsDeviceManager)this.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
            if (device == null)
            {
                throw new InvalidOperationException("No Graphics Device Service");
            }

            if ((typeof(T) == typeof(Texture2D)))
            {
                assetName = Texture2DReader.Normalize(assetName);
            }
            else if ((typeof(T) == typeof(SpriteFont)))
            {
                assetName = SpriteFontReader.Normalize(assetName);
            }
            else if ((typeof(T) == typeof(Effect)))
            {
                assetName = EffectReader.Normalize(assetName);
            }

            if (assetName != null)
            {

                if ((typeof(T) == typeof(Texture2D)))
                {
                    using (Stream assetStream = TitleContainer.OpenStream(assetName))
                    {
                        Texture2D texture = Texture2D.FromStream(device.GraphicsDevice, assetStream);
                        texture.Name = originalAssetName;
                        result = texture;
                    }
                }
                else if ((typeof(T) == typeof(SpriteFont)))
                {
                    //result = new SpriteFont(Texture2D.FromFile(graphicsDeviceService.GraphicsDevice,assetName), null, null, null, 0, 0.0f, null, null);
                    throw new NotImplementedException();
                }


                if ((typeof(T) == typeof(Effect)))
                {
                    using (Stream assetStream = TitleContainer.OpenStream(assetName))
                    {
                        var data = new byte[assetStream.Length];
                        assetStream.Read(data, 0, (int)assetStream.Length);
                        result = new Effect(device.GraphicsDevice, data);
                    }
                }
            }

            return (T)result;
        }

        protected override void ReloadAsset(string originalAssetName, object currentAsset)
        {
            string assetName = originalAssetName;

            if ((currentAsset is Texture2D))
            {
                assetName = Texture2DReader.Normalize(assetName);
            }
            else if ((currentAsset is SpriteFont))
            {
                assetName = SpriteFontReader.Normalize(assetName);
            }

            if ((currentAsset is Texture2D))
            {
                using (Stream assetStream = OpenStream(assetName))
                {
                    var asset = currentAsset as Texture2D;
                    asset.Reload(assetStream);
                }
            }
            else if ((currentAsset is SpriteFont))
            {
            }
        }
    }
}
