// #region License
// /*
// Microsoft Public License (Ms-PL)
// MonoGame - Copyright © 2009 The MonoGame Team
// 
// All rights reserved.
// 
// This license governs use of the accompanying software. If you use the software, you accept this license. If you do not
// accept the license, do not use the software.
// 
// 1. Definitions
// The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under 
// U.S. copyright law.
// 
// A "contribution" is the original software, or any additions or changes to the software.
// A "contributor" is any person that distributes its contribution under this license.
// "Licensed patents" are a contributor's patent claims that read directly on its contribution.
// 
// 2. Grant of Rights
// (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
// each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
// (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
// each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
// 
// 3. Conditions and Limitations
// (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
// (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
// your patent license from such contributor to the software ends automatically.
// (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution 
// notices that are present in the software.
// (D) If you distribute any portion of the software in source code form, you may do so only under this license by including 
// a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object 
// code form, you may only do so under a license that complies with this license.
// (E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees
// or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent
// permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular
// purpose and non-infringement.
// */
// #endregion License
// 
using System;
using System.Collections.Generic;
using System.IO;
#if ANDROID
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using System.Linq;
using System.IO.Compression.Zip;
using Android.OS;
#endif

#if WINRT
using System.Threading.Tasks;
#elif IOS
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#elif MONOMAC
using MonoMac.Foundation;
#elif PSM
using Sce.PlayStation.Core;
#endif

namespace Microsoft.Xna.Framework
{
    public static class TitleContainer
    {
        static TitleContainer()
        {
#if WINDOWS || LINUX
            Location = AppDomain.CurrentDomain.BaseDirectory;
#elif WINRT
            Location = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
#elif IOS || MONOMAC
			Location = NSBundle.MainBundle.ResourcePath;
#elif PSM
			Location = "/Application";
#else
            Location = string.Empty;
#endif

#if IOS
			SupportRetina = UIScreen.MainScreen.Scale == 2.0f;
#endif
        }

        static internal string Location { get; private set; }
#if IOS
        static internal bool SupportRetina { get; private set; }
#endif

#if WINRT

        private static async Task<Stream> OpenStreamAsync(string name)
        {
            var package = Windows.ApplicationModel.Package.Current;

            try
            {
                var storageFile = await package.InstalledLocation.GetFileAsync(name);
                var randomAccessStream = await storageFile.OpenReadAsync();
                return randomAccessStream.AsStreamForRead();
            }
            catch (IOException)
            {
                // The file must not exist... return a null stream.
                return null;
            }
        }

#endif // WINRT

        /// <summary>
        /// Returns an open stream to an exsiting file in the title storage area.
        /// </summary>
        /// <param name="name">The filepath relative to the title storage area.</param>
        /// <returns>A open stream or null if the file is not found.</returns>
        public static Stream OpenStream(string name)
        {
            // Normalize the file path.
            var safeName = GetFilename(name);

            // We do not accept absolute paths here.
            if (Path.IsPathRooted(safeName))
                throw new ArgumentException("Invalid filename. TitleContainer.OpenStream requires a relative path.");

#if WINRT
            var stream = Task.Run( () => OpenStreamAsync(safeName).Result ).Result;
            if (stream == null)
                throw new FileNotFoundException(name);

            return stream;
#elif ANDROID
            return OpenStreamInternal(safeName);
#elif IOS
            var absolutePath = Path.Combine(Location, safeName);
            if (SupportRetina)
            {
                // Insert the @2x immediately prior to the extension. If this file exists
                // and we are on a Retina device, return this file instead.
                var absolutePath2x = Path.Combine(Path.GetDirectoryName(absolutePath),
                                                  Path.GetFileNameWithoutExtension(absolutePath)
                                                  + "@2x" + Path.GetExtension(absolutePath));
                if (File.Exists(absolutePath2x))
                    return File.OpenRead(absolutePath2x);
            }
            return File.OpenRead(absolutePath);
#else
            var absolutePath = Path.Combine(Location, safeName);
            return File.OpenRead(absolutePath);
#endif
        }

        // TODO: This is just path normalization.  Remove this
        // and replace it with a proper utility function.  I'm sure
        // this same logic is duplicated all over the code base.
        internal static string GetFilename(string name)
        {
#if WINRT
            // Replace non-windows seperators.
            name = name.Replace('/', '\\');
#else
            // Replace Windows path separators with local path separators
            name = name.Replace('\\', Path.DirectorySeparatorChar);
#endif

#if ANDROID
            name = name.Replace("//", "/");
#endif
            return name;
        }


#if ANDROID

        enum AssetLocationEnum { Assets, External }

        static Dictionary<string, AssetLocationEnum> assets = new Dictionary<string, AssetLocationEnum>(StringComparer.OrdinalIgnoreCase);


        internal static void InitActivity()
        {
            try
            {
                assets.Clear();
                using (var stream = Game.Activity.Assets.Open("resourcelist.txt"))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] item = line.Split(new char[1] { ',' });
                            assets.Add(item[0].Replace("\\", "/"), (AssetLocationEnum)Enum.Parse(typeof(AssetLocationEnum), item[1]));
                        }
                    }
                }
            }
            catch
            {
                // no resource list... hmm.
            }
        }

        internal static void Close()
        {
            if (expansionFile != null)
            {
                expansionFile.Close();
                expansionFile.Dispose();
            }
        }


        internal static Stream OpenStreamInternal(string safeName)
        {
            MemoryStream ms;
            try
            {
		var sn = safeName.Replace ("\\", "/");
		KeyValuePair<string, AssetLocationEnum> kvp = assets.Where (x => string.Compare(x.Key,sn, StringComparison.OrdinalIgnoreCase)== 0).FirstOrDefault ();
		if (kvp.Key == null) {
			if (assets.Count == 0) {
				kvp = new KeyValuePair<string, AssetLocationEnum> (safeName,AssetLocationEnum.Assets);
			} else
				return null;
		}
		
                if (kvp.Value == AssetLocationEnum.Assets)
                {
                    using (var s = Game.Activity.Assets.Open(kvp.Key))
                    {
                        if (s != null)
                        {
                            ms = new MemoryStream();
                            s.CopyTo(ms);
                            ms.Seek(0, SeekOrigin.Begin);
                            return ms;
                        }
                    }
                }
                else
                {
                    return OpenExpansionStream(kvp.Key);
                }
            }
            catch (Java.IO.IOException)
            {
                // not in assets try an external source
            }

            return null;
        }

        internal static Android.Content.Res.AssetFileDescriptor OpenFd(string safeName)
        {
            try
            {
                var sn = safeName.Replace("\\", "/");
                KeyValuePair<string, AssetLocationEnum> kvp = assets.Where(x => string.Compare(x.Key, sn, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();
                if (kvp.Key == null)
                    return null;

                if (kvp.Value == AssetLocationEnum.Assets)
                {
                    return Game.Activity.Assets.OpenFd(kvp.Key);
                }
                else
                {
                    return OpenExpansionFd(kvp.Key);
                }
            }
            catch (Java.IO.IOException)
            {
                // not in assets try external source
            }
            return null;
        }

        internal static string[] List(string path)
        {

            var external = assets.Where(x => x.Key.StartsWith(path) && x.Value == AssetLocationEnum.External).Select(x => x.Key).ToArray();
            return Game.Activity.Assets.List(path).Concat(external).ToArray();
        }

        static string expansionPath = null;

        static string ExpansionPath
        {
            get
            {
                if (expansionPath == null)
                {
                    Activity activity = Game.Activity;

                    ApplicationInfo ainfo = activity.ApplicationInfo;
                    PackageInfo pinfo = activity.PackageManager.GetPackageInfo(ainfo.PackageName, PackageInfoFlags.MetaData);

                    var dir = "/mnt/sdcard/Download";
                    // Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
                    //dir = "/mnt/sdcard";
                    expansionPath = Path.Combine(
                        dir,
                        "Android",
                        "obb",
#if !PACKAGE
 "test",
#else
                        ainfo.PackageName,
#endif
 String.Format("main.{0}.{1}.obb", pinfo.VersionCode, ainfo.PackageName));
                }
                return "/mnt/sdcard/Downloads/test.zip";// expansionPath;
            }
        }

        static ZipFile expansionFile = null;

        static AssetFileDescriptor OpenExpansionFd(string assetsPath)
        {
            try
            {

                if (expansionFile == null)
                    expansionFile = new ZipFile(ExpansionPath);

                var entry = expansionFile.GetAllEntries().Where(x => string.Compare(x.FilenameInZip, assetsPath, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();
                if (entry != null)
                {
                    ParcelFileDescriptor pfd = ParcelFileDescriptor.Open(
                       new Java.IO.File(entry.ZipFileName), ParcelFileMode.ReadOnly);

                    return new AssetFileDescriptor(pfd, entry.FileOffset, entry.FileSize);
                }

            }
            catch
            {
                return null;
            }
            return null;
        }

        static Stream OpenExpansionStream(string assetsPath)
        {
            Stream stream = null;
            try
            {
                if (expansionFile == null)
                    expansionFile = new ZipFile(ExpansionPath);
                var entry = expansionFile.GetAllEntries().Where(x => string.Compare(x.FilenameInZip, assetsPath, StringComparison.OrdinalIgnoreCase) == 0).FirstOrDefault();
                if (entry != null)
                {

                    MemoryStream mstream = new MemoryStream();
                    expansionFile.ExtractFile(entry, mstream);
                    mstream.Seek(0, SeekOrigin.Begin);
                    stream = mstream;
                }
                return stream;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw new FileNotFoundException("The content file was not found in exapnsions.");
            }
        }

#endif


    }
}

