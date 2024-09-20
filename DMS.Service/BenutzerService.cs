using DMS.DataAccess;
using DMS.Model;
using Microsoft.EntityFrameworkCore;

namespace DMS.Service
{
    public class BenutzerService
    {
        public bool CreateUser(string username, string password)
        {
            try
            {
                using var context = new DataContext();
                if (context.Benutzer.Any(b => b.Name == username))
                {
                    return false;
                }

                var newUser = new Benutzer
                {
                    Name = username,
                    Passwort = password,
                    IsActive = true,
                    IsAdmin = false
                };

                context.Benutzer.Add(newUser);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public Benutzer? LoginUser(string username, string password)
        {
            using var context = new DataContext();
            return context.Benutzer.FirstOrDefault(b => b.Name == username && b.Passwort == password && b.IsActive);
        }

        public async Task<List<Benutzer>> GetAllUsers()
        {
            using var context = new DataContext();
            return context.Benutzer.ToList();
        }

        public void SaveChanges(Benutzer benutzer)
        {
            try
            {
                using var context = new DataContext();
                var entity = context.Benutzer.FirstOrDefault(b => b.Id == benutzer.Id);
                if (entity != null)
                {
                    entity.Name = benutzer.Name;
                    entity.Passwort = benutzer.Passwort;
                    entity.IsAdmin = benutzer.IsAdmin;
                    entity.IsActive = benutzer.IsActive; 
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public async Task<bool> UserExist(string username)
        {
            using var context = new DataContext();
            return await context.Benutzer.AnyAsync(b => b.Name == username);
        }
    }
}
