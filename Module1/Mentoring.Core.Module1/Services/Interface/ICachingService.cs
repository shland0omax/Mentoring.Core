using System.Threading.Tasks;

namespace Mentoring.Core.Module1.Services.Interface
{
    public interface ICachingService
    {
        Task<bool> TryAddToCacheAsync(string key, byte[] data);
        Task<bool> IsCachedAsync(string key);
        Task<byte[]> GetAsync(string key);
    }
}