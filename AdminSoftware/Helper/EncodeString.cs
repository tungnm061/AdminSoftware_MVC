using System.Text.RegularExpressions;

namespace AdminSoftware.Helper
{
    public class EncodeString
    {
        private static readonly string[] VietNamChar =
        {
            "aAeEoOuUiIdDyY",
            "áàạảãâấầậẩẫăắằặẳẵ",
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
            "éèẹẻẽêếềệểễ",
            "ÉÈẸẺẼÊẾỀỆỂỄ",
            "óòọỏõôốồộổỗơớờợởỡ",
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
            "úùụủũưứừựửữ",
            "ÚÙỤỦŨƯỨỪỰỬỮ",
            "íìịỉĩ",
            "ÍÌỊỈĨ",
            "đ",
            "Đ",
            "ýỳỵỷỹ",
            "ÝỲỴỶỸ"
        };

        /// <summary>
        /// Replaces the unicode.
        /// </summary>
        /// <param name="input">The string input.</param>
        /// <returns></returns>
        public static string ReplaceUnicode(string input)
        {
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                {
                    input = input.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
                }
            }
            return Regex.Replace(input.Replace(" ", "").ToUpper(), @"[^\w\d-]", ""); 
        }

    }
}