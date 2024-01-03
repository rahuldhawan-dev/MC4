using System.Collections;
using System.Security.Principal;
using System.Web;
using MMSINC.Interface;
using MMSINC.Utilities.Permissions;
using StructureMap;

namespace MMSINC.Common
{
    public class HttpContextWrapper : IHttpContext
    {
        #region Private Members

        protected readonly HttpContext _context;
        protected readonly HttpContextBase _contextBase;
        protected readonly IContainer _container;
        protected IRequest _request;
        protected IResponse _response;
        protected IServer _server;
        protected IUser _user;

        #endregion

        #region Properties

        protected dynamic Context
        {
            get { return (object)_context ?? _contextBase; }
        }

        public IHttpHandler Handler
        {
            get { return Context.Handler; }
            set { Context.Handler = value; }
        }

        public IDictionary Items
        {
            get { return Context.Items; }
        }

        public IRequest Request
        {
            get
            {
                if (_request == null)
                    _request = new RequestWrapper(Context.Request);
                return _request;
            }
        }

        public IResponse Response
        {
            get
            {
                if (_response == null)
                {
                    _response = new ResponseWrapper(Context.Response);
                }

                return _response;
            }
        }

        public IServer Server
        {
            get
            {
                if (_server == null)
                    _server = new ServerWrapper(Context.Server);
                return _server;
            }
        }

        // Context.User must be cast to IPrincipal because container.With will otherwise register it as "dynamic" or the
        // actual object type instead of IPrincipal.
        public IUser User =>
            _user ?? (_user = _container.With((IPrincipal)Context.User).GetInstance<SiteUserWrapper>());

        #endregion

        #region Constructors

        private HttpContextWrapper(IContainer container)
        {
            _container = container;
        }

        public HttpContextWrapper(IContainer container, HttpContext context) : this(container)
        {
            _context = context;
        }

        public HttpContextWrapper(IContainer container, HttpContextBase httpContext) : this(container)
        {
            _contextBase = httpContext;
        }

        #endregion
    }
}
