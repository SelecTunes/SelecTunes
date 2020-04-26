﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SelecTunes.Backend.Data;
using SelecTunes.Backend.Helper;
using SelecTunes.Backend.Models;
using SelecTunes.Backend.Models.Auth;
using System;
using System.Collections.Generic;

namespace SelecTunes.Backend.Test.Controllers
{

    internal class AuthHelperTest
    {
        private readonly AuthHelper authHelper;

        private static readonly DbContextOptions contextOptions = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: "selectunes").Options;
        private readonly Mock<ApplicationContext> mockContext = new Mock<ApplicationContext>(contextOptions);

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

        [Test]
        public void AssertBanUserReturnsFalseOnNullToBan()
        {
            User CurrentUser = new User()
            {
                IsBanned = false,
                Party = null,
                PartyId = null,
                Token = null,
                Strikes = 0
            };

            User ToBan = new User()
            {
                IsBanned = false,
                Party = null,
                PartyId = null,
                Token = null,
                Strikes = 0
            };

            Assert.False(authHelper.BanUser(null, null, null));
        }

        [Test]
        public void AssertBanUserReturnsFalseOnNullCurrent()
        {
            User CurrentUser = new User()
            {
                IsBanned = false,
                Party = null,
                PartyId = null,
                Token = null,
                Strikes = 0
            };

            User ToBan = new User()
            {
                IsBanned = false,
                Party = null,
                PartyId = null,
                Token = null,
                Strikes = 0
            };

            Assert.False(authHelper.BanUser(ToBan, null, null));
        }

        [Test]
        public void AssertBanUserReturnsFalseOnNullContext()
        {
            User CurrentUser = new User()
            {
                IsBanned = false,
                Party = null,
                PartyId = null,
                Token = null,
                Strikes = 0
            };

            User ToBan = new User()
            {
                IsBanned = false,
                Party = null,
                PartyId = null,
                Token = null,
                Strikes = 0
            };

            Assert.False(authHelper.BanUser(ToBan, CurrentUser, null));
        }

        public class IdentityResultMock : IdentityResult
        {
            public IdentityResultMock(bool succeeded)
            {
                this.Succeeded = succeeded;
            }
        }

        [Test]
        public void AssertSucceededReturnsSuccess()
        {
            IdentityResultMock identityResultMock = new IdentityResultMock(true);
            Assert.AreEqual("Success", authHelper.ParseIdentityResult(identityResultMock));
        }
    }
}