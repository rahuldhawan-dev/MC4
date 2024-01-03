using System;
using System.Web.Mvc;
using System.Web.Routing;
using JetBrains.Annotations;
using MMSINC.Metadata;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Helpers
{
    public class SecureUrlHelper : UrlHelper
    {
        #region Private Members

        private readonly Func<string, RouteValueDictionary, string> _processUrl;

        #endregion

        #region Properties

        protected virtual Func<string, RouteValueDictionary, string> ProcessUrl
        {
            get { return _processUrl; }
        }

        #endregion

        #region Constructors

        public SecureUrlHelper(RequestContext requestContext) : base(requestContext)
        {
            _processUrl = GetFn();
        }

        public SecureUrlHelper(RequestContext requestContext, RouteCollection routeCollection) : base(requestContext,
            routeCollection)
        {
            _processUrl = GetFn();
        }

        #endregion

        #region Private Methods

        private Func<string, RouteValueDictionary, string> GetFn()
        {
            return FormBuilder.SecureFormsEnabled
                ? SecureUrl
                : (Func<string, RouteValueDictionary, string>)((s, d) => s);
        }

        protected virtual string SecureUrl(string url, RouteValueDictionary routeValues = null)
        {
            RouteContext routeContext;

            if (!FormBuilder.RequiresSecureForm(routeContext = new RouteContext(RequestContext, url)))
            {
                return url;
            }

            var token = DependencyResolver.Current.GetService<ISecureFormTokenService>()
                                          .CreateToken(routeValues, routeContext);

            return string.Format("{0}{1}{2}={3}", url, url.Contains("?") ? "&" : "?",
                FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME, token);
        }

        #endregion

        #region Exposed Methods

        public override string Action([AspMvcAction] string actionName, [AspMvcController] string controllerName,
            RouteValueDictionary routeValues)
        {
            return ProcessUrl(base.Action(actionName, controllerName, routeValues), routeValues);
        }

        public override string Action([AspMvcAction] string actionName, [AspMvcController] string controllerName)
        {
            return ProcessUrl(SecureUrl(base.Action(actionName, controllerName)), null);
        }

        #endregion
    }
}
