using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using TCPHunter.TCPHelper.Common.Enums;
using TCPHunter.TCPHelper.Common.Exceptions;
using TCPHunter.TCPHelper.Common.Interfaces;
using TCPHunter.TCPHelper.Common.Models;
using TCPHunter.TCPHelper.Common.Structs;

namespace TCPHunter.TCPHelper.Implementation
{
    public sealed class TCPReader: ITCPReader
    {
        public IEnumerable<TCPEntry> GetAllTCPEntries()
        {
            var tcpTable = GetTcpTable();

            foreach (var tcpRow in tcpTable)
                yield return (TCPEntry)tcpRow;
        }

        private MIB_TCPROW_OWNER_PID[] GetTcpTable()
        {
            int pdwSize; // the estimated size of the structure returned in pTcpTable, in bytes.
            MIB_TCPROW_OWNER_PID[] tcpTable = null;

            IntPtr ptrTcpTable = IntPtr.Zero;
            const int AF_INET = 2; // IP_v4

            // first call is made in order to set pdwSize value
            GetExtendedTcpTable(ptrTcpTable, out pdwSize, false, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);

            ptrTcpTable = Marshal.AllocHGlobal(pdwSize);

            try
            {
                var ret = GetExtendedTcpTable(ptrTcpTable, out pdwSize, false, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL,0);

                if(ret == (int)ErrorCodes.ERROR_SUCCESS)
                {
                    var table = (MIB_TCPTABLE_OWNER_PID)Marshal.PtrToStructure(ptrTcpTable, typeof(MIB_TCPTABLE_OWNER_PID));
                    tcpTable = new MIB_TCPROW_OWNER_PID[table.dwNumEntries];

                    IntPtr rowPtr = (IntPtr)((long)ptrTcpTable + Marshal.SizeOf(table.dwNumEntries));

                    for (int i = 0; i < table.dwNumEntries; i++)
                    {
                        MIB_TCPROW_OWNER_PID tcpRow = (MIB_TCPROW_OWNER_PID)Marshal.PtrToStructure(rowPtr, typeof(MIB_TCPROW_OWNER_PID));
                        tcpTable[i] = tcpRow;
                        rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(tcpRow));
                    }
                }
                else
                {
                    string errorValue = "UNKNOWN";

                    if(Enum.IsDefined(typeof(ErrorCodes), Convert.ToInt32(ret)))
                    {
                        errorValue = ((ErrorCodes)ret).ToString();
                    }

                    throw new TCPRetrievalFailedException(Convert.ToInt32(ret), errorValue);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("GetTcpTable failed: " + ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(ptrTcpTable);
            }

            return tcpTable;
        }

        [DllImport("iphlpapi.dll")]
        public static extern UInt32 GetExtendedTcpTable(IntPtr pTcpTable, out int pdwSize, bool bOrder, UInt32 ulAf, TCP_TABLE_CLASS TableClass, UInt32 Reserved);

        enum ErrorCodes
        {
            ERROR_SUCCESS = 0,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_BUFFER_OVERFLOW = 111,
            ERROR_INSUFFICIENT_BUFFER = 122,
            ERROR_NO_DATA = 232,
        }
    }
}
