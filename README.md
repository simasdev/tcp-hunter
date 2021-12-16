
# TCP-Hunter

It's a simple library, that helps you to get all TCP connections on a local Windows machine.
This project comes with both, TCP-Hunter library and a simple WPF app that allows you to
overview active TCP connections as well as kill a selected process to end undesired TCP connection.

## Prerequisites

Target: Windows

Minimum supported client: Windows Vista, Windows XP with SP2

Minimum supported server: Windows Server 2008, Windows Server 2003 with SP1
## Get it running
You can find an example of the library use in TCPHunter.WindowsUI WPF application.

First of all you should add a reference to TCPHunter.TCPHelper library.

There are 2 public interfaces:
- **ITCPReader** - TCPReader is responsible for getting all active TCP connections using the 
   GetExtendedTcpTable function in Iphlpapi.dll 
- **ITCPManager** - TCPManager is using ITCPReader and has two main events: TCPEntryAdded and 
  TCPEntryRemoved - it simplifies TCP entries management for you. You must call ITCPManager.Start()
  in order to start receiving notifications.

You can either use previously mentioned interfaces' **implementations in TCPHunter.TCPHelper.Implementation
namespace** or **by adding services by calling .AddTCPHunter which adds the TCP-Hunter services to the services container**


## Technologies

- .NET Core
- Windows Presentation Foundation (WPF)
- Platform Invoke (P/Invoke)
## Features
- Get all TCP connections on a local machine
- TCPManager that simplifies TCP entries retrieval through TCPEntryAdded and TCPEntryRemoved
  events
## Future development
- As for now TCP-Hunter only implements IPv4 version of IP used by the TCP endpoints, however,
  it is possible to implement IPv6 as well.
- GetExtendedTcpTable function has TableClass parameter which defines the type of the TCP table 
  structure to retrieve. At this moment I have only implemented TCP_TABLE_OWNER_PID_ALL which
  returns [MIB_TCPTABLE_OWNER_PID](https://docs.microsoft.com/en-us/windows/desktop/api/tcpmib/ns-tcpmib-mib_tcptable_owner_pid)
  pTcpTable structure; other pTcpTable structures might be implemented in the future
## Resources that I have used

- [GetExtendedTcpTable function's microsoft documentation](https://docs.microsoft.com/en-us/windows/win32/api/iphlpapi/nf-iphlpapi-getextendedtcptable)
