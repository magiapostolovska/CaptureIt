using CaptureIt.Data;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaptureIt.Repos
{
    public class BadgeRepository : IBadgeRepository
    {
        private readonly CaptureItContext _context;

        public BadgeRepository(CaptureItContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Badge>> GetAll()
        {
            return await _context.Badges.ToListAsync();
        }

        public async Task<Badge> GetById(int id)
        {
            return await _context.Badges.FirstOrDefaultAsync(b => b.BadgeId == id);
        }

        public async Task<Badge> Add(Badge badge)
        {
            await _context.Badges.AddAsync(badge);
            await _context.SaveChangesAsync();
            return badge;
        }

        public async Task<Badge> Update(Badge badge)
        {
            _context.Badges.Update(badge);
            await _context.SaveChangesAsync();
            return badge;
        }

        public async Task<bool> Delete(int id)
        {
            var badge = await GetById(id);
            if (badge == null)
            {
                return false;
            }

            _context.Badges.Remove(badge);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
