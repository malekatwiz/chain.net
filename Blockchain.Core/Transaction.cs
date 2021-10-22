namespace Blockchain.Core
{
    public class Transaction
    {
        public string SenderAddress { get; set; }
        public string ReceiverAddress { get; set; }
        public double Amount { get; set; }

        public Transaction(string sender, string receiver, double amount)
        {
            SenderAddress = sender;
            ReceiverAddress = receiver;
            Amount = amount;
        }
    }
}
