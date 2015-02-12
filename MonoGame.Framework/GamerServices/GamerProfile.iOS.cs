using System;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.GamerServices
{
	public sealed partial class GamerProfile : IDisposable
	{
		Texture2D gamerPicture = null;

		public Texture2D GamerPicture {
			get {
				return gamerPicture;
			}
		}
	}
}

