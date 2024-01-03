using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MMSINC.Data.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Contractors.Tests.Controllers
{
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class MainBreakControllerTest : ContractorControllerTestBase<MainBreakController, MainBreak, SecuredRepositoryBase<MainBreak, ContractorUser>>
    {
        #region Setup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateReturnsPartialShowViewOnSuccess = true;
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.ExpectedShowViewName = "_Show";
            options.ShowReturnsPartialView = true;
            options.UpdateReturnsPartialShowViewOnSuccess = true;
        }

        #endregion

        #region Private Methods

        private void SetupDropDownData()
        {
            GetFactory<MainConditionFactory>().CreateList(3);
            GetFactory<MainFailureTypeFactory>().CreateList(3);
            GetFactory<ServiceSizeFactory>().CreateList(3);
            GetFactory<MainBreakMaterialFactory>().CreateList(3);
            GetFactory<MainBreakSoilConditionFactory>().CreateList(3);
            GetFactory<MainBreakDisinfectionMethodFactory>().CreateList(3);
            GetFactory<MainBreakFlushMethodFactory>().CreateList(3);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/MainBreak/Create");
                a.RequiresLoggedInUserOnly("~/MainBreak/New");
                a.RequiresLoggedInUserOnly("~/MainBreak/Show");
               // a.RequiresLoggedInUserOnly("~/MainBreak/Index");
                a.RequiresLoggedInUserOnly("~/MainBreak/Destroy");
                a.RequiresLoggedInUserOnly("~/MainBreak/Edit");
                a.RequiresLoggedInUserOnly("~/MainBreak/Update");
            });
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to parameter on New action.
            SetupDropDownData();
            var workOrderId = 919;

            var result = _target.New(workOrderId);
            var resultModel = (CreateMainBreak)((ViewResultBase)result).Model;

            Assert.AreEqual(resultModel.WorkOrder, workOrderId);
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("This returns an EmptyResult for some reason.");
        }

        #region Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            SetupDropDownData();
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var mainBreak = GetFactory<GenericMainBreakFactory>().Create(new { WorkOrder = workOrder });
            var mainFailureType = GetFactory<MainFailureTypeFactory>().Create();
            var serviceSize = GetFactory<ServiceSizeFactory>().Create();
            var mainCondition = GetFactory<MainConditionFactory>().Create();
            var mainBreakMaterial = GetFactory<MainBreakMaterialFactory>().Create();
            var mainBreakSoilCondition = GetFactory<MainBreakSoilConditionFactory>().Create();
            var mainBreakDisinfectionMethod = GetFactory<MainBreakDisinfectionMethodFactory>().Create();
            var mainBreakFlushMethod = GetFactory<MainBreakFlushMethodFactory>().Create();
            var model = _viewModelFactory.BuildWithOverrides<EditMainBreak, MainBreak>(mainBreak, new {
                BoilAlertIssued = true,
                ChlorineResidual = 2.0m,
                CustomersAffected = 2,
                Depth = 2.0m,
                ShutdownTime = 2.0m,
                MainFailureType = mainFailureType.Id,
                ServiceSize = serviceSize.Id,
                MainCondition = mainCondition.Id,
                MainBreakMaterial = mainBreakMaterial.Id,
                MainBreakSoilCondition =
                    mainBreakSoilCondition.Id,
                MainBreakDisinfectionMethod =
                    mainBreakDisinfectionMethod.Id,
                MainBreakFlushMethod =
                    mainBreakFlushMethod.Id,
                ReplacedWith = mainBreakMaterial.Id,
                FootageReplaced = 5
            });

            var result = (PartialViewResult)_target.Update(model);
            var actual = (MainBreak)result.Model;

            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(actual.BoilAlertIssued, mainBreak.BoilAlertIssued);
            Assert.AreEqual(actual.ChlorineResidual, mainBreak.ChlorineResidual);
            Assert.AreEqual(actual.CustomersAffected, mainBreak.CustomersAffected);
            Assert.AreEqual(actual.Depth, mainBreak.Depth);
            Assert.AreEqual(actual.ShutdownTime, mainBreak.ShutdownTime);
            Assert.AreEqual(actual.MainFailureType.Id, mainFailureType.Id);
            Assert.AreEqual(actual.ServiceSize.Id, serviceSize.Id);
            Assert.AreEqual(actual.MainCondition.Id, mainCondition.Id);
            Assert.AreEqual(actual.MainBreakMaterial.Id, mainBreakMaterial.Id);
            Assert.AreEqual(actual.MainBreakSoilCondition.Id, mainBreakSoilCondition.Id);
            Assert.AreEqual(actual.MainBreakDisinfectionMethod.Id, mainBreakDisinfectionMethod.Id);
            Assert.AreEqual(actual.MainBreakFlushMethod.Id, mainBreakFlushMethod.Id);
            Assert.AreEqual(mainBreakMaterial.Id, actual.ReplacedWith.Id);
            Assert.AreEqual(5, actual.FootageReplaced);
        }

        #endregion

        #endregion
    }
}
