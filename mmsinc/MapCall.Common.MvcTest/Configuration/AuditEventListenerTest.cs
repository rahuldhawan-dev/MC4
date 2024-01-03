using System;
using System.Linq;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Metadata;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Linq;
using NHibernate.Persister.Entity;
using StructureMap;

namespace MapCall.Common.MvcTest.Configuration
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
        public void TestInitialize()
        {
            _persister = new Mock<IEntityPersister>();
            _source = new Mock<IEventSource>();
            _source
                .Setup(s => s.GetSession(EntityMode.Poco))
                .Returns(Session);
            _target = _container.GetInstance<TListener>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _dateTimeProvider.VerifyAll();
            _authenticationSerivce.VerifyAll();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _authenticationSerivce = e.For<IAuthenticationService<User>>().Mock();
        }

        #endregion
    }

    [TestClass]
    public class AuditUpdateListenerTest : AuditEventListenerTest<AuditUpdateListener>
    {
        #region OnPostUpdate(PostUpdateEvent)

        [TestMethod]
        public void TestOnPostUpdateCreatesAuditLogEntries()
        {
            var user = GetFactory<UserFactory>().Create();
            var permit = GetFactory<StateFactory>().Create();
            var now = DateTime.Now;
            var oldState = new object[] { "foo" };
            var newState = new object[] { "bar" };
            var fieldNames = new[] { "FooField" };
            var @event = new PostUpdateEvent(permit, permit.Id, newState, oldState, _persister.Object,
                _source.Object);

            _persister
                .Setup(p => p.FindDirty(newState, oldState, permit, It.IsAny<ISessionImplementor>()))
                .Returns(new[] { 0 });
            _persister
                .SetupGet(p => p.PropertyNames)
                .Returns(fieldNames);
            _dateTimeProvider
                .Setup(dt => dt.GetCurrentDate())
                .Returns(now);
            _authenticationSerivce
                .SetupGet(@as => @as.CurrentUser)
                .Returns(user);

            MyAssert.CausesIncrease(
                () => _target.OnPostUpdate(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());

            var entry = _container.GetInstance<ISession>().Query<AuditLogEntry>().First();

            Assert.AreEqual("State", entry.EntityName);
            Assert.AreEqual(fieldNames[0], entry.FieldName);
            Assert.AreEqual(oldState[0], entry.OldValue);
            Assert.AreEqual(newState[0], entry.NewValue);
            Assert.AreEqual(user.Id, entry.User.Id);
            Assert.AreEqual(permit.Id, entry.EntityId);
            Assert.AreEqual("Update", entry.AuditEntryType);
            MyAssert.AreClose(now, entry.Timestamp);

            _persister.VerifyAll();
        }

        [TestMethod]
        public void TestOnPostUpdateCreatesAuditLogEntriesWithIdsAndValues()
        {
            var user = GetFactory<UserFactory>().Create();
            var premise = GetFactory<PremiseFactory>().Create();
            var now = DateTime.Now;
            var operatingCenter1 = new OperatingCenter { Id = 1, OperatingCenterCode = "NJ1", OperatingCenterName = "one" };
            var operatingCenter2 = new OperatingCenter { Id = 2, OperatingCenterCode = "NJ2", OperatingCenterName = "two" };
            var oldState = new object[] { operatingCenter1 };
            var newState = new object[] { operatingCenter2 };
            var fieldNames = new[] { "OperatingCenter" };
            var @event = new PostUpdateEvent(premise, premise.Id, newState, oldState, _persister.Object,
                _source.Object);

            _persister
               .Setup(p => p.FindDirty(newState, oldState, premise, It.IsAny<ISessionImplementor>()))
               .Returns(new[] { 0 });
            _persister
               .SetupGet(p => p.PropertyNames)
               .Returns(fieldNames);
            _dateTimeProvider
               .Setup(dt => dt.GetCurrentDate())
               .Returns(now);
            _authenticationSerivce
               .SetupGet(@as => @as.CurrentUser)
               .Returns(user);

            MyAssert.CausesIncrease(
                () => _target.OnPostUpdate(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());

            var entry = _container.GetInstance<ISession>().Query<AuditLogEntry>().First();

            Assert.AreEqual("Premise", entry.EntityName);
            Assert.AreEqual(fieldNames[0], entry.FieldName);
            Assert.AreEqual($"{operatingCenter1.Id} - {operatingCenter1}", entry.OldValue);
            Assert.AreEqual($"{operatingCenter2.Id} - {operatingCenter2}", entry.NewValue);
            Assert.AreEqual(user.Id, entry.User.Id);
            Assert.AreEqual(premise.Id, entry.EntityId);
            Assert.AreEqual("Update", entry.AuditEntryType);
            MyAssert.AreClose(now, entry.Timestamp);

            _persister.VerifyAll();
        }

        [TestMethod]
        public void TestOnPostUpdateDoesNotUseProxyClasses()
        {
            var user = GetFactory<UserFactory>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create();

            var now = DateTime.Now;
            var oldState = new object[] { "foo" };
            var newState = new object[] { "bar" };
            var fieldNames = new[] { "FooField" };
            var @event = new PostUpdateEvent(hydrant, hydrant.Id, newState, oldState, _persister.Object,
                _source.Object);

            _persister
                .Setup(p => p.FindDirty(newState, oldState, hydrant, It.IsAny<ISessionImplementor>()))
                .Returns(new[] { 0 });
            _persister
                .SetupGet(p => p.PropertyNames)
                .Returns(fieldNames);
            _dateTimeProvider
                .Setup(dt => dt.GetCurrentDate())
                .Returns(now);
            _authenticationSerivce
                .SetupGet(@as => @as.CurrentUser)
                .Returns(user);

            MyAssert.CausesIncrease(
                () => _target.OnPostUpdate(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());

            var entry = _container.GetInstance<ISession>().Query<AuditLogEntry>().First();

            Assert.AreEqual("Hydrant", entry.EntityName);
            _persister.VerifyAll();
        }

        [TestMethod]
        public void TestOnPostUpdateSkipsOverAuditLogEntries()
        {
            var entry = new AuditLogEntry();
            var oldState = new[] { "foo" };
            var newState = new[] { "bar" };
            var @event = new PostUpdateEvent(entry, 666, newState, oldState, _persister.Object,
                _source.Object);
            _persister.Setup(x => x.FindDirty(@event.State, @event.OldState, @event.Entity, @event.Session))
                .Returns(new[] { 0, 1 });

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostUpdate(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        [TestMethod]
        public void TestOnPostUpdateSkipsOverSecureFormTokens()
        {
            var entry = new SecureFormToken();
            var oldState = new[] { "foo" };
            var newState = new[] { "bar" };
            var @event = new PostUpdateEvent(entry, 666, newState, oldState, _persister.Object,
                _source.Object);
            _persister.Setup(x => x.FindDirty(@event.State, @event.OldState, @event.Entity, @event.Session))
                .Returns(new[] { 0, 1 });

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostUpdate(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        [TestMethod]
        public void TestOnPostUpdateSkipsOverSecureFormDynamicValues()
        {
            var entry = new SecureFormDynamicValue();
            var oldState = new[] { "foo" };
            var newState = new[] { "bar" };
            var @event = new PostUpdateEvent(entry, 666, newState, oldState, _persister.Object,
                _source.Object);
            _persister.Setup(x => x.FindDirty(@event.State, @event.OldState, @event.Entity, @event.Session))
                .Returns(new[] { 0, 1 });

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostUpdate(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        [TestMethod]
        public void TestOnPostUpdateSkipsOverValuesThatHaveNotChanged()
        {
            var state = GetFactory<StateFactory>().Create();
            var oldState = new object[] { "foo" };
            var newState = new object[] { "foo" };
            var @event = new PostUpdateEvent(state, state.Id, newState, oldState, _persister.Object,
                _source.Object);

            _persister
                .Setup(p => p.FindDirty(newState, oldState, state, It.IsAny<ISessionImplementor>()))
                .Returns(new[] { 0 });

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostUpdate(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());

            _persister.VerifyAll();
        }

        [TestMethod]
        public void TestOnPostUpdateThrowsArgumentExceptionIfOldStateIsNull()
        {
            var permit = GetFactory<StateFactory>().Create();
            var newState = new object[] { "bar" };
            var @event = new PostUpdateEvent(permit, permit.Id, newState, null, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => MyAssert.Throws<ArgumentNullException>(() =>
                                                             _target.OnPostUpdate(@event)),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        #endregion
    }

    [TestClass]
    public class AuditInsertListenerTest : AuditEventListenerTest<AuditInsertListener>
    {
        #region OnPostInsert(PostInsertEvent)

        [TestMethod]
        public void TestOnPostInsertCreatesAuditLogEntries()
        {
            var user = GetFactory<UserFactory>().Create();
            var permit = GetFactory<StateFactory>().Create();
            var now = DateTime.Now;
            var newState = new object[] { "foo" };
            var fieldNames = new[] { "FooField" };
            var @event = new PostInsertEvent(permit, permit.Id, newState, _persister.Object,
                _source.Object);

            _persister
                .SetupGet(p => p.PropertyNames)
                .Returns(fieldNames);
            _dateTimeProvider
                .Setup(dt => dt.GetCurrentDate())
                .Returns(now);
            _authenticationSerivce
                .SetupGet(@as => @as.CurrentUser)
                .Returns(user);

            MyAssert.CausesIncrease(
                () => _target.OnPostInsert(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());

            var entry = _container.GetInstance<ISession>().Query<AuditLogEntry>().First();

            Assert.AreEqual("State", entry.EntityName);
            Assert.AreEqual(fieldNames[0], entry.FieldName);
            Assert.AreEqual(newState[0], entry.NewValue);
            Assert.AreEqual(user.Id, entry.User.Id);
            Assert.AreEqual(permit.Id, entry.EntityId);
            Assert.AreEqual("Insert", entry.AuditEntryType);
            MyAssert.AreClose(now, entry.Timestamp);

            _persister.VerifyAll();
        }

        [TestMethod]
        public void TestOnPostInsertSkipsOverAuditLogEntries()
        {
            _persister.Setup(x => x.PropertyNames).Returns(new[] { "Field1", "Field2" });

            var entry = new AuditLogEntry();
            var newState = new object[] { "foo" };
            var @event = new PostInsertEvent(entry, 666, newState, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostInsert(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        [TestMethod]
        public void TestOnPostInsertSkipsOverSecureFormTokens()
        {
            _persister.Setup(x => x.PropertyNames).Returns(new[] { "Field1", "Field2" });

            var entry = new SecureFormToken();
            var newState = new object[] { "foo" };
            var @event = new PostInsertEvent(entry, 666, newState, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostInsert(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        [TestMethod]
        public void TestOnPostInsertSkipsOverSecureFormDynamicValue()
        {
            _persister.Setup(x => x.PropertyNames).Returns(new[] { "Field1", "Field2" });
            var entry = new SecureFormDynamicValue();
            var newState = new object[] { "foo" };
            var @event = new PostInsertEvent(entry, 666, newState, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostInsert(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }
        #endregion
    }

    [TestClass]
    public class AuditDeleteListenerTest : AuditEventListenerTest<AuditDeleteListener>
    {
        #region OnPostDelete(PostDeleteEvent)

        [TestMethod]
        public void TestOnPostDeleteCreatesAuditLogEntries()
        {
            var user = GetFactory<UserFactory>().Create();
            var permit = GetFactory<StateFactory>().Create();
            var now = DateTime.Now;
            var newState = new object[] { "foo" };
            var @event = new PostDeleteEvent(permit, permit.Id, newState, _persister.Object,
                _source.Object);

            _dateTimeProvider
                .Setup(dt => dt.GetCurrentDate())
                .Returns(now);
            _authenticationSerivce
                .SetupGet(@as => @as.CurrentUser)
                .Returns(user);

            MyAssert.CausesIncrease(
                () => _target.OnPostDelete(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());

            var entry = _container.GetInstance<ISession>().Query<AuditLogEntry>().First();

            Assert.AreEqual("State", entry.EntityName);
            Assert.AreEqual(user.Id, entry.User.Id);
            Assert.AreEqual(permit.Id, entry.EntityId);
            Assert.AreEqual("Delete", entry.AuditEntryType);
            MyAssert.AreClose(now, entry.Timestamp);
        }

        [TestMethod]
        public void TestOnPostDeleteSkipsAuditLogEntries()
        {
            var entry = new AuditLogEntry();
            var newState = new object[] { "foo" };
            var @event = new PostDeleteEvent(entry, 666, newState, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostDelete(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        [TestMethod]
        public void TestOnPostDeleteSkipsSecureFormTokens()
        {
            var entry = new SecureFormToken();
            var newState = new object[] { "foo" };
            var @event = new PostDeleteEvent(entry, 666, newState, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostDelete(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        [TestMethod]
        public void TestOnPostDeleteSkipsSecureFormDynamicValues()
        {
            var entry = new SecureFormDynamicValue();
            var newState = new object[] { "foo" };
            var @event = new PostDeleteEvent(entry, 666, newState, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostDelete(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }
        #endregion
    }
}
