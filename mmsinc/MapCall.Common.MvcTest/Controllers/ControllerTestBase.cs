using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace MapCall.Common.MvcTest.Controllers
{
    public abstract class ControllerTestBase<TController>
        where TController : Controller
    {
        #region Private Members

        protected TController _target;
        protected IContainer _container;

        #endregion

        #region Private Methods

        protected static TController GetController(IContainer container)
        {
            var requestContext = new RequestContext(new MockHttpContext(), new RouteData());
            var controller = container.GetInstance<TController>();
            controller.Url = new UrlHelper(requestContext);
            controller.ControllerContext = new ControllerContext {
                Controller = controller,
                RequestContext = requestContext
            };
            return controller;
        }

        protected void BaseInitialize()
        {
            _container = new Container();
            _target = GetController(_container);
        }

        #endregion

        #region Test Classes

        public class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);
            private readonly HttpRequestBase _request = new MockHttpRequest();

            public override IPrincipal User
            {
                get { return _user; }
                set { base.User = value; }
            }

            public override HttpRequestBase Request
            {
                get { return _request; }
            }
        }

        public class MockHttpRequest : HttpRequestBase
        {
            private readonly Uri _url = new Uri("http://mysite.example.com/");
            private NameValueCollection _serverVariables;

            public override NameValueCollection ServerVariables
            {
                get
                {
                    if (_serverVariables == null)
                    {
                        _serverVariables = new NameValueCollection();
                    }

                    return _serverVariables;
                }
            }

            public override Uri Url
            {
                get { return _url; }
            }
        }

        #endregion
    }
}
