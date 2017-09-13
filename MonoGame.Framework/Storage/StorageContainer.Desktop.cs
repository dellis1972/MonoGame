using System;
using System.IO;
using System.Reflection;

namespace Microsoft.Xna.Framework.Storage
{
    public partial class StorageContainer
    {
        string gameSavePath;

        internal void PlatformCreateContainer ()
        {
            string assembly;
            if (Assembly.GetEntryAssembly() != null)
                assembly = Assembly.GetEntryAssembly().GetName().Name;
            else
                assembly = Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);
            gameSavePath = Path.Combine(StorageDevice.PlatformGetSaveGamePath (), Path.GetFileName(assembly), DisplayName,
                StorageDevice.PlayerIndex.HasValue ? string.Format("Player{0}", (int)StorageDevice.PlayerIndex.Value) : "AllPlayers");
            Directory.CreateDirectory(gameSavePath);
        }

        internal void PlatformCreateDirectory (string directory)
        {
            Directory.CreateDirectory(Path.Combine(gameSavePath, directory));
        }

        internal Stream PlatformCreateFile(string file)
        {
            return File.OpenWrite(Path.Combine(gameSavePath, file));
        }

        internal void PlatformDeleteDirectory(string directory)
        {
            Directory.Delete(Path.Combine(gameSavePath, directory), recursive: true);
        }

        internal void PlatformDeleteFile(string file)
        {
            File.Delete(Path.Combine(gameSavePath, file));
        }

        internal bool PlatformDirectoryExists (string directory)
        {
            return Directory.Exists(Path.Combine(gameSavePath, directory));
        }

        internal bool PlatformFileExists(string file)
        {
            return File.Exists(Path.Combine(gameSavePath, file));
        }

        internal string[] PlatformGetDirectoryNames(string searchPattern)
        {
            var directories = Directory.GetDirectories(gameSavePath, searchPattern);
            for (int i = 0; i < directories.Length; i++)
                directories[i] = Path.GetDirectoryName(directories[i]);
            return directories;
        }

        internal string[] PlatformGetFileNames(string searchPattern)
        {
            var files = Directory.GetFiles(gameSavePath, searchPattern);
            for (int i = 0; i < files.Length; i++)
                files[i] = Path.GetFileName(files[i]);
            return files;
        }

        internal Stream PlatformOpenFile (string file, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return File.Open(Path.Combine(gameSavePath, file), fileMode, fileAccess, fileShare);
        }
    }
}
