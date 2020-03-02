using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SelecTunes.Backend.Controllers;
using System;
using System.Collections.Generic;
using Moq;
using System.Net.Http;
using Microsoft.Extensions.Caching.Distributed;
using SelecTunes.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using SelecTunes.Backend.Models;
using Microsoft.Extensions.Logging;
using SelecTunes.Backend.Helper;

namespace SelecTunes.Backend.Test.Controllers
{
    internal class AuthControllerTest
    {
        private static readonly DbContextOptions optionBuilder = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: "selectunes").Options;

        private readonly Mock<ApplicationContext> mockContext = new Mock<ApplicationContext>(optionBuilder);
        private readonly Mock<IDistributedCache> mockCache = new Mock<IDistributedCache>();
        private readonly Mock<IHttpClientFactory> mockFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
        private readonly Mock<IOptions<AppSettings>> mockOptions = new Mock<IOptions<AppSettings>>();
        private readonly Mock<UserManager<User>> mockUserManager = new Mock<UserManager<User>>();
        private readonly Mock<SignInManager<User>> mockSignInManager = new Mock<SignInManager<User>>();
        private readonly Mock<ILogger<AuthController>> mockLogger = new Mock<ILogger<AuthController>>();

        private readonly Mock<AuthHelper> mockHelper = new Mock<AuthHelper>();

        private readonly Mock<Random> mockRandom = new Mock<Random>();

        private readonly AuthController controller;

        public AuthControllerTest() => controller = new AuthController(mockContext.Object, mockCache.Object, mockFactory.Object, mockConfig.Object, mockOptions.Object, mockUserManager.Object, mockSignInManager.Object, mockLogger.Object, mockHelper.Object);

        [Test]
        public void AssertThatInstantiatedControllerIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<AuthController>(controller);
        }
    }
}
