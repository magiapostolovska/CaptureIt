using CaptureIt.Authentication;
using CaptureIt.Data;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaptureIt.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly CaptureItContext _context;

        public UserRepository(CaptureItContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await GetById(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User> Register(RegisterModel registerModel)
        {
            var user = new User
            {
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                PhoneNumber = registerModel.PhoneNumber,
                Gender = registerModel.Gender,
                DateOfBirth = registerModel.DateOfBirth,
                Username = registerModel.Username,
                Email = registerModel.Email,
                Password = registerModel.Password

            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> AddBadge(int userId, int badgeId)
        {
            try
            {
                var userEntity = await _context.Users.FindAsync(userId);
                var badgeEntity = await _context.Badges.FindAsync(badgeId);
                if (badgeEntity != null && userEntity != null)
                {
                    userEntity.Badges.Add(badgeEntity);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveBadge(int userId, int badgeId)
        {
            try
            {
                var userEntity = await _context.Users.Include(e => e.Badges).FirstOrDefaultAsync(e => e.UserId == userId);

                if (userEntity == null)
                {
                    return false;
                }

                var badgeToRemove = userEntity.Badges.FirstOrDefault(p => p.BadgeId == badgeId);
                if (badgeToRemove == null)
                {
                    return false;
                }

                userEntity.Badges.Remove(badgeToRemove);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<CaptureIt.DTOs.User.EventParticipant> GetParticipantById(int userId)
        {
            var userEntity = await _context.Users.FindAsync(userId);
            if (userEntity == null)
            {
                return null;
            }

            var eventParticipant = new CaptureIt.DTOs.User.EventParticipant
            {
                UserId = userEntity.UserId,
                Username = userEntity.Username,
                ProfilePicture = userEntity.ProfilePicture
            };

            return eventParticipant;
        }
    }

    
}






