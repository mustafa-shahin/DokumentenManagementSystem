using DMS.DataAccess;
using DMS.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Service
{
    public class OrdnerService
    {
        private readonly DataContext _context;

        public OrdnerService(DataContext context)
        {
            _context = context;
        }
        public async Task<Ordner> CreateFolder()
        {
            var newFolder = new Ordner
            {
                Name = "Neuer Ordner",
                Order = await GetNextFolderOrder(),
                SearchTags = []
            };

            _context.Ordner.Add(newFolder); 
            await _context.SaveChangesAsync();  
            return newFolder; 
        }

        private async Task<int> GetNextFolderOrder()
        {
            return await _context.Ordner.CountAsync() + 1;  
        }

        public async Task UpdateFolder(Ordner folder)
        {
            var existingFolder = await _context.Ordner.FindAsync(folder.Id); 
            if (existingFolder != null)
            {
                existingFolder.Name = folder.Name;  
                _context.Ordner.Update(existingFolder);  
                await _context.SaveChangesAsync(); 
            }
        }
        public async Task<List<Ordner>> GetFolders()
        {
            return await _context.Ordner.ToListAsync(); 
        }
    }
}
