using CaptureIt.Data;
using CaptureIt.DTOs.Event;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaptureIt.Repos
{
    public class EventRepository : IEventRepository
    {
        private readonly CaptureItContext _context;

        public EventRepository(CaptureItContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            return await _context.Events
                .Include(p => p.Owner)
                .Include(p => p.Albums)
                .ThenInclude(a => a.Pictures)
                .Include(e => e.Participants)
                .ToListAsync();
        }

        public async Task<Event> GetById(int id)
        {
            return await _context.Events
                .Include(p => p.Owner)
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(p => p.EventId == id);
        }

        public async Task<Event> Add(Event @event)
        {
            await _context.Events.AddAsync(@event);
            await _context.SaveChangesAsync();
            return @event;
        }

        public async Task<Event> Update(Event @event)
        {
            _context.Events.Update(@event);
            await _context.SaveChangesAsync();
            return @event;
        }

        public async Task<bool> Delete(int id)
        {
            var @event = await GetById(id);
            if (@event == null)
            {
                return false;
            }

            var albumsToDelete = _context.Albums.Where(album => album.EventId == id).ToList();
            foreach (var album in albumsToDelete)
            {
                var albumId = album.AlbumId;
                var picturesToDelete = _context.Pictures.Where(picture => picture.AlbumId == albumId).ToList();
                foreach (var picture in picturesToDelete)
                {
                    var commentsToDelete = _context.Comments.Where(comment => comment.PictureId == picture.PictureId);
                    _context.Comments.RemoveRange(commentsToDelete);
                    var likesToDelete = _context.Likes.Where(like => like.PictureId == picture.PictureId);
                    _context.Likes.RemoveRange(likesToDelete);
                }

                _context.Pictures.RemoveRange(picturesToDelete);
                _context.Albums.RemoveRange(albumsToDelete);

                await _context.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM EventParticipants WHERE EventId = {id}");
            }
            _context.Events.Remove(@event);
                await _context.SaveChangesAsync();
                return true;
            }

        public async Task<Event> GetEventParticipant(int eventId)
        {
            return await _context.Events
               .Include(e => e.Participants)
               .ThenInclude(e => e.EventsNavigation)
               .FirstOrDefaultAsync(e => e.EventId == eventId);      
        }


        public async Task<Event> AddParticipantToEvent(int eventId, int? userId)
        {
            try
            {
                var eventEntity = await _context.Events.FindAsync(eventId);
                var userEntity = await _context.Users.FindAsync(userId);

                if (eventEntity == null || userEntity == null)
                {
                    return null;
                }

                eventEntity.Participants.Add(userEntity);
                await _context.SaveChangesAsync();

                return eventEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<bool> RemoveParticipantFromEvent(int eventId, int userId)
        {
            try
            {
                var eventEntity = await _context.Events.Include(e => e.Participants).FirstOrDefaultAsync(e => e.EventId == eventId);

                if (eventEntity == null)
                {
                    return false;
                }

                var userToRemove = eventEntity.Participants.FirstOrDefault(p => p.UserId == userId);
                if (userToRemove == null)
                {
                    return false;
                }

                eventEntity.Participants.Remove(userToRemove);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
     


    }
}


