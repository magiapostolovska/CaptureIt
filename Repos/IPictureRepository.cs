using CaptureIt.Models;

namespace CaptureIt.Repos
{
    public interface IPictureRepository
    {
        Task<IEnumerable<Picture>> GetAll();
        Task<Picture> GetById(int id);
        Task<Picture> Add(Picture picture);
        Task<Picture> Update(Picture picture);
        Task<bool> Delete(int id);

    }
}