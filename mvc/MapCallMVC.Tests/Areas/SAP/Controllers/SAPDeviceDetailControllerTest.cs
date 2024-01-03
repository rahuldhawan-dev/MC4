using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.SAP.Controllers;
using MapCallMVC.Areas.SAP.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.ViewModels;
using MapCall.SAP.Model.ViewModels.SAPDeviceDetail;

namespace MapCallMVC.Tests.Areas.SAP.Controllers
{
    [TestClass]
    public class SAPDeviceDetailControllerTest : MapCallMvcControllerTestBase<SAPDeviceDetailController, WorkOrder>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/SAP/SAPDeviceDetail/Index", role, RoleActions.Read);
            });
        }
        
        #region Index

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            var sapRepo = new Mock<ISAPDeviceRepository>();
            _container.Inject(sapRepo.Object);
            var results = new SAPDeviceCollection();
            results.EquipmentData = new SAPDeviceDetailResponse[] {new SAPDeviceDetailResponse {
                ActionCode = "actionCode",
                DeviceLocation = "123456",
                Manufacturer = "manufacturer",
                MaterialNumber = "materialnumber"
            }};
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { DeviceLocation = (long)123456 });
            var search = new SearchSAPDeviceDetail { WorkOrderID = workOrder.Id, MeterSerialNumber = "000123"};
            sapRepo.Setup(x => x.Search(It.Is<SAPDeviceDetailRequest>(s => s.MeterSerialNumber == search.MeterSerialNumber && s.ActionCode == search.ActionCode && s.DeviceLocation == workOrder.DeviceLocation.ToString()))).Returns(results);

            InitializeControllerAndRequest($"~/SAP/SAPDeviceDetail/Index.json?meterManufacturerSerialNumber={search.MeterSerialNumber}&workOrderID={workOrder.Id}");

            var result = _target.Index(search) as JsonResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }

        #endregion
    }
}