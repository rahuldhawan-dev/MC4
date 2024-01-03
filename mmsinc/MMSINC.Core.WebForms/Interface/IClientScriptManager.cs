using System.Web.UI;

namespace MMSINC.Interface
{
    public interface IClientScriptManager
    {
        #region Methods

        bool TryRegisterClassScriptInclude();
        bool TryRegisterClientScriptInclude(string key, string url);
        bool ClientScriptIsRegistered(string key);
        bool ClientScriptExists(string url);
        bool CssFileExists(string url);
        bool TryRegisterCssInclude(string url);
        string GetPostBackEventReference(Control control, string id);

        #endregion
    }
}
