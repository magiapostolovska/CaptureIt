using AutoMapper;
using CaptureIt.Authentication;
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
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
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

        public async Task<UserResponse> Update(int id, UserRequest userRequest)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return null;
            }

            _mapper.Map(userRequest, user);

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
       
    }
}
