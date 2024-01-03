using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.UI;
using MMSINC.Common;
using MMSINC.Controls;

namespace MapCall.Controls
{
    [ParseChildren(true, "Tabs")]
    public class TabView : MvpUserControl
    {

        #region Fields

        private string _cssClass;

        #endregion

        #region Properties

        [CssClassProperty]
        public string CssClass
        {
            get
            {
                // Return string.Empty for consistancy with all other ASP controls.
                return _cssClass ?? string.Empty;
            }
            set
            {
                _cssClass = value;
            }
        }
        public Collection<Tab> Tabs { get; private set; }

        #endregion

        #region Constructors

        public TabView()
        {
            Tabs = new Collection<Tab>();
        }

        #endregion

        #region Private Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var c = this.Controls;
            for (var i = 0; i < Tabs.Count; i++)
            {
                c.Add(Tabs[i]);
            }
        }

   
        protected override void Render(HtmlTextWriter writer)
        {
            RenderAddAttribute(writer, "id", ID);

            var css = CssClass;
            // TODO: Come back to this.
            //if (string.IsNullOrEmpty(css))
            //{
            //    css = "tabsContainer";
            //}
            //else if (!css.Contains("tabsContainer"))
            //{
            //    css += " tabsContainer";
            //}

            css = "tabsContainer ui-tabs ui-widget ui-widget-content ui-corner-all";

            RenderAddAttribute(writer, "class", css);

            writer.RenderBeginTag("div");
            RenderTabs(writer);
            base.Render(writer);
            writer.RenderEndTag();
        }

        // Adds an attribute only if the value is not null/empty.
        protected virtual void RenderAddAttribute(HtmlTextWriter writer, string attributeName, string attributeValue)
        {
            if (!string.IsNullOrEmpty(attributeValue))
            {
                writer.AddAttribute(attributeName, attributeValue);
            }
        }

        protected virtual void RenderTabs(HtmlTextWriter writer)
        {
            writer.WriteLine("<ul class=\"ui-tabs-nav ui-helper-reset ui-helper-clearfix ui-widget-header ui-corner-all\">");
            writer.Indent += 1;

            const string tabLabelFormat = "<li class=\"ui-state-default ui-corner-top\"><a href=\"#{0}\" class=\"tab\"><span>{1}</span></a></li>";

            // We don't wanna display the tab at all if it's not visible, since none of its controls will be
            // on the page. 
            foreach (var t in (from tab in Tabs where tab.Visible select tab))
            {
                writer.WriteLine(String.Format(tabLabelFormat, t.ClientID, t.Label));
            }

            writer.Indent -= 1;
            writer.WriteLine("</ul>");

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a child Tab with the given ID. Returns null if there isn't one.
        /// This is the workaround at the moment since adding a Tab to the Tabs collection
        /// doesn't add it to the .designer.cs files for access.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tab FindTabById(string id)
        {
            return (from t in Tabs
                    where t.ID == id
                    select t).FirstOrDefault();
        }

        #endregion


    }


    public class Tab : MvpPanel
    {

        #region Properties

        public string Label { get; set; }

        /// <summary>
        /// Gets/sets whether this tab is the selected one when the page loads. 
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Set this to true when this tab and its children need to be visible when a
        /// page is currently in insert mode. 
        /// </summary>
        public bool VisibleDuringInsert { get; set; }

        /// <summary>
        /// Same as VisibleDuringInsert, just with update mode instead.
        /// </summary>
        public bool VisibleDuringUpdate { get; set; }

        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            const string SELECTED_TAB = "ui-tabs-panel ui-widget-content ui-corner-bottom";
            const string NON_SELECTED_TAB = "ui-tabs-panel ui-widget-content ui-corner-bottom";

            if (string.IsNullOrEmpty(Label))
            {
                throw new InvalidOperationException("Label property must be set for a Tab.");
            }

            EnsureID();
            writer.AddAttribute("id", ClientID);

           // var css = CssClass;
            //TODO: Come back to this
            //if (string.IsNullOrEmpty(css))
            //{
            //    css = "ui-tabs-panel";
            //}
            //else if (!css.Contains("ui-tabs-panel"))
            //{
            //    css += " ui-tabs-panel";
            //}

           var css = (Selected ? SELECTED_TAB : NON_SELECTED_TAB);

            writer.AddAttribute("class", css);
            writer.RenderBeginTag("div");

            RenderContents(writer);
           
            writer.RenderEndTag();
            writer.WriteLine();
        }

    }
}
