using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Management.Controllers;
using MapCallMVC.Areas.Management.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Management.Controllers
{
    [TestClass]
    public class UserViewedControllerTest : MapCallMvcControllerTestBase<UserViewedController, UserViewed>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.ManagementGeneral;

            Authorization.Assert(a => {
                a.RequiresRole("~/Management/UserViewed/Index/", role);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because the search model needs to be setup in a specific way.
            var userViewed = GetFactory<UserViewedTapImageFactory>().Create();

            var searchModel = new SearchUserViewed();
            searchModel.User = userViewed.User.Id;
            searchModel.ViewedAt = new RequiredDateRange
            {
                Start = userViewed.ViewedAt.Date,
                Operator = RangeOperator.Between,
                End = userViewed.ViewedAt.Date.AddDays(1)
            };

            var result = _target.Index(searchModel);
            MvcAssert.IsViewNamed(result, "Index");
            Assert.IsFalse(searchModel.EnablePaging, "EnablePaging must always be set to false.");
            Assert.IsTrue(searchModel.Results.Contains(userViewed));
        }

        #endregion
    }
}
