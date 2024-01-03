using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using NHibernate;
using StructureMap;

// NOTE:
//  These are tests for MMSINC.Core.SearchMapper that test MVC-specific implementations of
//  ISearchCriterion. These tests should be moved to the other SearchMapperTest when 
//  ViewModelToSearchMapper is retired.

namespace MMSINC.Core.MvcTest.Data
{
    [TestClass]
    public class SearchMapperTest : InMemoryDatabaseTest<TestUser, TestUserRepository>
    {
        #region Private Members

        // these aren't meant to be used, they're just so that the necessary
        // they come from get copied over.  if you have tests that pass in
        // visual studio but fail in TC and from the command line, you might
        // need these in your project also.
#pragma warning disable 169
        private System.Data.SQLite.SQLiteException _doNotUseThisException;
#pragma warning restore 169

        #endregion

        #region Tests

        #region Or Constraints

        //[TestMethod]
        //public void TestHandlesOrConstraints()
        //{
        //    var users = new[] {
        //        new TestUser { IsAdmin = true },
        //        new TestUser { HasAccess = true },
        //        new TestUser { IsAdmin = true, HasAccess = true },
        //        new TestUser { IsAdmin = false, HasAccess = false },
        //        new TestUser()
        //    };

        //    foreach (var user in users) Repository.Save(user);

        //    var searchUser = new TestSearchableSearch
        //    {
        //        IsAdmin = true,
        //        HasAccess = true,
        //        EnsureSearchValuesCallback = args =>
        //        {
        //            args.Properties.Remove("IsAdmin");
        //            args.Properties.Remove("HasAccess");
        //        }
        //    };

        //    var expected = new[] {
        //        Repository.Find(users[0].Id),
        //        Repository.Find(users[1].Id),
        //        Repository.Find(users[2].Id),
        //    };
        //    var actual = Repository
        //        .Search(ViewModelToSearchMapper<TestSearchableSearch>.Map(searchUser))
        //        .List<TestUser>();

        //    Assert.AreEqual(expected.Count(), actual.Count());
        //    for (int i = 0, len = expected.Count(); i < len; ++i)
        //    {
        //        Assert.AreEqual(expected[i], actual[i]);
        //    }
        //}

        #endregion

        #region Ranges

        [TestMethod]
        public void TestShouldGetByDateRangeIfRequested()
        {
            var now = DateTime.Now;
            var users = new[] {
                new TestUser {Email = "SomeGuy@site.com", SomeForeignId = 1, CreatedAt = now.AddDays(-4)},
                new TestUser {Email = "ThisGuy@site.com", SomeForeignId = 2, CreatedAt = now},
                new TestUser {Email = "ThatGuy@site.com", SomeForeignId = 2, CreatedAt = now}
            };

            Repository.Save(users);

            // Equal To Today
            var dateRange = new DateRange {End = now, Operator = RangeOperator.Equal};
            var args = new TestSearchSet {CreatedAt = dateRange};
            var actual = Repository.Search(args).ToArray();
            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(actual.Contains(users[1]));
            Assert.IsTrue(actual.Contains(users[2]));

            // Less Than Today
            dateRange = new DateRange {End = now, Operator = RangeOperator.LessThan};
            args = new TestSearchSet {CreatedAt = dateRange};
            actual = Repository.Search(args).ToArray();
            Assert.AreEqual(1, actual.Count());
            Assert.IsTrue(actual.Contains(users[0]));

            // Between
            dateRange = new DateRange {Start = now.AddDays(-4), End = now, Operator = RangeOperator.Between};
            args = new TestSearchSet {CreatedAt = dateRange};
            actual = Repository.Search(args).ToArray();

            Assert.AreEqual(3, actual.Count());
            Assert.IsTrue(actual.Contains(users[0]));
            Assert.IsTrue(actual.Contains(users[1]));
            Assert.IsTrue(actual.Contains(users[2]));
        }

        // TModel/TEntity - NumericRange
        [TestMethod]
        public void TestShouldGetByNumericRangeIfRequestedTEntity()
        {
            var users = new[] {
                new TestUser {Email = "SomeGuy@site.com", SomeForeignId = 1, Age = 10},
                new TestUser {Email = "ThisGuy@site.com", SomeForeignId = 2, Age = 25},
                new TestUser {Email = "ThatGuy@site.com", SomeForeignId = 2, Age = 40}
            };

            Repository.Save(users);

            var numericRange = new NumericRange {End = 10, Operator = RangeOperator.GreaterThan};
            var args = new TestSearchSet {Age = numericRange};
            var actual = Repository.Search(args).ToArray();

            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(actual.Contains(users[1]));
            Assert.IsTrue(actual.Contains(users[2]));
        }

        #endregion

        #region String Arrays

        [TestMethod]
        public void TestStringArraysDoORSearching()
        {
            var users = new[] {
                new TestUser {Email = "SomeGuy@site.com"},
                new TestUser {Email = "ThisGuy@site.com"},
                new TestUser {Email = "ThatGuy@site.com"}
            };

            Repository.Save(users);

            // Test single search value exact match
            var args = new StringArrayTestSearchSet {Email = new[] {"SomeGuy@site.com"}};
            var actual = Repository.Search(args).ToArray();
            Assert.AreEqual(1, actual.Length);
            Assert.IsTrue(actual.Contains(users[0]));
            Assert.IsFalse(actual.Contains(users[1]));
            Assert.IsFalse(actual.Contains(users[2]));

            // Test single search value partial match.
            args.Email = new[] {"site.com"};
            actual = Repository.Search(args).ToArray();
            Assert.AreEqual(3, actual.Length);
            Assert.IsTrue(actual.Contains(users[0]));
            Assert.IsTrue(actual.Contains(users[1]));
            Assert.IsTrue(actual.Contains(users[2]));

            // Test multiple search value exact match.
            args.Email = new[] {"SomeGuy@site.com", "ThatGuy@site.com"};
            actual = Repository.Search(args).ToArray();
            Assert.AreEqual(2, actual.Length);
            Assert.IsTrue(actual.Contains(users[0]));
            Assert.IsFalse(actual.Contains(users[1]));
            Assert.IsTrue(actual.Contains(users[2]));

            // Test multiple search value partial match.
            args.Email = new[] {"SomeGuy", "ThatGuy"};
            actual = Repository.Search(args).ToArray();
            Assert.AreEqual(2, actual.Length);
            Assert.IsTrue(actual.Contains(users[0]));
            Assert.IsFalse(actual.Contains(users[1]));
            Assert.IsTrue(actual.Contains(users[2]));

            // Test an empty array doesn't cause an error
            args.Email = new string[] { };
            actual = Repository.Search(args).ToArray();
            Assert.AreEqual(3, actual.Length,
                "This should return everything because this shouldn't cause a search filter.");
        }

        [TestMethod]
        public void TestStringArraysSearchIgnoresEmptyStrings()
        {
            var users = new[] {
                new TestUser {Email = "SomeGuy@site.com"},
                new TestUser {Email = "ThisGuy@site.com"},
                new TestUser {Email = null}
            };

            Repository.Save(users);

            // Test single search value exact match
            var args = new StringArrayTestSearchSet {Email = new[] {"SomeGuy@site.com", ""}};
            var actual = Repository.Search(args).ToArray();
            Assert.AreEqual(1, actual.Length);
            Assert.IsTrue(actual.Contains(users[0]));
            Assert.IsFalse(actual.Contains(users[1]));
            Assert.IsFalse(actual.Contains(users[2]));
        }

        #endregion

        #endregion

        #region Test classes

        private class TestSearchSet : SearchSet<TestUser>
        {
            #region Properties

            public int? Id { get; set; }
            public string Email { get; set; }
            public bool? HasAccess { get; set; }
            public bool? IsAdmin { get; set; }
            public DateRange CreatedAt { get; set; }
            public NumericRange Age { get; set; }

            //public OrConstraint<TestSearchSet, bool?, bool?> OrConstraint
            //{
            //    get
            //    {
            //        return (IsAdmin.HasValue && HasAccess.HasValue) ? new OrConstraint<TestSearchSet, bool?, bool?>(this, s => s.HasAccess, s => s.IsAdmin) : null;
            //    }
            //}

            #endregion
        }

        private class StringArrayTestSearchSet : SearchSet<TestUser>
        {
            public string[] Email { get; set; }
        }

        private class TestUserFactory : TestDataFactory<TestUser>
        {
            static TestUserFactory()
            {
                Defaults(new { });
            }

            public TestUserFactory(IContainer container) : base(container) { }
        }

        private class TestGroupFactory : TestDataFactory<TestGroup>
        {
            static TestGroupFactory()
            {
                Defaults(new { });
            }

            public TestGroupFactory(IContainer container) : base(container) { }
        }

        #endregion
    }
}
