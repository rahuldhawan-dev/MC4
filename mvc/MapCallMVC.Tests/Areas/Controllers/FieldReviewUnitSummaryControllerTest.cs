using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class FieldReviewUnitSummaryControllerTest : MapCallMvcControllerTestBase<FieldReviewUnitSummaryController, EstimatingProject, EstimatingProjectRepository>
    {
        #region Constants

        public const RoleModules ROLE = FieldReviewUnitSummaryController.ROLE;

        #endregion
        
        #region Private Members

        private User _user;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _now = DateTime.Now;
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/ProjectManagement/FieldReviewUnitSummary/Show/", ROLE);
            });
        }

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // override needed because the action creates a view model rather
            // than returning the entity type.
            Assert.Inconclusive("Test me. Also test my PDF version.");
        }

        [TestMethod]
        public void TestShowXLSExportsExcel()
        {
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var town = GetEntityFactory<Town>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var contractor2 = GetEntityFactory<Contractor>().Create();
            var estimatingProject = GetEntityFactory<EstimatingProject>().Create(new
            {
                Contractor = contractor,
                OperatingCenter = operatingCenter,
                Town = town,
                ProjectName = "Estimating Project X"
            });
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var cost1 = GetEntityFactory<ContractorLaborCost>().Create(new
            {
                Cost = 5.55m
            });
            var cost2 = GetEntityFactory<ContractorLaborCost>().Create(new
            {
                Cost = 5.55m
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new
            {
                AssetType = hydrantAssetType,
                EstimatingProject = estimatingProject,
                ContractorLaborCost = cost1,
                Quantity = 1
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new
            {
                AssetType = valveAssetType,
                EstimatingProject = estimatingProject,
                ContractorLaborCost = cost1,
                Quantity = 1
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new
            {
                AssetType = valveAssetType,
                EstimatingProject = estimatingProject,
                ContractorLaborCost = cost2,
                Quantity = 1
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new
            {
                AssetType = hydrantAssetType,
                EstimatingProject = estimatingProject,
                ContractorLaborCost = cost2,
                Quantity = 4
            });
            GetEntityFactory<ContractorOverrideLaborCost>().Create(new
            {
                EffectiveDate = _now.AddSeconds(1),
                Cost = 6.66m,
                ContractorLaborCost = cost1,
                Contractor = contractor2,
                OperatingCenter = operatingCenter
            });
            Session.Clear();
            estimatingProject = Session.Load<EstimatingProject>(estimatingProject.Id);

            var result = _target.Show(estimatingProject.Id) as ExcelResult;
            var groupedContractorLaborCosts = estimatingProject.GroupedContractorLaborCosts.ToList();

            using (var helper = new ExcelResultTester(_container, result, true, 2))
            {
                helper.AreEqual(groupedContractorLaborCosts[0].ContractorLaborCost.Description, "Description");
                helper.AreEqual(groupedContractorLaborCosts[1].ContractorLaborCost.Description, "Description", 1);
                helper.AreEqual(groupedContractorLaborCosts[0].Quantity, "Quantity");
                helper.AreEqual(groupedContractorLaborCosts[1].Quantity, "Quantity", 1);
                helper.AreEqual(groupedContractorLaborCosts[0].ContractorLaborCost.Unit, "Unit");
                helper.AreEqual(groupedContractorLaborCosts[1].ContractorLaborCost.Unit, "Unit", 1);
            }
        }

        #endregion
    }
}