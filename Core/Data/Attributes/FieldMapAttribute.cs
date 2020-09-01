using System;

namespace Core.Data.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class FieldMapAttribute : Attribute
    {
        public string Column { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldMapAttribute"/> class.
        /// </summary>
        public FieldMapAttribute()
        {
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("");
        }
    }
}
