using CaptureIt.Data;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaptureIt.Repos
{
    public class PictureRepository : IPictureRepository
    {
        private readonly CaptureItContext _context;

        public PictureRepository(CaptureItContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Picture>> GetAll()
        {
            return await _context.Pictures
                .Include(p => p.Album)
                .Include(p => p.Author)
                .ToListAsync();
        }

        public async Task<Picture> GetById(int id)
        {
            return await _context.Pictures
                .Include(p => p.Album)
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.PictureId == id);
        }

        public async Task<Picture> Add(Picture picture)
        {
            await _context.Pictures.AddAsync(picture);
            await _context.SaveChangesAsync();
            return picture;
        }

        public async Task<Picture> Update(Picture picture)
        {
            _context.Pictures.Update(picture);
            await _context.SaveChangesAsync();
            return picture;
        }

        public async Task<bool> Delete(int id)
        {
            var picture = await GetById(id);
            if (picture == null)
            {
                return false;
            }
            var likesToDelete = _context.Likes.Where(like => like.PictureId == id);
            _context.Likes.RemoveRange(likesToDelete);

            var commentsToDelete = _context.Comments.Where(comment => comment.PictureId == id);
            _context.Comments.RemoveRange(commentsToDelete);


            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetNumberOfPhotos(int albumId)
        {
            return await _context.Pictures.CountAsync(picture => picture.AlbumId == albumId);
        }

       
    }
}


