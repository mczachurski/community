using System;

namespace SunLine.Community.Common
{
    public static class DateTimeExtensions
    {
        private static readonly long DatetimeMinTimeTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        public static long ToJavaScriptMilliseconds(this DateTime dt)
        {
            return (dt.ToUniversalTime().Ticks - DatetimeMinTimeTicks) / 10000;
        }

        public static string GetTimeString(this DateTime dateTime)
        {
            var diff = DateTime.UtcNow - dateTime;

            if (diff.TotalHours < 1)
            {
                return string.Format("{0:0.} minutes ago", diff.TotalMinutes);
            }

            if (diff.TotalDays < 1)
            {
                return string.Format("{0:0.} hours ago", diff.TotalHours);
            }

            if (diff.TotalDays < 30)
            {
                return dateTime.ToString("M");
            }

            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
