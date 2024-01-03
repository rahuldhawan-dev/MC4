using System;
using System.Net.Mail;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class SmtpClientWrapper : ISmtpClient
    {
        #region Private Members

        protected SmtpClient _client;

        #endregion

        #region Constructors

        public SmtpClientWrapper()
        {
            _client = new SmtpClient();
        }

        #endregion

        #region Exposed Methods

        public void Send(IMailMessage message)
        {
            if (_client == null)
            {
                throw new InvalidOperationException("_client is null!");
            }

            if (message == null)
            {
                throw new InvalidOperationException("message is null!");
            }

            if (message.InnerMessage == null)
            {
                throw new InvalidOperationException("message.InnerMessage is null!");
            }

            _client.Send(message.InnerMessage);
        }

        public void Dispose()
        {
            try
            {
                if (_client != null)
                {
                    _client.Dispose();
                }
            }
            finally
            {
                _client = null;
            }
        }

        #endregion
    }
}
