using System;
using System.Collections.Generic;
using System.Text;
using TCPHunter.TCPHelper.Common.Models;

namespace TCPHunter.TCPHelper.Common.CustomEventArguments
{
    public class TCPEntryEventArgs: EventArgs
    {
        public TCPEntryEventArgs()
        {

        }

        public TCPEntryEventArgs(TCPEntry tcpEntry)
        {
            this.TcpEntry = tcpEntry;
        }

        public TCPEntry TcpEntry { get; set; }
    }
}
