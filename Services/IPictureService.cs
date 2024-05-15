using CaptureIt.DTOs.Picture;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public interface IPictureService
    {
        Task<IEnumerable<PictureResponse>> GetAll();
        Task<PictureResponse> GetById(int id);
        Task<PictureResponse> Add(PictureRequest pictureRequest);
        Task<PictureResponse> Update(int id, PictureUpdate pictureUpdate);
        Task<bool> Delete(int id);
    }
}
