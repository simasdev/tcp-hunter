using Microsoft.Extensions.Hosting;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace TCPHunter.WindowsUI.Common.Base
{
    public class ViewModelBase: BindableBase
    {
        public IHost Host => ((App)Application.Current).AppHost;

        public IServiceProvider Services => Host.Services;
    }
}
