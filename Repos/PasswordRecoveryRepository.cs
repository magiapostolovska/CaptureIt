using CaptureIt.Data;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;


namespace CaptureIt.Repos
{
    public class PasswordRecoveryRepository : IPasswordRecoveryRepository
    {
        private readonly CaptureItContext _context;

        public PasswordRecoveryRepository(CaptureItContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PasswordRecovery>> GetAll()
        {
            return await _context.PasswordRecoveries
                .Include(p => p.UserId)
                .ToListAsync();
        }

        public async Task<PasswordRecovery> GetById(int id)
        {
            return await _context.PasswordRecoveries
                .Include(p => p.UserId)
                .FirstOrDefaultAsync(p => p.RequestId == id);
        }

        public async Task<PasswordRecovery> Add(PasswordRecovery passwordRecovery)
        {
            await _context.PasswordRecoveries.AddAsync(passwordRecovery);
            await _context.SaveChangesAsync();
            return passwordRecovery;
        }

        public async Task<PasswordRecovery> Update(PasswordRecovery passwordRecovery)
        {
            _context.PasswordRecoveries.Update(passwordRecovery);
            await _context.SaveChangesAsync();
            return passwordRecovery;
        }

        public async Task<bool> Delete(int id)
        {
            var passwordRecovery = await GetById(id);
            if (passwordRecovery == null)
            {
                return false;
            }

            _context.PasswordRecoveries.Remove(passwordRecovery);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
