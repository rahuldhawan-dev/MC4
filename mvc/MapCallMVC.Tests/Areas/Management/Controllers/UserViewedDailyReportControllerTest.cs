using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Management.Controllers;
using MapCallMVC.Areas.Management.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Management.Controllers
{
    [TestClass]
    public class UserViewedDailyReportControllerTest : MapCallMvcControllerTestBase<UserViewedDailyReportController, UserViewed>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.ManagementGeneral;

            Authorization.Assert(a => {
                a.RequiresRole("~/Management/UserViewedDailyReport/Search/", role);
                a.RequiresRole("~/Management/UserViewedDailyReport/Index/", role);
			});
		}

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var userViewed = GetFactory<UserViewedTapImageFactory>().Create();

            var searchModel = new SearchUserViewedDailyRecordItem();
            searchModel.ViewedAt = new RequiredDateRange
            {
                Operator = RangeOperator.Equal,
                End = userViewed.ViewedAt.Date
            };

            var result = _target.Index(searchModel);
            MvcAssert.IsViewNamed(result, "Index");
            Assert.IsFalse(searchModel.EnablePaging, "EnablePaging must always be set to false.");
        }

        #endregion

        #region Search

        [TestMethod]
        public void TestSearchReturnsSearchViewWithSearchModelDefaultValues()
        {
            var expectedEndDate = new DateTime(1984, 4, 24);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedEndDate.AddMinutes(123)); // Fuzz the date a bit, it should be getting truncated to DateTime.Date.

            var result = (ViewResult)_target.Search();
            MvcAssert.IsViewNamed(result, "Search");

            var model = (SearchUserViewedDailyRecordItem)result.Model;
            Assert.AreEqual(expectedEndDate, model.ViewedAt.End);
            Assert.AreEqual(MMSINC.Data.RangeOperator.Equal, model.ViewedAt.Operator);
        }

        #endregion
	}
}
