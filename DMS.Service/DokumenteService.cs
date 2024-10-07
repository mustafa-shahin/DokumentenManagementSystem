using DMS.DataAccess;
using DMS.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DMS.Service
{
    public class DokumenteService
    {
        private readonly DataContext _context;

        public DokumenteService(DataContext context)
        {
            _context = context;
        }

        public void AddFile(Ordner currentFolder, Benutzer currentUser)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var fileContent = File.ReadAllBytes(filePath);

                var dokument = new Dokument
                {
                    Name = Path.GetFileName(filePath),
                    Description = "New file",
                    Version = "1.0",
                    Ersteller = currentUser,
                    ErstellDatum = DateTime.Now,
                    Content = fileContent,
                    IsVisibleAllUser = false,
                    OrdnerId = currentFolder.Id
                };

                dokument.Searchtags ??= [];
                // Ensure the 'Ersteller' (Benutzer) is tracked, so EF doesn't try to insert a duplicate
                if (_context.Benutzer.Any(b => b.Id == dokument.Ersteller.Id))
                {
                    // Attach the existing Benutzer to the context to avoid duplication
                    _context.Entry(dokument.Ersteller).State = EntityState.Unchanged;
                }
                _context.Dokumente.Add(dokument);
                _context.SaveChanges();
            }
        }

        public void UpdateFile(Dokument dokument)
        {
            _context.Dokumente.Update(dokument);
            _context.SaveChanges();
        }

        public async Task<List<Dokument>> GetFilesForFolder(int folderId, Benutzer user)
        {
            return await _context.Dokumente
                .Where(d => d.OrdnerId == folderId && (d.IsVisibleAllUser || d.Ersteller.Id == user.Id))
                .ToListAsync();
        }

        public static void DownloadFile(Dokument dokument)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = dokument.Name
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, dokument.Content);
            }
        }
    }
}
