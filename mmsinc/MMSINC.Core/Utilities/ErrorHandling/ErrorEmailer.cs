using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Common;
using MMSINC.Interface;
using StructureMap;

namespace MMSINC.Utilities.ErrorHandling
{
    public class ErrorEmailer : IErrorEmailer
    {
        #region Private Members

        protected IErrorMessageGenerator _messageGenerator;

        #endregion

        #region Exposed Properties

        public virtual IErrorMessageGenerator ErrorMessageGenerator
        {
            get
            {
                return _messageGenerator ??
                       (_messageGenerator = new ErrorMessageGenerator());
            }
        }

        public static List<string> Exceptions = new List<string> {
            "Message:<br/>Path 'OPTIONS' is forbidden.<br/><br/>",
            "Page:<br/>/includes/js/mambojavascript.js",
            "Page:<br/>/public/default.aspx"
        };

        #endregion

        #region Constructors

        public ErrorEmailer()
        {
            _messageGenerator = new ErrorMessageGenerator();
        }

        #endregion

        #region Private Methods

        protected virtual ISmtpClient CreateSmtpClient()
        {
            // Always create a new SmtpClient instance as this
            // instance will be disposed after sending a message.
            return new SmtpClientWrapper();
        }

        #endregion

        #region Exposed Methods

        public void SendEmail(IHttpContext context, Exception e = null)
        {
            using (var message = _messageGenerator.GenerateMailMessage(context, e))
            {
                if (Exceptions.Any(exception => message.Body.Contains(exception)))
                {
                    return;
                }

                using (var smtp = CreateSmtpClient())
                {
                    smtp.Send(message);
                }
            }
        }

        public void Dispose()
        {
            // noop
        }

        #endregion
    }

    public interface IErrorEmailer : IDisposable
    {
        #region Methods

        void SendEmail(IHttpContext context, Exception e = null);

        #endregion
    }
}
