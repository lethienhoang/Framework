using System;

namespace Framework.Types
{
    public static class DateTimeHelper
    {
        public static DateTime GenerateDateTime() => DateTime.UtcNow;

        public static DateTime GenerateTodayUTC() => DateTime.UtcNow.Date;

        public static string DateTimeToString() => GenerateDateTime().ToLongTimeString();
    }
}
