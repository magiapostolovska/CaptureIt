using AutoMapper;
using CaptureIt.DTOs.Picture;
using CaptureIt.Models;
using CaptureIt.Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaptureIt.Services
{
    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;

        public PictureService(IPictureRepository pictureRepository, IMapper mapper)
        {
            _pictureRepository = pictureRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PictureResponse>> GetAll()
        {
            var pictures = await _pictureRepository.GetAll();
            return _mapper.Map<IEnumerable<PictureResponse>>(pictures);
        }

        public async Task<PictureResponse> GetById(int id)
        {
            var picture = await _pictureRepository.GetById(id);
            return _mapper.Map<PictureResponse>(picture);
        }

        public async Task<PictureResponse> Add(PictureRequest pictureRequest)
        {
            var picture = _mapper.Map<Picture>(pictureRequest);
            await _pictureRepository.Add(picture);
            return _mapper.Map<PictureResponse>(picture);
        }

        public async Task<PictureResponse> Update(int id, PictureRequest pictureRequest)
        {
            var picture = await _pictureRepository.GetById(id);
            if (picture == null)
            {
                return null;
            }

            _mapper.Map(pictureRequest, picture);

            await _pictureRepository.Update(picture);
            return _mapper.Map<PictureResponse>(picture);
        }

        public async Task<bool> Delete(int id)
        {
            var picture = await _pictureRepository.GetById(id);
            if (picture == null)
            {
                return false;
            }

            await _pictureRepository.Delete(id);
            return true;
        }
    }
}
