using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallApi.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;
using NHibernate;
using NHibernate.Event;
using NHibernate.Persister.Entity;

namespace MapCallApi.Tests.Configuration
{
    [TestClass]
    public abstract class AuditEventListenerTest<TListener> : MapCallMvcInMemoryDatabaseTestBase<AuditLogEntry> 
        where TListener : AuditEventListener
    {
        #region Private Members

        protected Mock<IEntityPersister> _persister;
        protected Mock<IEventSource> _source;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected Mock<IAuthenticationService<User>> _authenticationSerivce;
        protected TListener _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void AuditEventListenerTestTestInitialize()
        {
            _persister = new Mock<IEntityPersister>();
            _source = new Mock<IEventSource>();
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authenticationSerivce = new Mock<IAuthenticationService<User>>();
            _source
               .Setup(s => s.GetSession(EntityMode.Poco))
               .Returns(Session);
            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(_authenticationSerivce.Object);
            _target = _container.GetInstance<TListener>();
        }

        [TestCleanup]
        public void AuditEventListenerTestCleanup()
        {
            _dateTimeProvider.VerifyAll();
            _authenticationSerivce.VerifyAll();
        }

        #endregion
    }
}