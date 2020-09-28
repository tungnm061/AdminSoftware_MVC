using System.ComponentModel.DataAnnotations;

namespace Entity.Helper.ExtendedAttributes
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

    }
}