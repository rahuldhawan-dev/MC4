using System;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using MapCall.Common.Resources.Masters;

namespace LINQTo271
{
    public partial class WorkOrders : MapCallHR, IUserControl
    {
        #region Constants

        private const string STYLESHEET_REFERENCE_FORMAT =
            "<link rel=\"stylesheet\" href=\"{0}\" />",
                             IE_STYLE_URL = "/Includes/WorkOrdersIE.css";


        #endregion

        #region Private Members

        private IServer _iServer;

        #endregion

        #region Properties

        public override IClientScriptManager ClientScriptManager
        {
            get
            {
                // this was neutered on purpose.  that way tests of base classes
                // don't have to provide anything here if they don't need to.  if
                // they do, they probably need the script manager anyway, so they'd
                // mock it out.
                if (_iClientScript == null)
                    _iClientScript = (IPage == null)
                                         ? null
                                         : new ClientScriptManagerWrapper(
                                               IPage.ClientScript, this);
                return _iClientScript;
            }
        }

        public override IServer IServer
        {
            get
            {
                if (_iServer == null)
                    _iServer = (IPage == null) ? null : IPage.IServer;
                return _iServer;
            }
        }

        #endregion

        #region Private Methods

        private static Literal GetIECssInclude()
        {
            return new Literal {
                Text = String.Format(STYLESHEET_REFERENCE_FORMAT, IE_STYLE_URL)
            };
        }

        private void IncludeExtraCSS()
        {
            if (Request.Browser.VBScript)
            {
                Page.Header.Controls.Add(GetIECssInclude());
            }
        }

        #endregion

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            IncludeExtraCSS();
        }

        #endregion
    }
}
