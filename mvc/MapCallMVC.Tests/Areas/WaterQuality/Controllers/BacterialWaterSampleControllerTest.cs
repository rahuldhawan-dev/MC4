using System;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class BacterialWaterSampleControllerTest : MapCallMvcControllerTestBase<BacterialWaterSampleController, BacterialWaterSample>
    {
        #region Private Members

        private Mock<INotificationService> _notifier;
        private Mock<IRoleService> _roleService;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _notifier = e.For<INotificationService>().Mock();
            _roleService = e.For<IRoleService>().Mock();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var sampleSite = GetEntityFactory<SampleSite>().Create(new { BactiSite = true });
                return GetEntityFactory<BacterialWaterSample>().Create(new { SampleSite = sampleSite });
            };
            options.InitializeSearchTester = (tester) => {
                // This is a read-only property, the tester can't set its value.
                tester.IgnoredPropertyNames.Add("BactiSite");

                tester.TestPropertyValues.Add(nameof(SearchBacterialWaterSample.BacterialSampleType), GetFactory<ConfirmationBacterialSampleTypeFactory>().Create().Id);
                tester.TestPropertyValues.Add(nameof(SearchBacterialWaterSample.LIMSStatus), GetFactory<ReadyToSendLIMSStatusFactory>().Create().Id);
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.WaterQualityGeneral;

            Authorization.Assert(a =>
            {
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/Search/", role);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/Show/", role);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/Index/", role);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/RecentByPwsid/", role);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/New/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/Create/", role, RoleActions.Add);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/Destroy/", role, RoleActions.Delete);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/InlineShow/", role);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/InlineEdit/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/InlineUpdate/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/GetBySampleSiteIdWithBracketSites", role, RoleActions.Read);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/ValidateTotalChlorine", role, RoleActions.Read);
                a.RequiresRole("~/WaterQuality/BacterialWaterSample/ValidateFreeChlorine", role, RoleActions.Read);
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestInlineShowInlineShowsTheBacterialWaterSampleIfItExists()
        {
            var entity = GetEntityFactory<BacterialWaterSample>().Create();

            var result = _target.InlineShow(entity.Id) as PartialViewResult;

            MvcAssert.IsViewWithModel(result, entity);
        }

        [TestMethod]
        public void TestInlineShow404sIfBacterialWaterSampleDoesNotExist()
        {
            Assert.IsNotNull(_target.InlineShow(666) as HttpNotFoundResult);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var sampleSite = GetEntityFactory<SampleSite>().Create(new { BactiSite = true });
            var entity0 = GetEntityFactory<BacterialWaterSample>().Create(new { Address = "Address 0", SampleSite = sampleSite });
            var entity1 = GetEntityFactory<BacterialWaterSample>().Create(new { Address = "Address 1", SampleSite = sampleSite });
            var search = new SearchBacterialWaterSample();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Address, "Address");
                helper.AreEqual(entity1.Address, "Address", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestInlineEditRendersViewIfBacterialWaterSampleExists()
        {
            var eq = GetEntityFactory<BacterialWaterSample>().Create();

            var result = _target.InlineEdit(eq.Id) as PartialViewResult;

            MvcAssert.IsViewNamed(result, "_InlineEdit");
            Assert.AreEqual(eq.Id, ((InlineEditBacterialWaterSample)result.Model).Id);
        }

        [TestMethod]
        public void TestInlineEdit404sIfBacterialWaterSampleNotFound()
        {
            Assert.IsNotNull(_target.InlineEdit(666) as HttpNotFoundResult);
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<BacterialWaterSample>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditBacterialWaterSample, BacterialWaterSample>(eq, new {
                Location = expected
            }));

            Assert.AreEqual(expected, Session.Get<BacterialWaterSample>(eq.Id).Location);
        }

        [TestMethod]
        public void TestInlineUpdateInlineUpdatesBacterialWaterSampleAndRedirectsBackToShow()
        {
            var eq = GetEntityFactory<BacterialWaterSample>().Create();
            var expected = "description field";

            decimal expectedVal = 45m;

            var result = _target.InlineUpdate(_viewModelFactory.BuildWithOverrides<InlineEditBacterialWaterSample, BacterialWaterSample>(eq, new {
                OrthophosphateAsP = expectedVal
            })) as PartialViewResult;

            Assert.AreEqual("_InlineShow", result.ViewName);
            Assert.AreEqual(expectedVal, Session.Get<BacterialWaterSample>(eq.Id).OrthophosphateAsP);
        }

        [TestMethod]
        public void TestInlineUpdate404sIfBacterialWaterSampleNotFound()
        {
            Assert.IsNotNull(_target.InlineUpdate(new InlineEditBacterialWaterSample(_container) { Id = 666 }) as HttpNotFoundResult);
        }

        #endregion

        #region Notifications

        private BacterialWaterSample CreateBacterialWaterSampleForNotification()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pws = GetEntityFactory<PublicWaterSupply>().Create();
            var opcpws = GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new { OperatingCenter = opc, PublicWaterSupply = pws });
            Session.Flush();

            var sampleSite = GetEntityFactory<SampleSite>().Create(new { PublicWaterSupply = pws });
            var ent = GetEntityFactory<BacterialWaterSample>().Create(new {
                SampleSite = sampleSite,
                SampleCoordinate = GetEntityFactory<Coordinate>().Create(),
                ComplianceSample = true,
                Cl2Free = 1.0m // This needs to be set because the BacterialWaterSampleFactory sets the default value to 
                               // one that triggers the notification every time.
            });
            return ent;
        }

        private void AssertNotificationIsSent(Action<BacterialWaterSample> setupViewModelAction)
        {
            var ent = CreateBacterialWaterSampleForNotification();
          //  var model = _viewModelFactory.Build<CreateBacterialWaterSample, BacterialWaterSample>( ent);
          //  model.Id = 0;
            setupViewModelAction(ent);
           // ValidationAssert.ModelStateIsValid(model);
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            BacterialWaterSampleController.SendCreationsMostBodaciousNotification(_container, ent);
           // var result = _target.Create(model);
           // var entity = Repository.Find(model.Id);

            Assert.IsNotNull(resultArgs, "No notification args were returned, which probably means the notification was not sent.");
            Assert.AreSame(ent, resultArgs.Data);
            Assert.AreEqual(BacterialWaterSampleController.ROLE, resultArgs.Module);
            Assert.AreEqual(BacterialWaterSampleController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
        }

        private void AssertNotificationIsNotSent(Action<BacterialWaterSample> setupViewModelAction)
        {
            var ent = CreateBacterialWaterSampleForNotification();
          //  var model = _viewModelFactory.Build<CreateBacterialWaterSample, BacterialWaterSample>( ent);
         //   model.Id = 0;
            setupViewModelAction(ent);
         //   ValidationAssert.ModelStateIsValid(model);
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            BacterialWaterSampleController.SendCreationsMostBodaciousNotification(_container, ent);
           // _target.Create(model);

            Assert.IsNull(resultArgs, "A notification was sent when it shouldn't have been sent.");
        }

        [TestMethod]
        public void TestCreateSendsNotificationsWhenSpecificConditionsAreMet()
        {
            var pwsidReportsFreeChlorine = GetEntityFactory<PublicWaterSupply>().Create(new{ FreeChlorineReported = true });
            var pwsidReportsTotalChlorine = GetEntityFactory<PublicWaterSupply>().Create(new{ TotalChlorineReported = true });

            // Sent when Cl2Total is < 0.2 and PWSID.TotalChlorineReported is true
            AssertNotificationIsSent(x => { x.Cl2Total = 0.199m; x.SampleSite.PublicWaterSupply = pwsidReportsTotalChlorine; });
            AssertNotificationIsSent(x => { x.Cl2Total = 0.199m; x.SampleSite = null; });
            AssertNotificationIsNotSent(x => { x.Cl2Total = null; x.SampleSite = null; });
            AssertNotificationIsNotSent(x => { x.Cl2Total = 0.199m; x.SampleSite.PublicWaterSupply = pwsidReportsFreeChlorine; });
            AssertNotificationIsNotSent(x => { x.Cl2Total = 0.199m; x.ComplianceSample = false; x.SampleSite.PublicWaterSupply = pwsidReportsTotalChlorine; });
            AssertNotificationIsNotSent(x => { x.Cl2Total = 0.2m; x.SampleSite.PublicWaterSupply = pwsidReportsTotalChlorine; });

            // Sent when Cl2Free is < 0.2
            AssertNotificationIsSent(x => { x.Cl2Free = 0.199m; x.SampleSite.PublicWaterSupply = pwsidReportsFreeChlorine; });
            AssertNotificationIsNotSent(x => { x.Cl2Free = null; x.SampleSite.PublicWaterSupply = pwsidReportsFreeChlorine; });
            AssertNotificationIsNotSent(x => { x.Cl2Free = 0.199m; x.SampleSite.PublicWaterSupply = pwsidReportsTotalChlorine; });
            AssertNotificationIsNotSent(x => { x.Cl2Free = 0.199m; x.ComplianceSample = false; x.SampleSite.PublicWaterSupply = pwsidReportsFreeChlorine; });
            AssertNotificationIsNotSent(x => { x.Cl2Free = 0.2m; x.SampleSite.PublicWaterSupply = pwsidReportsFreeChlorine; });

            // Sent when Nitrite is > 0.5
            AssertNotificationIsSent(x => { x.Nitrite = 0.051m; });
            AssertNotificationIsNotSent(x => { x.Nitrite = 0.051m; x.ComplianceSample = false; });
            AssertNotificationIsNotSent(x => { x.Nitrite = 0.05m; });
            AssertNotificationIsNotSent(x => { x.Nitrite = null; });

            // Sent when FreeAmonia is > 0.2
            AssertNotificationIsSent(x => { x.FreeAmmonia = 0.21m; });
            AssertNotificationIsNotSent(x => { x.FreeAmmonia = 0.21m; x.ComplianceSample = false; });
            AssertNotificationIsNotSent(x => { x.FreeAmmonia = 0.2m; });
            AssertNotificationIsNotSent(x => { x.FreeAmmonia = null; });

            // Sent when Ph is < 6.5
            AssertNotificationIsSent(x => { x.Ph = 6.49m; });
            AssertNotificationIsNotSent(x => { x.Ph = 6.49m; x.ComplianceSample = false; });
            AssertNotificationIsNotSent(x => { x.Ph = 6.5m; });
            AssertNotificationIsNotSent(x => { x.Ph = null; });

            // Sent when ColiformConfirm is true
            AssertNotificationIsSent(x => { x.ColiformConfirm = true; });
            AssertNotificationIsNotSent(x => { x.ColiformConfirm = true; x.ComplianceSample = false; });
            AssertNotificationIsNotSent(x => { x.ColiformConfirm = false; });

            // Sent when EColiConfirm is true
            AssertNotificationIsSent(x => { x.EColiConfirm = true; });
            AssertNotificationIsNotSent(x => { x.EColiConfirm = true; x.ComplianceSample = false; });
            AssertNotificationIsNotSent(x => { x.EColiConfirm = false; });
            AssertNotificationIsNotSent(x => { x.EColiConfirm = null; });
        }

        #endregion

        #region ValidateTotalChlorine

        [TestMethod]
        public void TestValidateTotalChlorineReturnsExpectedValues()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var sampleSite = GetFactory<SampleSiteFactory>().Create();

            Func<decimal?, int?, bool> getResult = (chlorine, sampleSiteId) => {
                var json = (JsonResult)_target.ValidateTotalChlorine(chlorine, sampleSiteId);
                return (bool)json.Data;
            };

            // Must always return true if SampleSite is null
            Assert.IsTrue(getResult(null, null));
            Assert.IsTrue(getResult(0m, null));

            // Must always return true if PWSID is null
            sampleSite.PublicWaterSupply = null;
            Assert.IsTrue(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));

            // Must always return true if PWSID is not null and TotalChlorineReported is false
            pwsid.TotalChlorineReported = false;
            sampleSite.PublicWaterSupply = pwsid;
            Assert.IsTrue(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));

            // Must return false if PWSID.TotalChlorineReported is true and the chlorine value is null
            pwsid.TotalChlorineReported = true;
            Assert.IsFalse(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));
        }

        #endregion

        #region ValidateFreeChlorine

        [TestMethod]
        public void TestValidateFreeChlorineReturnsExpectedValues()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var sampleSite = GetFactory<SampleSiteFactory>().Create();

            Func<decimal?, int?, bool> getResult = (chlorine, sampleSiteId) => {
                var json = (JsonResult)_target.ValidateFreeChlorine(chlorine, sampleSiteId);
                return (bool)json.Data;
            };

            // Must always return true if SampleSite is null
            Assert.IsTrue(getResult(null, null));
            Assert.IsTrue(getResult(0m, null));

            // Must always return true if PWSID is null
            sampleSite.PublicWaterSupply = null;
            Assert.IsTrue(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));

            // Must always return true if PWSID is not null and FreeChlorineReported is false
            pwsid.FreeChlorineReported = false;
            sampleSite.PublicWaterSupply = pwsid;
            Assert.IsTrue(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));

            // Must return false if PWSID.FreeChlorineReported is true and the chlorine value is null
            pwsid.FreeChlorineReported = true;
            Assert.IsFalse(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));
        }

        #endregion
    }
}
