using AutoMapper;
using CaptureIt.Authentication;
using CaptureIt.DTOs.PasswordRecovery;
using CaptureIt.DTOs.User;
using CaptureIt.Models;
using CaptureIt.Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaptureIt.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordRecoveryRepository _passwordRecoveryRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IPasswordRecoveryRepository passwordRecoveryRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordRecoveryRepository = passwordRecoveryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponse>> GetAll()
        {
            var users = await _userRepository.GetAll();
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task<UserResponse> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> Add(UserRequest userRequest)
        {
            var user = _mapper.Map<User>(userRequest);
            await _userRepository.Add(user);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> Update(int id, UserUpdate userUpdate)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return null;
            }

            _mapper.Map(userUpdate, user);

            await _userRepository.Update(user);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return false;
            }

            await _userRepository.Delete(id);
            return true;
        }
        public async Task<UserResponse> Register(RegisterModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);
            await _userRepository.Add(user);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> GetByUsername(string username)
        {
            var user = await _userRepository.GetByUsername(username);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> Update(string username, int recoveryCode, NewPassword newPassword)
        {
            var user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                return null;
            }
            var request = await _passwordRecoveryRepository.GetByUserId(user.UserId);
            if (request == null)
            {
                return null;
            }

            _mapper.Map(newPassword, user);
            await _userRepository.Update(user);
            return _mapper.Map<UserResponse>(user);
        }

    }

    //public async Task<bool> AddBadge(int userId, int badgeId)
    //{
    //    return await _userRepository.AddBadge(userId,badgeId);
    //}

    //public async Task<bool> RemoveBadge(int eventId, int userId)
    //{
    //    return await _eventRepository.RemoveParticipantFromEvent(eventId, userId);
    //}

}
