using System;
using System.Collections.Generic;
using System.Text;
using TCPHunter.WindowsUI.Common.Base;
using TCPHunter.TCPHelper.Common.Interfaces;
using TCPHunter.TCPHelper.Implementation;
using TCPHunter.TCPHelper.Common.Models;
using TCPHunter.TCPHelper.Common.Enums;
using System.Collections.ObjectModel;
using System.Linq;
using TCPHunter.WindowsUI.Common;
using System.Diagnostics;
using System.Windows;
using System.ComponentModel;

namespace TCPHunter.WindowsUI.ViewModel
{
    public class ManagerViewModel: ViewModelBase
    {
        #region Private Properties
        private readonly TCPManager tcpManager;
        #endregion

        #region Commands

        public RelayCommand EndProcessCommand { get; private set; }

        #endregion

        #region Public Properties
        private bool refreshTCPEntriesAutomatically = false;
        public bool RefreshTCPEntriesAutomatically
        {
            get => refreshTCPEntriesAutomatically;
            set
            {
                SetProperty(ref refreshTCPEntriesAutomatically, value);

                if (refreshTCPEntriesAutomatically)
                {
                    tcpManager.Start();
                }
                else
                {
                    tcpManager.Stop();
                }
            }
        }

        public TCPEntry SelectedTCPEntry { get; set; }
        private readonly ObservableCollection<TCPEntry> tcpEntries;
        public ObservableCollection<TCPEntry> TCPEntries
        {
            get => tcpEntries;
        }
        #endregion

        public ManagerViewModel(TCPManager tcpManager)
        {
            this.tcpManager = tcpManager;
            this.tcpEntries = new ObservableCollection<TCPEntry>();

            EndProcessCommand = new RelayCommand(OnEndProcess);

            ConfigureTCPManager();

            this.RefreshTCPEntriesAutomatically = true;
        }

        private void OnEndProcess()
        {
            if (SelectedTCPEntry == null || SelectedTCPEntry.ProcessID <= 0)
                return;

            try
            {
                var process = Process.GetProcessById(SelectedTCPEntry.ProcessID);

                if(process != null)
                {
                    try
                    {
                        process.Kill(true);
                    }
                    catch(NotSupportedException)
                    {
                        MessageBox.Show("Unable to end process that is running on a remote comuper");
                    }
                    catch(Win32Exception)
                    {
                        MessageBox.Show("The associated process could not be terminated");
                    }
                    catch(InvalidOperationException)
                    {
                        MessageBox.Show("Unable to end process, it might have already been ended");
                    }
                }
            }
            catch(Exception ex)
            {
                if (ex.InnerException?.Message == "Access is denied.")
                    MessageBox.Show("Unable to end process, you do not have necessary permissions");
                else
                    MessageBox.Show("Unable to end process, it might have already been ended");
            }
        }

        private void ConfigureTCPManager()
        {
            tcpManager.TCPEntryAdded += TcpManager_TCPEntryAdded;
            tcpManager.TCPEntryRemoved += TcpManager_TCPEntryRemoved;
        } 

        private void TcpManager_TCPEntryAdded(object sender, TCPHelper.Common.CustomEventArguments.TCPEntryEventArgs e)
        {
            if (e.TcpEntry.State != MIB_TCP_STATE.MIB_TCP_STATE_ESTAB && e.TcpEntry.State != MIB_TCP_STATE.MIB_TCP_STATE_LISTEN)
                return; // I want to display only connections in established or listening state, I do not want to display e.g. connection that is in syn-sent state

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (tcpEntries.Count(tcpEntry => tcpEntry.Equals(e.TcpEntry)) == 0)
                {
                    TCPEntries.Add(e.TcpEntry);
                }
            });
        }

        private void TcpManager_TCPEntryRemoved(object sender, TCPHelper.Common.CustomEventArguments.TCPEntryEventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                TCPEntries.Remove(e.TcpEntry);
            });
        }
    }
}
