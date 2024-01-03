using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.StructureMap;
using NHibernate.Type;
using StructureMap;

namespace MMSINC.CoreTest.Data.NHibernate
{
    [TestClass]
    public class ChangeTrackingInterceptorTest
    {
        #region Private Members

        private ChangeTrackingInterceptor<TestUser> _target;
        private DateTime _now;
        private TestUser _currentTestUser;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void Initialize()
        {
            var container = new Container(InitializeContainer);
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));

            _target = container.GetInstance<ChangeTrackingInterceptor<TestUser>>();
        }

        #endregion

        #region Nested Type: TestUser

        public class TestUser : IAdministratedUser
        {
            #region Properties

            public int Id { get; }
            public string UniqueName { get; }
            public bool IsAdmin { get; }
            public bool HasAccess { get; }
            public string Email { get; }
            public string Password { get; }
            public Guid PasswordSalt { get; }
            public DateTime? LastLoggedInAt { get; set; }

            #endregion
        }

        #endregion

        #region Nested Type: TestWidget

        public class TestWidget : IEntityWithChangeTracking<TestUser>
        {
            #region Properties

            public int Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public TestUser CreatedBy { get; set; }
            public DateTime UpdatedAt { get; set; }
            public TestUser UpdatedBy { get; set; }

            #endregion
        }

        public class TestUntrackedWidget : IEntity
        {
            #region Properties

            public int Id { get; set; }

            #endregion
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression registry)
        {
            registry
               .For<IDateTimeProvider>()
               .Use(new TestDateTimeProvider(_now = DateTime.Now));
            var authServ = registry.For<IAuthenticationService<TestUser>>().Mock();
            authServ.Setup(x => x.CurrentUser).Returns(_currentTestUser = new TestUser());
            authServ.Setup(x => x.CurrentUserIsAuthenticated).Returns(true);
        }

        #endregion

        #region OnSave

        [TestMethod]
        public void Test_OnSave_SetsCreatedAt()
        {
            var entity = new TestWidget();

            _target.OnSave(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());
            
            Assert.AreEqual(_now, entity.CreatedAt);
        }

        [TestMethod]
        public void Test_OnSave_ReturnsTrue_WhenCreatedAtSet()
        {
            var entity = new TestWidget {
                CreatedBy = _currentTestUser
            };

            var result = _target.OnSave(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_OnSave_DoesNotOverrideCreatedAt()
        {
            var test = new DateTime(1997, 8, 4, 2, 14, 0);
            var entity = new TestWidget {
                CreatedAt = test
            };
            
            _target.OnSave(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());

            Assert.AreEqual(test, entity.CreatedAt);
        }

        [TestMethod]
        public void Test_OnSave_SetsCreatedBy()
        {
            var entity = new TestWidget();

            _target.OnSave(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());
            
            Assert.AreEqual(_currentTestUser, entity.CreatedBy);
        }

        [TestMethod]
        public void Test_OnSave_ReturnsTrue_WhenCreatedBySet()
        {
            var entity = new TestWidget {
                CreatedAt = _now
            };

            var result = _target.OnSave(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_OnSave_DoesNotOverrideCreatedBy()
        {
            var existingCreatedBy = new TestUser();
            var entity = new TestWidget {
                CreatedBy = existingCreatedBy
            };

            _target.OnSave(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());

            Assert.AreEqual(existingCreatedBy, entity.CreatedBy);
        }

        [TestMethod]
        public void Test_OnSave_SetsUpdatedAt()
        {
            var entity = new TestWidget();

            _target.OnSave(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());
            
            Assert.AreEqual(_now, entity.UpdatedAt);
        }

        [TestMethod]
        public void Test_OnSave_SetsUpdatedBy()
        {
            var entity = new TestWidget();

            _target.OnSave(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());
            
            Assert.AreEqual(_currentTestUser, entity.UpdatedBy);
        }

        [TestMethod]
        public void Test_OnSave_ReturnsFalse_WhenEntityIsNotChangeTracked()
        {
            var entity = new TestUntrackedWidget();

            var result = _target.OnSave(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());

            Assert.IsFalse(result);
        }

        #endregion

        #region OnFlushDirty

        [TestMethod]
        public void Test_OnFlushDirty_SetsUpdatedAt()
        {
            var entity = new TestWidget();

            _target.OnFlushDirty(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());
            
            Assert.AreEqual(_now, entity.UpdatedAt);
        }

        [TestMethod]
        public void Test_OnFlushDirty_SetsUpdatedBy()
        {
            var entity = new TestWidget();

            _target.OnFlushDirty(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());
            
            Assert.AreEqual(_currentTestUser, entity.UpdatedBy);
        }

        [TestMethod]
        public void Test_OnFlushDirty_ReturnsTrue_WhenUpdatedAtAndUpdatedBySet()
        {
            var entity = new TestWidget();

            var result = _target.OnFlushDirty(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_OnFlushDirty_ReturnsFalse_WhenEntityIsNotChangeTracked()
        {
            var entity = new TestUntrackedWidget();

            var result = _target.OnFlushDirty(
                entity,
                null,
                Array.Empty<object>(),
                Array.Empty<object>(),
                Array.Empty<string>(),
                Array.Empty<IType>());

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Test_OnFlushDirty_ResetsCreatedAt()
        {
            var yesterday = _now.AddDays(-1);
            var entity = new TestWidget();

            _target.OnFlushDirty(
                entity,
                null,
                new object[1],
                new object[] {yesterday},
                new[] {nameof(entity.CreatedAt)},
                Array.Empty<IType>());
            
            Assert.AreEqual(yesterday, entity.CreatedAt);
        }

        [TestMethod]
        public void Test_OnFlushDirty_ResetsCreatedBy()
        {
            var creator = new TestUser();
            var entity = new TestWidget();

            _target.OnFlushDirty(
                entity,
                null,
                new object[1],
                new object[] {creator},
                new[] {nameof(entity.CreatedBy)},
                Array.Empty<IType>());
            
            Assert.AreSame(creator, entity.CreatedBy);
        }

        #endregion
    }
}
