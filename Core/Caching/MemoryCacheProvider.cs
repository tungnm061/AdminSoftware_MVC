using System;

namespace Core.Caching
{
    public class MemoryCacheProvider : ICacheProvider
    {
        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value)
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object value, int timeout)
        {
            throw new NotImplementedException();
        }

        public bool IsSet(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveByPattern(string pattern)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public T GetInstance<T>()
        {
            throw new NotImplementedException();
        }
    }
}
