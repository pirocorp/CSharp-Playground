namespace Coin.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.Json;

    public static class ConverterExtensions
    {
        public static byte[] ConvertToByte(this Transaction[] transactions)
            => JsonSerializer.SerializeToUtf8Bytes(transactions);

        public static byte[] ConvertToBytes(this string arg)
            => Encoding.UTF8.GetBytes(arg);

        public static byte[] ConvertHexToByteArray(this string hex)
        {
            var length = hex.Length;

            var bytes = new byte[length / 2];

            for (var i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static string ConvertToDateTime(this long timestamp, string format = "dd MMM yyyy hh:mm:ss")
            => new DateTime(timestamp).ToString(format);

        public static string ConvertToHexString(this byte[] byteArray)
            => Convert.ToHexString(byteArray).ToLower();

        public static string GenerateHash(this string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var hash = SHA256.Create().ComputeHash(bytes);

            return hash.ConvertToHexString();
        }

        public static string ConvertToString(this IEnumerable<Transaction> transactions)
            => JsonSerializer.Serialize(transactions);
    }
}
