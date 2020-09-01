using System;
using System.Collections.Generic;

namespace Core.Data
{

    /// <summary>
    /// Class DataSettings
    /// </summary>
    public partial class DataSettings
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSettings"/> class.
        /// </summary>
        public DataSettings()
        {
            RawDataSettings = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets or sets the data provider.
        /// </summary>
        /// <value>The data provider.</value>
        public string DataProvider { get; set; }

        /// <summary>
        /// Gets or sets the data connection string.
        /// </summary>
        /// <value>The data connection string.</value>
        public string DataConnectionString { get; set; }

        /// <summary>
        /// Gets the raw data settings.
        /// </summary>
        /// <value>The raw data settings.</value>
        public IDictionary<string, string> RawDataSettings { get; private set; }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
        public bool IsValid()
        {
            return !String.IsNullOrEmpty(this.DataProvider) && !String.IsNullOrEmpty(this.DataConnectionString);
        }
    }
}
