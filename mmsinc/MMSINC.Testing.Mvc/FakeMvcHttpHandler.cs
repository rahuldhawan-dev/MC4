using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MMSINC.Testing
{
    /// <summary>
    /// Pre-built instance of a simulated request that would be entirely processed by MVC. Mock objects for the HttpContext,
    /// Request, Response, etc are all created automatically to reduce the need to write these things over and over again.
    /// </summary>
    public class FakeMvcHttpHandler : MvcHttpHandler
    {
        #region Fields

        private Lazy<UrlHelper> _urlHelper;
        private RouteContext _routeContext;

        private string _applicationPath,
                       _httpMethod,
                       _requestVirtualPath,
                       _requestVirtualPathWithoutQueryString;

        private Uri _requestUrl;
        private readonly IContainer _container;

        #endregion

        #region Properties

        // HttpContext
        public Mock<HttpContextBase> HttpContext { get; private set; }
        public Hashtable HttpContextItems { get; private set; }

        // HttpRequest
        public RequestContext RequestContext { get; private set; }
        public Mock<HttpRequestBase> Request { get; private set; }
        public HttpCookieCollection RequestCookies { get; private set; }
        public NameValueCollection RequestForm { get; private set; }
        public NameValueCollection RequestHeaders { get; private set; }
        public NameValueCollection RequestQueryString { get; private set; }
        public NameValueCollection RequestServerVariables { get; private set; }

        // HttpResponse
        public Mock<HttpResponseBase> Response { get; private set; }
        public Mock<HttpCachePolicyBase> ResponseCache { get; private set; }
        public HttpCookieCollection ResponseCookies { get; private set; }
        public MemoryStream ResponseOutputStream { get; private set; }

        public RouteContext RouteContext
        {
            get
            {
                if (_routeContext == null)
                {
                    _routeContext = new RouteContext(RequestContext);
                }

                return _routeContext;
            }
        }

        public RouteData RouteData { get; private set; }
        public Mock<HttpServerUtilityBase> Server { get; private set; }
        public Mock<HttpSessionStateBase> Session { get; private set; }
        public TempDataDictionary TempData { get; private set; }

        public UrlHelper UrlHelper
        {
            get { return _urlHelper.Value; }
        }

        /// <summary>
        /// Returns the mock used for HttpContext.User
        /// </summary>
        public Mock<IPrincipal> User { get; private set; }

        public Mock<IIdentity> UserIdentity { get; private set; }

        /// <summary>
        /// Returns true if the currently setup user is considered authorized/authenticated
        /// for this request.
        /// </summary>
        public bool IsAuthorized
        {
            get { return RouteContext.IsAuthorized(); }
        }

        #endregion

        #region Constructors

        private FakeMvcHttpHandler()
        {
            InitLazies();
            ResponseCache = new Mock<HttpCachePolicyBase>();
            ResponseCookies = new HttpCookieCollection();
            RequestCookies = new HttpCookieCollection();
            RequestForm = new NameValueCollection();
            RequestHeaders = new NameValueCollection();
            RequestQueryString = new NameValueCollection();
            RequestServerVariables = new NameValueCollection();
            HttpContext = new Mock<HttpContextBase>();
            Request = new Mock<HttpRequestBase>();
            Response = new Mock<HttpResponseBase>();
            ResponseOutputStream = new MemoryStream();
            Server = new Mock<HttpServerUtilityBase>();
            Session = new Mock<HttpSessionStateBase>();
            TempData = new TempDataDictionary();
            User = new Mock<IPrincipal>();
            UserIdentity = new Mock<IIdentity>();
            HttpContextItems = new Hashtable();
            InitMocks();
        }

        /// <summary>
        /// Initializes this FakeMvcHandler instance to use a specific url. Use this when using the empty constructor.
        /// STRONGLY RECOMMENDED TO USE NAMED PARAMETERS CAUSE THERE'S A LOT OF STRINGS
        /// </summary>
        /// <param name="hostName">www.domain.com</param>
        /// <param name="appPath">/somesubdir/</param>
        /// <param name="requestPath">~/some/controller/action</param>
        /// <param name="httpMethod">GET/POST</param>
        /// <param name="urlProtocol">HTTP/HTTPS</param>
        /// <param name="port">Wine</param>
        /// <param name="throwIfRouteDataIsMissing">Set this to false if you're gonna test calling Process/ProcessRequest on a route that will not exist.</param>
        /// <param name="routes">By default, the base UrlRoutingHandler class uses RouteTable.Routes if the RouteCollection property is set to null. </param>
        public FakeMvcHttpHandler(IContainer container, string hostName = "localhost", string appPath = "/",
            string requestPath = "~/",
            string httpMethod = "GET", string urlProtocol = "HTTP", int port = -1,
            bool throwIfRouteDataIsMissing = true, RouteCollection routes = null)
            : this()
        {
            _container = container;
            InitRequest(hostName: hostName,
                appPath: appPath,
                requestPath: requestPath,
                httpMethod: httpMethod,
                urlProtocol: urlProtocol,
                port: port,
                throwIfRouteDataIsMissing: throwIfRouteDataIsMissing,
                routes: routes);

            //// Default RouteData because a lot of tests just require that these values
            //// exist but don't actually care what the values are. Though if requestPath
            //// is not null, we should try creating valid RouteData if possible.
        }

        #endregion

        #region Private Methods

        private void InitLazies()
        {
            _urlHelper = new Lazy<UrlHelper>(() => new UrlHelper(RequestContext, RouteCollection));
        }

        private void InitMocks()
        {
            User.Setup(x => x.Identity).Returns(() => UserIdentity.Object);
            HttpContext.Setup(x => x.Request).Returns(() => Request.Object);
            HttpContext.Setup(x => x.Response).Returns(() => Response.Object);
            HttpContext.Setup(x => x.Session).Returns(() => Session.Object);
            HttpContext.Setup(x => x.Items).Returns(() => HttpContextItems);
            HttpContext.Setup(x => x.Server).Returns(() => Server.Object);
            HttpContext.Setup(x => x.User).Returns(() => User.Object);

            Response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => r);
            Response.Setup(x => x.Cache).Returns(() => ResponseCache.Object);
            Response.Setup(x => x.Cookies).Returns(() => ResponseCookies);
            Response.Setup(x => x.OutputStream).Returns(() => ResponseOutputStream);

            Request.Setup(x => x.ApplicationPath).Returns(() => _applicationPath);
            Request.Setup(x => x.AppRelativeCurrentExecutionFilePath)
                   .Returns(() => _requestVirtualPathWithoutQueryString);
            Request.Setup(x => x.Cookies).Returns(() => RequestCookies);
            Request.Setup(x => x.Form).Returns(() => RequestForm);
            Request.Setup(x => x.Headers).Returns(() => RequestHeaders);
            Request.Setup(x => x.HttpMethod).Returns(() => _httpMethod);
            Request.Setup(x => x.QueryString).Returns(() => RequestQueryString);
            Request.Setup(x => x.RawUrl).Returns(() => _requestVirtualPath);
            Request.Setup(x => x.RequestContext).Returns(() => RequestContext);
            Request.Setup(x => x.PathInfo).Returns(() => string.Empty);
            Request.Setup(x => x.ServerVariables).Returns(() => RequestServerVariables);
            Request.Setup(x => x.Url).Returns(() => _requestUrl);

            var browser = new Mock<HttpBrowserCapabilitiesBase>();
            Request.Setup(x => x.Browser).Returns(browser.Object);
        }

        protected override void VerifyAndProcessRequest(IHttpHandler httpHandler, HttpContextBase httpContext)
        {
            // This method is stupid. Internally, there's an MvcHttpHandler(what we're faking) and an MvcHandler.
            // The MvcHttpHandler is called and creates HttpContextWrapper of the current actual HttpContext.
            // Then it just passes HttpContext.Current to the handler, where the handler makes a new HttpContextWrapper.
            // It hurts my brain.

            if (httpHandler is MvcHandler)
            {
                var processRequestMethod = typeof(MvcHandler).GetMethod("ProcessRequest",
                    BindingFlags.Instance | BindingFlags.NonPublic, null, CallingConventions.Any,
                    new[] {typeof(HttpContextBase)},
                    null);
                processRequestMethod.Invoke(httpHandler, new object[] {httpContext});
            }
            else
            {
                base.VerifyAndProcessRequest(httpHandler, httpContext);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes this FakeMvcHandler instance to use a specific url. Use this when using the empty constructor.
        /// Not recommended to be used in trying to do a second request with the same FakeMvcHandler instance.
        /// STRONGLY RECOMMENDED TO USE NAMED PARAMETERS CAUSE THERE'S A LOT OF STRINGS
        /// </summary>
        /// <param name="hostName">www.domain.com</param>
        /// <param name="appPath">/somesubdir/</param>
        /// <param name="requestPath">~/some/controller/action</param>
        /// <param name="httpMethod">GET/POST</param>
        /// <param name="urlProtocol">HTTP/HTTPS</param>
        /// <param name="port">Wine</param>
        /// <param name="throwIfRouteDataIsMissing">Set this to false if you're gonna test calling Process/ProcessRequest on a route that will not exist.</param>
        /// <param name="routes">By default, the base UrlRoutingHandler class uses RouteTable.Routes if the RouteCollection property is set to null. </param>
        public void InitRequest(string hostName = "localhost", string appPath = "/", string requestPath = "~/",
            string httpMethod = "GET", string urlProtocol = "HTTP", int port = -1,
            bool throwIfRouteDataIsMissing = true, RouteCollection routes = null)
        {
            if (routes == null)
            {
                // This is here so we can make standalone FakeMvcHttpHandler instances and 
                // either customize the routes or have it work by default. MvcApplicationTester
                // will always set this.
                routes = new RouteCollection();
                FakeMvcApplication.MapTypicalDefaultRoute(routes);
            }

            if (routes.Count == 0)
            {
                throw new Exception(
                    "Unable to initialize request because the RouteCollection used for this test doesn't have any routes registered");
            }

            RouteCollection = routes;

            _httpMethod = httpMethod;
            _applicationPath = (appPath ?? "/").Replace("//", "/");
            _requestVirtualPath = (requestPath ?? "~/").Replace("//", "/"); // Not sure if ~/ is supposed to be null.

            if (!_requestVirtualPath.StartsWith("~/"))
            {
                throw ExceptionHelper.Format<Exception>(
                    "Request path must be a virtual path that starts with \"~/\". This one starts with \"{0}\"",
                    _requestVirtualPath);
            }

            // Did you know that MVC routing will start failing in all sorts of 
            // random places when you have two slashes? So kill them dead.
            _requestVirtualPathWithoutQueryString = _requestVirtualPath.Split('?')[0];
            var relativeReqPath = _requestVirtualPath.Replace("~/", "");
            var relativeUrlPath = (_applicationPath + relativeReqPath).Replace(@"//", "/");

            var uriBuilder = new UriBuilder(urlProtocol, hostName);

            // Check for querystring, because the UrlBuilder's Path property
            // will replace the ? with a uri escaped version instead of properly
            // parsing out the querystring.
            if (relativeUrlPath.Contains("?"))
            {
                var pathQuerySplit = relativeUrlPath.Split('?');
                uriBuilder.Path = pathQuerySplit[0];
                uriBuilder.Query = pathQuerySplit[1];
            }
            else
            {
                uriBuilder.Path = relativeUrlPath;
            }

            if (port >= 0)
            {
                uriBuilder.Port = port;
            }

            _requestUrl = uriBuilder.Uri;

            RequestQueryString = HttpUtility.ParseQueryString(_requestUrl.Query);

            RouteData = RouteCollection.GetRouteData(HttpContext.Object);

            if (RouteData == null)
            {
                throw ExceptionHelper.Format<Exception>(
                    "Unable to create RouteData for '{0}'. It wasn't created. You should probably ensure your route and/or url are correct.",
                    _requestUrl);
            }

            RequestContext = new RequestContext(HttpContext.Object, RouteData);
        }

        /// <summary>
        /// Calls IHttpHandler.ProcessRequest for the mock HttpContext instance.
        /// </summary>
        public void Process()
        {
            ProcessRequest(HttpContext.Object);
        }

        #region Controllers

        public T CreateAndInitializeController<T>()
            where T : ControllerBase
        {
            var controller = _container.GetInstance<T>();
            InitializeController(controller);
            controller.ControllerContext = CreateControllerContext(controller);
            return controller;
        }

        /// <summary>
        /// Initializes a controller so that its ControllerContext is setup with this fake request.
        /// </summary>
        /// <param name="controller"></param>
        public void InitializeController(ControllerBase controller)
        {
            controller.ControllerContext = CreateControllerContext(controller);
            controller.TempData = TempData;

            if (controller is Controller)
            {
                var mvcController = (Controller)controller;
                mvcController.Url = UrlHelper;
            }
        }

        public ControllerContext CreateControllerContext(ControllerBase controller)
        {
            return new ControllerContext(RequestContext, controller);
        }

        #endregion

        #region CreateHtmlHelper

        /// <summary>
        /// Creates an HtmlHelper object for a model for this faked request.
        /// </summary>
        public HtmlHelper<T> CreateHtmlHelper<T>(T model = null) where T : class
        {
            return CreateHtmlHelper(null, null, model);
        }

        /// <summary>
        /// Creates an HtmlHelper object for a model for this faked request.
        /// </summary>
        public HtmlHelper<T> CreateHtmlHelper<T>(ViewDataDictionary vdd, T model) where T : class
        {
            return CreateHtmlHelper(null, vdd, model);
        }

        /// <summary>
        /// Creates an HtmlHelper object for a model for this faked request and also sets up the 
        /// Controller for the ViewContext.
        /// </summary>
        public HtmlHelper<T> CreateHtmlHelper<T>(ControllerBase controller, T model = null) where T : class
        {
            return CreateHtmlHelper(controller, null, model);
        }

        /// <summary>
        /// Creates an HtmlHelper object for a model for this faked request and also sets up the 
        /// Controller for the ViewContext.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="vdd">ViewDataDictionary to be used for both the Helper and the ViewDataContainer. 
        /// This is important as ViewDataDictionaries get copied by MVC instead of passing the same instance
        /// around.</param>
        /// <param name="model"></param>
        public HtmlHelper<T> CreateHtmlHelper<T>(ControllerBase controller, ViewDataDictionary vdd, T model = null)
            where T : class
        {
            if (vdd == null)
            {
                // This needs to be ViewDataDictionary<T> due to some HtmlHelper extensions requiring it.
                vdd = new ViewDataDictionary<T>(model);
                // This needs to be ModelMetadataProviders.Current so that it's done the same way whatever ApplicationTester
                // is setup to use. And if the metadata isn't set, then tests using ModelMetadata.FromStringExpression will 
                // fail.
                var metaProv = ModelMetadataProviders.Current ?? new EmptyModelMetadataProvider();
                vdd.ModelMetadata = metaProv.GetMetadataForType(() => model, typeof(T));
            }

            var view = new Mock<IView>();

            var viewContext = new ViewContext {
                HttpContext = HttpContext.Object,
                // FormContext has to be created after HttpContext, or else the
                // property setter throws a NotImplementedException.
                FormContext = new FakeFormContext(),
                RouteData = RouteData,
                ViewData = vdd,
                View = view.Object,
                Controller = controller,
                TempData = TempData,
                Writer = new StringWriter()
            };

            var mockVdc = new Mock<IViewDataContainer>();
            mockVdc.Setup(vdc => vdc.ViewData).Returns(vdd);

            var htmlHelper = new HtmlHelper<T>(viewContext, mockVdc.Object, RouteCollection);
            htmlHelper.EnableClientValidation(true);
            htmlHelper.EnableUnobtrusiveJavaScript(true);
            return htmlHelper;
        }

        #endregion

        #endregion
    }
}
