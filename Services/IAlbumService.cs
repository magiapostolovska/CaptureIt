using CaptureIt.DTOs.Album;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<AlbumResponse>> GetAll();
        Task<AlbumResponse> GetById(int id);
        Task<AlbumResponse> Add(AlbumRequest albumRequest);
        Task<AlbumResponse> Update(int id, AlbumUpdate albumUpdate);
        Task<bool> Delete(int id);
    }
}
