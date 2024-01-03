using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net.Mail;
using System.Text;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class MailMessageWrapper : IMailMessage
    {
        #region Properties

        public MailMessage InnerMessage { get; protected set; }

        public MailAddress From
        {
            get { return InnerMessage.From; }
            set { InnerMessage.From = value; }
        }

        public MailAddressCollection To
        {
            get { return InnerMessage.To; }
        }

        public bool IsBodyHtml
        {
            get { return InnerMessage.IsBodyHtml; }
            set { InnerMessage.IsBodyHtml = value; }
        }

        public MailAddressCollection ReplyToList
        {
            get { return InnerMessage.ReplyToList; }
        }

        public string Subject
        {
            get { return InnerMessage.Subject; }
            set { InnerMessage.Subject = value; }
        }

        public MailPriority Priority
        {
            get { return InnerMessage.Priority; }
            set { InnerMessage.Priority = value; }
        }

        public string Body
        {
            get => InnerMessage.Body;
            set => InnerMessage.Body = value;
        }

        public IAttachmentCollection Attachments
        {
            get { return new AttachmentCollectionWrapper(InnerMessage.Attachments); }
        }

        public NameValueCollection Headers
        {
            get { return InnerMessage.Headers; }
        }

        public MailAddress Sender
        {
            get => InnerMessage.Sender;
            set => InnerMessage.Sender = value;
        }

        #endregion

        #region Constructors

        public MailMessageWrapper(MailMessage message)
        {
            InnerMessage = message;
        }

        public MailMessageWrapper(String from, String to)
        {
            InnerMessage = new MailMessage(from, to);
        }

        protected MailMessageWrapper() { }

        #endregion

        #region Exposed Methods

        public void AddAttachment(string fileName, byte[] binaryData)
        {
            var ms = new MemoryStream(binaryData);
            var att = new Attachment(ms, fileName);
            InnerMessage.Attachments.Add(att);
        }

        public string GetEncodedContent()
        {
            var sb = new StringBuilder();

            foreach (var view in InnerMessage.AlternateViews)
            {
                var byteArray = new byte[view.ContentStream.Length];
                sb.Append(Encoding.ASCII.GetString(byteArray, 0,
                    view.ContentStream.Read(byteArray, 0, byteArray.Length)));
            }

            return sb.ToString();
        }

        public void Dispose()
        {
            InnerMessage.Dispose();
        }

        #endregion
    }

    public class AttachmentCollectionWrapper : IAttachmentCollection
    {
        #region Private Members

        private readonly Collection<Attachment> _collection;

        public AttachmentCollectionWrapper(Collection<Attachment> attachments)
        {
            _collection = attachments;
        }

        #endregion

        #region Properties

        public virtual Attachment this[int index]
        {
            get { return _collection[index]; }
            set { _collection[index] = value; }
        }

        public virtual int Count
        {
            get { return _collection.Count; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Exposed Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_collection).GetEnumerator();
        }

        public virtual void CopyTo(Array array, int index)
        {
            ((ICollection)_collection).CopyTo(array, index);
        }

        public virtual int Add(object value)
        {
            return ((IList)_collection).Add(value);
        }

        public virtual bool Contains(object value)
        {
            return ((IList)_collection).Contains(value);
        }

        public virtual int IndexOf(object value)
        {
            return ((IList)_collection).IndexOf(value);
        }

        public virtual void Insert(int index, object value)
        {
            ((IList)_collection).Insert(index, value);
        }

        public virtual void Remove(object value)
        {
            ((IList)_collection).Remove(value);
        }

        public virtual void Add(Attachment item)
        {
            _collection.Add(item);
        }

        public virtual void Clear()
        {
            _collection.Clear();
        }

        public virtual void CopyTo(Attachment[] array, int index)
        {
            _collection.CopyTo(array, index);
        }

        public virtual bool Contains(Attachment item)
        {
            return _collection.Contains(item);
        }

        public virtual IEnumerator<Attachment> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public virtual int IndexOf(Attachment item)
        {
            return _collection.IndexOf(item);
        }

        public virtual void Insert(int index, Attachment item)
        {
            _collection.Insert(index, item);
        }

        public virtual bool Remove(Attachment item)
        {
            return _collection.Remove(item);
        }

        public virtual void RemoveAt(int index)
        {
            _collection.RemoveAt(index);
        }

        public virtual void Dispose()
        {
            // noop
        }

        #endregion
    }
}
