using System;
using System.IO;

namespace Microsoft.Xna.Framework.Storage
{
    public partial class StorageDevice
    {
        internal string PlatformGetSaveGamePath()
        {
            string homeDirectory = ".";
            switch (MonoGame.Utilities.CurrentPlatform.OS)
            {
                case MonoGame.Utilities.OS.Windows:
                    homeDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SavedGames");
                    break;
                case MonoGame.Utilities.OS.Linux:
                    homeDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".local", "share");
                    break;
                case MonoGame.Utilities.OS.MacOSX:
                    homeDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Application Support");
                    break;
            }
            return homeDirectory;
        }

        DriveInfo driveInfo;

        internal long PlatformFreeSpace ()
        {
            return driveInfo.AvailableFreeSpace;
        }

        internal long PlatformTotalSpace()
        {
            return driveInfo.TotalSize;
        }

        internal void PlatformCalcuateFreeSpace ()
        {
            driveInfo = new DriveInfo(Path.GetPathRoot(PlatformGetSaveGamePath ()));
            IsConnected = driveInfo.IsReady;
        }
    }
}
