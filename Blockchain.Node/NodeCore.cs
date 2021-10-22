using Serilog;

namespace Blockchain.Node
{
    public class NodeCore
    {
        private readonly Core.Blockchain _chain;
        private readonly string _nodeAddress;
        private bool _isSynced = true;
        private readonly BlockchainBus _bus;
        private readonly ILogger _logger;

        public NodeCore(string nodeId, ILogger logger)
        {
            _chain = new Core.Blockchain();
            _nodeAddress = nodeId;
            _logger = logger;
            _bus = new BlockchainBus(logger);
            _bus.Subscribe(_nodeAddress, this);
        }

        public bool Send(string sender, string receiver, double amount)
        {
            _chain.CreateTransaction(new Core.Transaction(sender, receiver, amount));
            _logger.Information($"created new transaction - sender '{sender}'");
            return true;
        }

        public void Mine()
        {
            if (!_isSynced)
            {
                _logger.Warning("blockchain is out of sync.");
                return;
            }

            var block = _chain.ProcessPending(_nodeAddress);
            _logger.Information($"node: '{_nodeAddress}'- mined new block. {block}");
            _bus.Publish(_nodeAddress, block);
            _logger.Information("published new block.");
        }

        public void SyncChain(Core.Block block)
        {
            if (_chain.SyncChain(block))
            {
                _isSynced = true;
            }
            else
            {
                _isSynced = false;
            }
        }
    }
}
