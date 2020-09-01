using System.Text.RegularExpressions;

namespace Core.Helper.Extensions
{
    public static class MobileExtension
    {
        public static bool IsPhoneNumber(string phone)
        {
            var regex = new Regex(@"^[\d\s\-\+\(\)]*$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(phone).Length > 0;
        }
    }
}
