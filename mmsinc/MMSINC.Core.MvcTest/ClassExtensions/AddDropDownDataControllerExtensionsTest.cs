using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MMSINC.Core.MvcTest.ClassExtensions
{
    [TestClass]
    public class AddDropDownDataControllerExtensionsTest : InMemoryDatabaseTest<TestUser>
    {
        #region Private Members

        private TestController _target;

        #endregion

        #region Private Methods

        private TEntity[] CreateData<TEntity>(params TEntity[] entities)
            where TEntity : class
        {
            var repo = _container.GetInstance<RepositoryBase<TEntity>>();

            foreach (var entity in entities)
            {
                repo.Save(entity);
            }

            return entities;
        }

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _target = _container.GetInstance<TestController>();
        }

        #endregion

        #region AddDropDownData<TRepository, TEntity>

        [TestMethod]
        public void
            TestAddDropDownDataGathersAllDataFromSpecifiedRepositoryTypeAndAddsADropDownItemCollectionWithTheSpecifiedKey()
        {
            var key = "users";
            var users = CreateData(
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"});

            var ret = _target.AddDropDownData<IRepository<TestUser>, TestUser>(key, u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData[key]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual<string>(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual<string>(users[i].Email, item.Text);
            });
        }

        [TestMethod]
        public void
            TestAddDropDownDataGathersDataFromSpecifiedRepositoryTypeUsingTheSpecifiedMethodAndAddsADropDownItemCollectionWithTheSpecifiedKey()
        {
            var key = "users";
            var users = CreateData(
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"});

            var ret = _target
               .AddDropDownData<IRepository<TestUser>, TestUser>(key, r => r.GetAll(), u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData[key]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual(users[i].Email, item.Text);
            });
        }

        [TestMethod]
        public void
            TestAddDropDownDataGathersDataFromSpecifiedRepositoryTypeUsingTheSpecifiedMethodAndAddsADropDownItemCollectionUsingAKeyBuiltFromTheEntityType()
        {
            var users = CreateData(
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"});

            var ret = _target
               .AddDropDownData<IRepository<TestUser>, TestUser>(r => r.GetAll(), u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData["TestUser"]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual(users[i].Email, item.Text);
            });
        }

        [TestMethod]
        public void
            TestAddDropDownDataGathersAllDataFromSpecifiedRepositoryTypeAndAddsADropDownItemCollectionUsingAKeyBuiltFromTheEntityType()
        {
            var users = CreateData(
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"});

            var ret = _target
               .AddDropDownData<IRepository<TestUser>, TestUser>(u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData["TestUser"]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual(users[i].Email, item.Text);
            });
        }

        #endregion

        #region AddDropDownData<TEntity>

        [TestMethod]
        public void
            TestAddDropDownDataGathersAllDataFromRepositoryWithSpecifiedEntityTypeAndAddsADropDownItemCollectionWithTheSpecifiedKey()
        {
            var key = "users";
            var users = CreateData(
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"});

            var ret = _target.AddDropDownData<TestUser>(key, u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData[key]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual(users[i].Email, item.Text);
            });
        }

        [TestMethod]
        public void
            TestAddDropDownDataGathersAllDataFromRepositoryWithSpecifiedEntityTypeAndAddsADropDownItemCollectionUsingAKeyBuiltFromTheEntityType()
        {
            var users = CreateData(
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"});

            var ret = _target.AddDropDownData<TestUser>(u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData["TestUser"]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual(users[i].Email, item.Text);
            });
        }

        [TestMethod]
        public void
            TestAddDropDownDataGathersAllDataFromRepositoryWithSpecifiedEntityTypeFiltersAndAddsADropDownItemCollectionUsingAKeyBuiltFromTheEntityType()
        {
            var users = CreateData(
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"});

            var ret = _target.AddDropDownData<TestUser>(u => u.SomeOrder == 1, u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData["TestUser"]).ToArray();

            Assert.AreSame(ret, _target);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(users[0].Email, actual[0].Text);
        }

        [TestMethod]
        public void
            TestAddDropDownDataGathersDataFromRepositoryWithSpecifiedEntityTypeUsingTheSpecifiedMethodAndAddsADropDownItemCollectionWithTheSpecifiedKey()
        {
            var key = "users";
            var users = CreateData(
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"});

            var ret = _target.AddDropDownData<TestUser>(key, r => r.Where(_ => true), u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData[key]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual(users[i].Email, item.Text);
            });
        }

        [TestMethod]
        public void
            TestAddDropDownDataGathersDataFromRepositoryWithSpecifiedEntityTypeUsingTheSpecifiedMethodAndAddsADropDownItemCollectionUsingAKeyBuiltFromTheEntityType()
        {
            var users = CreateData(
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"});

            var ret = _target.AddDropDownData<TestUser>(r => r.Where(_ => true), u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData["TestUser"]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual(users[i].Email, item.Text);
            });
        }

        [TestMethod]
        public void TestAddDropDownDataUsesSpecifiedDataAndAddsADropDownItemCollectionWithTheSpecifiedKey()
        {
            var key = "users";
            var users = new[] {
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"}
            };

            var ret = _target.AddDropDownData(key, users, u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData[key]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual(users[i].Email, item.Text);
            });
        }

        [TestMethod]
        public void TestAddDropDownDataUsesSpecifiedDataAndAddsADropDownItemCollectionUsingAKeyBuiltFromTheEntityType()
        {
            var users = new[] {
                new TestUser {SomeOrder = 1, Email = "foo@bar.com"},
                new TestUser {SomeOrder = 2, Email = "bar@foo.com"}
            };

            var ret = _target.AddDropDownData(users, u => u.SomeOrder, u => u.Email);
            var actual = ((IEnumerable<SelectListItem>)_target.ViewData["TestUser"]).ToArray();

            Assert.AreSame(ret, _target);
            actual.EachWithIndex((item, i) => {
                Assert.AreEqual(users[i].SomeOrder.ToString(), item.Value);
                Assert.AreEqual(users[i].Email, item.Text);
            });
        }

        #endregion
    }
}
