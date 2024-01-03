using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class WaterQualityComplaintControllerTest : MapCallMvcControllerTestBase<WaterQualityComplaintController, WaterQualityComplaint>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateWaterQualityComplaint)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
                model.InitialLocalContact = GetEntityFactory<Employee>().Create().Id;
                model.State = GetEntityFactory<State>().Create().Id;
                model.Town = GetEntityFactory<Town>().Create().Id;
                model.PremiseNumber = "0000000000";
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditWaterQualityComplaint)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
                model.InitialLocalContact = GetEntityFactory<Employee>().Create().Id;
                model.State = GetEntityFactory<State>().Create().Id;
                model.Town = GetEntityFactory<Town>().Create().Id;
                model.PremiseNumber = "0000000000";
            };
        }

        #endregion
        
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = WaterQualityComplaintController.ROLE;

            Authorization.Assert(a => {
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/Search/", role);
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/Show/", role);
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/Index/", role);
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/New/", role, RoleActions.Add);
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/Create/", role, RoleActions.Add);
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/Edit/", role, RoleActions.Edit);
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/Update/", role, RoleActions.Edit);
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/Destroy/", role, RoleActions.Delete);
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/AddWaterQualityComplaintSampleResult/", role,
                        RoleActions.Edit);
                    a.RequiresRole("~/WaterQuality/WaterQualityComplaint/RemoveWaterQualityComplaintSampleResult/",
                        role, RoleActions.Edit);
                }
            );
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<WaterQualityComplaint>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditWaterQualityComplaint, WaterQualityComplaint>(eq, new {
                CustomerName = expected
            }));

            Assert.AreEqual(expected, Session.Get<WaterQualityComplaint>(eq.Id).CustomerName);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestShowSetsViewDataWorkOrderIdWhenAWorkOrderExists()
        {
            const long sapNotificationNumber = 1234567;
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { SAPNotificationNumber = sapNotificationNumber });
            var waterQualityComplaint = GetEntityFactory<WaterQualityComplaint>()
               .Create(new { OrcomOrderNumber = sapNotificationNumber.ToString() });
            
            var result = _target.Show(workOrder.Id) as ViewResult;
            
            Assert.IsNotNull(result.ViewData[WaterQualityComplaint.DisplayNames.WORK_ORDER_ID]);
            Assert.AreEqual(result.ViewData[WaterQualityComplaint.DisplayNames.WORK_ORDER_ID], workOrder.Id);
        }
        
        [TestMethod]
        public void TestShowDoesNotSetViewDataWorkOrderIdWhenAWorkOrderDoesNotExist()
        {
            const long complaintSAPNotificationNumber = 1234567;
            const long workOrderSAPNotificationNumber = 89101112;
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { SAPNotificationNumber = workOrderSAPNotificationNumber });
            var waterQualityComplaint = GetEntityFactory<WaterQualityComplaint>()
               .Create(new { OrcomOrderNumber = complaintSAPNotificationNumber.ToString() });
            
            var result = _target.Show(workOrder.Id) as ViewResult;
            
            Assert.IsNull(result.ViewData[WaterQualityComplaint.DisplayNames.WORK_ORDER_ID]);
            Assert.AreNotEqual(result.ViewData[WaterQualityComplaint.DisplayNames.WORK_ORDER_ID], workOrder.Id);
        }

        #endregion
    }
}
