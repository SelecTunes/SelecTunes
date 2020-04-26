using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SelecTunes.Backend.Controllers;
using SelecTunes.Backend.Data;

namespace SelecTunes.Backend.Test.Controllers
{
    internal class PartyControllerTest
    {
        private static readonly DbContextOptions optionBuilder = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: "selectunes").Options;

        private readonly Mock<ApplicationContext> mockContext = new Mock<ApplicationContext>(optionBuilder);
        private static readonly Mock<FakeUserManager> mockUserManager = new Mock<FakeUserManager>();
        private readonly Mock<ILogger<PartyController>> mockLogger = new Mock<ILogger<PartyController>>();

        private readonly PartyController controller;

        public PartyControllerTest() => controller = new PartyController(mockContext.Object, mockUserManager.Object, mockLogger.Object);

        [Test]
        public void AssertThatInstantiatedControllerIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<PartyController>(controller);
        }
    }
}
