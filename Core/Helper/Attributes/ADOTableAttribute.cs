using System;

namespace Core.Helper.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class ADOTableAttribute : Attribute
    {
        public readonly string Table;
        public ADOTableAttribute(string table)  // url is a positional parameter
        {
            Table = table;
        }
    }
}
