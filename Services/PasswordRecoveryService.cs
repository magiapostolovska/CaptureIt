using AutoMapper;
using CaptureIt.DTOs.PasswordRecovery;
using CaptureIt.DTOs.User;
using CaptureIt.Models;
using CaptureIt.Repos;

namespace CaptureIt.Services
{
    public class PasswordRecoveryService : IPasswordRecoveryService
    {
        private readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
        private readonly IMapper _mapper;

        public PasswordRecoveryService(IPasswordRecoveryRepository passwordRecoveryRepository, IMapper mapper)
        {
            _passwordRecoveryRepository = passwordRecoveryRepository;
            _mapper = mapper;
        }

        public async Task Add(int userId, int recoveryCode, DateTime expirationTime)
        {
            var passwordRecoveryResponse = new PasswordRecoveryResponse
            {
                UserId = userId,
                RecoveryCode = recoveryCode,
                ExpirationTime = expirationTime
            };
            var passwordRecovery = _mapper.Map<PasswordRecovery>(passwordRecoveryResponse);
            await _passwordRecoveryRepository.Add(passwordRecovery);
        }
    }
}

