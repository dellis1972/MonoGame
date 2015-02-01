using System;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content.Pipeline
{
	public class AtcBitmapContent : BitmapContent
	{
		internal byte[] _bitmapData;
		internal int _bitsPerPixel;

		internal SurfaceFormat _format;

		public AtcBitmapContent(int bitsPerPixel)
		{
			_bitsPerPixel = bitsPerPixel;
			_format = SurfaceFormat.RgbaATCExplicitAlpha;
		}

		public AtcBitmapContent(int bitsPerPixel, int width, int height) : this(bitsPerPixel)
		{
			Width = width;
			Height = height;

			int size = ((width + 3) / 4) * ((height + 3) / 4) * 16;

			_bitmapData = new byte[size];
		}

		public override byte[] GetPixelData()
		{
			return _bitmapData;
		}

		public override void SetPixelData(byte[] sourceData)
		{
			_bitmapData = sourceData;
		}

		/// <summary>
		/// Gets the corresponding GPU texture format for the specified bitmap type.
		/// </summary>
		/// <param name="format">Format being retrieved.</param>
		/// <returns>The GPU texture format of the bitmap type.</returns>
		public override bool TryGetFormat(out SurfaceFormat format)
		{
			format = _format;
			return true;
		}

		protected override bool TryCopyFrom(BitmapContent sourceBitmap, Rectangle sourceRegion, Rectangle destRegion)
		{
			throw new NotImplementedException();
		}

		protected override bool TryCopyTo(BitmapContent destBitmap, Rectangle sourceRegion, Rectangle destRegion)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}

