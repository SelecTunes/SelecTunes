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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace SelecTunes.Backend.Test.Controllers
{
    internal class AuthControllerTest
    {
        private static readonly DbContextOptions contextOptions = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase(databaseName: "selectunes").Options;

        private readonly Mock<ApplicationContext> mockContext = new Mock<ApplicationContext>(contextOptions);
        private readonly Mock<IDistributedCache> mockCache = new Mock<IDistributedCache>();
        private readonly Mock<IHttpClientFactory> mockFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
        private readonly Mock<IOptions<AppSettings>> mockOptions = new Mock<IOptions<AppSettings>>();
        private readonly Mock<ILogger<AuthController>> mockLogger = new Mock<ILogger<AuthController>>();
        private static readonly Mock<FakeUserManager> mockUserManager = new Mock<FakeUserManager>();
        private static readonly Mock<FakeSignInManager> mockSignInManager = new Mock<FakeSignInManager>();
        private readonly Mock<AuthHelper> mockHelper = new Mock<AuthHelper>();
        private readonly AuthController controller;

        public AuthControllerTest() => controller = new AuthController(mockContext.Object, mockCache.Object, mockFactory.Object, mockConfig.Object, mockOptions.Object, mockUserManager.Object, mockSignInManager.Object, mockLogger.Object, mockHelper.Object);

        [Test]
        public void AssertThatInstantiatedControllerIsInstanceOfTheClass()
        {
            Assert.IsInstanceOf<AuthController>(controller);
        }
    }

    public class FakeSignInManager : SignInManager<User>
    {
        public FakeSignInManager()
                : base(new FakeUserManager(),
                     new Mock<IHttpContextAccessor>().Object,
                     new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                     new Mock<IOptions<IdentityOptions>>().Object,
                     new Mock<ILogger<SignInManager<User>>>().Object,
                     new Mock<IAuthenticationSchemeProvider>().Object,
                     new Mock<IUserConfirmation<User>>().Object)
        { }
    }



    public class FakeUserManager : UserManager<User>
    {
        public FakeUserManager()
            : base(new Mock<IUserStore<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<IPasswordHasher<User>>().Object,
                  Array.Empty<IUserValidator<User>>(),
                  Array.Empty<IPasswordValidator<User>>(),
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<IServiceProvider>().Object,
                  new Mock<ILogger<UserManager<User>>>().Object)
        { }

        public override Task<User> FindByEmailAsync(string email)
        {
            return Task.FromResult(new User { Email = email });
        }

        public override Task<bool> IsEmailConfirmedAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User user is null");
            }
            return Task.FromResult(user.Email == "test@test.com");
        }

        public override Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return Task.FromResult("---------------");
        }
    }
}
