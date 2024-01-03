using System.Collections.Generic;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Utility.Notifications
{
    public class NotifierArgs
    {
        #region Fields

        private readonly List<Attachment> _attachments = new List<Attachment>();

        #endregion

        #region Properties

        public IList<Attachment> Attachments => _attachments;

        public int OperatingCenterId { get; set; }
        public RoleModules Module { get; set; }

        /// <summary>
        /// This property serves two purposes:
        ///   * A human readable name of the file used as a template for this notification.
        ///   * Represents the purpose of the notification to be sent, exists as
        ///   a record in NotificationPurposes table.
        /// </summary>
        public string Purpose { get; set; }

        public string Subject { get; set; }

        /// <summary>
        /// A human readable name of the file used as a template for this notification. If this
        /// property is provided a value, it will be used as the template for the notification
        /// instead of <see cref="Purpose"/>. 
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// If this is set, it overrides any configurations and just sends the email to 
        /// this email address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets/sets the data model used for the notification template
        /// </summary>
        public object Data { get; set; }

        #endregion

        #region Public Methods

        public Attachment AddAttachment(string fileName, byte[] binaryData)
        {
            var att = new Attachment(fileName, binaryData);
            Attachments.Add(att);
            return att;
        }

        #endregion
    }
}
