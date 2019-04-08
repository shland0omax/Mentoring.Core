using System;
using System.IO;
using System.Threading.Tasks;
using Mentoring.Core.Module1.Models.Middleware;
using Mentoring.Core.Module1.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Mentoring.Core.Module1.Services
{
    public class LocalCachingService: ICachingService
    {
        private readonly CachingServiceOptions _options;
        private readonly string _baseFolder;
        private static readonly object _clearLock = new object();

        public LocalCachingService(IConfiguration config , IHostingEnvironment env)
        {
            _options = new CachingServiceOptions();
            config.Bind("ImageCache", _options);
            _baseFolder = env.ContentRootPath;
        }


        public async Task<bool> TryAddToCacheAsync(string key, byte[] data)
        {
            var dirPath = Path.Combine(_baseFolder, _options.DirectoryPath);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            CleanCache(dirPath);

            if (await IsCachedAsync(key) || Directory.GetFiles(dirPath).Length >= _options.MaxImagesCached)
                return false;
            await File.WriteAllBytesAsync(Path.Combine(dirPath, $"{key}.jpg"), data);
            return true;

        }

        public Task<bool> IsCachedAsync(string key)
        {
            return Task.FromResult(File.Exists(Path.Combine(_baseFolder, _options.DirectoryPath, $"{key}.jpg")));
        }

        public async Task<byte[]> GetAsync(string key)
        {
            if (await IsCachedAsync(key))
            {
                return await File.ReadAllBytesAsync(Path.Combine(_baseFolder, _options.DirectoryPath, $"{key}.jpg"));
            }

            return null;
        }

        private void CleanCache(string directory)
        {
            Task.Run(() =>
            {
                if (Directory.GetFiles(directory).Length < _options.MaxImagesCached) return;

                lock (_clearLock)
                {
                    if (Directory.GetFiles(directory).Length < _options.MaxImagesCached) return;
                    foreach (string file in Directory.GetFiles(directory))
                    {
                        var filePath = Path.Combine(directory, file);
                        var lastAccess = (new FileInfo(Path.Combine(directory, file))).LastAccessTime;
                        if ((lastAccess + _options.ExpirationTime) < DateTime.Now)
                            File.Delete(filePath);
                    }
                }
            });
        }
    }
}