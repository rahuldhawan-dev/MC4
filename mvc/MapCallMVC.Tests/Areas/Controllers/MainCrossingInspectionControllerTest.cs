using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class MainCrossingInspectionControllerTest : MapCallMvcControllerTestBase<MainCrossingInspectionController, MainCrossingInspection>
    {
        #region Fields

        private Mock<IDateTimeProvider> _dateProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _dateProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IMainCrossingRepository>().Use<MainCrossingRepository>();
            e.For<IMainCrossingInspectionAssessmentRatingRepository>().Use<MainCrossingInspectionAssessmentRatingRepository>();
            e.For<IUserRepository>().Use<UserRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needs to use Create, or else the Create action tests will fail because
            // two MainCrossingInspections end up being created. This is due to the Build
            // method adding references to the MainCrossing owner.
            options.CreateValidEntity = () => GetEntityFactory<MainCrossingInspection>().Create();
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "MainCrossing", area = "Facilities", id = vm.MainCrossing };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var entity = Repository.Find(vm.Id);
                return new { action = "Show", controller = "MainCrossing", area = "Facilities", id = entity.MainCrossing.Id };
            };
            options.DestroyRedirectsToRouteOnSuccessArgs = (id) => {
                var mainCrossingId = Repository.Find(id).MainCrossing.Id;
                return new { action = "Show", controller = "MainCrossing", area = "Facilities", id = mainCrossingId };
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.FieldServicesAssets;
                a.RequiresRole("~/Facilities/MainCrossingInspection/Show", role, RoleActions.Read);
                a.RequiresRole("~/Facilities/MainCrossingInspection/New", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/MainCrossingInspection/Create", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/MainCrossingInspection/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/MainCrossingInspection/Update", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/MainCrossingInspection/Destroy", role, RoleActions.Delete);
            });
        }

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override because of New parameter
            var mc = GetFactory<MainCrossingFactory>().Create();
            var result = _target.New(mc.Id);
            MvcAssert.IsViewNamed(result, "New");

            var model = (CreateMainCrossingInspection)((ViewResult)result).Model;
            Assert.AreEqual(mc.Id, model.MainCrossing);
            Assert.AreSame(mc, model.DisplayMainCrossing);
        }

        [TestMethod]
        public void TestNewSetsInspectedOnDateToTodayByDefault()
        {
            var expectedDate = DateTime.Today;
            _dateProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            var mc = GetFactory<MainCrossingFactory>().Create();
            var result = _target.New(mc.Id);
            var model = (CreateMainCrossingInspection)((ViewResult)result).Model;
            Assert.AreEqual(expectedDate, model.InspectedOn);
        }

        [TestMethod]
        public void TestNewReturns404WhenMainCrossingDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.New(0));
        }

        #endregion

        #endregion
    }
}
