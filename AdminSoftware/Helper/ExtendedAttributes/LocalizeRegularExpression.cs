using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminSoftware.Helper.ExtendedAttributes
{
    public class LocalizeRegularExpressionAttribute : RegularExpressionAttribute
    {
        private string keyResource = string.Empty;
        private string[] listParam = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pattern"></param>
        public LocalizeRegularExpressionAttribute(string pattern) : base(pattern)
        {

        }

        /// <summary>
        /// Constructor（リソースとパラメータ指定）
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="keyResource"></param>
        /// <param name="listParam"></param>
        public LocalizeRegularExpressionAttribute(string pattern, string keyResource, params string[] listParam) : base(pattern)
        {
            this.keyResource = keyResource;
            this.listParam = listParam;
        }

        /// <summary>
        /// リソースからリソース名を基づいて、エラーメッセージを取得する。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            if(!string.IsNullOrEmpty(keyResource))
            {
                string fromResource = "[ {0} ] sai định dạng";

                // パラメータがなければ、項目名だけをメッセージに差し替える。
                if (listParam == null || listParam.Length == 0)
                {
                    return string.Format(fromResource, name);
                }

                // パラメータがあれば、項目名・パラメータ値をメッセージに差し替える。
                List<string> tmpParams = new List<string>();
                tmpParams.Add(name);
                tmpParams.AddRange(this.listParam);
                return string.Format(fromResource, tmpParams.ToArray());
            }

            return base.FormatErrorMessage(name);
        }

    }
}