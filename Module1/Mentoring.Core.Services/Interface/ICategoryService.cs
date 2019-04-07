using System.Threading.Tasks;
using Mentoring.Core.Services.Models;

namespace Mentoring.Core.Services.Interface
{
    public interface ICategoryService: IService<Category>
    {
        Task<byte[]> GetPictureAsync(int id);
        Task UploadPictureAsync(int id, byte[] picture);
    }
}