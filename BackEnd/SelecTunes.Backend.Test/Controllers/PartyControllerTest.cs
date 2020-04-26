using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SelecTunes.Backend.Controllers;
using Moq;
using SelecTunes.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace SelecTunes.Backend.Test.Controllers
{

    internal class PartyControllerTest
    {
        private static readonly DbContextOptions optionBuilder = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: "selectunes").Options;

        private readonly Mock<ApplicationContext> mockContext = new Mock<ApplicationContext>(optionBuilder);
        private static readonly Mock<FakeUserManager> mockUserManager = new Mock<FakeUserManager>();
        private readonly Mock<ILogger<PartyController>> mockLogger = new Mock<ILogger<PartyController>>();
        private readonly Mock<IHttpClientFactory> mockHttpFactory = new Mock<IHttpClientFactory>();

        private readonly PartyController controller;

        public PartyControllerTest() => controller = new PartyController(mockContext.Object, mockUserManager.Object, mockLogger.Object, mockHttpFactory.Object);

        [Test]
        public void AssertThatInstantiatedControllerIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<PartyController>(controller);
        }
    }
}
