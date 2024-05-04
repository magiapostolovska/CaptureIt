using CaptureIt.Models;

namespace CaptureIt.Repos
{
    public interface IAlbumRepository
    {
        Task<IEnumerable<Album>> GetAll();
        Task<Album> GetById(int id);
        Task<Album> Add(Album album);
        Task<Album> Update(Album album);
        Task<bool> Delete(int id);
    }
}
