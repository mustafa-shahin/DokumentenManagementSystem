using DMS.Model;
using ViewModel.Interface;

namespace DMS.ViewModel.Nutzerverwaltung;

public interface INutzerView1VM : IViewModelBase
{
    void Init(Benutzer currentUser);
}