using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TCPHunter.TCPHelper.Common.Structs
{
    public struct MIB_TCPROW_OWNER_PID
    {
        public UInt32 dwState;
        public UInt32 dwLocalAddr;
        public UInt32 dwLocalPort;
        public UInt32 dwRemoteAddr;
        public UInt32 dwRemotePort;
        public UInt32 dwOwningPid;
    }
}
