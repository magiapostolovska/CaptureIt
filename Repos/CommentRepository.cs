using CaptureIt.Data;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;


namespace CaptureIt.Repos
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CaptureItContext _context;

        public CommentRepository(CaptureItContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            return await _context.Comments
                .Include(p => p.UserId)
                .Include(p => p.PictureId)
                .ToListAsync();
        }

        public async Task<Comment> GetById(int id)
        {
            return await _context.Comments
                .Include(p => p.UserId)
                .Include(p => p.PictureId)
                .FirstOrDefaultAsync(p => p.CommentId == id);
        }

        public async Task<Comment> Add(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment> Update(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<bool> Delete(int id)
        {
            var comment = await GetById(id);
            if (comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
