namespace MMSINC.Models.ViewModels
{
    public enum NotificationItemType
    {
        Success,
        Warning,
        Error
    }

    /// <summary>
    /// Model used for the _NotificationItem template and the DisplayInlineNotification HtmlHelper method.
    /// </summary>
    public class NotificationItem
    {
        #region Properties

        /// <summary>
        /// The message that should be displayed.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The type of notication being displayed. This dictates the styling used when displayed.
        /// </summary>
        public NotificationItemType NotificationType { get; set; }

        #endregion
    }
}
