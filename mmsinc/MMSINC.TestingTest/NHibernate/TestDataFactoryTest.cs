using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.TestingTest.NHibernate
{
    [TestClass]
    public class TestDataFactoryTest : InMemoryDatabaseTest<TestUser>
    {
        #region Setup/Teardown

        [TestCleanup]
        public void TestDataFactoryTestCleanup()
        {
            TestDataFactory.ResetAll();
        }

        #endregion

        [TestMethod]
        public void TestUserFactoryBuildsNonAdminUsers()
        {
            var actual = new TestUserFactory(_container).Build();

            Assert.IsFalse(actual.IsAdmin,
                "IsAdmin was not set false by the defaults");
            MyAssert.IsMatch(new Regex("^someuser\\d+@site.com$"), actual.Email,
                "Name was not set by the defaults");
        }

        [TestMethod]
        public void TestAdminFactoryBuildsAdminUsers()
        {
            var actual = new AdminUserFactory(_container).Build();

            // should override the default from UserFactory
            Assert.IsTrue(actual.IsAdmin,
                "IsAdmin was not set true by the overridden defaults");
            // should maintain the default from UserFactory
            MyAssert.IsMatch(new Regex("^someuser\\d+@site.com$"), actual.Email,
                "Name was not set by the defaults");
        }

        [TestMethod]
        public void TestUserWithNoDefaultsFactoryBuildsUserWithNoDefaults()
        {
            var user =
                new UserWithNoDefaultsFactory(_container).Build();

            Assert.AreEqual(0, user.Id, "Id should never be set");
            Assert.IsNull(user.Email, "Email should not have been set");
            Assert.IsNull(user.MainGroupId,
                "MainGroupId should not have been set");
            Assert.IsNull(user.SomeForeignId,
                "SomeForeignId should not have been set");
            Assert.IsFalse(user.IsAdmin,
                "IsAdmin should have defaulted to false");
            Assert.AreEqual(0, user.SomeOrder, "Order should not have been set");
            Assert.AreEqual(0, user.OtherOrder,
                "OtherOrder should not have been set");
            Assert.IsNull(user.MainGroup, "MainGroup should not have been set");
            Assert.AreEqual(0, user.Groups.Count,
                "No groups should have been added");
        }

        [TestMethod]
        public void TestBuildAllowsOverrides()
        {
            var actual = new TestUserFactory(_container).Build(new {
                IsAdmin = true
            });

            Assert.IsTrue(actual.IsAdmin);
        }

        //[TestMethod]
        //public void TestUserOrderSequence()
        //{
        //    var users =
        //        new TestUserFactory(_container).BuildList(3).ToArray();

        //    for (var i = 0; i < users.Count(); ++i)
        //    {
        //        Assert.AreEqual(i + 1, users[i].SomeOrder);
        //        Assert.AreEqual(i + 1, users[i].OtherOrder);
        //    }
        //}

        [TestMethod]
        public void TestUserWithLambdaForEmail()
        {
            var factory = new UserFactoryWithLambdaForEmail(_container);
            var user = factory.Build();

            Assert.AreEqual(UserFactoryWithLambdaForEmail.DEFAULT_LAMBDA_EMAIL,
                user.Email);

            user = factory.Create();

            Assert.AreNotEqual(0, user.Id);
            Assert.AreEqual(UserFactoryWithLambdaForEmail.DEFAULT_LAMBDA_EMAIL,
                user.Email);
        }

        //[TestMethod]
        //public void TestUserOrderSequenceInherited()
        //{
        //    var admins =
        //        new AdminUserFactory(_container).BuildList(3).ToArray();

        //    for (var i = 0; i < admins.Count(); ++i)
        //    {
        //        Assert.AreEqual(i + 1, admins[i].SomeOrder);
        //        Assert.AreEqual(i + 1, admins[i].OtherOrder);
        //    }
        //}

        [TestMethod]
        public void TestCreateSavesRecords()
        {
            var user = new TestUserFactory(_container).Create();

            Assert.AreNotEqual(0, user.Id);
        }

        [TestMethod]
        public void TestPropertiesSetFromDefaultsSetToFactoryTypes()
        {
            var factory = new UserWithDefaultGroupFactory(_container);
            var user = factory.Build();

            Assert.IsNotNull(user.MainGroup);

            user = factory.Create();

            Assert.IsNotNull(user.MainGroup);
            Assert.AreNotEqual(0, user.MainGroup.Id);
        }

        [TestMethod]
        public void TestPropertiesSetFromOverridesSetToFactoryTypes()
        {
            var user = new TestUserFactory(_container).Create(new {
                MainGroup = new TestGroupFactory(_container).Create()
            });

            Assert.IsNotNull(user.MainGroup);
        }

        [TestMethod]
        public void TestCreateThrowsExceptionIfOverridesObjectHasPropertyThatDoesNotExistOnActualObject()
        {
            MyAssert.Throws<InvalidOperationException>(() =>
                new TestUserFactory(_container).Create(new {
                    JingleHeimer = "Junction"
                }));
        }

        [TestMethod]
        public void TestOnSaving()
        {
            // use the UserWithDefaultGroupFactory, if OnSaving works then
            // the group will have an id
            var user = new UserWithDefaultGroupFactory(_container).Create();

            Assert.AreNotEqual(0, user.MainGroup.Id);
            Assert.AreEqual(user.Id, user.MainGroup.Administrator.Id);
            // need to be sure that the group name was defaulted for the next
            // test
            Assert.AreEqual("Default Group Name", user.MainGroup.Name);
        }

        [TestMethod]
        public void TestOnSavingIsInherited()
        {
            var user = new UserWithOwnGroupFactory(_container).Create();

            Assert.AreEqual(user.Id, user.MainGroup.Administrator.Id);
            // group name will have been changed by UserWithOwnGroupFactory's
            // on save, after having been saved by UserWithDefaultGroupFactory's
            // on save (the parent class)
            Assert.AreEqual(string.Format("{0}'s Group", user.Email), user.MainGroup.Name);
        }

        #region Session Issues

        [TestMethod]
        public void TestChildCreationWithLinkToParentPopulatesParentChildList()
        {
            var child = GetFactory<ChildFactory>().Create();
            var parent = GetFactory<ParentFactory>().Create(new {Children = new List<TestChild> {child}});

            MyAssert.IsGreaterThan(parent.Children.Count, 0);
        }

        #endregion
    }
}
