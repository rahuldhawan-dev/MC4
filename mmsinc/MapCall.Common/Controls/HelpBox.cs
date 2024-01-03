using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI;
using MMSINC.Utilities;

namespace MapCall.Common.Controls
{
    // TODO: Would probably be nice if we could link the HelpBox to an existing control
    //       so we don't end up having lots of duplicate content.

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// ParseChildren(false) is needed so child controls can be added to it.
    /// </remarks>
    [ParseChildren(false)]
    public class HelpBox : UserControl
    {
        public const string BASE_CSS_CLASS = "helpBoxButton";
        public const string INNER_CONTENT_WRAPPER_CSS_CLASS = "content";
        public const string HELP_BOX_CLICK_HANDLER = "jQuery(this).helpBox({0});";

        #region Properties

        /// <summary>
        /// Gets/sets any css classes in addition to the base css class.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Gets/sets the title of the help dialog displays.
        /// </summary>
        public string Title { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        #endregion

        #region Private Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            // No Viewstate!
            ViewStateMode = ViewStateMode.Disabled;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!string.IsNullOrWhiteSpace(ClientID))
            {
                writer.AddAttribute("id", ClientID);
            }

            writer.AddAttribute("class", GetCssClass());
            writer.AddAttribute("type", "button");
            writer.AddAttribute("title", GetTitleText());
            writer.AddAttribute("onclick", GetOnClickHandler());
            writer.RenderBeginTag("button");
            writer.Write("<div class=\"" + INNER_CONTENT_WRAPPER_CSS_CLASS + "\">");
            RenderChildren(writer);
            writer.Write("</div>");
            writer.RenderEndTag();
        }

        private string GetCssClass()
        {
            if (string.IsNullOrWhiteSpace(CssClass))
            {
                return BASE_CSS_CLASS;
            }

            return BASE_CSS_CLASS + " " + CssClass;
        }

        private string GetTitleText()
        {
            if (!string.IsNullOrWhiteSpace(Title))
            {
                return Title;
            }

            return "Additional Help";
        }

        private string GetOnClickHandler()
        {
            return string.Format(HELP_BOX_CLICK_HANDLER, GetJsonArgs());
        }

        private string GetJsonArgs()
        {
            var d = new Dictionary<string, object>();
            if (Width > 0)
            {
                d["width"] = Width;
            }

            if (Height > 0)
            {
                d["height"] = Height;
            }

            var json = JavaScriptSerializerFactory.Build();
            return json.Serialize(d);
        }

        #endregion
    }
}
