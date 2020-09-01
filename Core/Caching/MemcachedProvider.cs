using System;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace Core.Caching
{
    public class MemcachedProvider : ICacheProvider
    {
        public static MemcachedClient _instant;
        private string _schema;

        public MemcachedProvider(string schema)
        {
            _schema = schema;
        }

        /// <summary>
        /// Gets the instant.
        /// </summary>
        /// <value>
        /// The instant.
        /// </value>
        public static MemcachedClient Instant
        {
            get { return _instant ?? (_instant = new MemcachedClient()); }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="duration"></param>
        /// <param name="getItemCallback"></param>
        /// <returns>
        /// The value associated with the specified key.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public T Get<T>(string key, int duration, Func<T> getItemCallback)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>
        /// The value associated with the specified key.
        /// </returns>
        public T Get<T>(string key)
        {
            return Instant.Get<T>(key);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string key, object value)
        {
            Instant.Store(StoreMode.Add, key, value);
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <param name="timeout">The timeout.</param>
        public void Set(string key, object data, int timeout)
        {
            Instant.Store(StoreMode.Add, key, data);
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>
        /// Result
        /// </returns>
        public bool IsSet(string key)
        {
            return false;
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            Instant.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            Instant.FlushAll();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public MemcachedClient GetInstance()
        {
            return Instant;
        }
    }
}
