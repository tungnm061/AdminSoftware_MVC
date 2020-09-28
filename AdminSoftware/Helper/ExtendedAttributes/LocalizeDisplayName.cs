//using System.ComponentModel;

//namespace AdminSoftware.Helper.ExtendedAttributes
//{
//    public class LocalizeDisplayName : DisplayNameAttribute
//    {
//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="input"></param>
//        public LocalizeDisplayName(string input) : base(input)
//        {

//        }

//        /// <summary>
//        /// Override Display Name to localizing
//        /// </summary>
//        public override string DisplayName
//        {
//            get
//            {
//                string fromResource = LocalizeHelper.GetValue(DisplayNameValue);
//                if(!string.IsNullOrEmpty(fromResource))
//                {
//                    return fromResource;
//                }
//                return base.DisplayName;
//            }
//        }
//    }
//}