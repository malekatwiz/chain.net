using Blockchain.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blockchain.Tests
{
    [TestClass]
    public class ChainTests
    {
        [TestMethod]
        public void NewChain_AddGenesisBlock()
        {
            var chain = new Core.Blockchain();

            Assert.IsNotNull(chain);
            Assert.AreEqual(1, chain.Blocks.Count);
        }

        [TestMethod]
        public void CreateTransaction_AddNewPendingTransaction()
        {
            var chain = new Core.Blockchain();
            chain.CreateTransaction(new Transaction("ma", "aa", 1));
            chain.CreateTransaction(new Transaction("aa", "za", 0.5));
            
            Assert.IsNotNull(chain);
            Assert.AreEqual(2, chain.Pending.Count);
        }

        [TestMethod]
        public void ProcessPending_ProcessesPendingTransactions()
        {
            var chain = new Core.Blockchain();
            chain.CreateTransaction(new Transaction("ma", "aa", 1));
            chain.ProcessPending("miner");

            Assert.IsNotNull(chain);
            Assert.AreEqual(2, chain.Blocks.Count);
            Assert.IsNotNull(chain.Pending);
        }

        [TestMethod]
        public void ProcessPending_AddNewRewardTransaction()
        {
            var chain = new Core.Blockchain();
            chain.CreateTransaction(new Transaction("ma", "aa", 1));
            chain.CreateTransaction(new Transaction("aa", "za", 0.5));
            chain.ProcessPending("miner");

            Assert.IsNotNull(chain);
            Assert.AreEqual(2, chain.Blocks.Count);
            Assert.AreEqual(1, chain.Pending.Count);
        }

        [TestMethod]
        public void NewBlockInChain_LinksToPreviousBlock()
        {
            var chain = new Core.Blockchain();
            chain.CreateTransaction(new Transaction("ma", "aa", 1));
            chain.CreateTransaction(new Transaction("aa", "za", 0.5));
            chain.ProcessPending("miner-1");

            chain.CreateTransaction(new Transaction("za", "mm", 0.15));
            chain.ProcessPending("miner-2");

            Assert.IsNotNull(chain);
            Assert.AreEqual(3, chain.Blocks.Count);
            Assert.IsNotNull(chain.Blocks[1].PreviousHash);
            Assert.IsNotNull(chain.Blocks[2].PreviousHash);
            Assert.AreEqual(chain.Blocks[0].HashValue, chain.Blocks[1].PreviousHash);
            Assert.AreEqual(chain.Blocks[1].HashValue, chain.Blocks[2].PreviousHash);
        }

        [TestMethod]
        public void ChainIsValid_ReturnsTrue_WhenAllBlocksAreValid()
        {
            var chain = new Core.Blockchain();
            chain.CreateTransaction(new Transaction("ma", "aa", 1));
            chain.CreateTransaction(new Transaction("aa", "za", 0.5));
            chain.ProcessPending("miner");

            Assert.IsNotNull(chain);
            Assert.IsTrue(chain.IsValid());
        }
    }
}
