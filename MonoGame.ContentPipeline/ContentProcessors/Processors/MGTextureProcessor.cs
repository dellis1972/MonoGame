#define DEBUGTEXT
using System;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using MonoGameContentProcessors.Content;
using System.Diagnostics;
using Nvidia.TextureTools;


namespace MonoGameContentProcessors.Processors
{
    [ContentProcessor(DisplayName = "MonoGame Texture")]
    public class MGTextureProcessor : TextureProcessor
    {
        private MGCompressionMode compressionMode = MGCompressionMode.PVRTCFourBitsPerPixel;

        [DisplayName("Compression Mode")]
        [Description("Specifies the type of compression to use, if any.")]
        [DefaultValue(MGCompressionMode.PVRTCFourBitsPerPixel)]
        public MGCompressionMode CompressionMode
        {
            get { return this.compressionMode; }
            set { this.compressionMode = value; }
        }

        public int CalcBrackets(int teamCount)
        {
            int positions = 1;

            while (positions < teamCount)
                positions *= 2;

            return positions;
        }

        public override TextureContent Process(TextureContent input, ContentProcessorContext context)
        {
            
            // Fallback if we aren't buiding for iOS.
            var platform = ContentHelper.GetMonoGamePlatform();
            context.Logger.LogImportantMessage("Building for "+platform);
            if (platform != MonoGamePlatform.iOS && platform != MonoGamePlatform.Android)
            {
                throw new Exception();
                context.Logger.LogImportantMessage("Not IOS format for compression - fallback processing");
                return base.Process(input, context);
            }
#if DEBUGTEXT
            context.Logger.LogImportantMessage("Trying to Process {0}", input.Identity.SourceFilename);
            context.Logger.LogImportantMessage("Original Texture width is {0} and", input.Faces[0][0].Width);
            context.Logger.LogImportantMessage("Texture height is {0}", input.Faces[0][0].Height);
#endif
            // force us to be <colo> format.
            input.ConvertBitmapType(typeof(PixelBitmapContent<Color>));

            // Force a resize if we are looking at a .dds file, since it's not settable in the content project settings
            string e = Path.GetExtension(input.Identity.SourceFilename.ToLower());
            if (e == ".dds" && input.Identity.SourceFilename.ToLower().IndexOf("general_spritesheet.dds") == -1)
            {
//                ResizeToPowerOfTwo = true;
            }

            // Resize the first face if we need to and let the mips get generated from the dll.
            var resizeRequired = input.Faces[0][0].Height != input.Faces[0][0].Width || !(isPowerOfTwo(input.Faces[0][0].Height) && isPowerOfTwo(input.Faces[0][0].Width));
            if (ResizeToPowerOfTwo && resizeRequired)
            {
                int newEdgeSize = Math.Max(input.Faces[0][0].Height, input.Faces[0][0].Width);
                newEdgeSize = CalcBrackets(newEdgeSize);
#if DEBUGTEXT
                context.Logger.LogImportantMessage("Attemping resize to size {0}", newEdgeSize);
#endif
                var resizedBitmap = (BitmapContent)Activator.CreateInstance(typeof(PixelBitmapContent<Color>), new object[] { newEdgeSize, newEdgeSize });
                var destBitmap = resizedBitmap as PixelBitmapContent<Color>;
                var sourceBitmap = input.Faces[0][0] as PixelBitmapContent<Color>;

                // do actual resize
                float xDelta = (float)input.Faces[0][0].Width / (float)newEdgeSize;
                float yDelta = (float)input.Faces[0][0].Height / (float)newEdgeSize;
                for (int y = 0; y < newEdgeSize; y++)
                {
                    var destRow = destBitmap.GetRow(y);
                    int ySourceOffset = (int)(yDelta * y);
                    var sourceRow = sourceBitmap.GetRow(ySourceOffset);
                    
                    for (int x = 0; x < newEdgeSize; x++)
                    {
                        int xSourceOffset = (int)(xDelta * x);
                        destRow[x] = sourceRow[xSourceOffset];
                    }
                }

                input.Faces[0].Clear();
                input.Faces[0].Add(resizedBitmap);
#if DEBUGTEXT
                context.Logger.LogImportantMessage("New Texture edgsize is {0} and", newEdgeSize);
#endif
            }

            // check to see if we can PVRT compress
            var height = input.Faces[0][0].Height;
            var width = input.Faces[0][0].Width;
            var mipLevels = 1;

            var invalidBounds = height != width || !(isPowerOfTwo(height) && isPowerOfTwo(width));

            // Only PVR compress square, power of two textures, and if we are actually asking to.
            if (invalidBounds || compressionMode == MGCompressionMode.NoCompression)
            {
                if (compressionMode != MGCompressionMode.NoCompression)
                {
                    context.Logger.LogImportantMessage("WARNING: PVR Texture {0} must be a square, power of two texture. Skipping Compression.",
                                                        Path.GetFileName(context.OutputFilename));
                }

                // Skip compressing this texture and process it normally.
                this.TextureFormat = TextureProcessorOutputFormat.Color;

                return base.Process(input, context);
            }

            // Calculate how many mip levels will be created, and pass that to our DLL.
            if (GenerateMipmaps)
            {
                while (height != 1 || width != 1)
                {
                    height = Math.Max(height / 2, 1);
                    width = Math.Max(width / 2, 1);
                    mipLevels++;
                }
            }

            // why are we doing this?
            if (PremultiplyAlpha)
            {
                // is it a byte per RGBA pixel?
                var colorTex = input.Faces[0][0] as PixelBitmapContent<Color>;
                if (colorTex != null)
                {
                    for (int x = 0; x < colorTex.Height; x++)
                    {
                        var row = colorTex.GetRow(x);
                        for (int y = 0; y < row.Length; y++)
                        {
                            if (row[y].A < 0xff)
                                row[y] = Color.FromNonPremultiplied(row[y].R, row[y].G, row[y].B, row[y].A);
                        }
                    }
                }
                else
                {
                    // or a float per RGBA pixel?
                    var vec4Tex = input.Faces[0][0] as PixelBitmapContent<Vector4>;
                    if (vec4Tex == null)
                        throw new NotSupportedException();

                    for (int x = 0; x < vec4Tex.Height; x++)
                    {
                        var row = vec4Tex.GetRow(x);
                        for (int y = 0; y < row.Length; y++)
                        {
                            if (row[y].W < 1.0f)
                            {
                                row[y].X *= row[y].W;
                                row[y].Y *= row[y].W;
                                row[y].Z *= row[y].W;
                            }
                        }
                    }
                }
            }
#if DEBUGTEXT
            context.Logger.LogImportantMessage("Successfully Compressing {0} mip levels", mipLevels);
#endif
            if (platform == MonoGamePlatform.iOS)
                ConvertToPVRTC(input, mipLevels, PremultiplyAlpha, compressionMode);
            else
            {
                //TextureFormat = TextureProcessorOutputFormat.DxtCompressed;
                //base.Process(input, context);
                ConvertToATC(input, mipLevels, PremultiplyAlpha, context);
            }
                

            return input;
        }

        public static void BGRAtoRGBA(byte[] data)
        {
            for (var x = 0; x < data.Length; x += 4)
            {
                data[x] ^= data[x + 2];
                data[x + 2] ^= data[x];
                data[x] ^= data[x + 2];
            }
        }

        public static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        internal static bool ContainsFractionalAlpha(byte[] data)
        {
            for (var x = 3; x < data.Length; x += 4)
            {
                if (data[x] != 0x0 && data[x] != 0xFF)
                    return true;
            }

            return false;
        }

        class DxtDataHandler
        {
            private TextureContent _content;
            private int _currentMipLevel;
            private int _levelWidth;
            private int _levelHeight;
            private Format _format;

            public OutputOptions.WriteDataDelegate WriteData { get; private set; }
            public OutputOptions.ImageDelegate BeginImage { get; private set; }

            public DxtDataHandler(TextureContent content, Format format)
            {
                _content = content;

                _currentMipLevel = 0;
                _levelWidth = content.Faces[0][0].Width;
                _levelHeight = content.Faces[0][0].Height;
                _format = format;
                
                WriteData = new OutputOptions.WriteDataDelegate(writeData);
                BeginImage = new OutputOptions.ImageDelegate(beginImage);
            }

            public void beginImage(int size, int width, int height, int depth, int face, int miplevel)
            {
                _levelHeight = height;
                _levelWidth = width;
                _currentMipLevel = miplevel;
            }

            protected bool writeData(IntPtr data, int length)
            {
                var dataBuffer = new byte[length];

                Marshal.Copy(data, dataBuffer, 0, length);

                DxtBitmapContent texContent = null;
                switch (_format)
                {
                    case Format.DXT1:
                        texContent = new Dxt1BitmapContent(_levelWidth, _levelHeight);
                        break;
                    case Format.DXT3:
                        texContent = new Dxt3BitmapContent(_levelWidth, _levelHeight);
                        break;
                    case Format.DXT5:
                        texContent = new Dxt5BitmapContent(_levelWidth, _levelHeight);
                        break;
                }
                texContent.SetPixelData(dataBuffer);
                
                if (_content.Faces[0].Count == _currentMipLevel)
                    _content.Faces[0].Add(texContent);
                else
                    _content.Faces[0][_currentMipLevel] = texContent;
                
                //_content.Faces[0][_currentMipLevel].SetPixelData(dataBuffer);

                return true;
            }
        }

        public static void ConvertToPVRTC(TextureContent sourceContent, int mipLevels, bool premultipliedAlpha, MGCompressionMode bpp)
        {
            IntPtr dataSizesPtr = IntPtr.Zero;

            var texDataPtr = ManagedPVRTC.ManagedPVRTC.CompressTexture(sourceContent.Faces[0][0].GetPixelData(), 
                                            sourceContent.Faces[0][0].Height, 
                                            sourceContent.Faces[0][0].Width, 
                                            mipLevels, 
                                            premultipliedAlpha,
                                            bpp == MGCompressionMode.PVRTCFourBitsPerPixel,
                                            ref dataSizesPtr);

            // Store the size of each mipLevel
            var dataSizesArray = new int[mipLevels];
            Marshal.Copy(dataSizesPtr, dataSizesArray, 0, dataSizesArray.Length);

            var levelSize = 0;
            byte[] levelData;
            var sourceWidth = sourceContent.Faces[0][0].Width;
            var sourceHeight = sourceContent.Faces[0][0].Height;

            // Set the pixel data for each mip level.
            sourceContent.Faces[0].Clear();

            for (int x = 0; x < mipLevels; x++)
            {
                levelSize = dataSizesArray[x];
                levelData = new byte[levelSize];

                Marshal.Copy(texDataPtr, levelData, 0, levelSize);

                var levelWidth = Math.Max(sourceWidth  >> x, 1);
                var levelHeight = Math.Max(sourceHeight >> x, 1);

                sourceContent.Faces[0].Add(new MGBitmapContent(levelData, levelWidth, levelHeight, bpp));

                texDataPtr = IntPtr.Add(texDataPtr, levelSize);
            }
        }

        private bool isPowerOfTwo(int x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        private static void Read_DDS_HEADER(DDS_HEADER h, System.IO.BinaryReader r)
        {
            h.dwSize = r.ReadInt32();
            h.dwFlags = r.ReadInt32();
            h.dwHeight = r.ReadInt32();
            h.dwWidth = r.ReadInt32();
            h.dwPitchOrLinearSize = r.ReadInt32();
            h.dwDepth = r.ReadInt32();
            h.dwMipMapCount = r.ReadInt32();
            for (int i = 0; i < 11; ++i)
            {
                h.dwReserved1[i] = r.ReadInt32();
            }
            Read_DDS_PIXELFORMAT(h.ddspf, r);
            h.dwCaps = r.ReadInt32();
            h.dwCaps2 = r.ReadInt32();
            h.dwCaps3 = r.ReadInt32();
            h.dwCaps4 = r.ReadInt32();
            h.dwReserved2 = r.ReadInt32();
        }

        private static void Read_DDS_PIXELFORMAT(DDS_PIXELFORMAT p, System.IO.BinaryReader r)
        {
            p.dwSize = r.ReadInt32();
            p.dwFlags = r.ReadInt32();
            p.dwFourCC = r.ReadInt32();
            p.dwRGBBitCount = r.ReadInt32();
            p.dwRBitMask = r.ReadInt32();
            p.dwGBitMask = r.ReadInt32();
            p.dwBBitMask = r.ReadInt32();
            p.dwABitMask = r.ReadInt32();
        }

        public static void ConvertToATC(TextureContent sourceContent, int mipLevels, bool premultipliedAlpha, ContentProcessorContext context)
        {
            // save the content to disk
            // convert
            // reload the content
            int w = sourceContent.Faces[0][0].Width;
            int h = sourceContent.Faces[0][0].Height;


            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            //sourceConent.Faces[0][0];
            var file = System.IO.Path.Combine(path, string.Format("{0}.png", Guid.NewGuid().ToString()));

            List<MGBitmapContent> faces = new List<MGBitmapContent>();

            var colorTex = sourceContent.Faces[0][0] as PixelBitmapContent<Microsoft.Xna.Framework.Color>;
            if (colorTex != null)
            {
                var bitmap = new System.Drawing.Bitmap(colorTex.Width, colorTex.Height);
                for (int x = 0; x < colorTex.Height; x++)
                {
                    var row = colorTex.GetRow(x);
                    for (int y = 0; y < row.Length; y++)
                    {
                        bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(row[y].A, row[y].R, row[y].G, row[y].B));
                    }
                }
                bitmap.Save(file, System.Drawing.Imaging.ImageFormat.Png);
            }



            var info = new ProcessStartInfo();
            info.FileName = @"C:\Program Files (x86)\AMD\The Compressonator 1.50\TheCompressonator.exe";
            info.Arguments = string.Format("-convert -overwrite {0} {1} -codec ATICompressor +fourCC ATCA -mipper BoxFilter ~MaxMipLevels {2} ", file, file.Replace(".png", ".DDS"), mipLevels);
            info.WorkingDirectory = path;
            info.CreateNoWindow = false;
            info.UseShellExecute = false;
            var p = System.Diagnostics.Process.Start(info);
            p.WaitForExit();

            // load the dds file and extract the raw compressed data
            byte[] data = null;

            using (var ms = new System.IO.FileStream(file.Replace(".png", ".DDS"), FileMode.Open))
            {
                using (var br = new System.IO.BinaryReader(ms))
                {
                    var magic = br.ReadInt32();
                    if (magic != 0x20534444)
                        throw new NotImplementedException();

                    DDS_HEADER header = new DDS_HEADER();

                    Read_DDS_HEADER(header, br);
                    w = header.dwWidth;
                    h = header.dwHeight;
                    int mipmaps = 1;
                    if ((header.dwFlags & 0x00020000) != 0)
                    {
                        mipmaps = header.dwMipMapCount;
                    }

                    int size = mipmaps > 1 ? header.dwPitchOrLinearSize * 2 : header.dwPitchOrLinearSize;

                    context.Logger.LogImportantMessage("w:{0} h:{1} size:{2}", w, h, size);   
                    // read all the data
                    data = br.ReadBytes(size);
                    int datasize = 0;
                    int offset = 0;
                    for (int i = 0; i < mipmaps; ++i)
                    {
                        datasize = ((w + 3) / 4) * ((h + 3) / 4) * 16;

                        byte[] ddata = new byte[datasize];
                        //try
                        {
                            context.Logger.LogImportantMessage("dest:{0} source:{1}", datasize, data.Length);   
                            Array.Copy(data, offset, ddata, 0, datasize);

                            faces.Add(new MGBitmapContent(ddata, w, h, MGCompressionMode.ATCExplicitAlpha));

                            offset += datasize;
                            w /= 2;
                            h /= 2;
                        }
                        //catch
                        //{
                        //}
                    }
                }
            }

            //if (File.Exists(file)) File.Delete(file);
            //if (File.Exists(file.Replace(".png", ".DDS"))) File.Delete(file.Replace(".png", ".DDS"));

            sourceContent.Faces[0].Clear();
            foreach (var f in faces)
                sourceContent.Faces[0].Add(f);

        }
    }

    class DDS_HEADER
    {
        public int dwSize;
        public int dwFlags;
        /*		DDPF_ALPHAPIXELS   0x00000001 
            DDPF_ALPHA   0x00000002 
            DDPF_FOURCC   0x00000004 
            DDPF_RGB   0x00000040 
            DDPF_YUV   0x00000200 
            DDPF_LUMINANCE   0x00020000 
         */
        public int dwHeight;
        public int dwWidth;
        public int dwPitchOrLinearSize;
        public int dwDepth;
        public int dwMipMapCount;
        public int[] dwReserved1 = new int[11];
        public DDS_PIXELFORMAT ddspf = new DDS_PIXELFORMAT();
        public int dwCaps;
        public int dwCaps2;
        public int dwCaps3;
        public int dwCaps4;
        public int dwReserved2;
    }

    class DDS_HEADER_DXT10
    {
        public DXGI_FORMAT dxgiFormat;
        public D3D10_RESOURCE_DIMENSION resourceDimension;
        public uint miscFlag;
        public uint arraySize;
        public uint reserved;
    }

    class DDS_PIXELFORMAT
    {
        public int dwSize;
        public int dwFlags;
        public int dwFourCC;
        public int dwRGBBitCount;
        public int dwRBitMask;
        public int dwGBitMask;
        public int dwBBitMask;
        public int dwABitMask;

        public DDS_PIXELFORMAT()
        {
        }
    }

    enum DXGI_FORMAT : uint
    {
        DXGI_FORMAT_UNKNOWN = 0,
        DXGI_FORMAT_R32G32B32A32_TYPELESS = 1,
        DXGI_FORMAT_R32G32B32A32_FLOAT = 2,
        DXGI_FORMAT_R32G32B32A32_UINT = 3,
        DXGI_FORMAT_R32G32B32A32_SINT = 4,
        DXGI_FORMAT_R32G32B32_TYPELESS = 5,
        DXGI_FORMAT_R32G32B32_FLOAT = 6,
        DXGI_FORMAT_R32G32B32_UINT = 7,
        DXGI_FORMAT_R32G32B32_SINT = 8,
        DXGI_FORMAT_R16G16B16A16_TYPELESS = 9,
        DXGI_FORMAT_R16G16B16A16_FLOAT = 10,
        DXGI_FORMAT_R16G16B16A16_UNORM = 11,
        DXGI_FORMAT_R16G16B16A16_UINT = 12,
        DXGI_FORMAT_R16G16B16A16_SNORM = 13,
        DXGI_FORMAT_R16G16B16A16_SINT = 14,
        DXGI_FORMAT_R32G32_TYPELESS = 15,
        DXGI_FORMAT_R32G32_FLOAT = 16,
        DXGI_FORMAT_R32G32_UINT = 17,
        DXGI_FORMAT_R32G32_SINT = 18,
        DXGI_FORMAT_R32G8X24_TYPELESS = 19,
        DXGI_FORMAT_D32_FLOAT_S8X24_UINT = 20,
        DXGI_FORMAT_R32_FLOAT_X8X24_TYPELESS = 21,
        DXGI_FORMAT_X32_TYPELESS_G8X24_UINT = 22,
        DXGI_FORMAT_R10G10B10A2_TYPELESS = 23,
        DXGI_FORMAT_R10G10B10A2_UNORM = 24,
        DXGI_FORMAT_R10G10B10A2_UINT = 25,
        DXGI_FORMAT_R11G11B10_FLOAT = 26,
        DXGI_FORMAT_R8G8B8A8_TYPELESS = 27,
        DXGI_FORMAT_R8G8B8A8_UNORM = 28,
        DXGI_FORMAT_R8G8B8A8_UNORM_SRGB = 29,
        DXGI_FORMAT_R8G8B8A8_UINT = 30,
        DXGI_FORMAT_R8G8B8A8_SNORM = 31,
        DXGI_FORMAT_R8G8B8A8_SINT = 32,
        DXGI_FORMAT_R16G16_TYPELESS = 33,
        DXGI_FORMAT_R16G16_FLOAT = 34,
        DXGI_FORMAT_R16G16_UNORM = 35,
        DXGI_FORMAT_R16G16_UINT = 36,
        DXGI_FORMAT_R16G16_SNORM = 37,
        DXGI_FORMAT_R16G16_SINT = 38,
        DXGI_FORMAT_R32_TYPELESS = 39,
        DXGI_FORMAT_D32_FLOAT = 40,
        DXGI_FORMAT_R32_FLOAT = 41,
        DXGI_FORMAT_R32_UINT = 42,
        DXGI_FORMAT_R32_SINT = 43,
        DXGI_FORMAT_R24G8_TYPELESS = 44,
        DXGI_FORMAT_D24_UNORM_S8_UINT = 45,
        DXGI_FORMAT_R24_UNORM_X8_TYPELESS = 46,
        DXGI_FORMAT_X24_TYPELESS_G8_UINT = 47,
        DXGI_FORMAT_R8G8_TYPELESS = 48,
        DXGI_FORMAT_R8G8_UNORM = 49,
        DXGI_FORMAT_R8G8_UINT = 50,
        DXGI_FORMAT_R8G8_SNORM = 51,
        DXGI_FORMAT_R8G8_SINT = 52,
        DXGI_FORMAT_R16_TYPELESS = 53,
        DXGI_FORMAT_R16_FLOAT = 54,
        DXGI_FORMAT_D16_UNORM = 55,
        DXGI_FORMAT_R16_UNORM = 56,
        DXGI_FORMAT_R16_UINT = 57,
        DXGI_FORMAT_R16_SNORM = 58,
        DXGI_FORMAT_R16_SINT = 59,
        DXGI_FORMAT_R8_TYPELESS = 60,
        DXGI_FORMAT_R8_UNORM = 61,
        DXGI_FORMAT_R8_UINT = 62,
        DXGI_FORMAT_R8_SNORM = 63,
        DXGI_FORMAT_R8_SINT = 64,
        DXGI_FORMAT_A8_UNORM = 65,
        DXGI_FORMAT_R1_UNORM = 66,
        DXGI_FORMAT_R9G9B9E5_SHAREDEXP = 67,
        DXGI_FORMAT_R8G8_B8G8_UNORM = 68,
        DXGI_FORMAT_G8R8_G8B8_UNORM = 69,
        DXGI_FORMAT_BC1_TYPELESS = 70,
        DXGI_FORMAT_BC1_UNORM = 71,
        DXGI_FORMAT_BC1_UNORM_SRGB = 72,
        DXGI_FORMAT_BC2_TYPELESS = 73,
        DXGI_FORMAT_BC2_UNORM = 74,
        DXGI_FORMAT_BC2_UNORM_SRGB = 75,
        DXGI_FORMAT_BC3_TYPELESS = 76,
        DXGI_FORMAT_BC3_UNORM = 77,
        DXGI_FORMAT_BC3_UNORM_SRGB = 78,
        DXGI_FORMAT_BC4_TYPELESS = 79,
        DXGI_FORMAT_BC4_UNORM = 80,
        DXGI_FORMAT_BC4_SNORM = 81,
        DXGI_FORMAT_BC5_TYPELESS = 82,
        DXGI_FORMAT_BC5_UNORM = 83,
        DXGI_FORMAT_BC5_SNORM = 84,
        DXGI_FORMAT_B5G6R5_UNORM = 85,
        DXGI_FORMAT_B5G5R5A1_UNORM = 86,
        DXGI_FORMAT_B8G8R8A8_UNORM = 87,
        DXGI_FORMAT_B8G8R8X8_UNORM = 88,
        DXGI_FORMAT_R10G10B10_XR_BIAS_A2_UNORM = 89,
        DXGI_FORMAT_B8G8R8A8_TYPELESS = 90,
        DXGI_FORMAT_B8G8R8A8_UNORM_SRGB = 91,
        DXGI_FORMAT_B8G8R8X8_TYPELESS = 92,
        DXGI_FORMAT_B8G8R8X8_UNORM_SRGB = 93,
        DXGI_FORMAT_BC6H_TYPELESS = 94,
        DXGI_FORMAT_BC6H_UF16 = 95,
        DXGI_FORMAT_BC6H_SF16 = 96,
        DXGI_FORMAT_BC7_TYPELESS = 97,
        DXGI_FORMAT_BC7_UNORM = 98,
        DXGI_FORMAT_BC7_UNORM_SRGB = 99,
        DXGI_FORMAT_AYUV = 100,
        DXGI_FORMAT_Y410 = 101,
        DXGI_FORMAT_Y416 = 102,
        DXGI_FORMAT_NV12 = 103,
        DXGI_FORMAT_P010 = 104,
        DXGI_FORMAT_P016 = 105,
        DXGI_FORMAT_420_OPAQUE = 106,
        DXGI_FORMAT_YUY2 = 107,
        DXGI_FORMAT_Y210 = 108,
        DXGI_FORMAT_Y216 = 109,
        DXGI_FORMAT_NV11 = 110,
        DXGI_FORMAT_AI44 = 111,
        DXGI_FORMAT_IA44 = 112,
        DXGI_FORMAT_P8 = 113,
        DXGI_FORMAT_A8P8 = 114,
        DXGI_FORMAT_B4G4R4A4_UNORM = 115,
        DXGI_FORMAT_FORCE_UINT = 0xffffffff
    }

    enum D3D10_RESOURCE_DIMENSION
    {
        D3D10_RESOURCE_DIMENSION_UNKNOWN = 0,
        D3D10_RESOURCE_DIMENSION_BUFFER = 1,
        D3D10_RESOURCE_DIMENSION_TEXTURE1D = 2,
        D3D10_RESOURCE_DIMENSION_TEXTURE2D = 3,
        D3D10_RESOURCE_DIMENSION_TEXTURE3D = 4
    }

}