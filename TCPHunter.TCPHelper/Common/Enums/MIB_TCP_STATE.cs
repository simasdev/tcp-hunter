using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TCPHunter.TCPHelper.Common.Enums
{
    public enum MIB_TCP_STATE
    {
        [Display(Name = "CLOSED")]
        MIB_TCP_STATE_CLOSED = 1,
        [Display(Name = "LISTEN")]
        MIB_TCP_STATE_LISTEN = 2,
        [Display(Name = "SYN-SENT")]
        MIB_TCP_STATE_SYN_SENT = 3,
        [Display(Name = "SYN-RECEIVED")]
        MIB_TCP_STATE_SYN_RCVD = 4,
        [Display(Name = "ESTABLISHED")]
        MIB_TCP_STATE_ESTAB = 5,
        [Display(Name = "FIN-WAIT-1")]
        MIB_TCP_STATE_FIN_WAIT1 = 6,
        [Display(Name = "FIN-WAIT-2")]
        MIB_TCP_STATE_FIN_WAIT2 = 7,
        [Display(Name = "CLOSE-WAIT")]
        MIB_TCP_STATE_CLOSE_WAIT = 8,
        [Display(Name = "CLOSING")]
        MIB_TCP_STATE_CLOSING = 9,
        [Display(Name = "LAST-ACK")]
        MIB_TCP_STATE_LAST_ACK = 10,
        [Display(Name = "TIME-WAIT")]
        MIB_TCP_STATE_TIME_WAIT = 11,
        [Display(Name = "DELETE-TCB")]
        MIB_TCP_STATE_DELETE_TCB = 12,
        [Display(Name = "UNKNOWN")]
        UNKNOWN = 13
    }
}
