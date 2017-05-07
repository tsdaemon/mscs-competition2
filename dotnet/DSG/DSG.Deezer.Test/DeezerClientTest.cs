using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DSG.Deezer.Test
{
    [TestFixture]
    public class DeezerClientTest
    {
        [Test]
        public void TestTrack()
        {
            var trackId = 3135556;
            var client = new DeezerClient();
            var track = client.GetTrack(trackId);

            Assert.AreEqual(trackId, track.Id);
            Assert.AreEqual("Harder Better Faster Stronger", track.Title);

            //Assert.AreEqual(302127, track.Album.Id);
            //Assert.AreEqual("Discovery", track.Album.Title);

            //Assert.AreEqual(27, track.Artist.Id);
            //Assert.AreEqual("Daft Punk", track.Artist.Name);
        }
    }
}
