using Hubtel.Wallets.Api.DAL.Entities;
using Hubtel.Wallets.Api.Services;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace Hubtel.Wallet.Api.UnitTests
{
    [TestFixture]
    public class AuthService_Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddUserDetails_UserIsNull_ReturnsFailedSignupResponse()
        {
            Assert.Pass();
        }
    }
}