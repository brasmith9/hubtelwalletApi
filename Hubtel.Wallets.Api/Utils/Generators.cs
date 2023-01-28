using System;
using System.Linq;

namespace Hubtel.Wallets.Api.Utils
{
    public static class Generators
    {
        private readonly static Random random = new Random();


        public static string GenerateRandomPin(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray()).ToString();
        }
    }
}