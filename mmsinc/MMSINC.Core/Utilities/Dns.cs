using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using MMSINC.Exceptions;
using SystemDns = System.Net.Dns;

namespace MMSINC.Utilities
{
    [ExcludeFromCodeCoverage]
    public static class Dns
    {
        #region Exposed Static Methods

        public static IPAddress GetHostAddressV4(string hostName)
        {
            return GetHostAddress(hostName, AddressType.V4);
        }

        public static IPAddress GetHostAddressV6(string hostName)
        {
            return GetHostAddress(hostName, AddressType.V6);
        }

        public static IPAddress GetHostAddress(string hostName, AddressType type)
        {
            var ips = SystemDns
                     .GetHostAddresses(hostName)
                     .Where(a => a.AddressFamily == type.ToAddressFamily())
                     .ToArray();

            if (ips.Length == 0)
            {
                throw new AddressNotFoundException(hostName, type);
            }

            return ips.First();
        }

        #endregion

        #region Enums

        public enum AddressType
        {
            V4,
            V6
        }

        #endregion
    }

    [ExcludeFromCodeCoverage]
    internal static class AddressTypeExtensions
    {
        #region Extension Methods

        public static AddressFamily ToAddressFamily(this Dns.AddressType type)
        {
            switch (type)
            {
                case Dns.AddressType.V4:
                    return AddressFamily.InterNetwork;
                case Dns.AddressType.V6:
                    return AddressFamily.InterNetworkV6;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static Dns.AddressType ToAddressType(this AddressFamily family)
        {
            switch (family)
            {
                case AddressFamily.InterNetwork:
                    return Dns.AddressType.V4;
                case AddressFamily.InterNetworkV6:
                    return Dns.AddressType.V6;
                default:
                    throw new InvalidOperationException();
            }
        }

        #endregion
    }
}
