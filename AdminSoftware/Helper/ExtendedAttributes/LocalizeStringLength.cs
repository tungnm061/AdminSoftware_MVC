using System.ComponentModel.DataAnnotations;

namespace AdminSoftware.Helper.ExtendedAttributes
{
    public class LocalizeStringLengthAttribute : StringLengthAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maximumLength"></param>
        public LocalizeStringLengthAttribute(int maximumLength) : base(maximumLength)
        {

        }

        /// <summary>
        /// リソースからリソース名を基づいて、エラーメッセージを取得する。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            if (MinimumLength > 0)
            {
                string fromResource = "[ {0} ] yêu cầu tối thiểu [ {1} ] ký tự";
                if (!string.IsNullOrEmpty(fromResource))
                {
                    return string.Format(fromResource, name, MinimumLength);
                }
            }
            else
            {
                string fromResource = "[ {0} ] cho phép tối đa [ {1} ] ký tự";
                if (!string.IsNullOrEmpty(fromResource))
                {
                    return string.Format(fromResource, name, MaximumLength);
                }
            }
            return base.FormatErrorMessage(name);
        }

        /// <summary>
        /// valid maxlength
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            bool bResult = base.IsValid(value);
            if (!bResult)
            {
                if (value != null && value.ToString().Length > this.MaximumLength)
                {
                    //to mark line break: Windows OS use two character CR and LF sequence but other only use one of them, so, The length is greater than the number of occurrences of line break
                    if (value.ToString().Length - System.Text.RegularExpressions.Regex.Matches(value.ToString(), @"\r\n").Count <= this.MaximumLength) {
                        bResult = true;
                    }
                }
            }
            return bResult;
        }

    }
}