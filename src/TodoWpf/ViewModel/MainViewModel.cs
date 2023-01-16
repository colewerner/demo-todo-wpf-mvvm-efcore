using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoWpf.ViewModel
{
    public partial class MainViewModel : ObservableRecipient
    {
        [ObservableProperty]
        public ObservableRecipient currentViewModel;

        public MainViewModel(TodoViewModel todoViewModel)
        {
            this.CurrentViewModel = todoViewModel;
        }
    }
}
