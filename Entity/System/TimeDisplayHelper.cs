using System;

namespace Entity.System
{
    public class TimeDisplayHelper
    {
        public static string TimeDisplay(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var timeSpan = toDate.Subtract(fromDate);
                if (timeSpan.TotalDays < 1)
                {
                    if (timeSpan.TotalHours < 1)
                    {
                        if (timeSpan.TotalMinutes < 1)
                        {
                            if (timeSpan.Seconds < 10)
                            {
                                return "vừa xong";
                            }
                            return timeSpan.Seconds + " giây trước";
                        }
                        return timeSpan.Minutes + " phút trước";
                    }
                    return timeSpan.Hours + " giờ trước";
                }
                if (timeSpan.TotalDays < 7)
                {
                    return timeSpan.Days + " ngày trước";
                }
                if (timeSpan.TotalDays < 30)
                {
                    var week = timeSpan.Days/7;
                    return week + " tuần trước";
                }
                if (timeSpan.TotalDays < 60)
                {
                    return " 1 tháng trước";
                }
                if (timeSpan.TotalDays < 90)
                {
                    return " 2 tháng trước";
                }
                return fromDate.ToString("dd/MM/yyyy");
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}