using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Data.Models.Repositories;
using Contractors.Models;
using MapCall.Common.Testing.Data;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.Model.ViewModels.SAPDeviceDetail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class SAPDeviceDetailControllerTest : ContractorControllerTestBase<SAPDeviceDetailController, MapCall.Common.Model.Entities.WorkOrder, WorkOrderRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/SAPDeviceDetail/Index");
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // noop override: Index deals with SAP stuff. Tested below.
        }


        [TestMethod]
        public void TestIndexReturnsJsonSAPDeviceDetails()
        {
            var sapRepo = new Mock<ISAPDeviceRepository>();
            _container.Inject(sapRepo.Object);
            var results = new SAPDeviceCollection();
            results.EquipmentData = new[] {
                new SAPDeviceDetailResponse {
                    ActionCode = "actionCode",
                    DeviceLocation = "123456",
                    Manufacturer = "manufacturer",
                    MaterialNumber = "materialnumber"
                }
            };
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { DeviceLocation = (long)123456 });
            var search = new SearchSAPDeviceDetail { WorkOrderID = workOrder.Id, MeterSerialNumber = "000123"};
            sapRepo.Setup(x => x.Search(It.Is<SAPDeviceDetailRequest>(s => s.MeterSerialNumber == search.MeterSerialNumber && s.ActionCode == search.ActionCode && s.DeviceLocation == workOrder.DeviceLocation.ToString()))).Returns(results);

            InitializeControllerAndRequest($"~/SAP/SAPDeviceDetail/Index.json?meterManufacturerSerialNumber={search.MeterSerialNumber}&workOrderID={workOrder.Id}");

            var result = _target.Index(search) as JsonResult;

            Assert.IsNotNull(result);
        }

        #endregion
    }
}