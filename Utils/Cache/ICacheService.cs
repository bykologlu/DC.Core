namespace DC.Core.Utils.Cache
{
    public interface ICacheService
    {
        Task<T> GetValueAsync<T>(string key);
        Task<bool> SetValueAsync(string key, string value, int? duration = null);
        Task<T> GetOrAddAsync<T>(string key, Func<T> action) where T : class;
        Task ClearAsync(string key);
        Task ClearAllASync();
        Task<bool> Lock(string key, string value, int? duration = null);
        Task<bool> UnLock(string key, string value);
        Task GetLock(string key, string value, Func<Task> action);
    }
}
