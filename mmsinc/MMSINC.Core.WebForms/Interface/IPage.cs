using System;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using MMSINC.Controls;

namespace MMSINC.Interface
{
    /// <summary>
    /// If you need IsPostBack please use MvpUserControl.IsMvpPostBack
    /// </summary>
    public interface IPage : IUserControl, IHttpHandler
    {
        #region Properties

        ClientScriptManager ClientScript { get; }
        bool IsValid { get; }
        EventHandler LoadComplete { get; set; }
        IPrincipal User { get; }
        IRequest IRequest { get; }
        IResponse IResponse { get; }
        IUser IUser { get; }
        IMasterPage IMaster { get; }
        ICache ICache { get; }
        IHtmlHead IHeader { get; }
        ControlCollection Controls { get; }
        bool IsPostBack { get; }

        #endregion

        #region Methods

        void AddHeaderControl(Control toAdd);
        void RegisterRequiresControlState(Control control);
        Control LoadControl(string virtualPath);
        IHttpHandler ToHttpHandler();

        #endregion
    }
}
