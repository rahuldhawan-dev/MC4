using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SmartCoverAlertControllerTest : MapCallMvcControllerTestBase<SmartCoverAlertController, SmartCoverAlert>
    {
        #region Fields

        private Mock<INotificationService> _noteServ;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {

            _noteServ = new Mock<INotificationService>();
            _container.Inject(_noteServ.Object);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesAssets;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/SmartCoverAlert/Show", role);
                a.RequiresRole("~/FieldOperations/SmartCoverAlert/Search", role);
                a.RequiresRole("~/FieldOperations/SmartCoverAlert/Index", role);
                a.RequiresRole("~/FieldOperations/SmartCoverAlert/Update", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SmartCoverAlert/Edit", role, RoleActions.Edit);
            });
        }

        #region LookupData

        [TestMethod]
        public void TestSetLookUpDataForHighLevelAlarmTypeAndAlertTypeOnSearch()
        {
            var smartCoverAlertAlarmTypeList = GetEntityFactory<SmartCoverAlertAlarmType>().CreateList(5);
            var smartCoverAlertTypeList = GetEntityFactory<SmartCoverAlertType>().CreateList(5);

            _target.SetLookupData(ControllerAction.Search);

            var highLevelAlarmTypes = (IEnumerable<SelectListItem>)_target.ViewData["HighLevelAlarmType"];
            var alertTypes = (IEnumerable<SelectListItem>)_target.ViewData["AlertType"];

            Assert.AreEqual(smartCoverAlertAlarmTypeList.Count, highLevelAlarmTypes.Count());
            Assert.AreEqual(smartCoverAlertTypeList.Count, alertTypes.Count());
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<SmartCoverAlert>().Create();
            var date = DateTime.Now;

            _target.Update(_viewModelFactory.BuildWithOverrides<EditSmartCoverAlert, SmartCoverAlert>(eq, new {
                Acknowledged = true,
                AcknowledgedOn = date
            }));

            Assert.AreEqual(date, Session.Get<SmartCoverAlert>(eq.Id).AcknowledgedOn);
            Assert.IsTrue(Session.Get<SmartCoverAlert>(eq.Id).Acknowledged);
        }

        [TestMethod]
        public override void TestUpdateRedirectsToShowActionAfterSuccessfulSave()
        {
            // override needed due to redirection to index action
            var entity = GetEntityFactory<SmartCoverAlert>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditSmartCoverAlert, SmartCoverAlert>(entity, new { Id = entity.Id });
            var result = _target.Update(model) as RedirectToRouteResult;

            Assert.AreEqual("Show", result.RouteValues["action"]);

            model.IndexSearch = "Testing Update";
            result = _target.Update(model) as RedirectToRouteResult;

            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestUpdateSendsAcknowledgedNotification()
        {
            var sewerOpening = GetEntityFactory<SewerOpening>().Create();
            var alert = GetEntityFactory<SmartCoverAlert>().Create( new { SewerOpening  = sewerOpening });

            _target.Update(_viewModelFactory.BuildWithOverrides<EditSmartCoverAlert, SmartCoverAlert>(alert, new {
                Acknowledged = true,
                AcknowledgedOn = DateTime.Now
            }));

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        #endregion

        #region Index/Show

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var user = GetFactory<UserFactory>().Create();
            var sewerOpening = GetEntityFactory<SewerOpening>().Create();
            sewerOpening.TownSection = null;
            var applicationDescriptionType = GetFactory<SmartCoverAlertApplicationDescriptionTypeFactory>().Create();
            var entity0 = GetEntityFactory<SmartCoverAlert>().Create(new {
                ApplicationDescription = applicationDescriptionType,
                SewerOpening = sewerOpening,
                AlertId = 12345,
                SewerOpeningNumber = "67890",
                Latitude = (decimal)23.1,
                Longitude = (decimal)23.2,
                Elevation = (decimal)23.3,
                SensorToBottom = (decimal)23.4,
                ManholeDepth = (decimal)23.5,
                Acknowledged = true,
                AcknowledgedOn = DateTime.Now,
                AcknowledgedBy = user,
                PowerPackVoltage = "Testing 1",
                WaterLevelAboveBottom = "Testing 2",
                Temperature = "Testing 3",
                SignalStrength = "Testing 4",
                SignalQuality = "Testing 5",
                HighAlarmThreshold = (decimal)23.6
            });
            var entity1 = GetEntityFactory<SmartCoverAlert>().Create(new {
                ApplicationDescription = applicationDescriptionType
            });
            var search = new SearchSmartCoverAlert();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity0.AlertId, "AlertId");
                helper.AreEqual(sewerOpening.State, "State");
                helper.AreEqual(sewerOpening.OperatingCenter, "OperatingCenter");
                helper.AreEqual(sewerOpening.Town, "Town");
                helper.IsNull("TownSection");
                helper.AreEqual(entity0.SewerOpeningNumber, "SewerOpeningNumber");
                helper.AreEqual(entity0.Latitude, "Latitude");
                helper.AreEqual(entity0.Longitude, "Longitude");
                helper.AreEqual(entity0.Elevation, "Elevation");
                helper.AreEqual(entity0.SensorToBottom, "SensorToBottom");
                helper.AreEqual(entity0.DateReceived, "DateReceived");
                helper.AreEqual(entity0.Acknowledged, "Acknowledged");
                helper.AreEqual(entity0.AcknowledgedBy, "AcknowledgedBy");
                helper.AreEqual(entity0.AcknowledgedOn, "AcknowledgedOn");
                helper.AreEqual(entity0.PowerPackVoltage, "PowerPackVoltage");
                helper.AreEqual(entity0.WaterLevelAboveBottom, "WaterLevelAboveBottom");
                helper.AreEqual(entity0.Temperature, "Temperature");
                helper.AreEqual(entity0.SignalStrength, "SignalStrength");
                helper.AreEqual(entity0.SignalQuality, "SignalQuality");
                helper.AreEqual(entity0.HighAlarmThreshold, "HighAlarmThreshold");
                helper.AreEqual(sewerOpening.Id, "SewerOpeningId");
                helper.AreEqual(entity0.SmartCoverAlertAlarms, "Notes");

                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/SmartCoverAlert/Index.map");
            var search = new SearchSmartCoverAlert();
            var result = _target.Index(search);
            Assert.IsInstanceOfType(result, typeof(MapResult));
        }

        [TestMethod]
        public void TestShowRespondsToMapAndFragment()
        {
            var applicationDescriptionType = GetFactory<SmartCoverAlertApplicationDescriptionTypeFactory>().Create();
            var good = GetEntityFactory<SmartCoverAlert>().Create(new {
                ApplicationDescription = applicationDescriptionType
            });
            var bad = GetEntityFactory<SmartCoverAlert>().Create(new {
                ApplicationDescription = applicationDescriptionType
            });
            InitializeControllerAndRequest("~/FieldOperations/SmartCoverAlert/Show" + good.Id + ".map");

            var result = (MapResult)_target.Show(good.Id);
            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();

            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));

            InitializeControllerAndRequest("~/FieldOperations/SmartCoverAlert/Show" + good.Id + ".frag");

            var fragResult = (PartialViewResult)_target.Show(good.Id);

            MvcAssert.IsViewNamed(fragResult, "_ShowPopup");
            Assert.AreSame(good, fragResult.Model);
        }

        #endregion
    }
}
