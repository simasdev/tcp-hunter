using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using TCPHunter.TCPHelper.Common.CustomEventArguments;
using TCPHunter.TCPHelper.Common.Exceptions;
using TCPHunter.TCPHelper.Common.Interfaces;
using TCPHunter.TCPHelper.Common.Models;

namespace TCPHunter.TCPHelper.Implementation
{
    public sealed class TCPManager : ITCPManager
    {
        #region Events
        public event EventHandler<TCPEntryEventArgs> TCPEntryAdded;
        public event EventHandler<TCPEntryEventArgs> TCPEntryRemoved;
        #endregion

        #region Private Properties
        Timer tcpCheckingTimer;
        const int tcpCheckingInervalMs = 3000;
        volatile bool tcpChekingStopRequested = false;
        volatile bool tcpFetchingInProgress = false;

        private readonly ITCPReader tcpReader;
        #endregion 

        public TCPManager(ITCPReader tcpReader)
        {
            this.tcpReader = tcpReader;
        }

        public void Start()
        {
            tcpChekingStopRequested = false;
            tcpCheckingTimer = new Timer(TCPCheckingTimerCallback, null, 0, tcpCheckingInervalMs);
        }

        public void Stop()
        {
            tcpChekingStopRequested = true;
        }

        private void TCPCheckingTimerCallback(object state)
        {
            if (!tcpChekingStopRequested)
            {
                if (tcpFetchingInProgress)
                    return;

                tcpFetchingInProgress = true;

                FetchAllTCPListeners();

                tcpFetchingInProgress = false;
            }
            else
            {
                tcpCheckingTimer?.Dispose();
                tcpCheckingTimer = null;
            }
        }

        private readonly HashSet<TCPEntry> tcpListeners = new HashSet<TCPEntry>();

        private void FetchAllTCPListeners()
        {
            IEnumerable<TCPEntry> tcpEntries;

            try
            {
                tcpEntries = tcpReader.GetAllTCPEntries();
            }
            catch(TCPRetrievalFailedException ex)
            {
                Debug.WriteLine("TCPRetrievalFailed: " + ex.ErrorCode + " " + ex.ErrorValue);

                tcpEntries = new List<TCPEntry>();
            }

            foreach (var tcpListener in tcpListeners.ToList())
            {
                if (tcpEntries.FirstOrDefault(tcp => tcp.Equals(tcpListener)) == null)
                {
                    tcpListeners.Remove(tcpListener);
                    this.TCPEntryRemoved?.Invoke(this, new TCPEntryEventArgs(tcpListener));
                }
            }

            foreach (var tcpEntry in tcpEntries.ToList())
            {
                if (tcpListeners.FirstOrDefault(tcp => tcp.Equals(tcpEntry)) == null)
                {
                    var objStr = tcpEntry.ToString();
                    tcpListeners.Add(tcpEntry);
                    this.TCPEntryAdded?.Invoke(this, new TCPEntryEventArgs(tcpEntry));
                }
            }           
        }
    }
}
