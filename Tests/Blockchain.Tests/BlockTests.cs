using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Blockchain.Tests
{
    [TestClass]
    public class BlockTests
    {
        [TestMethod]
        public void MinBlock_HashesBlockWithDifficulity()
        {
            var block = new Core.Block(null, new System.Collections.Generic.List<Core.Transaction>(), DateTime.UtcNow);
            block.Mine(3);

            Assert.IsNotNull(block);
            Assert.IsNotNull(block.HashValue);
        }

        [TestMethod]
        public void HashBlock_MustReturnSameMinedHashValue()
        {
            var block = new Core.Block(null, new System.Collections.Generic.List<Core.Transaction>(), DateTime.UtcNow);
            block.Mine(3);

            Assert.IsNotNull(block);
            Assert.AreEqual(block.CalculatedHash(), block.HashValue);
        }
    }
}
