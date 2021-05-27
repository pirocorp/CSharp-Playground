namespace ConsoleWallet.Utilities
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using GrpcService;

    public static class TransactionHelpers
    {
        public static string GenerateHash(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            var hash = SHA256.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static string GetTransactionHash(TransactionInput transactionInput, TransactionOutput transactionOutput)
            => GenerateHash(transactionInput.TimeStamp + transactionInput.SenderAddress + transactionOutput.Amount + transactionOutput.Fee + transactionOutput.RecipientAddress);
    }
}
