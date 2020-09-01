using System;
using System.Globalization;

namespace Core.Helper.Extensions
{
    public static class FileExtension
    {
        public static string FormatFileSize(long bytes)
        {
            if (bytes == 0)
                return string.Empty;

            if (bytes >= 1073741824)
                return String.Format(CultureInfo.InvariantCulture, "{0:0.##}", (double)bytes / 1073741824) + " GB";
            if (bytes >= 1048576)
                return String.Format(CultureInfo.InvariantCulture, "{0:0.##}", (double)bytes / 1048576) + " MB";
            return String.Format(CultureInfo.InvariantCulture, "{0:0.##}", (double)bytes / 1024) + " KB";
        }
    }
}
