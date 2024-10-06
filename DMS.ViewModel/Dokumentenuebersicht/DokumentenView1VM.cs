using DMS.Model;

namespace DMS.ViewModel.Dokumentenuebersicht
{
    public class DokumentenView1VM : ViewModelBase, IDokumentenView1VM
    {
        private Ordner m_currentFolder;
        private Benutzer m_currentUser;
        public string FolderName
        {
            get => m_currentFolder?.Name;
            private set
            {
                if (m_currentFolder != null)
                {
                    m_currentFolder.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        public void Init(Ordner folder, Benutzer currentUser)
        {
            m_currentFolder = folder;
            m_currentUser = currentUser;
            OnPropertyChanged(nameof(FolderName));
        }
    }
}
