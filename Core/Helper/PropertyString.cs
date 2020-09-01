using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Core.Helper
{
    public class PropertyString
    {
        //private static CoreDbContext db = new CoreDbContext();

        private static Dictionary<string, Dictionary<string, string>> m_resourceCache = new Dictionary<string, Dictionary<string, string>>();

        private static SqlConnection m_connection;
        private static SqlCommand m_cmdGetPropertyByTypeAndKey;

        public static String GetString(string propertyType, string propertyKey)
        {
            if (propertyKey == "-1")
                return "Root";
            string resourceValue = null;
            string propertyKeys = propertyType + "." + propertyKey;
            Dictionary<string, string> resCacheByCulture = null;
            // check the cache first
            // find the dictionary for this culture
            // check for the inner dictionary entry for this key
            if (m_resourceCache.ContainsKey(CultureInfo.CurrentCulture.TwoLetterISOLanguageName))
            {
                resCacheByCulture = m_resourceCache[CultureInfo.CurrentCulture.TwoLetterISOLanguageName];
                if (resCacheByCulture.ContainsKey(propertyKeys))
                {
                    resourceValue = resCacheByCulture[propertyKeys];
                }
            }

            // if not in the cache, go to the database
            if (resourceValue == null)
            {
                resourceValue = GetPropertyByTypeAndKey(propertyType, propertyKey);

                // add this result to the cache
                // find the dictionary for this culture
                // add this key/value pair to the inner dictionary
                lock (typeof(ResourceString))
                {
                    if (resCacheByCulture == null)
                    {
                        resCacheByCulture = new Dictionary<string, string>();
                        m_resourceCache.Add(CultureInfo.CurrentCulture.TwoLetterISOLanguageName, resCacheByCulture);
                    }
                    resCacheByCulture.Add(propertyKeys, resourceValue);
                }
            }
            return resourceValue;
        }

        private static string GetPropertyByTypeAndKey(string propertyType, string propertyKey)
        {

            m_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CoreDBContext"].ConnectionString);

            //command to retrieve the resource the matches 
            //a specific type, culture and key
            m_cmdGetPropertyByTypeAndKey = new SqlCommand("SELECT propertyType, CultureId, propertyKey, propertyValue FROM LocalizeProperties WHERE (propertyType=@propertyType) AND (CultureId=@CultureId) AND (propertyKey=@propertyKey)");
            m_cmdGetPropertyByTypeAndKey.Connection = m_connection;
            m_cmdGetPropertyByTypeAndKey.Parameters.AddWithValue("propertyType", propertyType);
            m_cmdGetPropertyByTypeAndKey.Parameters.AddWithValue("CultureId", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            m_cmdGetPropertyByTypeAndKey.Parameters.AddWithValue("propertyKey", propertyKey);

            // we should only get one back, but just in case, we'll iterate reader results
            StringCollection resources = new StringCollection();
            string resourceValue = null;
            //var _resource = db.resources.Where(r => r.propertyType == propertyType && r.CultureId == CultureInfo.CurrentCulture.TwoLetterISOLanguageName && r.propertyKey == propertyKey);
            try
            {
                m_connection.Open();
                // get resources from the database
                using (SqlDataReader reader = m_cmdGetPropertyByTypeAndKey.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        resources.Add(reader.GetString(reader.GetOrdinal("propertyValue")));
                    }
                }
            }
            finally
            {
                m_connection.Close();
            }
            //we should only get 1 back, this is just to verify the tables aren't incorrect
            if (resources.Count == 0)
            {
                ////if not exists return its key
                //resourceValue = propertyType + "-" + propertyKey;
                ////update new key to database
                //Core.Data.CoreDbContext db = new Data.CoreDbContext();
                //Core.Data.LocalizeProperties lp = new Data.LocalizeProperties { CultureId = CultureInfo.CurrentCulture.TwoLetterISOLanguageName, PropertyType = propertyType, PropertyKey = propertyKey, PropertyValue = resourceValue, SeoValue = resourceValue };
                //db.localizeProperties.Add(lp);
                //db.SaveChanges();
            }
            else if (resources.Count == 1)
            {
                resourceValue = resources[0];
            }
            else
            {
                // if > 1 row returned, log an error, we shouldn't have > 1 value for a propertyKey!
                throw new DataException(String.Format("Duplicated resource key {0}.{1}", propertyType, propertyKey));
            }
            return resourceValue;
        }
        public static void RemoveKey(string propertyType, string propertyKey)
        {
            string propertyKeys = propertyType + "." + propertyKey;
            Dictionary<string, string> resCacheByCulture = null;
            // check the cache first
            // find the dictionary for this culture
            // check for the inner dictionary entry for this key
            if (m_resourceCache.ContainsKey(CultureInfo.CurrentCulture.TwoLetterISOLanguageName))
            {
                resCacheByCulture = m_resourceCache[CultureInfo.CurrentCulture.TwoLetterISOLanguageName];
                if (resCacheByCulture.ContainsKey(propertyKeys))
                {
                    resCacheByCulture.Remove(propertyKeys);
                }
            }
        }
        public static void UpdateKey(string propertyType, string propertyKey, string NewValue)
        {
            string propertyKeys = propertyType + "." + propertyKey;
            Dictionary<string, string> resCacheByCulture = null;
            if (m_resourceCache.ContainsKey(CultureInfo.CurrentCulture.TwoLetterISOLanguageName))
            {
                resCacheByCulture = m_resourceCache[CultureInfo.CurrentCulture.TwoLetterISOLanguageName];
                if (resCacheByCulture.ContainsKey(propertyKeys))
                {
                    resCacheByCulture[propertyKeys] = NewValue;
                }
            }
        }
        public static void Clear()
        {
            m_resourceCache.Clear();
        }
    }
}
