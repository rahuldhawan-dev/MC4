using System.Security.Principal;
using log4net;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using MMSINC.Utilities.ActiveMQ;
using Moq;
using NHibernate;
using StructureMap;

namespace MapCallMVC.Tests
{
// ReSharper disable RedundantNameQualifier
    public class MapCallMvcApplicationTester : MvcApplicationTester<MvcApplication>
// ReSharper restore RedundantNameQualifier
    {
        #region Constructors

        public MapCallMvcApplicationTester(IContainer container) : this(container, true) {}

        /// <param name="initializeApplicationInstance">Set to true if the ApplicationInstance should have all of its initializers called in the constructor.</param>
        public MapCallMvcApplicationTester(IContainer container, bool initializeApplicationInstance) : base(container, initializeApplicationInstance) {}

        #endregion

        /// <summary>
        /// Useful for non-in memory database tests.
        /// </summary>
        public static IContainer InitializeDummyObjectFactory()
        {
            return new Container(i => {
                MapCallDependencies.RegisterRepositories(i);
                i.For<ILog>().Use(new Mock<ILog>().Object);
                i.For<ISession>().Singleton().Use(new Mock<ISession>().Object);
                i.For<IPrincipal>().Singleton().Use(new Mock<IPrincipal>().Object);
                i.For<IDateTimeProvider>().Singleton().Use(new Mock<IDateTimeProvider>().Object);
                i.For<IAuthenticationService<User>>().Singleton().Use(new Mock<IAuthenticationService<User>>().Object);
                i.For<IAuthenticationService>().Use(ctx => ctx.GetInstance<IAuthenticationService<User>>());
                i.For<IRoleService>().Use(new Mock<IRoleService>().Object);
                i.For<IAuthenticationCookieFactory>().Use<AuthenticationCookieFactory>();
                i.For<IAssetCoordinateService>().Use(new Mock<IAssetCoordinateService>().Object);
                i.For<INotificationService>().Use(new Mock<INotificationService>().Object);
                i.For<IActiveMQServiceFactory>().Use(new Mock<IActiveMQServiceFactory>().Object);
            });
        }
    }
}
