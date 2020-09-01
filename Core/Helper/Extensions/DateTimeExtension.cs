using System;
using System.Linq;

namespace Core.Helper.Extensions
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Convert DateTime to DateTime by User Timezone Id
        /// </summary>
        /// <param name="timeZoneId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToUserTime(string timeZoneId, DateTime dateTime)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            if (timeZoneInfo != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc), timeZoneInfo);
            }
            return dateTime;
        }

        public static DateTime GetUserTimeNow(string timeZoneId)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            if (timeZoneInfo != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
            }
            return DateTime.UtcNow;
        }

        /// <summary>
        /// Convert DateTime to UTC DateTime by User Timezone Id
        /// </summary>
        /// <param name="timeZoneId"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToUtcTime(string timeZoneId, DateTime dateTime)
        {
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            if (timeZoneInfo != null)
            {
                return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified), timeZoneInfo);
            }
            return dateTime;
        }

        /// <summary>
        /// Convert DateTime to UTC DateTime by Server Timezone
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime ToUtcTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified), TimeZoneInfo.Local);
        }
    }
}
