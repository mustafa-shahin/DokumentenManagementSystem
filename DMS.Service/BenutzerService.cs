using DMS.DataAccess;
using DMS.Model;

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
        public bool ValidateUser(string username, string password)
        {
            using var context = new DataContext();
            return context.Benutzer.Any(b => b.Name == username && b.Passwort == password && b.IsActive);
        }
    }

}