using CaptureIt.Data;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;


namespace CaptureIt.Repos
{
    public class PasswordRecoveryRepository : IPasswordRecoveryRepository
    {
        private readonly CaptureItContext _context;

        public PasswordRecoveryRepository(CaptureItContext context)
        {
            _context = context;
        }

        public async Task Add(PasswordRecovery recovery)
        {
            await _context.PasswordRecoveries.AddAsync(recovery);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordRecovery> GetByUserId(int userId)
        {
            return await _context.PasswordRecoveries
        .Where(u => u.UserId == userId)
        .OrderBy(u => u.RequestId)
        .LastOrDefaultAsync();
        }

    }
}

