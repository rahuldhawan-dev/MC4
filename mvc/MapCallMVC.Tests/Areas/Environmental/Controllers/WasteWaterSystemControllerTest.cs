using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels.WasteWaterSystems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class WasteWaterSystemControllerTest : MapCallMvcControllerTestBase<WasteWaterSystemController, WasteWaterSystem>
    {
        #region Private Members

        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _notifier = e.For<INotificationService>().Mock();
        }

        #endregion
        
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.EnvironmentalWasteWaterSystems;
            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/WasteWaterSystem/Show", role);
                a.RequiresRole("~/Environmental/WasteWaterSystem/Search", role);
                a.RequiresRole("~/Environmental/WasteWaterSystem/Index", role);
                a.RequiresRole("~/Environmental/WasteWaterSystem/New", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/WasteWaterSystem/Create", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/WasteWaterSystem/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/WasteWaterSystem/Update", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/WasteWaterSystem/Destroy", role, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/Environmental/WasteWaterSystem/ByOperatingCenterAndTown");
                a.RequiresLoggedInUserOnly("~/Environmental/WasteWaterSystem/ByOperatingCenters");
                a.RequiresLoggedInUserOnly("~/Environmental/WasteWaterSystem/ActiveByOperatingCenter");
                a.RequiresLoggedInUserOnly("~/Environmental/WasteWaterSystem/ByOperatingCenter");
                a.RequiresLoggedInUserOnly("~/Environmental/WasteWaterSystem/GetSystemNameByOperatingCenter");
                a.RequiresLoggedInUserOnly("~/Environmental/WasteWaterSystem/ActiveByStateOrOperatingCenter");
            });
        }
        
        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var entity0 = GetEntityFactory<WasteWaterSystem>().Create(new { PermitNumber = "x" });
            var entity1 = GetEntityFactory<WasteWaterSystem>().Create(new { PermitNumber = "y" });
            var result = _target.Index(new SearchWasteWaterSystem()) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.PermitNumber, "PermitNumber");
                helper.AreEqual(entity1.PermitNumber, "PermitNumber", 1);
            }
        }
        
        [TestMethod]
        public void TestShowSetsUpAListOfPlanningPlantsAsDropDownData()
        {
            var expected = GetFactory<PlanningPlantFactory>().CreateList(3);
            var wws = GetEntityFactory<WasteWaterSystem>().Create();

            _target.Show(wws.Id);

            _target.AssertHasDropDownData(expected, pp => pp.Id, pp => pp.ToString());
        }

        #endregion

        #region Create

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            var entity =
                _viewModelFactory.Build<WasteWaterSystemViewModel, WasteWaterSystem>(
                    GetEntityFactory<WasteWaterSystem>().Build());

            var result = _target.Create(entity);
            
            _notifier.Verify(i => i.Notify(It.Is<NotifierArgs>(na => na.Module == WasteWaterSystemController.ROLE)), Times.Once);
        }

        #endregion
        
        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<WasteWaterSystemFactory>().Create();
            var expected = 42;

            _target.Update(_viewModelFactory.BuildWithOverrides<WasteWaterSystemViewModel, WasteWaterSystem>(eq, new {
                ForceLength = expected
            }));

            Assert.AreEqual(expected, Session.Get<WasteWaterSystem>(eq.Id).ForceLength);
        }

        [TestMethod]
        public void TestUpdateSendsNotification()
        {
            var entity = GetEntityFactory<WasteWaterSystem>().Create();
            var model = _viewModelFactory.Build<WasteWaterSystemViewModel, WasteWaterSystem>(entity);

            _target.Update(model);
            
            _notifier.Verify(i => i.Notify(It.Is<NotifierArgs>(na => na.Module == WasteWaterSystemController.ROLE)), Times.Once);
        }
        
        #endregion

        #region Cascading Endpoints

        [TestMethod]
        public void TestGetSystemNameByOperatingCenter()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var wasteWater = GetEntityFactory<WasteWaterSystem>().Create(new { OperatingCenter = opc });

            var result = (CascadingActionResult)_target.GetSystemNameByOperatingCenter(opc.Id);
            var data = (IEnumerable<WasteWaterSystemDisplayItem>)result.Data;

            Assert.AreEqual(wasteWater.Id, data.Single().Id);
        }

        [TestMethod]
        public void Test_ActiveByOperatingCenter_ReturnsActiveWasteWaterSystemsForTheGivenOperatingCenter()
        {
            var ocX = GetFactory<UniqueOperatingCenterFactory>().Create();
            var ocY = GetFactory<UniqueOperatingCenterFactory>().Create();
            var ocZ = GetFactory<UniqueOperatingCenterFactory>().Create();

            GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocX, 
                Status = GetFactory<PendingWasteWaterSystemStatusFactory>().Create()
            });

            GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocY, 
                Status = GetFactory<InactiveWasteWaterSystemStatusFactory>().Create()
            });

            GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocY, 
                Status = GetFactory<InactiveSeeNoteWasteWaterSystemStatusFactory>().Create()
            });

            var wwsActive1InY = GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocY, 
                Status = GetFactory<ActiveWasteWaterSystemStatusFactory>().Create(),
                WasteWaterSystemName = "1inY"
            });

            var wwsActive1InZ = GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocZ, 
                Status = GetFactory<ActiveWasteWaterSystemStatusFactory>().Create(),
                WasteWaterSystemName = "1inZ"
            });

            var wwsActive2InY = GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocY, 
                Status = GetFactory<ActiveWasteWaterSystemStatusFactory>().Create(),
                WasteWaterSystemName = "2inY"
            });

            /* null operating center */

            var result = (CascadingActionResult)_target.ActiveByOperatingCenter();
            var wasteWaterSystems = ((IEnumerable<WasteWaterSystemDisplayItem>)result.Data).ToList();

            Assert.AreEqual(3, wasteWaterSystems.Count);
            Assert.AreEqual(wwsActive1InY.Id, wasteWaterSystems[0].Id);
            Assert.AreEqual(wwsActive1InZ.Id, wasteWaterSystems[1].Id);
            Assert.AreEqual(wwsActive2InY.Id, wasteWaterSystems[2].Id);

            /* valid operating center */

            result = (CascadingActionResult)_target.ActiveByOperatingCenter(ocZ.Id);
            wasteWaterSystems = ((IEnumerable<WasteWaterSystemDisplayItem>)result.Data).ToList();

            Assert.AreEqual(1, wasteWaterSystems.Count);
            Assert.AreEqual(wwsActive1InZ.Id, wasteWaterSystems[0].Id);

            /* 2 valid operating centers */

            result = (CascadingActionResult)_target.ActiveByOperatingCenter(ocZ.Id, ocY.Id);
            wasteWaterSystems = ((IEnumerable<WasteWaterSystemDisplayItem>)result.Data).ToList();

            Assert.AreEqual(3, wasteWaterSystems.Count);
            Assert.AreEqual(wwsActive1InY.Id, wasteWaterSystems[0].Id);
            Assert.AreEqual(wwsActive1InZ.Id, wasteWaterSystems[1].Id);
            Assert.AreEqual(wwsActive2InY.Id, wasteWaterSystems[2].Id);

            /* no valid operating centers */

            result = (CascadingActionResult)_target.ActiveByOperatingCenter(ocX.Id);
            wasteWaterSystems = ((IEnumerable<WasteWaterSystemDisplayItem>)result.Data).ToList();

            Assert.AreEqual(0, wasteWaterSystems.Count);
        }

        [TestMethod]
        public void TestActiveByStateOrOperatingCenterReturnsActiveWasteWaterSystem()
        {
            var ocX = GetFactory<UniqueOperatingCenterFactory>().Create();
            var ocY = GetFactory<UniqueOperatingCenterFactory>().Create();
            var ocZ = GetFactory<UniqueOperatingCenterFactory>().Create();

            GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocX,
                Status = GetFactory<PendingWasteWaterSystemStatusFactory>().Create()
            });

            GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocY,
                Status = GetFactory<InactiveWasteWaterSystemStatusFactory>().Create()
            });

            GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocY,
                Status = GetFactory<InactiveSeeNoteWasteWaterSystemStatusFactory>().Create()
            });

            var wwsActive1InY = GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocY,
                Status = GetFactory<ActiveWasteWaterSystemStatusFactory>().Create(),
                WasteWaterSystemName = "1inY"
            });

            var wwsActive1InZ = GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocZ,
                Status = GetFactory<ActiveWasteWaterSystemStatusFactory>().Create(),
                WasteWaterSystemName = "1inZ"
            });

            var wwsActive2InY = GetEntityFactory<WasteWaterSystem>().Create(new {
                OperatingCenter = ocY,
                Status = GetFactory<ActiveWasteWaterSystemStatusFactory>().Create(),
                WasteWaterSystemName = "2inY"
            });

            /* null operating center */

            var result = (CascadingActionResult)_target.ActiveByStateOrOperatingCenter(new[] { ocX.State.Id }, null);
            var wasteWaterSystems = ((IEnumerable<WasteWaterSystemDisplayItem>)result.Data).ToList();

            Assert.AreEqual(3, wasteWaterSystems.Count);
            Assert.AreEqual(wwsActive1InY.Id, wasteWaterSystems[0].Id);
            Assert.AreEqual(wwsActive1InZ.Id, wasteWaterSystems[1].Id);
            Assert.AreEqual(wwsActive2InY.Id, wasteWaterSystems[2].Id);

            /* valid operating center */

            result = (CascadingActionResult)_target.ActiveByStateOrOperatingCenter(null, new[]{ ocZ.Id });
            wasteWaterSystems = ((IEnumerable<WasteWaterSystemDisplayItem>)result.Data).ToList();

            Assert.AreEqual(1, wasteWaterSystems.Count);
            Assert.AreEqual(wwsActive1InZ.Id, wasteWaterSystems[0].Id);

            /* 2 valid state and operating center */

            result = (CascadingActionResult)_target.ActiveByStateOrOperatingCenter(new[]{ocX.State.Id}, new[]
                { ocY.Id });
            wasteWaterSystems = ((IEnumerable<WasteWaterSystemDisplayItem>)result.Data).ToList();

            Assert.AreEqual(2, wasteWaterSystems.Count);

            /* no valid operating centers */

            result = (CascadingActionResult)_target.ActiveByOperatingCenter(ocX.Id);
            wasteWaterSystems = ((IEnumerable<WasteWaterSystemDisplayItem>)result.Data).ToList();

            Assert.AreEqual(0, wasteWaterSystems.Count);
        }

        #endregion

        #region Lookup Data

        [TestMethod]
        public void TestSetLookUpDataForOperatingCenterSetsCorrectlyOnNew()
        {
            var opc1 = GetEntityFactory<OperatingCenter>().CreateList(5);
            var opc2 = GetEntityFactory<OperatingCenter>().Create(new { IsActive = true });

            _target.SetLookupData(ControllerAction.New);

            var opcs = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            Assert.AreNotEqual(opc1.Count, opcs.Count());
            Assert.AreEqual(1, opcs.Count());
            Assert.IsTrue(opc2.Id.ToString() == opcs.First().Value);
        }

        #endregion

        #endregion
    }
}
