using System;
using System.Web;
using System.Web.UI;
using MMSINC.Controls;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class MvpPage : Page, IPage
    {
        #region Private Members

        protected bool? _isMvpPostBack;
        protected IPage _iPage;
        protected IUser _iUser;
        protected IRequest _iRequest;
        protected IResponse _iResponse;
        protected IServer _iServer;
        protected IClientScriptManager _iClientScript;
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

        public virtual IPage IPage
        {
            get
            {
                if (_iPage == null)
                    _iPage = new PageWrapper(this);
                return _iPage;
            }
        }

        public virtual IRoles IRoles
        {
            get { return IPage.IRoles; }
        }

        public virtual IMasterPage IMaster
        {
            get { return IPage.IMaster; }
        }

        public virtual ICache ICache
        {
            get { return IPage.ICache; }
        }

        public virtual IHtmlHead IHeader
        {
            get { return IPage.IHeader; }
        }

        public virtual EventHandler LoadComplete
        {
            get { return IPage.LoadComplete; }
            set { IPage.LoadComplete = value; }
        }

        public virtual IRequest IRequest
        {
            get
            {
                if (_iRequest == null)
                    _iRequest = IPage.IRequest;
                return _iRequest;
            }
        }

        public virtual IResponse IResponse
        {
            get
            {
                if (_iResponse == null)
                    _iResponse = IPage.IResponse;
                return _iResponse;
            }
        }

        public virtual IServer IServer
        {
            get
            {
                if (_iServer == null)
                {
                    try
                    {
                        _iServer = new ServerWrapper(Server);
                    }
                    catch (NullReferenceException e)
                    {
                        _iServer = null;
                    }
                }

                return _iServer;
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
                    _iClientScript = new ClientScriptManagerWrapper(
                        ClientScript, this);
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

        #endregion

        #region Event Handlers

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            // this was neutered on purpose.  that way tests of base classes
            // don't have to provide anything here if they don't need to.  if
            // they do, they probably need the script manager anyway, so they'd
            // mock it out.
            if (ClientScriptManager != null)
                ClientScriptManager.TryRegisterClassScriptInclude();
        }

        #endregion

        #region Exposed Methods

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            throw new NotImplementedException();
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            throw new NotImplementedException();
        }

        // TODO: Test this.
        public void AddControl(Control control)
        {
            Controls.Add(control);
        }

        public void AddHeaderControl(Control toAdd)
        {
            IPage.AddHeaderControl(toAdd);
        }

        public IHttpHandler ToHttpHandler()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
