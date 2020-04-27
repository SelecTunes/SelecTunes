using Moq;
using NUnit.Framework;
using SelecTunes.Backend.Helper;
using SelecTunes.Backend.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using SelecTunes.Backend.Models.Auth;
using Newtonsoft.Json;
using SelecTunes.Backend.Models.OneOff;
using System.Linq;

namespace SelecTunes.Backend.Test.Controllers
{
    internal class DelegatingHandlerStub : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
        public DelegatingHandlerStub()
        {
            _handlerFunc = (request, cancellationToken) => Task.FromResult(request.CreateResponse(HttpStatusCode.OK));
        }

        public DelegatingHandlerStub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
        {
            _handlerFunc = handlerFunc;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handlerFunc(request, cancellationToken);
        }
    }

    internal class PlaybackHelperTest
    {
        private readonly Mock<IHttpClientFactory> mockFactory = new Mock<IHttpClientFactory>();

        private readonly PlaybackHelper _playback;

        public PlaybackHelperTest() => _playback = new PlaybackHelper
        {
            ClientFactory = mockFactory.Object
        };

        private void SetupFactory(Devices device = null)
        {
            using HttpConfiguration configuration = new HttpConfiguration();

            using DelegatingHandlerStub clientHandlerStub = new DelegatingHandlerStub((request, cancellationToken) => {
                request.SetConfiguration(configuration);

                if (request.RequestUri.PathAndQuery.Contains("devices", StringComparison.OrdinalIgnoreCase))
                {
                    HttpResponseMessage devices = request.CreateResponse(HttpStatusCode.OK);

                    devices.Content = new StringContent(JsonConvert.SerializeObject(device));

                    return Task.FromResult(devices);
                }

                HttpResponseMessage response = request.CreateResponse(HttpStatusCode.OK, "");

                return Task.FromResult(response);
            });

            mockFactory.Setup(s => s.CreateClient(It.IsAny<string>())).Returns(() =>
            {
                HttpClient c = new HttpClient(clientHandlerStub)
                {
                    BaseAddress = new Uri("https://accounts.spotify.com/api/")
                };

                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "SelecTunes-Api");

                return c;
            });
        }

        [Test]
        public async Task AssertThatSpotifyQueueWorks()
        {
            SetupFactory();

            User user = new User
            {
                Token = new AccessAuthToken()
                {
                    AccessToken = "",
                }
            };

            bool result = await _playback.SendToSpotifyQueue(user, "spotify:test:123").ConfigureAwait(false);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AssertThatSpotifyPlaybackWorks()
        {
            Devices device = new Devices
            {
                Ope = new List<Device>
                {
                    new Device
                    {
                        IsActive = true,
                        Id = "1234",
                        IsPrivateSession = false,
                        IsRestricted = false,
                        Name = "Test Device",
                        Type = "Smartphone",
                        VolumePercent = 100,
                    }
                }
            };

            SetupFactory(device);

            User user = new User
            {
                Token = new AccessAuthToken()
                {
                    AccessToken = "",
                }
            };

            bool result = await _playback.BeginPlayback(user).ConfigureAwait(false);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task AssertThatGetRandomDeviceReturnsTheTestDevice()
        {
            Devices device = new Devices
            {
                Ope = new List<Device>
                {
                    new Device
                    {
                        IsActive = true,
                        Id = "1234",
                        IsPrivateSession = false,
                        IsRestricted = false,
                        Name = "Test Device",
                        Type = "Smartphone",
                        VolumePercent = 100,
                    }
                }
            };

            SetupFactory(device);

            User user = new User
            {
                Token = new AccessAuthToken()
                {
                    AccessToken = "",
                }
            };

            Device result = await _playback.GetRandomDevice(user).ConfigureAwait(false);

            Assert.AreEqual(device.Ope.First().Id, result.Id);
        }
    }
}
