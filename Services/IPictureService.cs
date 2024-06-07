using CaptureIt.DTOs.Picture;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IPictureService
    {
        Task<IEnumerable<PictureResponse>> GetAll(DateTime? createdAt, int albumId );
        Task<PictureResponse> GetById(int id);
        Task<PictureResponse> Add(PictureRequest pictureRequest);
        Task<PictureResponse> Update(int id, PictureUpdate pictureUpdate);
        Task<bool> Delete(int id);
        Task<PictureResponse> UpdateLikeCount(int id, PictureLikes pictureUpdate);
        Task<PictureResponse> UpdateCommentCount(int id, PictureComments pictureUpdate);

        Task<int> GetNumberOfPhotos(int albumId);
        Task<PictureResponse> Update(int id, PictureAuthor pictureUpdate);
        Task<string> AnalyzePicture(int pictureId);
    }
}
