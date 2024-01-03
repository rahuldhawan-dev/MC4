using System;

namespace MMSINC.Interface
{
    public interface ISmtpClient : IDisposable
    {
        #region Methods

        void Send(IMailMessage message);

        #endregion
    }
}
