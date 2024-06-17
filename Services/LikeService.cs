using AutoMapper;
using CaptureIt.DTOs.Like;
using CaptureIt.Models;
using CaptureIt.Repos;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CaptureIt.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;

        public LikeService(ILikeRepository likeRepository, IMapper mapper)
        {
            _likeRepository = likeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LikeResponse>> GetAll(int pictureId = default)
        {
            var likes = await _likeRepository.GetAll();
            if (pictureId != default)
            {
                likes = likes.Where(e => e.PictureId == pictureId);
            }
            return _mapper.Map<IEnumerable<LikeResponse>>(likes);
        }

        public async Task<LikeResponse> GetById(int id)
        {
            var like = await _likeRepository.GetById(id);
            return _mapper.Map<LikeResponse>(like);
        }

        public async Task<LikeResponse> GetByPictureAndUserId(int userId, int pictureId)
        {
            var like = await _likeRepository.GetByPictureAndUserId(userId, pictureId);
            return _mapper.Map<LikeResponse>(like);
        }

        public async Task<LikeResponse> Add(LikeRequest likeRequest)
        {
            var like = _mapper.Map<Like>(likeRequest);
            await _likeRepository.Add(like);
            return _mapper.Map<LikeResponse>(like);
        }


        public async Task<bool> Delete(int id)
        {
            var like = await _likeRepository.GetById(id);
            if (like == null)
            {
                return false;
            }

            await _likeRepository.Delete(id);
            return true;
        }

        public async Task<int> GetLikeCount(int pictureId)
        {
            return await _likeRepository.GetLikeCount(pictureId);
        }
        public async Task<LikeResponse> Update(int id, LikeUser likeUpdate)
        {
            var like = await _likeRepository.GetById(id);
            if (like == null)
            {
                return null;
            }

            _mapper.Map(likeUpdate, like);

            await _likeRepository.Update(like);
            return _mapper.Map<LikeResponse>(like);
        }
    }
}
