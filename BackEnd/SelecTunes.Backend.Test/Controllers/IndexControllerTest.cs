using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SelecTunes.Backend.Controllers;

namespace SelecTunes.Backend.Test.Controllers
{
    internal class IndexControllerTest
    {
        private readonly IndexController controller;

        public IndexControllerTest() => controller = new IndexController();

        [Test]
        public void AssertThatInstantiatedControllerIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<IndexController>(controller);
        }

        [Test]
        public void AssertIndexWhenCalledReturnsOkResult()
        {
            RedirectResult response = controller.Index();

            Assert.IsInstanceOf<RedirectResult>(response);
        }
    }
}
