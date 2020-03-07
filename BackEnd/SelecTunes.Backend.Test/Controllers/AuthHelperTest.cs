using NUnit.Framework;
using SelecTunes.Backend.Helper;
using System;

namespace SelecTunes.Backend.Test.Controllers
{

    internal class AuthHelperTest
    {
        private readonly AuthHelper authHelper;

        public AuthHelperTest() => authHelper = new AuthHelper();

        [Test]
        public void AssertThatInstantiatedControllerIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<AuthHelper>(authHelper);
        }

        [Test]
        public void AssertUpdateUserTokenNullHandling()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await authHelper.UpdateUserTokens(null));
        }
    }
}