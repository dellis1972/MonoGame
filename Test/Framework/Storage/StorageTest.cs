using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using NUnit.Framework;

namespace MonoGame.Tests.Framework.Storage
{
    [TestFixture]
    public class StorageTest
    {
        [Test]
        public void CreateDeviceWithoutPlayer ()
        {
            var asyncResult = StorageDevice.BeginShowSelector(null, null);
            var device = StorageDevice.EndShowSelector(asyncResult);
            Assert.IsNotNull(device);
            Assert.AreNotEqual(0, device.FreeSpace);
            Assert.AreNotEqual(0, device.TotalSpace);
            Assert.AreNotEqual(false, device.IsConnected);
            Assert.IsNull(device.PlayerIndex);
        }

        [Test]
        public void CreateDeviceWithPlayer([Values(PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four)] PlayerIndex player)
        {
            var asyncResult = StorageDevice.BeginShowSelector(player, null, null);
            var device = StorageDevice.EndShowSelector(asyncResult);
            Assert.IsNotNull(device);
            Assert.AreNotEqual(0, device.FreeSpace);
            Assert.AreNotEqual(0, device.TotalSpace);
            Assert.AreNotEqual(false, device.IsConnected);
            Assert.IsNotNull(device.PlayerIndex);
            Assert.AreEqual(player, device.PlayerIndex.Value);
        }

        [Test]
        public void CreateAndDeleteStorageContainer ([Values(PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four)] PlayerIndex player)
        {
            var asyncResult = StorageDevice.BeginShowSelector(player, null, null);
            var device = StorageDevice.EndShowSelector(asyncResult);
            Assert.IsNotNull(device);
            var ar = device.BeginOpenContainer("Me", null, null);
            var container = device.EndOpenContainer(ar);
            Assert.IsNotNull(container);
            Assert.AreEqual("Me", container.DisplayName);
            var expectedPath = Path.Combine(GetPlatformExpectedPath(), "test-domain-MonoGameTests", "Me", string.Format("Player{0}", (int)player));
            Assert.IsTrue(Directory.Exists (expectedPath));
            container.CreateDirectory("Foo");
            Assert.IsTrue(Directory.Exists(Path.Combine(expectedPath, "Foo")));
            container.DeleteDirectory("Foo");
            Assert.IsFalse(Directory.Exists(Path.Combine(expectedPath, "Foo")));
        }

        string GetPlatformExpectedPath ()
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
    }
}
