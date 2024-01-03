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
using NHibernate.Event;
using NHibernate.Linq;

namespace MapCallApi.Tests.Configuration
{
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
            var newState = new object[] {"foo"};
            var fieldNames = new[] {"FooField"};
            var @event = new PostInsertEvent(permit, permit.Id, newState, _persister.Object,
                _source.Object);
            var session = _container.GetInstance<ISession>();
            _source.Setup(x => x.ConnectionManager).Returns(new ConnectionManager(session.GetSessionImplementation(), session.Connection,
                ConnectionReleaseMode.AfterTransaction, new EmptyInterceptor(), false));
            _source.Setup(x => x.SessionFactory).Returns(session.SessionFactory);

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
                () => session.Query<AuditLogEntry>().Count());

            var entry = session.Query<AuditLogEntry>().First();

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
            _persister.Setup(x => x.PropertyNames).Returns(new[] {"Field1", "Field2"});

            var entry = new AuditLogEntry();
            var newState = new object[] {"foo"};
            var @event = new PostInsertEvent(entry, 666, newState, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostInsert(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        [TestMethod]
        public void TestOnPostInsertSkipsOverSecureFormTokens()
        {
            _persister.Setup(x => x.PropertyNames).Returns(new[] {"Field1", "Field2"});

            var entry = new SecureFormToken();
            var newState = new object[] {"foo"};
            var @event = new PostInsertEvent(entry, 666, newState, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostInsert(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        [TestMethod]
        public void TestOnPostInsertSkipsOverSecureFormDynamicValue()
        {
            _persister.Setup(x => x.PropertyNames).Returns(new[] {"Field1", "Field2"});
            var entry = new SecureFormDynamicValue();
            var newState = new object[] {"foo"};
            var @event = new PostInsertEvent(entry, 666, newState, _persister.Object,
                _source.Object);

            MyAssert.DoesNotCauseIncrease(
                () => _target.OnPostInsert(@event),
                () => _container.GetInstance<ISession>().Query<AuditLogEntry>().Count());
        }

        #endregion
    }
}