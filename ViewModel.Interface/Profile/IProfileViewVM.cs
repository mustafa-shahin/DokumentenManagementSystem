using DMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.Interface.Profile
{
    public interface IProfileViewVM : IViewModelBase
    {
        event EventHandler GoBack;
        void Init(Benutzer benutzer);
    }
}
