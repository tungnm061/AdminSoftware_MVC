using System.ComponentModel.DataAnnotations;

namespace AdminSoftware.Helper.ExtendedAttributes
{
    public class LocalizeRequiredAttribute : RequiredAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LocalizeRequiredAttribute() : base()
        {
            AllowEmptyStrings = true;
        }
        
        /// <summary>
        /// リソースからリソース名を基づいて、エラーメッセージを取得する。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override string FormatErrorMessage(string name)
        {
            string fromResource = "[ {0} ] chưa có dữ liệu";
            if(!string.IsNullOrEmpty(fromResource))
            {
                return string.Format(fromResource, name);
            }            

            return base.FormatErrorMessage(name);
        }
    }
}