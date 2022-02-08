using System;

namespace MarketingBox.Reporting.Service.Domain
{
    public static class DateTimeExtensions
    {
        public static DateTime ToUtc(this DateTime value)
        {
            return DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
}