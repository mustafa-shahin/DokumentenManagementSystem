using DMS.Model;
using ViewModel.Interface;

namespace DMS.ViewModel.Ordneruebersicht;

public interface IOrdnerView1VM : IViewModelBase
{
    void Init(Benutzer currentUser);
}