using CaptureIt.DTOs.Album;
using CaptureIt.DTOs.Picture;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<AlbumResponse>> GetAll(DateTime? createdAt, int eventId);
        Task<AlbumResponse> GetById(int id);
        Task<AlbumResponse> Add(AlbumRequest albumRequest);
        Task<AlbumResponse> Update(int id, AlbumUpdate albumUpdate);
        Task<bool> Delete(int id);
        Task<AlbumResponse> UpdateNumberOfPhotos(int id, AlbumNumberOfPhotos albumUpdate);
        Task<AlbumResponse> Update(int id, AlbumCreator albumUpdate);
    }
}
