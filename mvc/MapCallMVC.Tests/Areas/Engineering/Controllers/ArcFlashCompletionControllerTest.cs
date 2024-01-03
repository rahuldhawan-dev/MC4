using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Engineering.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Areas.Engineering.Controllers
{
    [TestClass]
    public class ArcFlashCompletionControllerTest : MapCallMvcControllerTestBase<ArcFlashCompletionController, OperatingCenter, OperatingCenterRepository>
    {
        #region Private Members

        private Mock<IDateTimeProvider> _dateProvider;

        #endregion

        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexDisplaysViewWhenNoResults = true;
        }

        protected override User CreateUser()
        {
            return GetFactory<UserFactory>().Create(new { IsAdmin = true });
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _dateProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_dateProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.EngineeringArcFlash;

            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Reports/ArcFlashCompletion/Index", role);
                a.RequiresRole("~/Engineering/ArcFlashCompletion/Index", role);
                a.RequiresRole("~/Engineering/ArcFlashCompletion/Show", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            var entity = GetEntityFactory<OperatingCenter>().Create();

            var result = (ActionResult)_target.AsDynamic().Show(entity.Id);

            var expectedViewName = "Show";
            MvcAssert.IsViewWithNameAndModel(result, expectedViewName, entity,
                $"ArcFlashCompletionController.Show failed.");
        }

        #endregion
    }
}