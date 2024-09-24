using DMS.DataAccess;
using DMS.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Service
{
    public class BenutzerService
    {
        private readonly DataContext _context;
        public BenutzerService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(string username, string password)
        {
            try
            {
                if (await _context.Benutzer.AnyAsync(b => b.Name == username))
                {
                    Console.WriteLine("User already exists.");
                    return false;
                }

                var newUser = new Benutzer
                {
                    Name = username,
                    Passwort = password,
                    IsActive = true,
                    IsAdmin = false
                };

                await _context.Benutzer.AddAsync(newUser);
                int changes = await _context.SaveChangesAsync();

                return changes > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UserExists(string username)
        {
            return await _context.Benutzer.AnyAsync(b => b.Name == username);
        }

        public async Task<Benutzer?> LoginUser(string username, string password)
        {
            return await _context.Benutzer
                .FirstOrDefaultAsync(b => b.Name == username && b.Passwort == password && b.IsActive);
        }

        public async Task<List<Benutzer>> GetAllUsers()
        {
            return await _context.Benutzer.ToListAsync();
        }

        public async Task SaveChanges(Benutzer benutzer)
        {
            try
            {
                var entity = await _context.Benutzer.FirstOrDefaultAsync(b => b.Id == benutzer.Id);
                if (entity != null)
                {
                    entity.Name = benutzer.Name;
                    entity.Passwort = benutzer.Passwort;
                    entity.IsAdmin = benutzer.IsAdmin;
                    entity.IsActive = benutzer.IsActive;

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
