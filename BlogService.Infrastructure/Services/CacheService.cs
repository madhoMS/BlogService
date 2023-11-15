using BlogService.Common.Utilities;
using BlogService.Core.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BlogService.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private IDatabase _db;
        public CacheService()
        {
            ConfigureRedis();
        }
        private void ConfigureRedis()
        {
            try
            {
                _db = ConnectionHelper.Connection.GetDatabase();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public T GetData<T>(string key)
        {
            try
            {
                var value = _db.StringGet(key);
                if (!string.IsNullOrEmpty(value))
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
            }
            catch (Exception ex)
            { 
            }          
            return default;
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }
        public object RemoveData(string key)
        {
            bool _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist == true)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }
}
