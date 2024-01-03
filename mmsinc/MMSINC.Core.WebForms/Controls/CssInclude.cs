using System;
using MMSINC.Common;
using MMSINC.Interface;

namespace MMSINC.Controls
{
    public class CssInclude : MvpUserControl
    {
        #region Constants

        public const string BASE_INCLUDES_PATH = "~/Includes/";

        #endregion

        #region Private Members

        private string _includesPath, _cssFileUrl;

        #endregion

        #region Properties

        public override IClientScriptManager ClientScriptManager
        {
            get { return IParent.ClientScriptManager; }
        }

        public string IncludesPath
        {
            get { return _includesPath ?? BASE_INCLUDES_PATH; }
            set { _includesPath = value; }
        }

        public string CssFileName { get; set; }

        public string CssFileUrl
        {
            get
            {
                if (_cssFileUrl == null)
                    _cssFileUrl = IncludesPath + CssFileName;
                return _cssFileUrl;
            }
        }

        #endregion

        #region Event Handlers

        protected override void CreateChildControls()
        {
            if (ClientScriptManager.CssFileExists(CssFileUrl))
                ClientScriptManager.TryRegisterCssInclude(CssFileUrl);
            else
                throw new ArgumentException(
                    String.Format(
                        "Script file {0} was not found at the path {1}.",
                        CssFileName, IncludesPath), "CssFileName");
        }

        #endregion
    }
}
