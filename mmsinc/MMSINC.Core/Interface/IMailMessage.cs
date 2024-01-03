using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Text;

namespace MMSINC.Interface
{
    public interface IMailMessage : IDisposable
    {
        #region Abstract Properties

        MailAddress From { get; set; }
        MailAddressCollection To { get; }
        bool IsBodyHtml { get; set; }
        string Subject { get; set; }
        MailPriority Priority { get; set; }
        string Body { get; set; }
        MailMessage InnerMessage { get; }
        IAttachmentCollection Attachments { get; }
        NameValueCollection Headers { get; }
        MailAddress Sender { get; set; }

        #endregion

        #region Abstract Methods

        void AddAttachment(string fileName, byte[] binaryData);
        string GetEncodedContent();

        #endregion
    }

    public interface IAttachmentCollection : IDisposable, ICollection<Attachment>
    {
        #region Abstract Properties

        Attachment this[int i] { get; }

        #endregion

        #region Abstract Methods

        int IndexOf(Attachment item);
        void Insert(int index, Attachment item);
        void RemoveAt(int index);

        #endregion
    }
}
