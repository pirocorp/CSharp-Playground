namespace Coin
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Json;

    public static class ConverterExtensions
    {
        public static byte[] ConvertToByte(this Transaction[] transactions)
            => JsonSerializer.SerializeToUtf8Bytes(transactions);

        public static byte[] ConvertToBytes(this string arg)
            => Encoding.UTF8.GetBytes(arg);

        public static string ConvertToDateTime(this long timestamp, string format = "dd MMM yyyy hh:mm:ss")
            => new DateTime(timestamp).ToString(format);

        public static string ConvertToHexString(this byte[] byteArray, string delimiter = "")
            => BitConverter.ToString(byteArray).Replace("-", delimiter);

        public static string ConvertToString(this IEnumerable<Transaction> transactions)
            => JsonSerializer.Serialize(transactions);
    }
}
