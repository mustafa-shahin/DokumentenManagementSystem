using DMS.DataAccess;
using DMS.Model;
using System.Collections.Generic;
using System.Linq;

namespace DMS.Service
{
    public class OrdnerService
    {
        private readonly DataContext _context;

        public OrdnerService(DataContext context) => _context = context;

        public void CreateFolder(string folderName, int userId, List<string> searchTags = null)
        {
            if (string.IsNullOrEmpty(folderName))
                throw new ArgumentException("Folder name cannot be null or empty.", nameof(folderName));

            var newFolder = new Ordner
            {
                Name = folderName,
                BenutzerId = userId,
                SearchTags = searchTags,
                Order = 0
            };

            _context.Ordner.Add(newFolder);
            _context.SaveChanges();
        }

        public bool DeleteFolder(int folderId, int userId)
        {
            var folder = _context.Ordner.FirstOrDefault(o => o.Id == folderId && o.BenutzerId == userId);
            if (folder != null)
            {
                _context.Ordner.Remove(folder);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Ordner> SearchFolders(int userId, string searchTerm)
        {
            return [.. _context.Ordner
                .Where(o => o.BenutzerId == userId &&
                            (o.Name.Contains(searchTerm) ||
                             o.SearchTags.Any(tag => tag.Contains(searchTerm))))];
        }

        public List<Ordner> GetUserFolders(int userId)
        {
            return [.. _context.Ordner
                .Where(o => o.BenutzerId == userId)
                .OrderBy(o => o.Order)];
        }
    }
}
