using System;
using System.Text.RegularExpressions;
using MMSINC.Common;
using MMSINC.Interface;

namespace MMSINC.Controls
{
    public class ScriptInclude : MvpUserControl
    {
        #region Constants

        public const string BASE_INCLUDES_PATH = "~/Includes/",
                            SCRIPT_KEY_FORMAT = "{0}_ScriptInclude";

        public static readonly Regex SCRIPT_EXTENSION_RGX =
            new Regex("\\.js$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region Private Members

        private string _scriptFileUrl, _includesPath, _scriptKey;

        #endregion

        #region Properties

        public override IClientScriptManager ClientScriptManager
        {
            get { return IParent.ClientScriptManager; }
        }

        public string ScriptFileName { get; set; }

        public string IncludesPath
        {
            get { return _includesPath ?? BASE_INCLUDES_PATH; }
            set { _includesPath = value; }
        }

        public string ScriptKey
        {
            get
            {
                if (_scriptKey == null)
                    _scriptKey = String
                       .Format(SCRIPT_KEY_FORMAT,
                            SCRIPT_EXTENSION_RGX.Replace(ScriptFileName,
                                String.Empty));
                return _scriptKey;
            }
        }

        public string ScriptFileUrl
        {
            get
            {
                if (_scriptFileUrl == null)
                    _scriptFileUrl = IncludesPath + ScriptFileName;
                return _scriptFileUrl;
            }
        }

        #endregion

        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ClientScriptManager.ClientScriptExists(ScriptFileUrl))
                ClientScriptManager.TryRegisterClientScriptInclude(ScriptKey,
                    ScriptFileUrl);
            else
                throw new ArgumentException(
                    String.Format(
                        "Script file {0} was not found at the path {1}.",
                        ScriptFileName, IncludesPath), "ScriptFileName");
        }

        #endregion
    }
}
