using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Blockchain.Core
{
    public static class Hasher
    {
        public static string HashBlock(Block block)
        {
            var sha256 = SHA256.Create();
            var input = Encoding.ASCII.GetBytes($"{block.TimeStamp}-{block.PreviousHash}-{JsonConvert.SerializeObject(block.Transactions)}-{block.Nonce}");

            return Convert.ToBase64String(sha256.ComputeHash(input));
        }
    }
}
