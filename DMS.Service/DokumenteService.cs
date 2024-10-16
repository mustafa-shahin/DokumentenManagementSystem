using DMS.DataAccess;
using DMS.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Windows;


namespace DMS.Service
{
    public class DokumenteService
    {
        private readonly DataContext _context;
        public string FilePath { get; private set; }
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

                // Check if the current user is already tracked
                var existingUser = _context.Benutzer.Find(currentUser.Id);
                var existingFolder = _context.Ordner.Find(currentFolder.Id);

                if (existingUser == null)
                {
                    // Attach the current user if it's not being tracked
                    existingUser = currentUser;
                    _context.Benutzer.Attach(existingUser);
                }

                if (existingFolder == null)
                {
                    // Attach the current folder if it's not being tracked
                    existingFolder = currentFolder;
                    _context.Ordner.Attach(existingFolder);
                }

                var dokument = new Dokument
                {
                    Name = Path.GetFileName(filePath),
                    Description = "New file",
                    Version = "1.0",
                    Ersteller = existingUser,  // Set the tracked or newly attached Benutzer
                    ErstellDatum = DateTime.Now,
                    Content = fileContent,
                    IsVisibleAllUser = false,
                    OrdnerId = existingFolder.Id,
                    Ordner = existingFolder
                };

                dokument.Searchtags ??= [];

                _context.Dokumente.Add(dokument);
                _context.SaveChanges();
            }
        }


        public void UpdateFile(Dokument dokument)
        {
            if (Haschanges(dokument))
            {
                _context.Dokumente.Update(dokument);
                _context.SaveChanges();
            }

        }

        public async Task<List<Dokument>> GetFilesForFolder(int folderId, Benutzer user)
        {
            return await _context.Dokumente
        .Include(d => d.Ersteller).Include(d => d.Ordner)
        .Where(d => d.OrdnerId == folderId && (d.IsVisibleAllUser || d.Ersteller.Id == user.Id))
        .ToListAsync();
        }

        public void DownloadFile(Dokument dokument, out bool openDialog)
        {
            var dialog = false;
            var saveFileDialog = new SaveFileDialog
            {
                FileName = dokument.Name
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                dialog = true;
                try
                {
                    File.WriteAllBytes(saveFileDialog.FileName, dokument.Content);

                    FilePath = saveFileDialog.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Herunterladen der Datei: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            openDialog = dialog;
        }
        private bool Haschanges(Dokument updatedDokument)
        {
            var existingDokument = _context.Dokumente.AsNoTracking().FirstOrDefault(d => d.Id == updatedDokument.Id);
            if (existingDokument != null)
            {
                bool hasChanges = existingDokument.Name != updatedDokument.Name ||
                                  existingDokument.Description != updatedDokument.Description ||
                                  existingDokument.Version != updatedDokument.Version ||
                                  !existingDokument.Content.SequenceEqual(updatedDokument.Content) ||
                                  existingDokument.IsVisibleAllUser != updatedDokument.IsVisibleAllUser;
                if (hasChanges)
                    return true; 
            }

            return false;
        }
        public async Task<List<Dokument>> GetAllDocumentsAsync()
        {
            return await _context.Dokumente
             .Include(d => d.Ersteller)
             .Include(d => d.Ordner)
             .ToListAsync();
        }
    }
}
