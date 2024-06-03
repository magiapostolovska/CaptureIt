using CaptureIt.Data;
using CaptureIt.DTOs.Like;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaptureIt.Repos
{
    public class LikeRepository : ILikeRepository
    {
        private readonly CaptureItContext _context;

        public LikeRepository(CaptureItContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Like>> GetAll()
        {
            return await _context.Likes

                .Include(p => p.Picture)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Like> GetById(int id)
        {
            return await _context.Likes

                .Include(p => p.Picture)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.LikeId == id);
        }

        public async Task<Like> GetByIds(int userId, int pictureId)
        {
            return await _context.Likes

                .Include(p => p.Picture)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId && p.PictureId == pictureId);
        }

        public async Task<Like> Add(Like like)
        {
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
            return like;
        }

        public async Task<Like> Update(Like like)
        {
            _context.Likes.Update(like);
            await _context.SaveChangesAsync();
            return like;
        }

        public async Task<bool> Delete(int id)
        {
            var like = await GetById(id);
            if (like == null)
            {
                return false;
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetLikeCount(int pictureId)
        {
            return await _context.Likes.CountAsync(like => like.PictureId == pictureId);
        }
     
    }
}
