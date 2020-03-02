using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SelecTunes.Backend.Controllers;
using System;
using System.Collections.Generic;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Models;

namespace SelecTunes.Backend.Test.Controllers
{
    
    internal class IndexControllerTest
    {
        private readonly Mock<ApplicationContext> mockContext = new Mock<ApplicationContext>();
        private readonly Mock<IDistributedCache> mockCache = new Mock<IDistributedCache>();
        private readonly Mock<IHttpClientFactory> mockFactory = new Mock<IHttpClientFactory>();

        private readonly IndexController controller;

        public IndexControllerTest() => controller = new IndexController(mockContext.Object, mockCache.Object, mockFactory.Object);

        [Test]
        public void AssertThatInstantiatedControllerIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<IndexController>(controller);
        }

        [Test]
        public void AssertIndexWhenCalledReturnsOkResult()
        {
            ActionResult<List<String>> response = controller.Index();

            Assert.IsInstanceOf<OkObjectResult>(response.Result);
        }

        [Test]
        public void AssertIndexWhenCalledReturnsListWithOneElement()
        {
            OkObjectResult result = (controller.Index()).Result as OkObjectResult;

            Assert.IsInstanceOf<List<String>>(result.Value);

            Assert.AreEqual(1, (result.Value as List<String>).Count);
        }
    }
    
}
