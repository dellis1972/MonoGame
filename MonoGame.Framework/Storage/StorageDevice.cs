using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Xna.Framework.Storage
{
    [Obsolete ("This class is for legacy applications only. Use native API's for game saves")]
    public partial class StorageDevice
    {
        public long FreeSpace { get { return PlatformFreeSpace(); } }

        public bool IsConnected { get; internal set; }

        public long TotalSpace { get { return PlatformTotalSpace(); } }

        public static event EventHandler<EventArgs> DeviceChanged;

        internal PlayerIndex? PlayerIndex { get; private set; }

        internal StorageDevice (PlayerIndex? player)
        {
            PlayerIndex = player;
            PlatformCalcuateFreeSpace();
        }

        public IAsyncResult BeginOpenContainer(string displayName, AsyncCallback callback, Object state)
        {
            IAsyncResult result = new StorageDeviceAsyncResult<string>(state, displayName);
            if (callback != null)
                callback(result);
            return result;
        }

        public StorageContainer EndOpenContainer(IAsyncResult result)
        {
            var storageResult = (StorageDeviceAsyncResult<string>)result;
            return new StorageContainer(storageResult.Value, this);
        }

        public static IAsyncResult BeginShowSelector(AsyncCallback callback, object state)
        {
            return BeginShowSelector(0, 0, callback, state);
        }

        public static IAsyncResult BeginShowSelector(PlayerIndex player, AsyncCallback callback, object state
        )
        {
            return BeginShowSelector(player, 0, 0, callback, state);
        }

        public static IAsyncResult BeginShowSelector(int sizeInBytes, int directoryCount, AsyncCallback callback, object state
        )
        {
            IAsyncResult result = new StorageDeviceAsyncResult<PlayerIndex?>(state, null);
            if (callback != null)
                callback(result);
            return result;
        }

        public static IAsyncResult BeginShowSelector(PlayerIndex player, int sizeInBytes, int directoryCount, AsyncCallback callback, object state
        )
        {
            IAsyncResult result = new StorageDeviceAsyncResult<PlayerIndex?>(state, player);
            if (callback != null)
                callback(result);
            return result;
        }

        public static StorageDevice EndShowSelector(IAsyncResult result)
        {
            var storageResult = (StorageDeviceAsyncResult<PlayerIndex?>)result;
            return new StorageDevice(storageResult.Value);
        }

        public void DeleteContainer(string titleName)
        {
            throw new NotImplementedException();
        }

        internal class StorageDeviceAsyncResult<T> : IAsyncResult
        {
            public object AsyncState { get; set; }

            public WaitHandle AsyncWaitHandle { get; set; }

            public bool CompletedSynchronously { get; set; }

            public bool IsCompleted { get; set; }

            public T Value { get; set; }

            public StorageDeviceAsyncResult (object state, T value) {
                AsyncState = state;
                CompletedSynchronously = true;
                IsCompleted = true;
                AsyncWaitHandle = new ManualResetEvent(true);
                Value = value;
            }
        }
    }

    public class StorageDeviceNotConnectedException : ExternalException {
        public StorageDeviceNotConnectedException() : base()
        {
        }

        public StorageDeviceNotConnectedException(string message) : base(message)
        {
        }

        public StorageDeviceNotConnectedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
