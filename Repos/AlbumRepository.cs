﻿using CaptureIt.Data;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;


namespace CaptureIt.Repos
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly CaptureItContext _context;
        public AlbumRepository(CaptureItContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Album>> GetAll()
        {
            return await _context.Albums
                .Include(p => p.Event)

                .ToListAsync();

        }
        public async Task<Album> GetById(int id) 
        {
            return await _context.Albums
                .Include(p => p.Event) 
                .FirstOrDefaultAsync(p => p.AlbumId == id);

        }
        public async Task<Album> Add(Album album)
        {
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
            return album;

        }
        public async Task<Album> Update(Album album)
        {
            _context.Albums.Update(album);
            await _context.SaveChangesAsync();
            return album;

        }
        public async Task<bool> Delete(int id)
        {
            var album = await GetById(id);
                if (album == null)
            {
                return false;
            }
            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

