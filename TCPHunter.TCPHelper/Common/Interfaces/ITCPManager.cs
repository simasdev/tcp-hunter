using System;
using System.Collections.Generic;
using System.Text;
using TCPHunter.TCPHelper.Common.CustomEventArguments;

namespace TCPHunter.TCPHelper.Common.Interfaces
{
    public interface ITCPManager
    {
        event EventHandler<TCPEntryEventArgs> TCPEntryAdded;
        event EventHandler<TCPEntryEventArgs> TCPEntryRemoved;

        void Start();
        void Stop();
    }
}
