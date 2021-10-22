using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Blockchain.Core
{
    [JsonObject]
    public class Block
    {
        [JsonProperty(nameof(Index))]
        public int Index { get; set; }

        [JsonProperty(nameof(TimeStamp))]
        public DateTime TimeStamp { get; set; } //TODO private set.

        [JsonProperty(nameof(PreviousHash))]
        public string PreviousHash { get; set; }

        [JsonProperty(nameof(HashValue))]
        public string HashValue { get; set; } //TODO private set.

        [JsonProperty(nameof(Nonce))]
        public int Nonce { get; set; } //TODO private set.

        [JsonProperty(nameof(Transactions))]
        public IList<Transaction> Transactions { get; set; } //TODO private set.

        public Block(string previousHash, List<Transaction> transactions, DateTime timeStamp)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Transactions = transactions;
        }

        public string CalculatedHash()
        {
            return Hasher.HashBlock(this);
        }

        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);
            while(string.IsNullOrEmpty(HashValue) ||
                !HashValue.Substring(0, difficulty).Equals(leadingZeros))
            {
                Nonce++;
                HashValue = CalculatedHash();
            }
        }
    }
}
