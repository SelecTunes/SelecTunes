using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SelecTunes.Controllers;
using System;
using System.Collections.Generic;

namespace SelecTunes.Test.Controllers
{
    internal class IndexControllerTest
    {
        IndexController controller;

        public IndexControllerTest() => controller = new IndexController();
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
