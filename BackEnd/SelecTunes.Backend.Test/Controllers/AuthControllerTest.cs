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

namespace SelecTunes.Backend.Test.Controllers
{

    internal class AuthControllerTest
    {
        private static readonly DbContextOptions optionBuilder = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: "selectunes").Options;

        private readonly Mock<ApplicationContext> mockContext = new Mock<ApplicationContext>(optionBuilder);
        private readonly Mock<IDistributedCache> mockCache = new Mock<IDistributedCache>();
        private readonly Mock<IHttpClientFactory> mockFactory = new Mock<IHttpClientFactory>();

        private readonly IndexController controller;

        public AuthControllerTest() => controller = new IndexController(mockContext.Object, mockCache.Object, mockFactory.Object);

        [Test]
        public void AssertThatInstantiatedControllerIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<AuthController>(controller);
        }
    }
}
