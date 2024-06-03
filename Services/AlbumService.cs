using AutoMapper;
using CaptureIt.DTOs.Album;
using CaptureIt.Repos;
using CaptureIt.Models;
using CaptureIt.DTOs.Picture;
using Microsoft.Extensions.Logging;

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

        public async Task<IEnumerable<AlbumResponse>> GetAll(DateTime? createdAt = default, int eventId = default)
        {
            var albums = await _albumRepository.GetAll();
            if (createdAt.HasValue)
            {
                albums = albums.Where(e => e.CreatedAt.HasValue && e.CreatedAt.Value.Date >= createdAt.Value.Date);
            }
            if (eventId != default)
            {
                albums = albums.Where(a => a.EventId == eventId);
            }
            return _mapper.Map<IEnumerable<AlbumResponse>>(albums);

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
        public async Task<AlbumResponse> Update(int id, AlbumUpdate albumUpdate)
        {
            var album = await _albumRepository.GetById(id);
            if (album == null)
            {
                return null;
            }

            _mapper.Map(albumUpdate, album);

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

        public async Task<AlbumResponse> UpdateNumberOfPhotos(int id, AlbumNumberOfPhotos albumUpdate)
        {
            var album = await _albumRepository.GetById(id);
            if (album == null)
            {
                return null;
            }

            _mapper.Map(albumUpdate, album);

            await _albumRepository.Update(album);
            return _mapper.Map<AlbumResponse>(album);
        }
        public async Task<AlbumResponse> Update(int id, AlbumCreator albumUpdate)
        {
            var album = await _albumRepository.GetById(id);
            if (album == null)
            {
                return null;
            }

            _mapper.Map(albumUpdate, album);

            await _albumRepository.Update(album);
            return _mapper.Map<AlbumResponse>(album);

        }

    }
}
