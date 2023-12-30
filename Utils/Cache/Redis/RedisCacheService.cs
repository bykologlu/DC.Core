using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Net;
using DC.Core.Extensions;
using System;

namespace DC.Core.Utils.Cache.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connection;
        private readonly IDatabase _cache;
        private const int DEFAULT_MINUTE_DURATION = 60;
        private const int DEFAULT_MINUTE_LOCK_DURATION = 5;

        public RedisCacheService(IConnectionMultiplexer connection)
        {
            _connection = connection;
            _cache = _connection.GetDatabase();
        }

        public async Task ClearAllASync()
        {
            EndPoint[] endpoints = _connection.GetEndPoints(true);
            foreach (EndPoint endpoint in endpoints)
            {
                IServer server = _connection.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }
        public async Task ClearAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<T> action) where T : class
        {
            RedisValue result = await _cache.StringGetAsync(key);
            if (result.IsNull)
            {
                result = JsonConvert.SerializeObject(action());
                await SetValueAsync(key, result);
            }
            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            var value = await _cache.StringGetAsync(key);

            if (!value.IsNull)
            {
                Type tType = typeof(T);

                try
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
                catch (JsonReaderException)
                {
                    return (T)Convert.ChangeType(value, tType);
                }
            }

            return default(T);
        }

        public async Task<bool> SetValueAsync<T>(string key, T value, int? duration = null)
        {
            TimeSpan ExpireTime = TimeSpan.FromHours(duration ?? DEFAULT_MINUTE_DURATION);

            string stringValue;

            Type tType = typeof(T);

            if (tType == typeof(string) || tType == typeof(int) || tType == typeof(decimal) || tType == typeof(DateTime))
                stringValue = value.ToString();
            else
                stringValue = JsonConvert.SerializeObject(value);


            return await _cache.StringSetAsync(key, stringValue, ExpireTime);
        }

        public async Task<bool> Lock(string key, string value, int? duration = null)
        {
            TimeSpan ExpireTime = TimeSpan.FromMinutes(duration ?? DEFAULT_MINUTE_LOCK_DURATION);

            return await _cache.LockTakeAsync(key, value, ExpireTime);

        }

        public async Task<bool> UnLock(string key, string value)
        {
            return await _cache.LockReleaseAsync(key, value);

        }

        public async Task GetLock(string key, string value, Func<Task> action)
        {

            try
            {
                bool isContinue = true;
                int maxRetry = 5;
                int retryCount = 0;
                int sleepingDuration = 3;
                do
                {
                    retryCount++;

                    Console.WriteLine($"{key} lock checking. {DateTime.Now.ToString("hh:mm:ss.fff")}");

                    if (await Lock(key, value))
                    {
                        Console.WriteLine($"{key} locked. {DateTime.Now.ToString("hh:mm:ss.fff")}");

                        await action.Invoke();


                        isContinue = false;
                    }
                    else
                    {
                        Console.WriteLine($"{key} is locked. Sleeping {sleepingDuration} seconds. {DateTime.Now.ToString("hh:mm:ss.fff")}");

                        Thread.Sleep(TimeSpan.FromSeconds(sleepingDuration));
                    }

                    if (maxRetry <= retryCount) isContinue = false;


                }
                while (isContinue);
            }
            catch (Exception err)
            {
                throw;
            }
            finally
            {
                Console.WriteLine($"{key} unlocked. {DateTime.Now.ToString("hh:mm:ss.fff")}");

                await UnLock(key, value);
            }
        }
    }

}
