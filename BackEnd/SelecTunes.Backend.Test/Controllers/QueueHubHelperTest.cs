using NUnit.Framework;
using SelecTunes.Backend.Helper.Hubs;
using SelecTunes.Backend.Models;
using System.Collections.Generic;
using System.Reflection;

namespace SelecTunes.Test.Controllers
{
    internal class QueueHubHelperTest
    {
        [Test]
        public void AssertThatBuildSongQueueWorks()
        {
#nullable enable
            MethodInfo? method = typeof(QueueHub).GetMethod("BuildSongList", BindingFlags.NonPublic | BindingFlags.Static);
#nullable restore

            Queue<Song> queue = new Queue<Song>();

            Song song1 = new Song
            {
                AlbumArt = "https://example.org",
                ArtistName = "Test Artist 1",
                Id = "1234",
                Name = "Test Song 1",
            };
            queue.Enqueue(song1);

            Song song2 = new Song
            {
                AlbumArt = "https://example.org",
                ArtistName = "Test Artist 2",
                Id = "5678",
                Name = "Test Song 2",
            };
            queue.Enqueue(song2);

            Assert.IsTrue(queue.Count == 2);

            List<Song> list = (List<Song>) method.Invoke(null, new object[] { queue });

            Assert.IsInstanceOf<List<Song>>(list);

            Assert.IsTrue(list.Count == 2);
            Assert.IsTrue(list.Contains(song1));
            Assert.IsTrue(list.Contains(song2));
        }
    }
}
