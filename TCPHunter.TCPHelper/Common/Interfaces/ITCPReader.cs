using System;
using System.Collections.Generic;
using System.Text;
using TCPHunter.TCPHelper.Common.Models;

namespace TCPHunter.TCPHelper.Common.Interfaces
{
    public interface ITCPReader
    {
        IEnumerable<TCPEntry> GetAllTCPEntries();
    }
}
