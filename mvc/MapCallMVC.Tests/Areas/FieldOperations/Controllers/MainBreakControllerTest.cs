using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.MainBreaks;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class MainBreakControllerTest : MapCallMvcControllerTestBase<MainBreakController, MainBreak>
    {
        #region Setup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            options.CreateReturnsPartialShowViewOnSuccess = true;
            options.NewReturnsPartialView = true;
            options.ExpectedNewViewName = "_New";
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.ShowReturnsPartialView = true;
            options.UpdateReturnsPartialShowViewOnSuccess = true;
            options.ExpectedShowViewName = "_Show";
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/MainBreak/New");
                a.RequiresLoggedInUserOnly("~/MainBreak/Create");
                a.RequiresLoggedInUserOnly("~/MainBreak/Edit");
                a.RequiresLoggedInUserOnly("~/MainBreak/Update");
                a.RequiresLoggedInUserOnly("~/MainBreak/Show");
                a.RequiresLoggedInUserOnly("~/MainBreak/Destroy");
            });
        }

        #endregion

        #region Create/New

        [TestMethod]
        public void TestCreateCreatesMainBreak()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var serviceSize = GetFactory<ServiceSizeFactory>().Create();
            var mainBreakMaterial = GetFactory<MainBreakMaterialFactory>().Create();
            var mainCondition = GetFactory<MainConditionFactory>().Create();
            var mainFailureType = GetFactory<MainFailureTypeFactory>().Create();
            var mainBreakSoilCondition = GetFactory<MainBreakSoilConditionFactory>().Create();
            var mainBreakDisInfectionMethod = GetFactory<MainBreakDisinfectionMethodFactory>().Create();
            var mainBreakFlushMethod = GetFactory<MainBreakFlushMethodFactory>().Create();

            var model = new CreateMainBreak(_container) {
                WorkOrder = workOrder.Id,
                BoilAlertIssued = true,
                ChlorineResidual = 17.0M,
                CustomersAffected = 23,
                Depth = 6.0M,
                MainBreakDisinfectionMethod = mainBreakDisInfectionMethod.Id,
                MainBreakFlushMethod = mainBreakFlushMethod.Id,
                MainBreakMaterial = mainBreakMaterial.Id,
                MainBreakSoilCondition = mainBreakSoilCondition.Id,
                MainCondition = mainCondition.Id,
                MainFailureType = mainFailureType.Id,
                ServiceSize = serviceSize.Id,
                ShutdownTime = 4.5M
            };

            var result = (PartialViewResult)_target.Create(model);
            var resultModel = (MainBreak)result.Model;

            Assert.AreEqual(17.0M, resultModel.ChlorineResidual);
            Assert.AreEqual(23, resultModel.CustomersAffected);
            Assert.AreEqual(6.0M, resultModel.Depth);
            Assert.AreEqual(4.5M, resultModel.ShutdownTime);
            Assert.AreEqual(mainBreakDisInfectionMethod.Description, resultModel.MainBreakDisinfectionMethod?.Description);
            Assert.AreEqual(mainBreakFlushMethod.Description, resultModel.MainBreakFlushMethod?.Description);
            Assert.AreEqual(mainBreakMaterial.Description, resultModel.MainBreakMaterial?.Description);
            Assert.AreEqual(mainBreakSoilCondition.Description, resultModel.MainBreakSoilCondition?.Description);
            Assert.AreEqual(mainCondition.Description, resultModel.MainCondition?.Description);
            Assert.AreEqual(mainFailureType.Description, resultModel.MainFailureType?.Description);
            Assert.AreEqual(serviceSize.Description, resultModel.ServiceSize?.Description);
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();

            var result = (PartialViewResult)_target.New(workOrder.Id);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(CreateMainBreak));
            Assert.AreEqual(((CreateMainBreak)result.Model).WorkOrder, workOrder.Id);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var expected = 17;
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            var mainBreak = GetEntityFactory<MainBreak>().Create(new {
                WorkOrder = workOrder,
                CustomersAffected = 23
            });

            var model = _viewModelFactory.BuildWithOverrides<EditMainBreak, MainBreak>(mainBreak, new {
                CustomersAffected = expected
            });

            var result = (PartialViewResult)_target.Update(model);
            var resultMaterialUsed = (MainBreak)result.Model;

            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(expected, resultMaterialUsed.CustomersAffected);
        }

        #endregion
    }
}
