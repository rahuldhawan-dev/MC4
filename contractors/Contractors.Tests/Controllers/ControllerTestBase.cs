using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace Contractors.Tests.Controllers
{
    [Obsolete("I shouldn't be used")]
    public abstract class ControllerTestBase<TController, TRepository>
        where TController : Controller where TRepository : class
    {
        #region Private Members

        protected TController _target;
        protected Mock<TRepository> _mockRepository;
        protected Mock<IAuthenticationService<ContractorUser>> _mockAuthenticationService;
        protected IContainer _container;

        #endregion

        #region Private Methods

        protected TController GetController()
        {
            var requestContext = new RequestContext(new MockHttpContext(), new RouteData());
            var controller = _container.GetInstance<TController>();
            controller.Url = new UrlHelper(requestContext);
            controller.ControllerContext = new ControllerContext {
                Controller = controller,
                RequestContext = requestContext
            };
            return controller;
        }

        protected virtual void InitializeObjectFactory(ConfigurationExpression e)
        {
            _mockRepository = e.For<TRepository>().Mock();
            _mockAuthenticationService =
                e.For<IAuthenticationService<ContractorUser>>().Mock();
        }

        protected void BaseInitialize()
        {
            _container = new Container(InitializeObjectFactory);
            _target = GetController();
        }

        #endregion
    }

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

    internal class MockHttpRequest : HttpRequestBase
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
}
