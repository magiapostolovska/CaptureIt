using AutoMapper;
using CaptureIt.DTOs.Album;
using CaptureIt.Repos;
using CaptureIt.Models;

namespace CaptureIt.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public AlbumService(IAlbumRepository albumRepository, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AlbumResponse>> GetAll()
        {
            var album = await _albumRepository.GetAll();
            return _mapper.Map<IEnumerable<AlbumResponse>>(album);

        }
        public async Task<AlbumResponse> GetById(int id)
        {
            var album = await _albumRepository.GetById(id);
            return _mapper.Map<AlbumResponse>(album);

        }
        public async Task<AlbumResponse> Add(AlbumRequest albumRequest)
        {
          
            var album = _mapper.Map<Album>(albumRequest);
            await _albumRepository.Add(album);
            return _mapper.Map<AlbumResponse>(album);

        }
        public async Task<AlbumResponse> Update(int id, AlbumRequest albumRequest)
        {
            var album = await _albumRepository.GetById(id);
            if (album == null)
            {
                return null;
            }

            _mapper.Map(albumRequest, album);

            await _albumRepository.Update(album);
            return _mapper.Map<AlbumResponse>(album);

        }
         public async Task<bool> Delete(int id)
        {
            var album = await _albumRepository.GetById(id);
            if (album == null)
            {
                return false;
            }

            await _albumRepository.Delete(id);
            return true;
        }
    }
}
