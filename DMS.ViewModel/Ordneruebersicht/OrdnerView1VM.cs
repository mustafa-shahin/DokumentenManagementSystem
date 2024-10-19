using DMS.Model;
using DMS.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DMS.ViewModel.Ordneruebersicht;

public class OrdnerView1VM : ViewModelBase, IOrdnerView1VM
{
    private Benutzer m_currentUser;
    private readonly OrdnerService m_ordnerService;
    private Ordner _selectedFolder;


    public Benutzer CurrentUser
    {
        get => m_currentUser;
        set
        {
            if (Equals(value, m_currentUser)) return;
            m_currentUser = value;
            OnPropertyChanged();
        }
    }
    public Ordner SelectedFolder
    {
        get { return _selectedFolder; }
        set
        {
            _selectedFolder = value;
            OnPropertyChanged(nameof(SelectedFolder));
        }
    }

    public void Init(Benutzer currentUser)
    {
        CurrentUser = currentUser;
    }

    public ObservableCollection<Ordner> OrdnerCollection { get; set; }
    public event EventHandler<Ordner> FolderCreated;
    public DelegateCommand OpenFolderCommand { get; set; }
    public event EventHandler<Ordner> FolderOpened;
    public DelegateCommand CreateFolderCommand { get; set; }
    public DelegateCommand SaveFolderNameCommand { get; set; }

    public OrdnerView1VM(OrdnerService ordnerService)
    {
        m_ordnerService = ordnerService;
        OrdnerCollection = [];
        OpenFolderCommand = new DelegateCommand(OnOpenFolder);
        CreateFolderCommand = new DelegateCommand(OnCreateFolder);
        LoadFoldersAsync();
    }

    private async void OnCreateFolder(object? o)
    {
        var newFolder = await m_ordnerService.CreateFolder();
        OrdnerCollection.Add(newFolder);
        FolderCreated?.Invoke(this, newFolder);
    }

    // Load folders by calling OrdnerService
    private async void LoadFoldersAsync()
    {
        var folders = await m_ordnerService.GetFolders();
        foreach (var folder in folders)
        {
            OrdnerCollection.Add(folder);
        }
    }

    public async Task SaveFolderChangesAsync(Ordner folder)
    {
        if (!string.IsNullOrWhiteSpace(folder.Name))
        {
            await m_ordnerService.UpdateFolder(folder);
        }
    }
    private void OnOpenFolder(object o)
    {
        if (o is Ordner folder)
        {
            FolderOpened?.Invoke(this, folder);
        }
    }
}