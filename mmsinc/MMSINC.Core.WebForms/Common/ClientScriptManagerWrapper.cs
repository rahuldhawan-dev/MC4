using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using MMSINC.Controls;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class ClientScriptManagerWrapper : IClientScriptManager
    {
        #region Constants

        public static readonly Regex CONTROL_EXTENSION_RGX =
            new Regex("as[a-zA-Z]x$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static readonly Regex CLASS_NAME_RGX =
            new Regex(".*/(.+)\\.as[a-zA-Z]x$",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public const string SCRIPT_FILE_EXTENSION = "js";
        public const string SCRIPT_KEY_FORMAT = "{0}_ClientScript";
        public const string CLASS_INIT_SCRIPT_KEY_FORMAT = "{0}_InitScript";

        public const string CLASS_INIT_SCRIPT_FORMAT =
            "if ({0} && {0}.initialize) {{ $(document).ready(function() {{ {0}.initialize(); }}); }}";

        public const string CSS_INCLUDE_FORMAT = "<link rel=\"stylesheet\" href=\"{0}\" />";

        #endregion

        #region Private Members

        private readonly ClientScriptManager _innerClientScriptManager;
        private readonly IUserControl _control;
        private string _className, _classScriptKey, _classScriptUrl, _classScriptMachinePath;
        private bool? _classScriptExists, _classScriptIsRegistered;

        #endregion

        #region Properties

        public IServer IServer
        {
            get { return _control.IServer; }
        }

        public string ClassName
        {
            get
            {
                if (_className == null)
                    _className =
                        CLASS_NAME_RGX.Match(_control.AppRelativeVirtualPath)
                                      .Groups[1].Value;
                return _className;
            }
        }

        public string ClassScriptKey
        {
            get
            {
                if (_classScriptKey == null)
                    _classScriptKey = String.Format(SCRIPT_KEY_FORMAT,
                        ClassName);
                return _classScriptKey;
            }
        }

        public string ClassScriptUrl
        {
            get
            {
                if (_classScriptUrl == null)
                    _classScriptUrl = CONTROL_EXTENSION_RGX.Replace(_control.AppRelativeVirtualPath ?? String.Empty,
                        SCRIPT_FILE_EXTENSION);
                return _classScriptUrl;
            }
        }

        public string ClassScriptMachinePath
        {
            get
            {
                if (_classScriptMachinePath == null && IServer != null)
                    _classScriptMachinePath = IServer.MapPath(ClassScriptUrl);
                return _classScriptMachinePath;
            }
        }

        public bool ClassScriptExists
        {
            get
            {
                if (_classScriptExists == null)
                    _classScriptExists = (ClassScriptMachinePath != null &&
                                          File.Exists(ClassScriptMachinePath));
                return _classScriptExists.Value;
            }
        }

        public bool ClassScriptIsRegistered
        {
            get
            {
                if (_classScriptIsRegistered == null)
                    _classScriptIsRegistered =
                        ClientScriptIsRegistered(ClassScriptKey);
                return _classScriptIsRegistered.Value;
            }
        }

        #endregion

        #region Constructors

        public ClientScriptManagerWrapper(ClientScriptManager scriptManager, IUserControl control)
        {
            _innerClientScriptManager = scriptManager;
            _control = control;
        }

        #endregion

        #region Private Methods

        private void RegisterClientScriptInclude(string key, string url)
        {
            // Use the control's IPage to call ResolveClientUrl instead of
            // from the control itself. ResolveClientUrl doesn't work properly
            // on a non-templated custom control. The control will just
            // return back the same string you passed in to it.
            _innerClientScriptManager.RegisterClientScriptInclude(key,
                _control.IPage.ResolveClientUrl(url));
        }

        private void RegisterClientScriptInitializer(string className)
        {
            _innerClientScriptManager.RegisterClientScriptBlock(
                _control.GetType(),
                String.Format(CLASS_INIT_SCRIPT_KEY_FORMAT, className),
                String.Format(CLASS_INIT_SCRIPT_FORMAT, className),
                true);
        }

        private void RegisterCssInclude(string url)
        {
            // TODO: FIX THIS.
            _control.IPage.AddHeaderControl(
                new LiteralControl(String.Format(CSS_INCLUDE_FORMAT,
                    _control.ResolveClientUrl(url))));
        }

        private bool FileExistsOnServer(string url)
        {
            // VirtualPathProvider considers a virtual path the absolute path. This is the
            // way it's used internally. Seems kind of dumb, but what can you do. 
            var absoluteUrl = VirtualPathUtility.ToAbsolute(url);
            return HostingEnvironment.VirtualPathProvider.FileExists(absoluteUrl);
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Attempts to register the script for the control.
        /// </summary>
        /// <returns>True if script was loaded, else false.</returns>
        public bool TryRegisterClassScriptInclude()
        {
            if (ClassScriptExists && !ClassScriptIsRegistered && ClassName != String.Empty)
            {
                RegisterClientScriptInclude(ClassScriptKey, ClassScriptUrl);
                RegisterClientScriptInitializer(ClassName);
                return true;
            }

            return false;
        }

        public bool TryRegisterClientScriptInclude(string key, string url)
        {
            if (ClientScriptExists(url) && !ClientScriptIsRegistered(key))
            {
                RegisterClientScriptInclude(key, url);
                return true;
            }

            return false;
        }

        public bool TryRegisterCssInclude(string url)
        {
            if (ClientScriptExists(url))
            {
                RegisterCssInclude(url);
                return true;
            }

            return false;
        }

        public string GetPostBackEventReference(Control control, string id)
        {
            return _innerClientScriptManager.GetPostBackEventReference(control,
                id);
        }

        public bool ClientScriptIsRegistered(string key)
        {
            return _innerClientScriptManager.IsClientScriptIncludeRegistered(key);
        }

        public bool ClientScriptExists(string url)
        {
            return FileExistsOnServer(url);
        }

        public bool CssFileExists(string url)
        {
            return FileExistsOnServer(url);
        }

        #endregion
    }
}
