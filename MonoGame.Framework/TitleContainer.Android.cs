// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using System.IO.Compression;
using Android.Content.PM;
using Android.App;
using Android.OS;
using Android.Content.Res;

namespace Microsoft.Xna.Framework
{
    partial class TitleContainer
    {
        static string expansionPath = null;
        static ZipArchive expansionFile = null;


        static partial void PlatformInit ()
        {
            if (ExpansionPath != null)
            {
                if (expansionFile != null)
                {
                    expansionFile.Dispose();
                    expansionFile = null;
                }
                expansionFile = new ZipArchive(File.OpenRead (ExpansionPath));
                if (expansionFile != null)
                {
                    System.Diagnostics.Debug.WriteLine("Expansion file found at {0}", ExpansionPath);
                    foreach (var entry in expansionFile.Entries)
                    {
                        System.Diagnostics.Debug.WriteLine ("Found Entry {0}", entry.FullName.Replace("\\", "/"));
                    }
                }
            }
        }

        private static Stream PlatformOpenStream(string safeName)
        {
            if (expansionFile != null) {
                var entry = expansionFile.GetEntry(safeName.Replace ('\\', '/'));
                if (entry != null) {
                    return OpenExpansionStream(entry);
                }
            }
            return Android.App.Application.Context.Assets.Open(safeName);
        }

        static string ExpansionPath
        {
            get
            {
                if (expansionPath == null)
                {
                    ApplicationInfo ainfo = Game.Activity.ApplicationInfo;
                    PackageInfo pinfo = Game.Activity.PackageManager.GetPackageInfo(ainfo.PackageName, PackageInfoFlags.MetaData);

                    if (Android.OS.Environment.ExternalStorageState.Equals(Android.OS.Environment.MediaMounted))
                    {
                        string expPath = Android.OS.Environment.ExternalStorageDirectory + "/Android/obb/" + ainfo.PackageName;

                        if (Directory.Exists(expPath))
                        {
                            expansionPath = Path.Combine(expPath, string.Format("main.{0}.{1}.obb", pinfo.VersionCode, ainfo.PackageName));
                            if (!File.Exists(expansionPath))
                            {
#if DEBUG
                                expansionPath = "/mnt/sdcard/Downloads/Content.zip";
                                if (!File.Exists(expansionPath))
                                    expansionPath = null;
#else
                                expansionPath = null;
#endif
                            }
                        } else {
#if DEBUG
                            expansionPath = "/mnt/sdcard/Downloads/Content.zip";
                            if (!File.Exists(expansionPath))
                                expansionPath = null;
#else
                            expansionPath = null;
#endif   
                        }
                    }
                }
                return expansionPath;
            }
        }

        static Stream OpenExpansionStream(ZipArchiveEntry entry)
        {
            try
            {
                return entry.Open();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw new FileNotFoundException("The content file was not found in expansions.");
            }
        }
    }
}

