using CaptureIt.Data;
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
                .ToListAsync();
        }

        public async Task<Event> GetById(int id)
        {
            return await _context.Events
                .Include(p => p.Owner)
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

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> AddParticipantToEvent(int eventId, int userId)
        {
            try
            {
                var eventEntity = await _context.Events.FindAsync(eventId);
                var userEntity = await _context.Users.FindAsync(userId);
                if (eventEntity != null && userEntity != null)
                {
                    eventEntity.Participants.Add(userEntity);
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

        public async Task<bool> RemoveParticipantFromEvent(int eventId, int userId)
        {
            try
            {
                var eventEntity = await _context.Events
                    .Include(e => e.Participants)
                    .FirstOrDefaultAsync(e => e.EventId == eventId);

                if (eventEntity != null)
                {
                    var userEntity = await _context.Users.FindAsync(userId);

                    if (userEntity != null)
                    {
                        eventEntity.Participants.Remove(userEntity);

                        await _context.SaveChangesAsync();

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}



