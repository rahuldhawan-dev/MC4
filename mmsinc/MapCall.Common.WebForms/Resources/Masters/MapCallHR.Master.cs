using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Menu = MapCall.Common.Resources.Controls.Menu.Menu;

namespace MapCall.Common.Resources.Masters
{
    public class MapCallHR : MapCallBase
    {
        #region Constants

        public const string ACCESS_DENIED = "Root Access Denied<br />";

        public struct QueryStringParameters
        {
            public const string HIDE_MENU = "hideMenu";
        }

        #endregion

        #region Control declarations

        protected ContentPlaceHolder cphHeadTagScripts;
        protected ContentPlaceHolder cphHeadTag;
        protected HtmlForm form1;
        protected ToolkitScriptManager ToolkitScriptManager1;
        protected Menu navMenu;
        protected ContentPlaceHolder cphHeader;
        protected ContentPlaceHolder cphInstructions;
        protected ContentPlaceHolder cphMain;

        #endregion

        #region Properties

        public bool IsMenuVisible
        {
            get { return navMenu.Visible; }
        }

        #endregion

        #region Exposed methods

        // This gets inlined by the template. 
        public string GetHeaderLinkUrl()
        {
            var reqUrl = IRequest.Url;

            return reqUrl.Contains('?') ? reqUrl.Split('?').First() : reqUrl;
        }

        public void ToggleMenu(bool visible)
        {
            if (navMenu != null)
            {
                navMenu.Visible = visible;
            }
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            if (IRequest.IQueryString[QueryStringParameters.HIDE_MENU] == "true")
                ToggleMenu(false);
        }

        #endregion
    }
}
