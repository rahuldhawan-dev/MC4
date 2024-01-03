using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Common.Controls
{
    // TODO: Add more types! 
    public enum NoticeBoxType
    {
        Question
    }

    public enum NoticeBoxIconSize
    {
        Large
    }

    public interface INoticeBox
    {
        #region Properties

        string Header { get; set; }
        NoticeBoxIconSize IconSize { get; set; }
        string Message { get; set; }
        NoticeBoxType NoticeType { get; set; }

        #endregion
    }

    /// <summary>
    /// A nifty little notification display that has a nice little icon. That's it. Maybe 
    /// people will actually read things when they show up.
    /// </summary>
    /// <remarks>
    /// 
    /// ParseChildren(false) is needed so child controls can be added to it.
    /// 
    /// </remarks>
    [ParseChildren(false)]
    public class NoticeBox : WebControl, INoticeBox
    {
        #region Consts

        private struct BASE
        {
            public const string CSS_CLASS = "nb";
            public const string SEPARATOR = "-";
            public const string CSS_CLASS_PREFIX = CSS_CLASS + SEPARATOR; // "nb-"
            public const string ICON_CLASS_PREFIX = CSS_CLASS_PREFIX + "icon"; // "nb-icon"
            public const string MESSAGE_CLASS = CSS_CLASS_PREFIX + "message"; // "nb-message"
            public const string MESSAGE_BODY_CLASS = CSS_CLASS_PREFIX + "body"; // "nb-body"
        }

        #endregion

        #region Fields

        private static readonly IDictionary<NoticeBoxIconSize, string> _wrapperSizeCssClasses;
        private static readonly IDictionary<NoticeBoxType, string> _wrapperTypeCssClasses;
        private static readonly IDictionary<NoticeBoxIconSize, string> _iconSizeCssClasses;

        private string _header;
        private string _message;

        #endregion

        #region Properties

        public string Header
        {
            get { return (_header ?? string.Empty); }
            set { _header = value; }
        }

        public NoticeBoxIconSize IconSize { get; set; }

        public string Message
        {
            get { return (_message ?? string.Empty); }
            set { _message = value; }
        }

        public NoticeBoxType NoticeType { get; set; }

        #endregion

        #region Constructors

        static NoticeBox()
        {
            _wrapperSizeCssClasses = new Dictionary<NoticeBoxIconSize, string>();
            _wrapperSizeCssClasses[NoticeBoxIconSize.Large] = BASE.CSS_CLASS_PREFIX + "large"; // "nb-large"

            _wrapperTypeCssClasses = new Dictionary<NoticeBoxType, string>();
            _wrapperTypeCssClasses[NoticeBoxType.Question] = BASE.CSS_CLASS_PREFIX + "question"; // "nb-question"

            _iconSizeCssClasses = new Dictionary<NoticeBoxIconSize, string>();
            _iconSizeCssClasses[NoticeBoxIconSize.Large] =
                BASE.ICON_CLASS_PREFIX + BASE.SEPARATOR + "large"; // nb-icon-large
        }

        #endregion

        #region Private Methods

        protected override void Render(HtmlTextWriter writer)
        {
            // No base render.

            if (!string.IsNullOrWhiteSpace(ID))
            {
                writer.AddAttribute("id", ClientID);
            }

            writer.AddAttribute("class", GetWrapperCssClass());

            var style = this.Attributes["style"];
            if (!string.IsNullOrWhiteSpace(style))
            {
                writer.AddAttribute("style", style);
            }

            // Starts our control's wrapper render.
            writer.RenderBeginTag("div");

            // Render icon html
            writer.Write(string.Format(@"<div class=""{0}""></div>", GetIconCssClass()));

            // Render opening for message wrapper.
            const string messageWrapper = "<div class=\"" + BASE.MESSAGE_CLASS + "\">";
            writer.Write(messageWrapper);

            // render header if one is set. Otherwise we don't want the h3 tag taking up space.
            if (!string.IsNullOrWhiteSpace(Header))
            {
                writer.Write(string.Format(@"<h3>{0}</h3>", HttpUtility.HtmlEncode(Header)));
            }

            // Render the wrapper for the message body. This
            // holds anything in the Message property or any child
            // controls we're given.
            const string messageBodyWrap = "<div class=\"" + BASE.MESSAGE_BODY_CLASS + "\">";
            writer.Write(messageBodyWrap);

            // Write out message if we have one.
            if (!string.IsNullOrWhiteSpace(Message))
            {
                writer.Write(string.Format(@"<p>{0}</p>", HttpUtility.HtmlEncode(Message)));
            }

            // render out our child controls.
            RenderChildren(writer);

            // Close out the message body div.
            writer.Write("</div>");

            // Close out the message wrapper div.
            writer.Write("</div>");

            // Close out our main wrapper.
            writer.RenderEndTag();
        }

        #region Rendering stuff

        private string GetWrapperCssClass()
        {
            const string wrapperFormatNoCustomClass = "{0} {1} {2}";
            const string wrapperFormatWithCustomClass = "{0} {1} {2} {3}";

            if (string.IsNullOrWhiteSpace(CssClass))
            {
                return string.Format(wrapperFormatNoCustomClass,
                    BASE.CSS_CLASS,
                    _wrapperSizeCssClasses[IconSize],
                    _wrapperTypeCssClasses[NoticeType]);
            }
            else
            {
                return string.Format(wrapperFormatWithCustomClass,
                    BASE.CSS_CLASS,
                    _wrapperSizeCssClasses[IconSize],
                    _wrapperTypeCssClasses[NoticeType],
                    CssClass);
            }
        }

        private string GetIconCssClass()
        {
            const string iconFormat = "{0} {1}";
            return string.Format(iconFormat,
                BASE.ICON_CLASS_PREFIX,
                _iconSizeCssClasses[IconSize]);
        }

        #endregion

        #endregion
    }
}
