using System;
using System.Collections.Generic;
using System.Text;

namespace TCPHunter.TCPHelper.Common.Exceptions
{
    public class TCPRetrievalFailedException: Exception
    {
        public TCPRetrievalFailedException(int errorCode, string errorValue)
        {
            this.ErrorCode = errorCode;
            this.ErrorValue = errorValue;
        }

        public int ErrorCode { get; set; }
        public string ErrorValue { get; set; }
    }
}
