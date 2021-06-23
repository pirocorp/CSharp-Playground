namespace Coin.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using EllipticCurve;

    public static class TransactionHelpers
    {
        public static string GenerateHash(string data)
        {
            var bytes = Encoding.ASCII.GetBytes(data);
            var hash = SHA256.Create().ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public static string GetTransactionHash(Transaction input)
        {
            var data =
                input.TimeStamp +
                input.Sender +
                input.Amount +
                input.Fee +
                input.Recipient;

            return GenerateHash(data);
        }

        public static bool VerifySignature(string publicKeyHex, string message, string signature)
        {
            var @byte = publicKeyHex.ConvertHexToByteArray();
            var publicKey = PublicKey.fromString(@byte);
            return Ecdsa.verify(message, Signature.fromBase64(signature), publicKey);
        }
    }
}
