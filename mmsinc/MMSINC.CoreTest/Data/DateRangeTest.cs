using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.StructureMap;
using NHibernate.Criterion;
using StructureMap;

namespace MMSINC.CoreTest.Data
{
    [TestClass]
    public class DateRangeTest : InMemoryDatabaseTest<TestUser, TestUserRepository>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(_container));
        }

        #endregion

        [TestMethod]
        public void TestIsValidReturnsTrue()
        {
            var target = new DateRange();

            Assert.IsTrue(target.IsValid);
        }

        [TestMethod]
        public void TestIsValidReturnsTrueWhenValid()
        {
            var target = new DateRange {Start = DateTime.Today, End = DateTime.Now};

            Assert.IsTrue(target.IsValid);
        }

        [TestMethod]
        public void TestIsValidReturnsFalseWhenStartNotProvided()
        {
            var target = new DateRange {End = DateTime.Now};

            Assert.IsFalse(target.IsValid);
        }

        [TestMethod]
        public void TestIsValidReturnsFalseWhenEndNotProvided()
        {
            var target = new DateRange {Start = DateTime.Today,};

            Assert.IsFalse(target.IsValid);
        }

        [TestMethod]
        public void TestValidateReturnsErrorIfOperatorIsBetweenAndEndHasValueButStartDoesNotHaveValue()
        {
            var target = new DateRange {Operator = RangeOperator.Between, End = DateTime.Today, Start = null};
            ValidationAssert.ModelStateHasError(target, "", "Start date is required.");
        }

        [TestMethod]
        public void TestValidateReturnsErrorIfOperatorIsBetweenAndStartHasValueGreaterThanEnd()
        {
            var target = new DateRange
                {Operator = RangeOperator.Between, End = DateTime.Today, Start = DateTime.Today.AddTicks(1)};
            ValidationAssert.ModelStateHasError(target, "", "Start date must be prior to end date.");
        }

        [TestMethod]
        public void TestEqualsReturnsDatesForTheSameDay()
        {
            var users = new[] {
                new TestUser {Email = "TestUser1@site.com", CreatedAt = DateTime.Now},
                new TestUser {Email = "TestUser1@site.com", CreatedAt = DateTime.Now.BeginningOfDay()},
                new TestUser {Email = "TestUser2@site.com", CreatedAt = DateTime.Now.AddDays(1)},
                new TestUser {Email = "TestUser1@site.com", CreatedAt = DateTime.Now.EndOfDay()},
                new TestUser {Email = "TestUser3@site.com", CreatedAt = DateTime.Now.AddDays(-1)},
                new TestUser {
                    Email = "TestUser3@site.com",
                    CreatedAt = DateTime.Now.Date.AddDays(1) /* midnight of the next day */
                }
            };
            foreach (var user in users) Repository.Save(user);
            var range = new DateRange {End = DateTime.Now, Operator = RangeOperator.Equal};
            ICriterion criterion = Restrictions.Conjunction();

            var results = Repository.Search(range.GetCriterion(criterion, "CreatedAt")).List<TestUser>();

            Assert.AreEqual(3, results.Count);
            Assert.IsTrue(results.Contains(users[0]));
            Assert.IsTrue(results.Contains(users[1]));
            Assert.IsTrue(results.Contains(users[3]));
        }

        [TestMethod]
        public void TestBetweenReturnsDatesBetween()
        {
            var users = new[] {
                new TestUser {Email = "TestUser1@site.com", CreatedAt = DateTime.Now},
                new TestUser {Email = "TestUser1@site.com", CreatedAt = DateTime.Now.BeginningOfDay()},
                new TestUser {Email = "TestUser2@site.com", CreatedAt = DateTime.Now.AddDays(1)},
                new TestUser {Email = "TestUser2@site.com", CreatedAt = DateTime.Now.AddDays(2)},
                new TestUser {Email = "TestUser2@site.com", CreatedAt = DateTime.Now.AddDays(3)},
                new TestUser {Email = "TestUser1@site.com", CreatedAt = DateTime.Now.EndOfDay()},
                new TestUser {Email = "TestUser3@site.com", CreatedAt = DateTime.Now.AddDays(-1)}
            };
            foreach (var user in users) Repository.Save(user);
            var range = new DateRange
                {Start = DateTime.Now, End = DateTime.Now.AddDays(2), Operator = RangeOperator.Between};
            ICriterion criterion = Restrictions.Conjunction();

            var results = Repository.Search(range.GetCriterion(criterion, "CreatedAt")).List<TestUser>();

            Assert.AreEqual(5, results.Count);
            Assert.IsTrue(results.Contains(users[0]));
            Assert.IsTrue(results.Contains(users[1]));
            Assert.IsTrue(results.Contains(users[2]));
            Assert.IsTrue(results.Contains(users[3]));
            Assert.IsTrue(results.Contains(users[5]));
        }
    }
}
