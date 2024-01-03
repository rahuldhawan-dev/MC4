using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallApi.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Linq;

namespace MapCallApi.Tests.Configuration
{
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
            var session = _container.GetInstance<ISession>();
            _source.Setup(x => x.ConnectionManager).Returns(new ConnectionManager(session.GetSessionImplementation(), session.Connection,
                ConnectionReleaseMode.AfterTransaction, new EmptyInterceptor(), false));
            _source.Setup(x => x.SessionFactory).Returns(session.SessionFactory);

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
            var session = _container.GetInstance<ISession>();
            _source.Setup(x => x.ConnectionManager).Returns(new ConnectionManager(session.GetSessionImplementation(),
                session.Connection,
                ConnectionReleaseMode.AfterTransaction, new EmptyInterceptor(), false));
            _source.Setup(x => x.SessionFactory).Returns(session.SessionFactory);

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
            var session = _container.GetInstance<ISession>();
            _source.Setup(x => x.ConnectionManager).Returns(new ConnectionManager(session.GetSessionImplementation(), session.Connection,
                ConnectionReleaseMode.AfterTransaction, new EmptyInterceptor(), false));
            _source.Setup(x => x.SessionFactory).Returns(session.SessionFactory);

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
            var session = _container.GetInstance<ISession>();
            _source.Setup(x => x.ConnectionManager).Returns(new ConnectionManager(session.GetSessionImplementation(), session.Connection,
                ConnectionReleaseMode.AfterTransaction, new EmptyInterceptor(), false));
            _source.Setup(x => x.SessionFactory).Returns(session.SessionFactory);

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
}
