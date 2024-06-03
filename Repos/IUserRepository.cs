﻿using CaptureIt.Authentication;
using CaptureIt.DTOs.User;
using CaptureIt.Models;

namespace CaptureIt.Repos
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> Add(User user);
        Task<User> Update(User user);
        Task<bool> Delete(int id);
        Task<User> Register(RegisterModel registerModel);
        Task<User> GetByUsername(string username);
        Task<bool> AddBadge(int userId, int badgeId);
        Task<bool> RemoveBadge(int userId, int badgeId);
        Task<CaptureIt.DTOs.User.EventParticipant> GetParticipantById(int userId);



    }
}

