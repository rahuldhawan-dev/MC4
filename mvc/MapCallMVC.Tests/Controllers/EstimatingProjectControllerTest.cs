using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EstimatingProjectControllerTest : MapCallMvcControllerTestBase<EstimatingProjectController, EstimatingProject, EstimatingProjectRepository>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = Request.CreateAndInitializeController<EstimatingProjectController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Contractor is needed because of a RequiredWhen on the property.
            // This comes up because the PermitType that's generated during this
            // test happens to make the Contractor field required. 
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateEstimatingProject)vm;
                model.Contractor = GetEntityFactory<Contractor>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditEstimatingProject)vm;
                model.Contractor = GetEntityFactory<Contractor>().Create().Id;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/EstimatingProject/Search/", ROLE);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/Index/", ROLE);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/Show/", ROLE);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/Edit/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/Update/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/New/", ROLE, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/Create/", ROLE, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/Destroy/", ROLE, RoleActions.Delete);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/AddEstimatingProjectOtherCost/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/RemoveEstimatingProjectOtherCost/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/AddEstimatingProjectMaterial/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/RemoveEstimatingProjectMaterial/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/AddEstimatingProjectCompanyLaborCost/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/RemoveEstimatingProjectCompanyLaborCost/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/AddEstimatingProjectContractorLaborCost/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/RemoveEstimatingProjectContractorLaborCost/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/AddEstimatingProjectPermit/", ROLE, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/EstimatingProject/RemoveEstimatingProjectPermit/", ROLE, RoleActions.Edit);
            });
        }

        #endregion

        #region New

        [TestMethod]
        public void TestNewOnlyIncludesActiveOperatingCentersInLookupData()
        {
            var activeOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var inactiveOpc = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _target.New();

            var opcData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];
            Assert.IsTrue(opcData.Any(x => x.Value == activeOpc.Id.ToString()));
            Assert.IsFalse(opcData.Any(x => x.Value == inactiveOpc.Id.ToString()));
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestOnlyMaterialsStockedByTheOperatingCenterTheEstimatingProjectBelongsToAreAvailable()
        {
            var entityOperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var extraOperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var entity = GetEntityFactory<EstimatingProject>().Create(new {OperatingCenter = entityOperatingCenter});
            var availableMaterial = GetEntityFactory<Material>().Create(new {
                PartNumber = "Hg", Description = "Mercury"
            });
            var unavailableMaterial = GetEntityFactory<Material>().Create(new {
                PartNumber = "Au", Description = "Gold"
            });
            GetEntityFactory<OperatingCenterStockedMaterial>().Create(new {
                OperatingCenter = entityOperatingCenter, Material = availableMaterial
            });
            GetEntityFactory<OperatingCenterStockedMaterial>().Create(new {
                OperatingCenter = extraOperatingCenter, Material = unavailableMaterial
            });

            Session.Flush();
            Session.Clear();

            _target.Show(entity.Id);

            _target.AssertHasDropDownData((IEnumerable<MaterialDisplayItem>)new[] {availableMaterial}.AsQueryable().SelectDynamic<Material, MaterialDisplayItem>().Result, m => m.Id, m => m.Display, "Material");
            _target.AssertDoesNotHaveDropDownData((IEnumerable<MaterialDisplayItem>)new[] {unavailableMaterial}.AsQueryable().SelectDynamic<Material, MaterialDisplayItem>().Result, m => m.Id, m => m.Display, "Material");
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<EstimatingProject>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<EstimatingProject>().Create(new {Description = "description 1"});
            var search = new SearchEstimatingProject();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<EstimatingProject>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEstimatingProject, EstimatingProject>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<EstimatingProject>(eq.Id).Description);
        }

        #endregion

        #region Delete

        [TestMethod]
        public void TestDeletingProjectDeletesAnyAdditionalCosts()
        {
            var project = GetEntityFactory<EstimatingProject>().Create();

            _target.AddEstimatingProjectOtherCost(new AddEstimatingProjectOtherCost(_container) { 
                Id = project.Id,
                Quantity = 1,
                Description = "vOv",
                Cost = 6.66m,
                AssetType = GetFactory<EquipmentAssetTypeFactory>().Create().Id
            });

            MyAssert.CausesDecrease(() => _target.Destroy(project.Id),
                _container.GetInstance<RepositoryBase<EstimatingProjectOtherCost>>().GetAll().Count);
        }

        #endregion

        #region Add/Remove Additional Costs

        [TestMethod]
        public void TestAddEstimatingProjectOtherCostCreatesAndAddsOtherCost()
        {
            var project = GetEntityFactory<EstimatingProject>().Create();

            MyAssert.CausesIncrease(() => _target.AddEstimatingProjectOtherCost(new AddEstimatingProjectOtherCost(_container) { 
                Id = project.Id,
                Quantity = 1,
                Description = "vOv",
                Cost = 6.66m,
                AssetType = GetFactory<EquipmentAssetTypeFactory>().Create().Id
            }), _container.GetInstance<RepositoryBase<EstimatingProjectOtherCost>>().GetAll().Count);
        }

        [TestMethod]
        public void TestRemoveEstimatingProjectOtherCostRemovesOtherCostRecord()
        {
            var project = GetEntityFactory<EstimatingProject>().Create();

            _target.AddEstimatingProjectOtherCost(new AddEstimatingProjectOtherCost(_container) { 
                Id = project.Id,
                Quantity = 1,
                Description = "vOv",
                Cost = 6.66m,
                AssetType = GetFactory<EquipmentAssetTypeFactory>().Create().Id
            });

            project = Session.Load<EstimatingProject>(project.Id);

            // ReSharper disable AccessToModifiedClosure
            MyAssert.CausesDecrease(() => _target.RemoveEstimatingProjectOtherCost(
                _viewModelFactory.BuildWithOverrides<RemoveEstimatingProjectOtherCost, EstimatingProject>(project, new {
                    OtherCostId = project.OtherCosts.First().Id
                })), _container.GetInstance<RepositoryBase<EstimatingProjectOtherCost>>().GetAll().Count);
            // ReSharper restore AccessToModifiedClosure

            project = Session.Load<EstimatingProject>(project.Id);

            Assert.AreEqual(0, project.OtherCosts.Count);
        }

        #endregion

        #region Add/Remove Materials

        [TestMethod]
        public void TestAddEstimatingProjectMaterialCreatesAndAddsMaterial()
        {
            var project = GetEntityFactory<EstimatingProject>().Create();
            var material = GetEntityFactory<Material>().Create();
            var assetType = GetFactory<ValveAssetTypeFactory>().Create();

            MyAssert.CausesIncrease(() => _target.AddEstimatingProjectMaterial(new AddEstimatingProjectMaterial(_container) {
                Id = project.Id,
                Material = material.Id,
                Quantity = 3,
                AssetType = assetType.Id
            }), _container.GetInstance<RepositoryBase<EstimatingProjectMaterial>>().GetAll().Count);
        }

        [TestMethod]
        public void TestRemoveEstimatingProjectMaterialRemovesMaterial()
        {
            var project = GetEntityFactory<EstimatingProject>().Create();
            var material = GetEntityFactory<Material>().Create();
            var assetType = GetFactory<ValveAssetTypeFactory>().Create();

            _target.AddEstimatingProjectMaterial(new AddEstimatingProjectMaterial(_container) {
                Id = project.Id,
                Material = material.Id,
                Quantity = 10,
                AssetType = assetType.Id
            });

            project = Session.Load<EstimatingProject>(project.Id);

            // ReSharper disable AccessToModifiedClosure
            MyAssert.CausesDecrease(() => _target.RemoveEstimatingProjectMaterial(
                _viewModelFactory.BuildWithOverrides<RemoveEstimatingProjectMaterial, EstimatingProject>(project, new {
                    EstimatingProjectMaterialId = project.Materials.First().Id
                })), _container.GetInstance<RepositoryBase<EstimatingProjectMaterial>>().GetAll().Count);
            // ReSharper restore AccessToModifiedClosure

            project = Session.Load<EstimatingProject>(project.Id);

            Assert.AreEqual(0, project.Materials.Count);
        }

        #endregion

        #region Add/Remove Company Labor Costs

        [TestMethod]
        public void TestAddEstimatingProjectCompanyLaborCostsCreatesAndAddsCompanyLaborCosts()
        {
            var project = GetEntityFactory<EstimatingProject>().Create();
            var companyLaborCost = GetEntityFactory<CompanyLaborCost>().Create(new { Description = "Foo", Unit = "Ea", Cost = 10.0m});
            var assetType = _container.GetInstance<ValveAssetTypeFactory>().Create();

            MyAssert.CausesIncrease(() => _target.AddEstimatingProjectCompanyLaborCost(_viewModelFactory.BuildWithOverrides<AddEstimatingProjectCompanyLaborCost, EstimatingProject>(project, new {
                Id = project.Id, 
                CompanyLaborCost = companyLaborCost.Id,
                Quantity = 2,
                AssetType = assetType.Id
            })), _container.GetInstance<RepositoryBase<EstimatingProjectCompanyLaborCost>>().GetAll().Count);
        }

        [TestMethod]
        public void TestRemoveEstimatingProjectCompanyLaborCostRemovesCompanyLaborCost()
        {
            var project = GetEntityFactory<EstimatingProject>().Create();
            var companyLaborCost = GetEntityFactory<CompanyLaborCost>().Create(new { Description = "Foo", Unit = "Ea", Cost = 10.0m });
            var assetType = _container.GetInstance<ValveAssetTypeFactory>().Create();

            _target.AddEstimatingProjectCompanyLaborCost(_viewModelFactory.BuildWithOverrides<AddEstimatingProjectCompanyLaborCost, EstimatingProject>(project, new {
                Id = project.Id,
                CompanyLaborCost = companyLaborCost.Id,
                Quantity = 4,
                AssetType = assetType.Id
            }));

            project = Session.Load<EstimatingProject>(project.Id);

            // ReSharper disable AccessToModifiedClosure
            MyAssert.CausesDecrease(() => _target.RemoveEstimatingProjectCompanyLaborCost(
                _viewModelFactory.BuildWithOverrides<RemoveEstimatingProjectCompanyLaborCost, EstimatingProject>(
                    project, new {
                        EstimatingProjectCompanyLaborCostId = project.CompanyLaborCosts.First().Id
                    })), _container.GetInstance<RepositoryBase<EstimatingProjectCompanyLaborCost>>().GetAll().Count);
            // ReSharper restore AccessToModifiedClosure

            project = Session.Load<EstimatingProject>(project.Id);

            Assert.AreEqual(0, project.CompanyLaborCosts.Count);
        }

        #endregion

        #region Add/Remove Contractor Labor Costs

        [TestMethod]
        public void TestAddEstimatingProjectContractorLaborCostsCreatesAndAddsContractorLaborCosts()
        {
            var project = GetEntityFactory<EstimatingProject>().Create();
            var ContractorLaborCost = GetEntityFactory<ContractorLaborCost>().Create(new { Description = "Foo", Unit = "Ea", Cost = 10.0m});
            var assetType = _container.GetInstance<ValveAssetTypeFactory>().Create();

            MyAssert.CausesIncrease(() => _target.AddEstimatingProjectContractorLaborCost(new AddEstimatingProjectContractorLaborCost(_container) {
                Id = project.Id, 
                ContractorLaborCost = ContractorLaborCost.Id,
                Quantity = 2,
                AssetType = assetType.Id
            }), _container.GetInstance<RepositoryBase<EstimatingProjectContractorLaborCost>>().GetAll().Count);
        }

        [TestMethod]
        public void TestRemoveEstimatingProjectContractorLaborCostRemovesContractorLaborCost()
        {
            var project = GetEntityFactory<EstimatingProject>().Create();
            var ContractorLaborCost = GetEntityFactory<ContractorLaborCost>().Create(new { Description = "Foo", Unit = "Ea", Cost = 10.0m });
            var assetType = _container.GetInstance<ValveAssetTypeFactory>().Create();

            _target.AddEstimatingProjectContractorLaborCost(new AddEstimatingProjectContractorLaborCost(_container) {
                Id = project.Id,
                ContractorLaborCost = ContractorLaborCost.Id,
                Quantity = 4,
                AssetType = assetType.Id
            });

            project = Session.Load<EstimatingProject>(project.Id);

            // ReSharper disable AccessToModifiedClosure
            MyAssert.CausesDecrease(() => _target.RemoveEstimatingProjectContractorLaborCost(
                _viewModelFactory.BuildWithOverrides<RemoveEstimatingProjectContractorLaborCost, EstimatingProject>(
                    project, new {
                        EstimatingProjectContractorLaborCostId = project.ContractorLaborCosts.First().Id
                    })), _container.GetInstance<RepositoryBase<EstimatingProjectContractorLaborCost>>().GetAll().Count);
            // ReSharper restore AccessToModifiedClosure

            project = Session.Load<EstimatingProject>(project.Id);

            Assert.AreEqual(0, project.ContractorLaborCosts.Count);
        }

        #endregion
    }
}
