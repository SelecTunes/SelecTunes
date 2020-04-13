using NUnit.Framework;
using SelecTunes.Backend.Helper;
using SelecTunes.Backend.Models.Auth;
using System;

namespace SelecTunes.Backend.Test.Controllers
{

    internal class AuthHelperTest
    {
        private readonly AuthHelper authHelper;

        public AuthHelperTest() => authHelper = new AuthHelper();

        [Test]
        public void AssertThatInstantiatedHelperIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<AuthHelper>(authHelper);
        }

        [Test]
        public void AssertUpdateUserTokenNullHandling()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await authHelper.UpdateUserTokens(null).ConfigureAwait(false));
        }

        [Test]
        public void AssertThatAssertValidLoginHandlesNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await authHelper.AssertValidLogin(null, false).ConfigureAwait(false));
        }

        [Test]
        public void AssertThatAssertValidLoginHandlesNonNullTokenButNullValues()
        {
            AccessAuthToken token = new AccessAuthToken()
            {
                AccessToken = null,
                RefreshToken = null,
                TokenType = "",
                CreateDate = DateTime.Now,
                Scope = ""
            };

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await authHelper.AssertValidLogin(token, false).ConfigureAwait(false));
        }
    }
}