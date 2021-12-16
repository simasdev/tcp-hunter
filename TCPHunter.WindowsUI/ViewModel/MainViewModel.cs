using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TCPHunter.WindowsUI.Common.Base;

namespace TCPHunter.WindowsUI.ViewModel
{
    public class MainViewModel: ViewModelBase
    {
        private readonly ManagerViewModel managerViewModel;

        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            set => SetProperty(ref currentViewModel, value);
        }

        public MainViewModel()
        {
            managerViewModel = Services.GetRequiredService<ManagerViewModel>();

            CurrentViewModel = managerViewModel;
        }
    }
}
