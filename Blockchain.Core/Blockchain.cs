using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blockchain.Core
{
    [JsonObject]
    public class Blockchain
    {
        [JsonProperty]
        public int Difficulity { get; set; } = 3;

        [JsonProperty]
        public IList<Block> Blocks { get; set; } //TODO private set.

        [JsonProperty]
        public IList<Transaction> Pending { get; set; } //TODO private set.

        public Blockchain()
        {
            var genesisBlock = CreateGenesisBlock();
            genesisBlock.Mine(Difficulity);

            Blocks = new List<Block>
            {
                genesisBlock
            };
            Pending = new List<Transaction>();
        }

        private static Block CreateGenesisBlock()
        {
            return new Block(null, new List<Transaction>(), DateTime.UtcNow);
        }

        public Block LastBlock()
        {
            return Blocks[Blocks.Count - 1];
        }

        public bool IsValid()
        {
            for (var index = 1; index < Blocks.Count; index++)
            {
                var previousBlock = Blocks[index - 1];

                if (!Blocks[index].HashValue.Equals(Blocks[index].CalculatedHash()))
                {
                    return false;
                }

                if (!previousBlock.HashValue.Equals(Blocks[index].PreviousHash))
                {
                    return false;
                }
            }
            return true;
        }

        public void CreateTransaction(Transaction transaction)
        {
            Pending.Add(transaction);
        }

        public bool SyncChain(Block block)
        {
            if (block.CalculatedHash().Equals(block.HashValue))
            {
                var previous = LastBlock();
                if (block.PreviousHash.Equals(previous.HashValue))
                {
                    block.Index = previous.Index + 1;
                    Blocks.Add(block);
                    return true;
                }
            }

            return false;
        }

        public Block ProcessPending(string minerAddress)
        {
            var previous = LastBlock();
            var block = new Block(previous.HashValue, Pending.ToList(), DateTime.UtcNow);
            if (SyncChain(block))
            {
                Pending = new List<Transaction>
                {
                    new Transaction("NETWORK", minerAddress, 0.25)
                };
            }
            return block;
        }
    }
}
