using System;
using System.Collections.Generic;
using System.Text;
using TCPHunter.TCPHelper.Common.Structs;
using TCPHunter.TCPHelper.Common.Helpers;
using TCPHunter.TCPHelper.Common.Enums;
using TCPHunter.TCPHelper.Common.CustomExtension;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace TCPHunter.TCPHelper.Common.Models
{
    public class TCPEntry
    {
        public UInt32 LocalAddressInt { get; private set; }
        public string LocalAddress
        {
            get => IPHelper.ConvertFromIntegerToIpAddress(this.LocalAddressInt);
        }

        public UInt32 LocalPort { get; private set; }
        public string LocalPortStr
        {
            get
            {
                byte[] bytes = BitConverter.GetBytes(this.LocalPort);
                return (bytes[0] * 256 + bytes[1]).ToString();
            }
        }

        public UInt32 ForeignAddressInt { get; private set; }
        public string ForeignAddress
        {
            get => IPHelper.ConvertFromIntegerToIpAddress(this.ForeignAddressInt);
        }
        
        public UInt32 ForeignPort { get; private set; }
        public string ForeignPortStr
        {
            get
            {
                byte[] bytes = BitConverter.GetBytes(this.ForeignPort);
                return (bytes[0] * 256 + bytes[1]).ToString();
            }
        }

        public MIB_TCP_STATE State { get; private set; }
        public string StateFriendlyName
        {
            get
            {
                return this.State
                    .GetAttribute<DisplayAttribute>()?.Name ?? this.State.ToString();
            }
        }
        public int ProcessID { get; private set; }
        public string ProcessName
        {
            get
            {
                try
                {
                    return Process.GetProcessById(this.ProcessID)?.ProcessName;
                }
                catch (Exception)
                {
                    return string.Empty;
                } 
            }
        }

        public override string ToString()
        {
            return string.Format("TCP {0}:{1} {2}:{3} {4} {5} {6}", 
                this.LocalAddress, this.LocalPortStr, this.ForeignAddress, this.ForeignPortStr, 
                this.StateFriendlyName, this.ProcessID, this.ProcessName);
        }

        public static explicit operator TCPEntry(MIB_TCPROW_OWNER_PID mib)
        {
            TCPEntry entry = new TCPEntry()
            {
               LocalAddressInt = mib.dwLocalAddr,
               LocalPort = mib.dwLocalPort,
               ForeignAddressInt = mib.dwRemoteAddr,
               ForeignPort = mib.dwLocalPort,
               ProcessID = Convert.ToInt32(mib.dwOwningPid)
            };

            if (Enum.IsDefined(typeof(MIB_TCP_STATE), Convert.ToInt32(mib.dwState)))
            {
                entry.State = (MIB_TCP_STATE)Convert.ToInt32(mib.dwState);
            }
            else
            {
                entry.State = MIB_TCP_STATE.UNKNOWN;
            }

            return entry;
        }

        public override bool Equals(object obj)
        {
            if (obj is TCPEntry c)
            {
                return this.LocalAddressInt == c.LocalAddressInt
                       && this.LocalPort == c.LocalPort
                       && this.ForeignAddressInt == c.ForeignAddressInt
                       && this.ForeignPort == c.ForeignPort
                       && this.State == c.State
                       && this.ProcessID == c.ProcessID;

            }

            return false;
        }

        public override int GetHashCode() => (
            this.LocalAddressInt, 
            this.LocalPort, 
            this.ForeignAddressInt, 
            this.ForeignPort, 
            this.State, 
            this.ProcessID)
            .GetHashCode();

    }
}
