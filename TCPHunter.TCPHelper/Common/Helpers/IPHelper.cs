using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TCPHunter.TCPHelper.Common.Helpers
{
    internal static class IPHelper
    {
        public static string ConvertFromIntegerToIpAddress(uint ipAddress)
        {
            byte[] bytes = BitConverter.GetBytes(ipAddress);
            bool ipAddressReversetionNeeded = false;

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
                ipAddressReversetionNeeded = true;
            }

            var ipAddressStr = new IPAddress(bytes).ToString();

            if (ipAddressReversetionNeeded)
                ipAddressStr = ReverseIPAddress(ipAddressStr);

            return ipAddressStr;
        }

        private static string ReverseIPAddress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
                return string.Empty;

            string[] splittedIPAddresses = ipAddress.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            return string.Join('.', splittedIPAddresses.Reverse());
        }
    }
}
