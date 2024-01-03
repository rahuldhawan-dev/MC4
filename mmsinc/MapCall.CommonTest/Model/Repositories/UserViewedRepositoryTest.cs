using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class UserViewedRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<UserViewed, UserViewedRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Tests

        #region SearchDailyReportItems

        [TestMethod]
        public void
            TestSearchDailyReportItemsReturnsExpectedValuesForASingleDayAndSingleUserThatLooksAtABunchOfSeperateImagesOneTime()
        {
            var expectedDate = new DateTime(1984, 4, 24);
            var employee = GetFactory<EmployeeFactory>().Create(new {EmployeeId = "424242"});
            var user1 = GetFactory<UserFactory>().Create(new {
                Address = "123 Fake St",
                Employee = employee
            });
            var tap1 = GetFactory<TapImageFactory>().Create();
            var tap2 = GetFactory<TapImageFactory>().Create();
            var valve1 = GetFactory<ValveImageFactory>().Create();
            var asbuilt1 = GetFactory<AsBuiltImageFactory>().Create();
            var asbuilt2 = GetFactory<AsBuiltImageFactory>().Create();
            var asbuilt3 = GetFactory<AsBuiltImageFactory>().Create();
            var user1ViewedTap1 = GetFactory<UserViewedFactory>()
               .Create(new {User = user1, TapImage = tap1, ViewedAt = expectedDate});
            var user1ViewedTap2 = GetFactory<UserViewedFactory>()
               .Create(new {User = user1, TapImage = tap2, ViewedAt = expectedDate.AddHours(2)});
            var user1ViewedValve1 = GetFactory<UserViewedFactory>().Create(new
                {User = user1, ValveImage = valve1, ViewedAt = expectedDate.AddHours(3)});
            var user1ViewedAsBuilt1 = GetFactory<UserViewedFactory>().Create(new
                {User = user1, AsBuiltImage = asbuilt1, ViewedAt = expectedDate.AddHours(4)});
            var user1ViewedAsBuilt2 = GetFactory<UserViewedFactory>().Create(new
                {User = user1, AsBuiltImage = asbuilt2, ViewedAt = expectedDate.AddHours(5)});
            var user1ViewedAsBuilt3 = GetFactory<UserViewedFactory>().Create(new
                {User = user1, AsBuiltImage = asbuilt3, ViewedAt = expectedDate.AddHours(6)});

            var searchModel = new TestSearchUserViewedDailyRecordItem();
            searchModel.ViewedAt = new RequiredDateRange {
                End = expectedDate,
                Operator = RangeOperator.Equal
            };

            var result =
                Repository.SearchDailyReportItems(searchModel)
                          .Single(); // There should only be one result for this test.

            Assert.AreEqual(2, result.TapImages);
            Assert.AreEqual(1, result.ValveImages);
            Assert.AreEqual(3, result.AsBuiltImages);
            Assert.AreEqual("123 Fake St", result.UserAddress);
            Assert.AreEqual("424242", result.EmployeeId);
        }

        [TestMethod]
        public void
            TestSearchDailyReportItemsReturnsExpectedValuesWhenSearchingForASingleDayAndSingleUserWhoViewsTheSameImageMultipleTimes()
        {
            var expectedDate = new DateTime(1984, 4, 24, 4, 0, 4);
            var employee = GetFactory<EmployeeFactory>().Create(new {EmployeeId = "424242"});
            var user1 = GetFactory<UserFactory>().Create(new {
                Address = "123 Fake St",
                Employee = employee
            });
            var tap1 = GetFactory<TapImageFactory>().Create();
            var user1ViewedTap1 = GetFactory<UserViewedFactory>()
               .Create(new {User = user1, TapImage = tap1, ViewedAt = expectedDate});
            var user1ViewedTap1Again = GetFactory<UserViewedFactory>()
               .Create(new {User = user1, TapImage = tap1, ViewedAt = expectedDate.AddHours(1)});
            var user1ViewedTap1AgainTheNextDay = GetFactory<UserViewedFactory>()
               .Create(new {User = user1, TapImage = tap1, ViewedAt = expectedDate.AddDays(1)});

            var searchModel = new TestSearchUserViewedDailyRecordItem();
            searchModel.ViewedAt = new RequiredDateRange {
                End = expectedDate,
                Operator = RangeOperator.Equal
            };

            var result =
                Repository.SearchDailyReportItems(searchModel)
                          .Single(); // There should only be one result for this test.

            Assert.AreEqual(2, result.TapImages);
            Assert.AreEqual(0, result.ValveImages);
            Assert.AreEqual(0, result.AsBuiltImages);
            Assert.AreEqual("123 Fake St", result.UserAddress);
            Assert.AreEqual("424242", result.EmployeeId);
        }

        [TestMethod]
        public void TestSearchDailyReportItemsSearchsManyUsersForManyDatesAndReturnsThemGroupedInOrder()
        {
            var expectedDate = new DateTime(1984, 4, 24);
            var employee1 = GetFactory<EmployeeFactory>().Create(new {EmployeeId = "424242"});
            var user1 = GetFactory<UserFactory>().Create(new {
                Address = "123 Fake St",
                Employee = employee1,
                UserName = "userA"
            });
            var employee2 = GetFactory<EmployeeFactory>().Create(new {EmployeeId = "11111"});
            var user2 = GetFactory<UserFactory>().Create(new {
                Address = "1 Other Ave",
                Employee = employee2,
                UserName = "userB"
            });

            // Having multiple tap images for this test shouldn't matter. We don't group on
            // that value. However, I made two just to make sure this test fails horribly if
            // the query gets messed up.
            var tap1 = GetFactory<TapImageFactory>().Create();
            var tap2 = GetFactory<TapImageFactory>().Create();

            // Setup what user1 views
            GetFactory<UserViewedFactory>().Create(new {User = user1, TapImage = tap1, ViewedAt = expectedDate});
            GetFactory<UserViewedFactory>()
               .Create(new {User = user1, TapImage = tap2, ViewedAt = expectedDate.AddHours(1)});
            GetFactory<UserViewedFactory>()
               .Create(new {User = user1, TapImage = tap1, ViewedAt = expectedDate.AddDays(1)});

            // Setup what user2 views.
            GetFactory<UserViewedFactory>().Create(new {User = user2, TapImage = tap1, ViewedAt = expectedDate});
            GetFactory<UserViewedFactory>()
               .Create(new {User = user2, TapImage = tap2, ViewedAt = expectedDate.AddDays(1)});
            GetFactory<UserViewedFactory>()
               .Create(new {User = user2, TapImage = tap2, ViewedAt = expectedDate.AddDays(1)});

            var searchModel = new TestSearchUserViewedDailyRecordItem();
            searchModel.ViewedAt = new RequiredDateRange {
                Start = expectedDate,
                End = expectedDate.AddDays(1),
                Operator = RangeOperator.Between
            };

            var results = Repository.SearchDailyReportItems(searchModel).ToList();

            Assert.AreEqual(4, results.Count);

            // Result order should be in descending date order and ascending user:
            // User1, 4/25/1984
            // User2, 4/25/1984
            // User1, 4/24/1984
            // User2, 4/24/1984

            var result = results[0];
            Assert.AreEqual(new DateTime(1984, 4, 25), result.ViewedAt);
            Assert.AreEqual(user1.UserName, result.Username);
            Assert.AreEqual(user1.Id, result.UserId);
            Assert.AreEqual(1, result.TapImages);
            Assert.AreEqual("123 Fake St", result.UserAddress);
            Assert.AreEqual("424242", result.EmployeeId);

            result = results[1];
            Assert.AreEqual(new DateTime(1984, 4, 25), result.ViewedAt);
            Assert.AreEqual(user2.UserName, result.Username);
            Assert.AreEqual(user2.Id, result.UserId);
            Assert.AreEqual(2, result.TapImages);
            Assert.AreEqual("1 Other Ave", result.UserAddress);
            Assert.AreEqual("11111", result.EmployeeId);

            result = results[2];
            Assert.AreEqual(new DateTime(1984, 4, 24), result.ViewedAt);
            Assert.AreEqual(user1.UserName, result.Username);
            Assert.AreEqual(user1.Id, result.UserId);
            Assert.AreEqual(2, result.TapImages);
            Assert.AreEqual("123 Fake St", result.UserAddress);
            Assert.AreEqual("424242", result.EmployeeId);

            result = results[3];
            Assert.AreEqual(new DateTime(1984, 4, 24), result.ViewedAt);
            Assert.AreEqual(user2.UserName, result.Username);
            Assert.AreEqual(user2.Id, result.UserId);
            Assert.AreEqual(1, result.TapImages);
            Assert.AreEqual("1 Other Ave", result.UserAddress);
            Assert.AreEqual("11111", result.EmployeeId);
        }

        [TestMethod]
        public void TestSearchDailyReportItemsCanSearchByUserAddress()
        {
            var expectedDate = new DateTime(1984, 4, 24);
            var employee1 = GetFactory<EmployeeFactory>().Create(new {EmployeeId = "424242"});
            var user1 = GetFactory<UserFactory>().Create(new {
                Address = "123 Fake St",
                Employee = employee1
            });
            var employee2 = GetFactory<EmployeeFactory>().Create(new {EmployeeId = "11111"});
            var user2 = GetFactory<UserFactory>().Create(new {
                Address = "1 Other Ave",
                Employee = employee2
            });

            var tap1 = GetFactory<TapImageFactory>().Create();

            // Setup what user1 views
            GetFactory<UserViewedFactory>().Create(new {User = user1, TapImage = tap1, ViewedAt = expectedDate});

            // Setup what user2 views.
            GetFactory<UserViewedFactory>().Create(new {User = user2, TapImage = tap1, ViewedAt = expectedDate});

            var searchModel = new TestSearchUserViewedDailyRecordItem();
            searchModel.UserAddress = "Fake St";

            var result = Repository.SearchDailyReportItems(searchModel).Single();
            Assert.AreEqual(new DateTime(1984, 4, 24), result.ViewedAt);
            Assert.AreEqual(user1.UserName, result.Username);
            Assert.AreEqual(user1.Id, result.UserId);
            Assert.AreEqual(1, result.TapImages);
            Assert.AreEqual("123 Fake St", result.UserAddress);
            Assert.AreEqual("424242", result.EmployeeId);
        }

        #endregion

        #region SearchWithImages

        [TestMethod]
        public void TestSearchWithImagesOnlyPerformsOneQuery()
        {
            // The SearchWithImages method should be returing a query result with all includes
            // entity references needed for the report it's used in. 

            var userViewed = GetFactory<UserViewedTapImageFactory>().Create();

            // Ensure we're clearing everything cached in-memory to ensure everything causes a database query.
            Session.Clear();

            // Start listening for database queries.
            Interceptor.Init();

            // Disable paging so we aren't including the initial count query.
            var search = new EmptySearchSet<UserViewed> {EnablePaging = false};
            var result = Repository.SearchWithImages(search).Single();
            Assert.AreEqual(userViewed.Id, result.Id, "Sanity.");
            Assert.AreNotSame(userViewed, result, "Sanity.");

            // Need to ensure that TapImage and Town are both fully loaded, so call a property on both TapImage and Town.
            var forceTapImageAndTownToLoad = result.TapImage.Town.Address;

            Assert.AreEqual(1, Interceptor.PreparedStatements.Count,
                "There should only have been a single select statement.");
        }

        #endregion

        #endregion

        #region Test classes

        private class TestSearchUserViewedDailyRecordItem : SearchSet<UserViewedDailyRecordItem>,
            ISearchUserViewedDailyRecordItem
        {
            public RequiredDateRange ViewedAt { get; set; }
            public string UserAddress { get; set; }
        }

        #endregion
    }
}
