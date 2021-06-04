namespace Coin
{
    using System;

    public class Transaction
    {
        public Transaction(string sender, string recipient, double amount, double fee)
        {
            this.TimeStamp = DateTime.Now.Ticks;
            this.Sender = sender;
            this.Recipient = recipient;
            this.Amount = amount;
            this.Fee = fee;
        }

        public string Hash { get; set; }

        public long TimeStamp { get; init;  }

        public string Sender { get; private set; }

        public string Recipient { get; private set; }

        public double Amount { get; private set; }

        public double Fee { get; private set; }
    }
}
