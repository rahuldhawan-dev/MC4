using System;
using MMSINC.Utilities;

namespace MMSINC.Exceptions
{
    public class AddressNotFoundException : Exception
    {
        #region Constants

        public const string MESSAGE_FORMAT =
            "Ip{0} address for host '{1}' could not be found.";

        #endregion

        #region Constructors

        private AddressNotFoundException()
            : base()
        {
            throw new InvalidOperationException();
        }

        private AddressNotFoundException(string message)
            : base(message)
        {
            throw new InvalidOperationException();
        }

        public AddressNotFoundException(string address,
            Dns.AddressType addressType)
            : base(String.Format(MESSAGE_FORMAT, address, addressType)) { }

        #endregion
    }
}
