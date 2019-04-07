using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mentoring.Core.Services.Interface;
using Mentoring.Core.Services.Models;
using Microsoft.Extensions.Configuration;

namespace Mentoring.Core.Services.Services
{
    public class CategoryService: Service<Category>, ICategoryService
    {
        private readonly uint _garbageBytesCount;

        public CategoryService(IRepository<Category> repository, IConfiguration config) : base(repository)
        {
            _garbageBytesCount = Convert.ToUInt32(config["garbageSize"]);
        }


        public async Task<byte[]> GetPictureAsync(int id)
        {
            var category = await _repository.GetAsync(id);
            return (category?.Picture?.Count() ?? 0) > _garbageBytesCount
                ? category.Picture.Skip((int) _garbageBytesCount).ToArray()
                : null;
        }

        public async Task UploadPictureAsync(int id, byte[] picture)
        {
            if (picture == null) return;
            var category = await _repository.GetAsync(id);
            if (category == null) return;

            var bytes = new List<byte>(Enumerable.Repeat((byte)0, (int)_garbageBytesCount).ToArray());
            bytes.AddRange(picture);
            category.Picture = bytes.ToArray();
            await _repository.EditAsync(category);
        }
    }
}
