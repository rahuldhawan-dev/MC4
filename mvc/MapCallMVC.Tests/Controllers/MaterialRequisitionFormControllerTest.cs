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

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class MaterialRequisitionFormControllerTest : MapCallMvcControllerTestBase<MaterialRequisitionFormController, EstimatingProject, EstimatingProjectRepository>
    {
        #region Constants

        public const RoleModules ROLE = MaterialRequisitionFormController.ROLE;

        #endregion

        #region Private Members
        
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/MaterialRequisitionForm/Show/", ROLE);
            });
        }

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // override needed because action returns a view model rather than the entity
            Assert.Inconclusive("Test me and the pdf version.");
        }

        [TestMethod]
        public void TestShowXLSExportsExcel()
        {
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var estimatingProject = GetEntityFactory<EstimatingProject>().Create(new
            {
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var material1 = GetEntityFactory<Material>().Create();
            var material2 = GetEntityFactory<Material>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
                .Create(new {OperatingCenter = operatingCenter, Material = material1, Cost = 10m});
            GetEntityFactory<OperatingCenterStockedMaterial>()
                .Create(new {OperatingCenter = operatingCenter, Material = material2, Cost = 15m});
            GetEntityFactory<EstimatingProjectMaterial>().Create(new
            {
                EstimatingProject = estimatingProject,
                Material = material1,
                Quantity = 1,
                AssetType = hydrantAssetType
            });
            GetEntityFactory<EstimatingProjectMaterial>().Create(new
            {
                EstimatingProject = estimatingProject,
                Material = material1,
                Quantity = 4,
                AssetType = valveAssetType
            });
            GetEntityFactory<EstimatingProjectMaterial>().Create(new
            {
                EstimatingProject = estimatingProject,
                Material = material2,
                Quantity = 3
            });

            Session.Clear();
            estimatingProject = Session.Load<EstimatingProject>(estimatingProject.Id);

            var result = _target.Show(estimatingProject.Id) as ExcelResult;
            var groupedMaterials = estimatingProject.GroupedMaterials.ToList();

            using (var helper = new ExcelResultTester(_container, result, true, 2))
            {
                helper.AreEqual(groupedMaterials[0].Material.PartNumber, "StockPartNumber");
                helper.AreEqual(groupedMaterials[1].Material.PartNumber, "StockPartNumber", 1);
                helper.AreEqual(groupedMaterials[0].Quantity, "Units");
                helper.AreEqual(groupedMaterials[1].Quantity, "Units", 1);
                helper.AreEqual(groupedMaterials[0].Material.Description, "SAPDescription");
                helper.AreEqual(groupedMaterials[1].Material.Description, "SAPDescription", 1);
            }
        }

        #endregion
    }
}
