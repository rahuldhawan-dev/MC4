using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Metadata;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Represents all the nice things you could ever wanna know about a route. 
    /// </summary>
    public class RouteContext
    {
        #region Fields

        private static readonly ControllerNameAttribute _defaultControllerNameAttr = new ControllerNameAttribute();

        #endregion

        #region Properties

        /// <summary>
        /// Returns the Area that this route belongs to. Returns null
        /// if the controller does not belong to any area.
        /// </summary>
        public string AreaName
        {
            get { return (string)RouteData.DataTokens["area"]; }
        }

        /// <summary>
        /// Returns the controller name as created by the controller's ControllerNameAttribute.
        /// </summary>
        public string ControllerName { get; private set; }

        /// <summary>
        /// Returns the controller name value from the route. Use this if you're generating a
        /// url from values in here.
        /// </summary>
        /// <remarks>
        /// RouteData.Values["controller"] returns the controller name as it appears in
        /// the url, not as it's registered with the site. So you'll need to use case
        /// insensitive equality checking against this usually.
        /// </remarks>
        public string RouteControllerName { get; private set; }

        /// <summary>
        /// Returns the properly-cased controller name from the ActionDescriptor.
        /// </summary>
        /// <remarks>
        /// Things to know about how MVC actually gets the ActionName for
        /// an ActionDescriptor:
        /// 
        /// RouteData.Values["action"] will always return the action name as it is typed
        /// in the url. ie: /Action will give you a result of "Action", /ACTION will give
        /// you "ACTION".
        /// 
        /// HOWEVER
        /// 
        /// If the url matches for one of the default values for one of the mapped Routes,
        /// then the RouteData will return the mapped default value, not the url value.
        /// 
        /// In otherwords, if you visit the url /Home/index and you have this route mapped:
        /// routes.MapRoute("Default", "{controller}/{action}", new { controller = "Home", action = "INDEX" });
        /// 
        /// Then RouteData.Values["action"] will return "INDEX" and not "index".
        ///
        /// So long story short, this is why the ActionDescriptor's MethodInfo.Name
        /// is compared against the ActionName. Also keep in mind that action names do not
        /// have to be defined by a matching method of the same name.
        /// </remarks>
        public string ActionName { get; private set; }

        public ReflectedActionDescriptor ActionDescriptor { get; private set; }

        /// <summary>
        /// Gets/sets a ControllerContext for this route. 
        /// NOTE: The ControllerContext.Controller property does not return the same
        /// Controller instance that is being used for the current requests.
        /// </summary>
        public ControllerContext ControllerContext { get; private set; }

        public ReflectedControllerDescriptor ControllerDescriptor { get; private set; }

        /// <summary>
        /// Returns a RequestContext for this RouteContext. Depending on the constructor
        /// you call, this may be the same one passed in or it may be a new RequestContext.
        /// </summary>
        public RequestContext RequestContext { get; private set; }

        /// <summary>
        /// Returns the RouteData for this route.
        /// </summary>
        public RouteData RouteData
        {
            get { return RequestContext.RouteData; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new RouteContext object for a given Request.
        /// </summary>
        public RouteContext(RequestContext requestContext)
        {
            requestContext.ThrowIfNull("requestContext");
            RequestContext = requestContext;
            Init();
        }

        public RouteContext(RequestContext requestContext, string url)
        {
            requestContext.ThrowIfNull("requestContext");
            RequestContext = CreateRequestContextFromUrl(requestContext, url);
            Init();
        }

        /// <summary>
        /// Creates a new RouteContext for a controller and action in an area, unrelated to the current request.
        /// </summary>
        public RouteContext(RequestContext requestContext, [AspMvcController] string controllerName,
            [AspMvcAction] string actionName, [AspMvcArea] string areaName)
        {
            // TODO: Need to add another parameter that takes in an area value. REQUIRED VALUE
            requestContext.ThrowIfNull("requestContext");
            controllerName.ThrowIfNullOrWhiteSpace("controllerName");
            actionName.ThrowIfNullOrWhiteSpace("actionName");
            // areaName is nullable

            RequestContext = CreateNewRequestContext(requestContext, controllerName, actionName, areaName);
            Init();
        }

        /// <summary>
        /// Creates a new RouteContext for an existing ControllerContext instance. Use this
        /// if you're trying to get route information for an exising request.
        /// </summary>
        public RouteContext(ControllerContext controllerContext)
            : this(controllerContext.RequestContext)
        {
            controllerContext.ThrowIfNull("controllerContext");
            controllerContext.RequestContext.ThrowIfNull("requestContext");
            RequestContext = controllerContext.RequestContext;
            InitWithControllerContext(controllerContext);
        }

        #endregion

        #region Private Methods

        private void Init()
        {
            // GetRequiredString will throw a useful exception for us.
            RouteControllerName = RouteData.GetRequiredString("controller");

            var controller = CreateControllerByName(RequestContext, RouteControllerName);
            InitWithControllerContext(controller.ControllerContext);
        }

        private void InitWithControllerContext(ControllerContext controllerContext)
        {
            ControllerContext = controllerContext;
            ControllerDescriptor = ControllerContext.Controller.GetReflectedControllerDescriptor();
            ControllerName = GetControllerName(ControllerDescriptor.ControllerType);

            // ActionDescriptors can't be cached easily(not by name, anyway) because an action that has the
            // same name but with different parameters would get screwed up. Explains why getting an
            // ActionDescriptor in the first place requires a ControllerContext to figure that jazz out.
            var actionName = GetRequiredActionName();
            ActionDescriptor = GetActionDescriptor(ControllerDescriptor, ControllerContext, actionName);

            // See the <remarks> section for ActionDescriptor provider to understand
            // why this is here.
            if (ActionDescriptor.ActionName.Equals(ActionDescriptor.MethodInfo.Name,
                StringComparison.InvariantCultureIgnoreCase))
            {
                // We want the expected "proper" case for the action name
                ActionName = ActionDescriptor.MethodInfo.Name;
            }
            else
            {
                ActionName = ActionDescriptor.ActionName;
            }
        }

        private string GetRequiredActionName()
        {
            // Don't call GetRequiredString here as it will throw an exception that
            // we will later throw ourselves.
            var actionName = (string)RouteData.Values["action"];
            if (string.IsNullOrWhiteSpace(actionName))
            {
                // When using AttributeRoutes, for whatever reason the action information
                // gets buried in this weird IEnumerable. This key will not exist at all
                // if there isn't an attribute route.
                if (RouteData.Values.ContainsKey("MS_DirectRouteMatches"))
                {
                    // Sometimes(or maybe all of the time, I don't know) there are multiple values in here.
                    // They seem to be duplicates, I don't know if that's true all of the time. 
                    var attributeRouteData =
                        ((IEnumerable<RouteData>)RouteData.Values["MS_DirectRouteMatches"]).First();
                    actionName = attributeRouteData.GetRequiredString("action");
                }
            }

            if (string.IsNullOrWhiteSpace(actionName))
            {
                throw new InvalidOperationException("RouteData must have an action with a string value.");
            }

            return actionName;
        }

        private static string GetControllerName(Type controllerType)
        {
            var attr = MemberInfoExtensions.GetCustomAttributes<ControllerNameAttribute>(controllerType, true)
                                           .SingleOrDefault() ?? _defaultControllerNameAttr;
            return attr.GetControllerName(controllerType);
        }

        private static RequestContext CreateNewRequestContext(RequestContext existing, string controllerName,
            string actionName, string areaName)
        {
            var urlHelper = new UrlHelper(existing);
            // This is how the MVC internals do it. 
            // null(not empty) is used as a way of saying the controller must exist in the same area as the current request.
            // An empty string(not null) is used as a way of saying that a controller must exist outside of any area.
            // And if the area's set, then a controller must exist in that area. 

            if (areaName == null)
            {
                areaName = (string)existing.RouteData.DataTokens["area"];
            }

            // This will prepopulate the builder with port/host/scheme/junk
            var url = urlHelper.Action(actionName, controllerName, new {area = areaName});
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException(
                    string.Format("Unable to create an action url for area '{0}', controller '{1}', action '{2}'.",
                        areaName, controllerName, actionName));
            }

            return CreateRequestContextFromUrl(existing, url);
        }

        private static RequestContext CreateRequestContextFromUrl(RequestContext existing, string url)
        {
            // see if GetRouteData even works with our mock httpContext
            var returnUri = new Uri(url, UriKind.RelativeOrAbsolute);

            if (!returnUri.IsAbsoluteUri)
            {
                // localhost used just to get absolute URL
                // But it might be bad I dunno. -Ross
                returnUri = new Uri("http://localhost" + returnUri);
            }

            var http = new InternalHttpContext(returnUri, existing.HttpContext);

            // ReSharper disable PossibleNullReferenceException
            if (http.Request.Url.AbsolutePath.Contains("//"))
                // ReSharper restore PossibleNullReferenceException
            {
                var err =
                    string.Format(
                        "The url generated contains double slashes which causes MVC's routing engine to choke. Url: {0}",
                        http.Request.Url);
                throw new InvalidOperationException(err);
            }

            var routeData = RouteTable.Routes.GetRouteData(http);

            if (routeData == null)
            {
                throw new InvalidOperationException("Unable to create route data from the generated url: " + returnUri +
                                                    ", Is the url missing an area?");
            }

            return new RequestContext(http, routeData);
        }

        /// <summary>
        /// Creates a new Controller instance that can be used to create a ControllerContext.
        /// </summary>
        private static ControllerBase CreateControllerByName(RequestContext requestContext, string controllerName)
        {
            controllerName = controllerName.Trim();

            // TODO: It'd be nice to get this working with ObjectFactory, but it always throws an exception
            //       when running the site.
            var factory = ControllerBuilder.Current.GetControllerFactory();

            ControllerBase controller;

            // NOTE: CreateController throws an HttpException if it can't find a controller. This can lead to bizarre
            // 404 errors on valid requests when trying to create a RouteContext for a different url(like when creating
            // a cascading dropdown). The TryGetControllerType method will not have this same result.
            var controllerType = TryGetControllerTypeByName(factory, requestContext, controllerName);

            if (controllerType == null)
            {
                throw new InvalidOperationException(string.Format(
                    "Unable to create a controller for '{0}'. This will happen because your controller does not exist, " +
                    "is in the wrong namespace, or the area supplied is invalid.", controllerName));
            }

            // TODO: See if this CreateController part needs to be in a try catch anymore since the line above should handle anything.
            try
            {
                // NOTE: DefaultControllerFactory never returns null, it only throws.
                controller = (ControllerBase)factory.CreateController(requestContext, controllerName);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    string.Format("Unable to create a controller for '{0}'. See inner exception.", controllerName), ex);
            }

            // ControllerContext is null when a controller is initially created with the ControllerFactory,
            // and we usually need a ControllerContext afterwards, so create one here.
            controller.ControllerContext = new ControllerContext(requestContext, controller);

            return controller;
        }

        private static Type TryGetControllerTypeByName(IControllerFactory factory, RequestContext requestContext,
            string controllerName)
        {
            // This is a smell because GetControllerType is a protected method on DefaultControllerFactory. A method with the same
            // signature needs to exist on any main ControllerFactory we use that does not inherit from DefaultControllerFactory. 
            // ie: I had to add this method to CompositeControllerFactory.
            var method = factory.GetType()
                                .GetMethod("GetControllerType", BindingFlags.NonPublic | BindingFlags.Instance);
            var neat = method.Invoke(factory, new object[] {requestContext, controllerName});

            return (Type)neat;
        }

        private static ReflectedActionDescriptor GetActionDescriptor(ReflectedControllerDescriptor controllerDescriptor,
            ControllerContext controllerContext, string actionName)
        {
            // Unlike ControllerFactory, FindAction *will* return null if something doesn't exist.
            // This should always be a ReflectedActionDescriptor since it's coming from 
            // a ReflectedControllerDescriptor.

            actionName = actionName.Trim();

            var actionDescriptor =
                (ReflectedActionDescriptor)controllerDescriptor.FindAction(controllerContext, actionName);
            if (actionDescriptor == null)
            {
                // If we need a RouteContext for a route that is HttpPost and we're HttpGet(or some other verb)
                // then FindAction won't return the correct action. This is the next best thing but doesn't take
                // into consideration other route data. 
                actionDescriptor =
                    controllerDescriptor.FindReflectedActionDescriptor(actionName);
            }

            if (actionDescriptor != null)
            {
                return actionDescriptor;
            }

            // The catch is gonna rethrow this and not offer up much info.
            throw new RouteContextException(string.Format("Unable to find action '{0}' on controller '{1}'.",
                actionName, controllerDescriptor.ControllerName));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns true if the user for the current request is authorized to access this route. 
        /// </summary>
        /// <returns></returns>
        public bool IsAuthorized()
        {
            var authContext = new AuthorizationContext(ControllerContext, ActionDescriptor);
            var filterInfo = new FilterInfo(FilterProviders.Providers.GetFilters(authContext, ActionDescriptor));

            foreach (var authFilter in filterInfo.AuthorizationFilters)
            {
                authFilter.OnAuthorization(authContext);

                // AuthorizationFilters are supposed to set the Result property when authorization fails,
                // so we should be find checking for null here.
                if (authContext.Result != null)
                {
                    return false;
                }
            }

            return true;
        }

        public string GetDisplayControllerName()
        {
            var displayNameAttr = MemberInfoExtensions
                                 .GetCustomAttributes<DisplayNameAttribute>(ControllerDescriptor.ControllerType)
                                 .SingleOrDefault();
            if (displayNameAttr != null)
            {
                return displayNameAttr.DisplayName;
            }

            // RouteContext.ControllerName is not case-sensitive, so pretty text will fail if the
            // text isn't PascalCased. We can check it against the ControllerDescriptor's name 
            // if the controller is handled by DefaultControllerFactory since those names will
            // match. If it's a different controller factory/controller that doesn't end with
            // the name "Controller" then it's on them to have a DisplayNameAttribute to get
            // rendered correctly. 

            var cName = ControllerName;
            if (cName.Equals(ControllerDescriptor.ControllerName,
                StringComparison.InvariantCultureIgnoreCase))
            {
                cName = ControllerDescriptor.ControllerName;
            }

            return HtmlHelperExtensions.PrettyText(cName.Pluralize());
        }

        public override string ToString()
        {
            return RequestContext.GetUrl();
        }

        #endregion

        #region Smelly Code

        private class InternalHttpContext : HttpContextBase
        {
            #region Fields

            private readonly HttpResponseBase _response;
            private readonly HttpRequestBase _request;
            private readonly HttpContextBase _originalHttpContext;

            #endregion

            #region Properties

            // Items prop needed because anything derived from AuthorizeAttribute
            // inevitably calls HttpContext.Items during OnAuthorize.
            public override System.Collections.IDictionary Items
            {
                get { return _originalHttpContext.Items; }
            }

            public override HttpRequestBase Request
            {
                get { return _request; }
            }

            public override HttpResponseBase Response
            {
                get { return _response; }
            }

            public override System.Security.Principal.IPrincipal User
            {
                get { return _originalHttpContext.User; }
                set
                {
                    throw new InvalidOperationException(
                        "The user should not be getting set on an InternalHttpContext instance.");
                }
            }

            #endregion

            #region Constructor

            public InternalHttpContext(Uri uri, HttpContextBase existingHttpContext)
            {
                existingHttpContext.ThrowIfNull("existingHttpContext");
                _originalHttpContext = existingHttpContext;
                _request = new InternalRequestContext(uri, existingHttpContext.Request);

                // TODO: This would be so much better if this worked 
                //       using a regular HttpRequest object so we don't have
                //       to implement all this crap.
                // var req = new HttpRequest(string.Empty, uri.ToString(), uri.Query);
                //_request = new HttpRequestWrapper(req);

                // We need our own Response object because stupid things like the base AuthorizeAttribute
                // will modify the Response when we don't want it doing that. 

                var response = new HttpResponse(new StringWriter());
                _response = new HttpResponseWrapper(response);
            }

            #endregion
        }

        private class InternalRequestContext : HttpRequestBase
        {
            #region Fields

            /// <summary>
            /// This is the actual current HttpRequest for right now for this instance.
            /// </summary>
            private readonly HttpRequestBase _originalRequest;

            private readonly Uri _originalUri;

            private readonly string _appRelativePath,
                                    _appRelativePathWithoutQueryString,
                                    _rawUrl;

            private string _requestType;

            private readonly NameValueCollection _form,
                                                 _queryString;

            #endregion

            #region Properties

            public sealed override string ApplicationPath
            {
                get
                {
                    // Using the original request's AppPath because it will already know
                    // about whether or not it's running in a virtual directory or as site root.
                    return _originalRequest.ApplicationPath;
                }
            }

            public override string AppRelativeCurrentExecutionFilePath
            {
                get { return "~" + _appRelativePathWithoutQueryString; }
            }

            public override NameValueCollection Form
            {
                get { return _form; }
            }

            public override NameValueCollection Headers
            {
                get { return _originalRequest.Headers; }
            }

            public override string HttpMethod
            {
                get { return _originalRequest.HttpMethod; }
            }

            public override bool IsSecureConnection
            {
                get { return _originalRequest.IsSecureConnection; }
            }

            public override bool IsLocal
            {
                get { return _originalRequest.IsLocal; }
            }

            public override string Path
            {
                get { return _appRelativePathWithoutQueryString; }
            }

            // Request.PathInfo is a really stupid property that we don't have to worry about in MVC.
            // http://www.nathanaeljones.com/blog/2008/pathinfo-woes
            public override string PathInfo
            {
                get { return string.Empty; }
            }

            public override NameValueCollection QueryString
            {
                get { return _queryString; }
            }

            public override string RawUrl
            {
                get { return _rawUrl; }
            }

            public override string RequestType { get; set; }

            public override Uri Url
            {
                get { return _originalUri; }
            }

            public override string this[string key]
            {
                get { return Headers[key]; }
            }

            #endregion

            #region Constructor

            public InternalRequestContext(Uri uri, HttpRequestBase originalActualLivingRequest)
            {
                originalActualLivingRequest.ThrowIfNull("originalActualLivingRequest");
                uri.ThrowIfNull("uri");
                _originalUri = uri;
                _originalRequest = originalActualLivingRequest;
                RequestType = originalActualLivingRequest.RequestType;

                var absPath = uri.PathAndQuery;
                _rawUrl = absPath; // According to documentation, RawUrl is the entire url minus the www.domain.com part
                _appRelativePath = GenerateAppRelativePath(absPath, ApplicationPath);
                _appRelativePathWithoutQueryString = _appRelativePath.Split('?').First();

                // I don't see any reason to actually populate this with the current request's data.
                _form = new NameValueCollection();
                _queryString = new NameValueCollection();
                _queryString = HttpUtility.ParseQueryString(_originalUri.Query);
            }

            #endregion

            #region Private Methods

            private static string GenerateAppRelativePath(string absolutePath, string appPath)
            {
                var relative = absolutePath;
                if (!string.IsNullOrEmpty(appPath) && appPath != "/" &&
                    absolutePath.StartsWith(appPath, StringComparison.OrdinalIgnoreCase))
                {
                    relative = absolutePath.Substring(appPath.Length);
                }

                // Need to be wary that we don't end up with double slashes on AppRelativeCurrentExecutionFilePath
                // because it cause the routing engine to go haywire.
                if (!relative.StartsWith("/"))
                {
                    relative = "/" + relative;
                }

                return relative;
            }

            #endregion
        }

        #endregion
    }

    public class RouteContextException : Exception
    {
        #region Constructor

        public RouteContextException(string message) : base(message) { }

        #endregion
    }
}
