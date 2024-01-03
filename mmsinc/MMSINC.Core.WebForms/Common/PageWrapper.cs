using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MMSINC.ClassExtensions;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Utilities.Permissions;
using StructureMap;

namespace MMSINC.Common
{
    public class PageWrapper : IPage
    {
        #region Private Members

        private readonly Page _innerPage;
        private IPrincipal _user;
        private IUser _iUser;
        private EventHandler _mockLoadComplete;
        private IRequest _iRequest;
        private IResponse _iResponse;
        private IServer _iServer;
        private IMasterPage _iMasterPage;
        protected IClientScriptManager _iClientScript;
        private ISessionState _iSession;
        private ICache _iCache;
        private IHtmlHead _header;
        protected IRoles _iRoles;

        #endregion

        #region Properties

        public bool IsReusable
        {
            get { return _innerPage.IsReusable; }
        }

        public bool EnableViewState
        {
            get { return _innerPage.EnableViewState; }
            set { _innerPage.EnableViewState = value; }
        }

        public string ID
        {
            get { return _innerPage.ID; }
            set { _innerPage.ID = value; }
        }

        public bool IsValid
        {
            get { return _innerPage.IsValid; }
        }

        public EventHandler LoadComplete
        {
            get { return _mockLoadComplete; }
            set
            {
#if DEBUG
                _mockLoadComplete = value;
#endif
                _innerPage.LoadComplete += value;
            }
        }

        public IPage IPage
        {
            get { return this; }
        }

        public virtual IRoles IRoles
        {
            get
            {
                if (_iRoles == null)
                    _iRoles = new RolesWrapper();
                return _iRoles;
            }
        }

        public ICache ICache
        {
            get
            {
                if (_iCache == null)
                {
                    _iCache = new CacheWrapper(_innerPage.Cache);
                }

                return _iCache;
            }
        }

        public IHtmlHead IHeader
        {
            get
            {
                if (_header == null)
                {
                    _header = new HtmlHeadWrapper(_innerPage.Header);
                }

                return _header;
            }
        }

        public IMasterPage IMaster
        {
            get
            {
                if (_iMasterPage == null)
                    _iMasterPage = new MasterPageWrapper(_innerPage.Master);
                return _iMasterPage;
            }
        }

        public IPrincipal User
        {
            get
            {
                if (_user == null)
                    _user = _innerPage.User;
                return _user;
            }
        }

        public IUser IUser
        {
            get
            {
                //TODO: Get IUser
                if (_iUser == null)
                {
                    _iUser = DependencyResolver.Current.GetService<IContainer>().With(User)
                                               .GetInstance<SiteUserWrapper>();
                }

                return _iUser;
            }
        }

        public IRequest IRequest
        {
            get
            {
                if (_iRequest == null)
                    _iRequest = new RequestWrapper(_innerPage.Request);
                return _iRequest;
            }
        }

        public IResponse IResponse
        {
            get
            {
                if (_iResponse == null)
                    _iResponse = new ResponseWrapper(_innerPage.Response);
                return _iResponse;
            }
        }

        public IServer IServer
        {
            get
            {
                if (_iServer == null)
                    _iServer = new ServerWrapper(_innerPage.Server);
                return _iServer;
            }
        }

        public bool Visible
        {
            get { return _innerPage.Visible; }
            set { _innerPage.Visible = value; }
        }

        public string ClientID
        {
            get { return _innerPage.ClientID; }
        }

        public string AppRelativeVirtualPath
        {
            get { return _innerPage.AppRelativeVirtualPath; }
        }

        public virtual ClientScriptManager ClientScript
        {
            get { return _innerPage.ClientScript; }
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
                    _iClientScript = (_innerPage == null)
                        ? null
                        : new ClientScriptManagerWrapper(
                            ClientScript, this);
                return _iClientScript;
            }
        }

        public virtual ISessionState ISession
        {
            get
            {
                if (_iSession == null)
                    _iSession = new SessionStateWrapper(_innerPage.Session);
                return _iSession;
            }
        }

        public ControlCollection Controls
        {
            get { return _innerPage.Controls; }
        }

        public bool IsPostBack
        {
            get { return _innerPage.IsPostBack; }
        }

        #endregion

        #region Constructors

        public PageWrapper(Page innerPage)
        {
            _innerPage = innerPage;
        }

        #endregion

        #region Exposed Methods

        public void DataBind()
        {
            _innerPage.DataBind();
        }

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions
                   .FindControl<TControl>(_innerPage, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            return (TIControl)(object)_innerPage.FindControl(id);
        }

        public string ResolveClientUrl(string url)
        {
            return _innerPage.ResolveClientUrl(url);
        }

        public Control FindControl(string id)
        {
            return _innerPage.FindControl(id);
        }

        // TODO: Test this.
        public void AddControl(Control control)
        {
            _innerPage.Controls.Add(control);
        }

        // NOTE: LoD violaion.
        // should have a wrapper for header that implements IUserControl,
        // which means it will have an AddControl method, and header will
        // then be injectable for testing.
        public void AddHeaderControl(Control toAdd)
        {
            _innerPage.Header.Controls.Add(toAdd);
        }

        public void RegisterRequiresControlState(Control control)
        {
            _innerPage.RegisterRequiresControlState(control);
        }

        public Control LoadControl(string virtualPath)
        {
            return _innerPage.LoadControl(virtualPath);
        }

        public void ProcessRequest(HttpContext context)
        {
            _innerPage.ProcessRequest(context);
        }

        public IHttpHandler ToHttpHandler()
        {
            return _innerPage;
        }

        #endregion
    }
}
