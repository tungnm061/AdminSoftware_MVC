using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace Core.Helper
{
    public class ResourceString
    {
        //private static CoreDbContext db = new CoreDbContext();

        private static Dictionary<string, Dictionary<string, string>> m_resourceCache = new Dictionary<string, Dictionary<string, string>>();

        private static SqlConnection m_connection;
        private static SqlCommand m_cmdGetResourceByTypeAndKey;

        public static String GetString(string resourceType, string resourceKey)
        {
            string resourceValue = null;
            string resourceKeys = resourceType + "." + resourceKey;
            Dictionary<string, string> resCacheByCulture = null;
            // check the cache first
            // find the dictionary for this culture
            // check for the inner dictionary entry for this key
            if (m_resourceCache.ContainsKey(CultureInfo.CurrentCulture.TwoLetterISOLanguageName))
            {
                resCacheByCulture = m_resourceCache[CultureInfo.CurrentCulture.TwoLetterISOLanguageName];
                if (resCacheByCulture.ContainsKey(resourceKeys))
                {
                    resourceValue = resCacheByCulture[resourceKeys];
                }
            }

            // if not in the cache, go to the database
            if (resourceValue == null)
            {
                resourceValue = GetResourceByTypeAndKey(resourceType, resourceKey);

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
                    resCacheByCulture.Add(resourceKeys, resourceValue);
                }
            }
            return resourceValue;
        }

        private static string GetResourceByTypeAndKey(string resourceType, string resourceKey)
        {

            m_connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CoreDBContext"].ConnectionString);

            //command to retrieve the resource the matches 
            //a specific type, culture and key
            m_cmdGetResourceByTypeAndKey = new SqlCommand("SELECT resourceType, CultureId, resourceKey, resourceValue FROM LocalizeResources WHERE (resourceType=@resourceType) AND (CultureId=@CultureId) AND (resourceKey=@resourceKey)");
            m_cmdGetResourceByTypeAndKey.Connection = m_connection;
            m_cmdGetResourceByTypeAndKey.Parameters.AddWithValue("resourceType", resourceType);
            m_cmdGetResourceByTypeAndKey.Parameters.AddWithValue("CultureId", CultureInfo.CurrentCulture.TwoLetterISOLanguageName);
            m_cmdGetResourceByTypeAndKey.Parameters.AddWithValue("resourceKey", resourceKey);

            // we should only get one back, but just in case, we'll iterate reader results
            StringCollection resources = new StringCollection();
            string resourceValue = null;
            //var _resource = db.resources.Where(r => r.ResourceType == resourceType && r.CultureId == CultureInfo.CurrentCulture.TwoLetterISOLanguageName && r.ResourceKey == resourceKey);
            try
            {
                m_connection.Open();
                // get resources from the database
                using (SqlDataReader reader = m_cmdGetResourceByTypeAndKey.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("resourceValue")))
                            resources.Add(reader.GetString(reader.GetOrdinal("resourceValue")));
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
                //resourceValue = resourceKey;
                ////update new key to database
                //Core.Data.CoreDbContext db = new Data.CoreDbContext();
                //Core.Data.LocalizeResources lr = new Data.LocalizeResources { CultureId = CultureInfo.CurrentCulture.TwoLetterISOLanguageName, ResourceType = resourceType, ResourceKey = resourceKey, ResourceValue = resourceValue };
                //db.localizeResources.Add(lr);
                //db.SaveChanges();
            }
            else if (resources.Count == 1)
            {
                resourceValue = resources[0];
            }
            else
            {
                // if > 1 row returned, log an error, we shouldn't have > 1 value for a resourceKey!
                throw new DataException(String.Format("Duplicated resource key {0}.{1}", resourceType, resourceKey));
            }
            return resourceValue;
        }
        public static void RemoveKey(string resourceType, string resourceKey)
        {
            string resourceKeys = resourceType + "." + resourceKey;
            Dictionary<string, string> resCacheByCulture = null;
            if (m_resourceCache.ContainsKey(CultureInfo.CurrentCulture.TwoLetterISOLanguageName))
            {
                resCacheByCulture = m_resourceCache[CultureInfo.CurrentCulture.TwoLetterISOLanguageName];
                if (resCacheByCulture.ContainsKey(resourceKeys))
                {
                    resCacheByCulture.Remove(resourceKeys);
                }
            }
        }
        public static void UpdateKey(string resourceType, string resourceKey, string NewValue)
        {
            string resourceKeys = resourceType + "." + resourceKey;
            Dictionary<string, string> resCacheByCulture = null;
            if (m_resourceCache.ContainsKey(CultureInfo.CurrentCulture.TwoLetterISOLanguageName))
            {
                resCacheByCulture = m_resourceCache[CultureInfo.CurrentCulture.TwoLetterISOLanguageName];
                if (resCacheByCulture.ContainsKey(resourceKeys))
                {
                    resCacheByCulture[resourceKeys] = NewValue;
                }
            }
        }
        public static void Clear()
        {
            m_resourceCache.Clear();
        }
    }
}
