namespace BlockExplorer.Utilities
{
    using System;

    public static class DateTimeHelpers
    {
        public static string ConvertToDateTime(this long timestamp)
        {
            return new DateTime(timestamp).ToString("dd MMM yyyy hh:mm:ss"); ;
        }
    }
}
