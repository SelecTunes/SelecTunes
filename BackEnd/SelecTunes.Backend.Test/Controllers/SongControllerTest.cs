using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SelecTunes.Backend.Controllers;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Helper;
using System;
using System.Net.Http;

namespace SelecTunes.Backend.Test.Controllers
{

    internal class SongControllerTest
    {
        private static readonly DbContextOptions optionBuilder = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: "selectunes").Options;

        private readonly Mock<ApplicationContext> mockContext = new Mock<ApplicationContext>(optionBuilder);
        private readonly Mock<IDistributedCache> mockCache = new Mock<IDistributedCache>();
        private static readonly Mock<FakeUserManager> mockUserManager = new Mock<FakeUserManager>();
        private readonly Mock<ILogger<SongController>> mockLogger = new Mock<ILogger<SongController>>();
        private readonly Mock<IHttpClientFactory> mockFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<AuthHelper> mockHelper = new Mock<AuthHelper>();

        private readonly SongController controller;

        public SongControllerTest() => controller = new SongController(mockContext.Object, mockCache.Object, mockFactory.Object, mockLogger.Object, mockUserManager.Object, mockHelper.Object);

        [Test]
        public void AssertThatInstantiatedControllerIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<SongController>(controller);
        }

        [Test]
        public void AssertNeedsAuth()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await controller.Queue());
        }
    }
}
