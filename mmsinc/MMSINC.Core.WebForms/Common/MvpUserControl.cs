using System;
using System.Web.UI;
using MMSINC.ClassExtensions;
using MMSINC.Controls;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class MvpUserControl : UserControl, IUserControl
    {
        #region Private Members

        protected bool? _isMvpPostBack;
        protected IUserControl _parent;
        protected IPage _iPage;
        protected IResponse _iResponse;
        protected IRequest _iRequest;
        protected IServer _iServer;
        protected IViewState _iViewState;
        protected IClientScriptManager _iClientScript;
        protected IUser _iUser;
        protected ISessionState _iSession;

        #endregion

        #region Properties

        public virtual string RelativeUrl
        {
            get { return IRequest.RelativeUrl; }
        }

        public virtual bool IsMvpPostBack
        {
            get
            {
                return (_isMvpPostBack == null)
                    ? IsPostBack
                    : _isMvpPostBack.Value;
            }
        }

        public IUserControl IParent
        {
            get
            {
                if (_parent == null)
                    _parent =
                        ControlExtensions
                           .FindIParent(Parent);
                return _parent;
            }
        }

        public virtual IPage IPage
        {
            get
            {
                if (_iPage == null)
                    _iPage = (Page == null) ? null : new PageWrapper(Page);
                return _iPage;
            }
        }

        public virtual IRoles IRoles
        {
            get { return IPage.IRoles; }
        }

        public virtual IResponse IResponse
        {
            get
            {
                if (_iResponse == null)
                    _iResponse = (IPage == null) ? null : IPage.IResponse;
                return _iResponse;
            }
        }

        public virtual IRequest IRequest
        {
            get
            {
                if (_iRequest == null)
                    _iRequest = (IPage == null) ? null : IPage.IRequest;
                return _iRequest;
            }
        }

        public virtual IServer IServer
        {
            get
            {
                if (_iServer == null)
                    _iServer = (IPage == null) ? null : IPage.IServer;
                return _iServer;
            }
        }

        public virtual IViewState IViewState
        {
            get
            {
                if (_iViewState == null)
                    _iViewState = new ViewStateWrapper(ViewState);
                return _iViewState;
            }
        }

        public virtual IClientScriptManager ClientScriptManager
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

        public virtual IUser IUser
        {
            get
            {
                if (_iUser == null)
                    _iUser = IPage.IUser;
                return _iUser;
            }
        }

        public virtual ISessionState ISession
        {
            get
            {
                if (_iSession == null)
                    _iSession = new SessionStateWrapper(Session);
                return _iSession;
            }
        }

        [Obsolete("Use the wrapped (testable) property IPage instead.")]
        public override Page Page
        {
            get { return base.Page; }
            set { base.Page = value; }
        }

        [Obsolete("Use the wrapped (testable) property IViewState instead.")]
        protected override StateBag ViewState
        {
            get { return base.ViewState; }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Page_Prerender in an MvpUserControl will attempt to load a client script
        /// by name and path if one exists on the webserver next to the as*x file.
        /// As such, calling Page_Load on a UserControl that does not exist as an as*x
        /// (pre-compiled UserControls, etc.) will result in a null reference error.
        /// To resolve such errors, simply override Page_Load and _do not_ call base.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_Prerender(object sender, EventArgs e)
        {
            // this was neutered on purpose.  that way tests of base classes
            // don't have to provide anything here if they don't need to.  if
            // they do, they probably need the script manager anyway, so they'd
            // mock it out.
            if (Visible && ClientScriptManager != null)
                ClientScriptManager.TryRegisterClassScriptInclude();
        }

        #endregion

        #region Exposed Methods

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions.FindControl
                    <TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return
                ControlExtensions.FindIControl
                    <TIControl>((Control)this, id);
        }

        // TODO: Test this.
        public void AddControl(Control control)
        {
            Controls.Add(control);
        }

        #endregion
    }
}
