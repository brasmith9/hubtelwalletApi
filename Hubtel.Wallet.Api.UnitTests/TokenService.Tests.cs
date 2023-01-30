using System;
using System.Collections.Generic;
using System.Linq;
using Hubtel.Wallets.Api.DAL.Entities;
using Hubtel.Wallets.Api.Services;
using Moq;
using NUnit.Framework;

namespace Hubtel.Wallet.Api.UnitTests
{
    [TestFixture]
    public class TokenService_Tests
    {

        public void GenerateTokenAsync_ReturnsJwt()
        {
            var users = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.it"
                }

            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();
            fakeUserManager.Setup(x => x.Users).Returns(users);

            var result = TokenService.GenerateTokenAsync(_userManager, fakeUserManager);

            Assert.That(result, Is.Not.Null);
        }
    }
}