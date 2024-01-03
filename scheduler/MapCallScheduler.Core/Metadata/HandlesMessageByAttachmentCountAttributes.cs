using System;
using System.Linq;
using MMSINC.Interface;

namespace MapCallScheduler.Metadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HandlesMessageWithAttachmentCountGreaterThanAttribute : HandlesMessageAttribute
    {
        #region Constructors

        public HandlesMessageWithAttachmentCountGreaterThanAttribute(int attachmentCount) : base(GetPredicate(attachmentCount)) {}

        #endregion

        #region Private Methods

        private static Func<IMailMessage, bool> GetPredicate(int attachmentCount)
        {
            return m => m.Attachments.Any() && m.Attachments.Count > attachmentCount;
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HandlesMessageWithAttachmentCountLessThanAttribute : HandlesMessageAttribute
    {
        #region Constructors

        public HandlesMessageWithAttachmentCountLessThanAttribute(int attachmentCount) : base(GetPredicate(attachmentCount)) {}

        #endregion

        #region Private Methods

        private static Func<IMailMessage, bool> GetPredicate(int attachmentCount)
        {
            return m => {
                return !m.Attachments.Any() || m.Attachments.Count < attachmentCount;
            };
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HandlesMessageWithAttachmentCountExactlyAttribute : HandlesMessageAttribute
    {
        #region Constructors

        public HandlesMessageWithAttachmentCountExactlyAttribute(int attachmentCount) : base(GetPredicate(attachmentCount)) {}

        #endregion

        #region Private Methods

        private static Func<IMailMessage, bool> GetPredicate(int attachmentCount)
        {
            return m => m.Attachments.Any() && m.Attachments.Count == attachmentCount;
        }

        #endregion
    }
}