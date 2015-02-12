using System;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.GamerServices
{
	public sealed partial class GamerProfile : IDisposable
	{
		Texture2D gamerPicture = null;

		public Texture2D GamerPicture {
			get {
				if (gamerPicture == null && GooglePlayHelper.Instance.Icon != null) {
					var data = GooglePlayHelper.Instance.Icon;
					using (var ms = new System.IO.MemoryStream (data)) {
						gamerPicture = Texture2D.FromStream (Game.Instance.GraphicsDevice, ms);
					}
				}
				return gamerPicture;
			}
		}
	}
}

