namespace Core.Helper.Extensions
{
    public static class EnumExtensions
    {
        #region Parse

        //Link ref: http://tikalk.com/net/enum-extensions
        /// <summary>
        /// Converts the string representation of the name or numeric value of one or
        /// more enumerated constants to an equivalent enumerated object
        /// </summary>
        /// <param name="value">A string containing the name or value to convert</param>
        /// <returns>Enum whose value is represented by value</returns>
        public static T Parse<T>(string value) where T : struct
        {
            return Parse<T>(value, false);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or
        /// more enumerated constants to an equivalent enumerated object
        /// </summary>
        /// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
        /// <param name="value">A string containing the name or value to convert</param>
        /// <returns>Enum whose value is represented by value</returns>
        private static T Parse<T>(string value, bool ignoreCase) where T : struct
        {
            //return (T)Enum.Parse(typeof(T), value, ignoreCase);
            return default(T);
        }

        #endregion
    }
}
