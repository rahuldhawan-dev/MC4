using System;
using System.Web.UI;
using MMSINC.Interface;

namespace MMSINC.Controls
{
    public interface IControl
    {
        #region Properties

        string ClientID { get; }
        bool EnableViewState { get; set; }
        string ID { get; set; }
        bool Visible { get; set; }

        #endregion

        #region Exposed Methods

        TControl FindControl<TControl>(string id) where TControl : Control;
        TIControl FindIControl<TIControl>(string id) where TIControl : IControl;
        string ResolveClientUrl(string url);

        [Obsolete(
            "Use the generic version (FindControl<TControl>) or the wrapped interface version (FindIControl<TIControl>) instead.")]
        Control FindControl(string id);

        void DataBind();

        #endregion
    }

    public interface IUserControl : IControl
    {
        #region Properties

        IClientScriptManager ClientScriptManager { get; }
        IServer IServer { get; }
        string AppRelativeVirtualPath { get; }
        IPage IPage { get; }
        ISessionState ISession { get; }
        IRoles IRoles { get; }

        #endregion

        #region Methods

        void AddControl(Control control);

        #endregion
    }
}
