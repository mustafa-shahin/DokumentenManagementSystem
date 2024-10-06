using DMS.Model;
using ViewModel.Interface;

namespace DMS.ViewModel.Dokumentenuebersicht;

public interface IDokumentenFrameVM : IViewModelBase
{
    void Init(Ordner folder, Benutzer m_currentUser);
}