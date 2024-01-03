using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallApi.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Metadata;
using MMSINC.Testing.MSTest.TestExtensions;
using NHibernate;
using NHibernate.Event;
using NHibernate.Linq;

namespace MapCallApi.Tests.Configuration
{
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
            _source.Setup(x => x.GetSession(EntityMode.Poco)).Returns(_container.GetInstance<ISession>());
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
