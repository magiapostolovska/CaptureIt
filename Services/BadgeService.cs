using AutoMapper;
using CaptureIt.Repos;
using CaptureIt.Models;
using CaptureIt.DTOs.Bagde;

namespace CaptureIt.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IMapper _mapper;

        public BadgeService(IBadgeRepository badgeRepository, IMapper mapper)
        {
            _badgeRepository = badgeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BadgeResponse>> GetAll()
        {
            var badges = await _badgeRepository.GetAll();
            return _mapper.Map<IEnumerable<BadgeResponse>>(badges);
        }

        public async Task<BadgeResponse> GetById(int id)
        {
            var badge = await _badgeRepository.GetById(id);
            return _mapper.Map<BadgeResponse>(badge);
        }

        public async Task<BadgeResponse> Add(BadgeRequest badgeRequest)
        {
            var badge = _mapper.Map<Badge>(badgeRequest);
            await _badgeRepository.Add(badge);
            return _mapper.Map<BadgeResponse>(badge);
        }

        public async Task<BadgeResponse> Update(int id, BadgeUpdate badgeUpdate)
        {
            var badge = await _badgeRepository.GetById(id);
            if (badge == null)
            {
                return null;
            }

            _mapper.Map(badgeUpdate, badge);

            await _badgeRepository.Update(badge);
            return _mapper.Map<BadgeResponse>(badge);
        }

        public async Task<bool> Delete(int id)
        {
            var badge = await _badgeRepository.GetById(id);
            if (badge == null)
            {
                return false;
            }

            await _badgeRepository.Delete(id);
            return true;
        }
    }
}
