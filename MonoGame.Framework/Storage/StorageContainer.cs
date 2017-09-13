using System;
using System.IO;

namespace Microsoft.Xna.Framework.Storage
{
    [Obsolete("This class is for legacy applications only. Use native API's for game saves")]
    public partial class StorageContainer : IDisposable
    {
        public string DisplayName { get; private set; }

        public bool IsDisposed { get { return disposed; } }

        public StorageDevice StorageDevice { get; private set; }

        public event EventHandler<EventArgs> Disposing;

        internal StorageContainer (string displayName, StorageDevice device)
        {
            StorageDevice = device;
            DisplayName = displayName;
            PlatformCreateContainer ();
        }

        public void CreateDirectory(string directory)
        {
            PlatformCreateDirectory(directory);
        }

        public Stream CreateFile(string file)
        {
            return PlatformCreateFile(file);
        }

        public void DeleteDirectory(string directory)
        {
            PlatformDeleteDirectory(directory);
        }

        public void DeleteFile(string file)
        {
            PlatformDeleteFile(file);
        }

        public bool DirectoryExists(string directory)
        {
            return PlatformDirectoryExists(directory);
        }

        public bool FileExists(string file)
        {
            return PlatformFileExists(file);
        }

        public string[] GetDirectoryNames()
        {
            return GetDirectoryNames("*.*");
        }

        public string[] GetDirectoryNames(string searchPattern)
        {
            return PlatformGetDirectoryNames(searchPattern);
        }

        public string[] GetFileNames()
        {
            return GetFileNames("*.*");
        }

        public string[] GetFileNames(string searchPattern)
        {
            return PlatformGetFileNames(searchPattern);
        }

        public Stream OpenFile(string file, FileMode fileMode)
        {
            return OpenFile(file, fileMode, FileAccess.ReadWrite, FileShare.ReadWrite);
        }

        public Stream OpenFile(string file, FileMode fileMode, FileAccess fileAccess)
        {
            return OpenFile(file, fileMode, fileAccess, FileShare.ReadWrite);
        }

        public Stream OpenFile(string file, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return PlatformOpenFile(file, fileMode, fileAccess, fileShare);
        }

        #region IDisposable Support
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Disposing != null)
                        Disposing(this, EventArgs.Empty);
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
