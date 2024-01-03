using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Models
{
    [TestClass]
    public class PipelineProjectCostEstimateTest : MapCallMvcInMemoryDatabaseTestBase<EstimatingProject>
    {
        #region Fields

        private User _user;
        private Mock<IAuthenticationService<User>> _authServ;

        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IEstimatingProjectRepository>().Use<EstimatingProjectRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Asset Specific

        #region Main

        [TestMethod]
        public void TestMainMaterialCostIsAccurate()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var mainAssetType = GetFactory<MainAssetTypeFactory>().Create();
            var material = GetEntityFactory<Material>().Create();
            var otherMaterial = GetEntityFactory<Material>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
                .Create(new {OperatingCenter = operatingCenter, Material = material, Cost = 15m});
            GetEntityFactory<OperatingCenterStockedMaterial>()
                .Create(new {OperatingCenter = operatingCenter, Material = otherMaterial, Cost = 10m});
            var estimatingProject = GetEntityFactory<EstimatingProject>().Create(new {OperatingCenter = operatingCenter});
            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(estimatingProject);

            Assert.AreEqual(0, target.GetCostForAssetType(mainAssetType, EstimatingProject.CostType.Material));

            GetEntityFactory<EstimatingProjectMaterial>().Create(new {
                AssetType = mainAssetType,
                Material = material,
                Quantity = 2,
                EstimatingProject = estimatingProject
            });
            GetEntityFactory<EstimatingProjectMaterial>().Create(new {
                AssetType = mainAssetType,
                Material = otherMaterial,
                Quantity = 1,
                EstimatingProject = estimatingProject
            });
            GetEntityFactory<EstimatingProjectMaterial>().Create(new  { AssetType = valveAssetType });
            Session.Flush();
            Session.Clear();
            target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(Session.Load<EstimatingProject>(estimatingProject.Id));

            Assert.AreEqual(40, target.GetCostForAssetType(mainAssetType, EstimatingProject.CostType.Material));
        }

        [TestMethod]
        public void TestMainContractorLaborCostIsAccurate()
        {
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var mainAssetType = GetFactory<MainAssetTypeFactory>().Create();
            var contractorLaborCost = GetEntityFactory<ContractorLaborCost>().Create(new { Cost = 15m });
            var otherContractorLaborCost = GetEntityFactory<ContractorLaborCost>().Create(new { Cost = 25m });

            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(GetEntityFactory<EstimatingProject>().Create());

            Assert.AreEqual(0, target.GetCostForAssetType(mainAssetType, EstimatingProject.CostType.ContractorLabor));

            target.ContractorLaborCosts.Add(new EstimatingProjectContractorLaborCost
            {
                AssetType = mainAssetType,
                ContractorLaborCost = contractorLaborCost,
                Quantity = 1
            });
            target.ContractorLaborCosts.Add(new EstimatingProjectContractorLaborCost
            {
                AssetType = mainAssetType,
                ContractorLaborCost = otherContractorLaborCost,
                Quantity = 1
            });
            target.ContractorLaborCosts.Add(new EstimatingProjectContractorLaborCost
            {
                AssetType = valveAssetType,
                ContractorLaborCost = otherContractorLaborCost,
                Quantity = 1
            });

            Assert.AreEqual(40, target.GetCostForAssetType(mainAssetType, EstimatingProject.CostType.ContractorLabor));
        }

        [TestMethod]
        public void TestMainCompanyLaborCostIsAccruate()
        {
            var equipmentAssetType = GetFactory<EquipmentAssetTypeFactory>().Create();
            var mainAssetType = GetFactory<MainAssetTypeFactory>().Create();
            var companyLaborCost = GetEntityFactory<CompanyLaborCost>().Create(new { Cost = 22m });
            var otherCompanyLaborCost = GetEntityFactory<CompanyLaborCost>().Create(new { Cost = 23m });

            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(GetEntityFactory<EstimatingProject>().Create());

            Assert.AreEqual(0, target.GetCostForAssetType(mainAssetType, EstimatingProject.CostType.CompanyLabor));

            target.CompanyLaborCosts.Add(new EstimatingProjectCompanyLaborCost
            {
                AssetType = equipmentAssetType,
                CompanyLaborCost = companyLaborCost,
                Quantity = 2
            });
            target.CompanyLaborCosts.Add(new EstimatingProjectCompanyLaborCost
            {
                AssetType = mainAssetType,
                CompanyLaborCost = otherCompanyLaborCost,
                Quantity = 2
            });
            target.CompanyLaborCosts.Add(new EstimatingProjectCompanyLaborCost
            {
                AssetType = mainAssetType,
                CompanyLaborCost = companyLaborCost,
                Quantity = 2
            });

            Assert.AreEqual(90, target.GetCostForAssetType(mainAssetType, EstimatingProject.CostType.CompanyLabor));
        }

        [TestMethod]
        public void TestMainPermitCostIsAccurate()
        {
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var mainAssetType = GetFactory<MainAssetTypeFactory>().Create();

            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(GetEntityFactory<EstimatingProject>().Create());

            target.Permits.Add(new EstimatingProjectPermit { AssetType = mainAssetType, Cost = 14, Quantity = 1 });
            target.Permits.Add(new EstimatingProjectPermit { AssetType = mainAssetType, Cost = 24, Quantity = 2 });
            target.Permits.Add(new EstimatingProjectPermit { AssetType = valveAssetType, Cost = 14, Quantity = 3 });

            Assert.AreEqual(62, target.GetCostForAssetType(mainAssetType, EstimatingProject.CostType.Permit));
        }

        [TestMethod]
        public void TestMainOtherCostIsAccurate()
        {
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var mainAssetType = GetFactory<MainAssetTypeFactory>().Create();

            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(GetEntityFactory<EstimatingProject>().Create());

            target.OtherCosts.Add(new EstimatingProjectOtherCost { AssetType = mainAssetType, Cost = 14, Quantity = 1 });
            target.OtherCosts.Add(new EstimatingProjectOtherCost { AssetType = mainAssetType, Cost = 23, Quantity = 2 });
            target.OtherCosts.Add(new EstimatingProjectOtherCost { AssetType = valveAssetType, Cost = 14, Quantity = 3 });

            Assert.AreEqual(60, target.GetCostForAssetType(mainAssetType, EstimatingProject.CostType.Other));
        }

        [TestMethod]
        public void TestMainTotalCostIsAccurate()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var mainAssetType = GetFactory<MainAssetTypeFactory>().Create();
            var material = GetEntityFactory<Material>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
                .Create(new {OperatingCenter = operatingCenter, Material = material, Cost = 15m});
            var contractorLaborCost = GetEntityFactory<ContractorLaborCost>().Create(new { Cost = 15m });
            var companyLaborCost = GetEntityFactory<CompanyLaborCost>().Create(new { Cost = 22m });
            var estimatingProject = GetEntityFactory<EstimatingProject>()
                .Create(new {OperatingCenter = operatingCenter});
            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(estimatingProject);

            GetEntityFactory<EstimatingProjectMaterial>().Create(new 
            {
                EstimatingProject = estimatingProject,
                AssetType = mainAssetType,
                Material = material,
                Quantity = 2
            });
            Session.Flush();
            Session.Clear();
            target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(Session.Load<EstimatingProject>(estimatingProject.Id));
            target.ContractorLaborCosts.Add(new EstimatingProjectContractorLaborCost
            {
                AssetType = mainAssetType,
                ContractorLaborCost = contractorLaborCost,
                Quantity = 1
            });
            target.CompanyLaborCosts.Add(new EstimatingProjectCompanyLaborCost
            {
                AssetType = mainAssetType,
                CompanyLaborCost = companyLaborCost,
                Quantity = 2
            });
            target.Permits.Add(new EstimatingProjectPermit { AssetType = mainAssetType, Cost = 14, Quantity = 1 });
            target.OtherCosts.Add(new EstimatingProjectOtherCost { AssetType = mainAssetType, Cost = 14, Quantity = 1 });

            Assert.AreEqual(117, target.GetCostTotalForAssetType(mainAssetType));
        }

        #endregion

        #region All Asset Types

        [TestMethod]
        public void TestAssetTypeMaterialCostIsAccurate()
        {
            var assetTypes = new[] {
                GetFactory<EquipmentAssetTypeFactory>().Create(),
                GetFactory<FacilityAssetTypeFactory>().Create(),
                GetFactory<HydrantAssetTypeFactory>().Create(),
                GetFactory<MainAssetTypeFactory>().Create(),
                GetFactory<ServiceAssetTypeFactory>().Create(),
                GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                GetFactory<SewerLateralAssetTypeFactory>().Create(),
                GetFactory<SewerMainAssetTypeFactory>().Create(),
                GetFactory<StormCatchAssetTypeFactory>().Create(),
                GetFactory<ValveAssetTypeFactory>().Create()
            };
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var material = GetEntityFactory<Material>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
                .Create(new {Material = material, OperatingCenter = operatingCenter, Cost = 15m});
            var estimatingProject = GetEntityFactory<EstimatingProject>()
                .Create(new {OperatingCenter = operatingCenter});
            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(estimatingProject);

            foreach (var assetType in assetTypes)
            {
                Assert.AreEqual(0, target.GetCostForAssetType(assetType, EstimatingProject.CostType.Material));

                GetEntityFactory<EstimatingProjectMaterial>()
                    .Create(new {AssetType = assetType, Material = material, Quantity = 2, EstimatingProject = estimatingProject});
                Session.Flush();
                Session.Clear();

                target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(Session.Load<EstimatingProject>(estimatingProject.Id));
                Assert.AreEqual(30, target.GetCostForAssetType(assetType, EstimatingProject.CostType.Material));
            }
        }

        [TestMethod]
        public void TestAssetTypeContractorLaborCostIsAccurate()
        {
            var assetTypes = new[] {
                GetFactory<EquipmentAssetTypeFactory>().Create(),
                GetFactory<FacilityAssetTypeFactory>().Create(),
                GetFactory<HydrantAssetTypeFactory>().Create(),
                GetFactory<MainAssetTypeFactory>().Create(),
                GetFactory<ServiceAssetTypeFactory>().Create(),
                GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                GetFactory<SewerLateralAssetTypeFactory>().Create(),
                GetFactory<SewerMainAssetTypeFactory>().Create(),
                GetFactory<StormCatchAssetTypeFactory>().Create(),
                GetFactory<ValveAssetTypeFactory>().Create()
            };
            var contractorLaborCost = GetEntityFactory<ContractorLaborCost>().Create(new { Cost = 21m });
            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(GetEntityFactory<EstimatingProject>().Create());

            foreach (var assetType in assetTypes)
            {
                Assert.AreEqual(0, target.GetCostForAssetType(assetType, EstimatingProject.CostType.ContractorLabor));
                target.ContractorLaborCosts.Add(new EstimatingProjectContractorLaborCost
                {
                    AssetType = assetType,
                    ContractorLaborCost = contractorLaborCost,
                    Quantity = 2
                });
                Assert.AreEqual(42, target.GetCostForAssetType(assetType, EstimatingProject.CostType.ContractorLabor));
            }
        }

        [TestMethod]
        public void TestAssetTypeCompanyLaborCostIsAccurate()
        {
            var assetTypes = new[] {
                GetFactory<EquipmentAssetTypeFactory>().Create(),
                GetFactory<FacilityAssetTypeFactory>().Create(),
                GetFactory<HydrantAssetTypeFactory>().Create(),
                GetFactory<MainAssetTypeFactory>().Create(),
                GetFactory<ServiceAssetTypeFactory>().Create(),
                GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                GetFactory<SewerLateralAssetTypeFactory>().Create(),
                GetFactory<SewerMainAssetTypeFactory>().Create(),
                GetFactory<StormCatchAssetTypeFactory>().Create(),
                GetFactory<ValveAssetTypeFactory>().Create()
            };
            var companyLaborCost = GetEntityFactory<CompanyLaborCost>().Create(new { Cost = 21m });
            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(GetEntityFactory<EstimatingProject>().Create());

            foreach (var assetType in assetTypes)
            {
                Assert.AreEqual(0, target.GetCostForAssetType(assetType, EstimatingProject.CostType.CompanyLabor));
            
                target.CompanyLaborCosts.Add(new EstimatingProjectCompanyLaborCost
                {
                    AssetType = assetType,
                    CompanyLaborCost = companyLaborCost,
                    Quantity = 2
                });
                Assert.AreEqual(42, target.GetCostForAssetType(assetType, EstimatingProject.CostType.CompanyLabor));
            }
        }

        [TestMethod]
        public void TestAssetTypeOtherCostIsAccurate()
        {
            var assetTypes = new[] {
                GetFactory<EquipmentAssetTypeFactory>().Create(),
                GetFactory<FacilityAssetTypeFactory>().Create(),
                GetFactory<HydrantAssetTypeFactory>().Create(),
                GetFactory<MainAssetTypeFactory>().Create(),
                GetFactory<ServiceAssetTypeFactory>().Create(),
                GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                GetFactory<SewerLateralAssetTypeFactory>().Create(),
                GetFactory<SewerMainAssetTypeFactory>().Create(),
                GetFactory<StormCatchAssetTypeFactory>().Create(),
                GetFactory<ValveAssetTypeFactory>().Create()
            };

            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(GetEntityFactory<EstimatingProject>().Create());

            foreach (var assetType in assetTypes)
            {
                Assert.AreEqual(0, target.GetCostForAssetType(assetType, EstimatingProject.CostType.Other));
                target.OtherCosts.Add(new EstimatingProjectOtherCost
                {
                    AssetType = assetType,
                    Quantity = 2,
                    Cost = 22
                });
                Assert.AreEqual(44, target.GetCostForAssetType(assetType, EstimatingProject.CostType.Other));
            }
        }

        [TestMethod]
        public void TestAssetTypePermitIsAccurate()
        {
            var assetTypes = new[] {
                GetFactory<EquipmentAssetTypeFactory>().Create(),
                GetFactory<FacilityAssetTypeFactory>().Create(),
                GetFactory<HydrantAssetTypeFactory>().Create(),
                GetFactory<MainAssetTypeFactory>().Create(),
                GetFactory<ServiceAssetTypeFactory>().Create(),
                GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                GetFactory<SewerLateralAssetTypeFactory>().Create(),
                GetFactory<SewerMainAssetTypeFactory>().Create(),
                GetFactory<StormCatchAssetTypeFactory>().Create(),
                GetFactory<ValveAssetTypeFactory>().Create()
            };

            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(GetEntityFactory<EstimatingProject>().Create());

            foreach (var assetType in assetTypes)
            {
                Assert.AreEqual(0, target.GetCostForAssetType(assetType, EstimatingProject.CostType.Permit));

                target.Permits.Add(new EstimatingProjectPermit
                {
                    AssetType = assetType,
                    Quantity = 2,
                    Cost = 22
                });

                Assert.AreEqual(44, target.GetCostForAssetType(assetType, EstimatingProject.CostType.Permit));
            }

        }

        [TestMethod]
        public void TestTotalCostIsAccurate()
        {
            var assetTypes = new[] {
                GetFactory<EquipmentAssetTypeFactory>().Create(),
                GetFactory<FacilityAssetTypeFactory>().Create(),
                GetFactory<HydrantAssetTypeFactory>().Create(),
                GetFactory<MainAssetTypeFactory>().Create(),
                GetFactory<ServiceAssetTypeFactory>().Create(),
                GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                GetFactory<SewerLateralAssetTypeFactory>().Create(),
                GetFactory<SewerMainAssetTypeFactory>().Create(),
                GetFactory<StormCatchAssetTypeFactory>().Create(),
                GetFactory<ValveAssetTypeFactory>().Create()
            };

            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var material = GetEntityFactory<Material>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
                .Create(new {OperatingCenter = operatingCenter, Material = material, Cost = 15m});
            var contractorLaborCost = GetEntityFactory<ContractorLaborCost>().Create(new {Cost = 15m});
            var companyLaborCost = GetEntityFactory<CompanyLaborCost>().Create(new {Cost = 22m});

            foreach (var assetType in assetTypes)
            {
                var estimatingProject =
                    GetEntityFactory<EstimatingProject>().Create(new {OperatingCenter = operatingCenter});
                GetEntityFactory<EstimatingProjectMaterial>().Create(new {
                    EstimatingProject = estimatingProject,
                    AssetType = assetType,
                    Material = material,
                    Quantity = 2
                });
                GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                    AssetType = assetType,
                    ContractorLaborCost = contractorLaborCost,
                    Quantity = 1,
                    EstimatingProject = estimatingProject
                });
                GetEntityFactory<EstimatingProjectCompanyLaborCost>().Create(new {
                    AssetType = assetType,
                    CompanyLaborCost = companyLaborCost,
                    Quantity = 2,
                    EstimatingProject = estimatingProject
                });
                GetEntityFactory<EstimatingProjectPermit>().Create(new {
                    AssetType = assetType,
                    Cost = 14m,
                    Quantity = 1,
                    EstimatingProject = estimatingProject
                });
                GetEntityFactory<EstimatingProjectOtherCost>().Create(new {
                    Description = "asdf",
                    AssetType = assetType,
                    Cost = 14m,
                    Quantity = 1,
                    EstimatingProject = estimatingProject
                });

                Session.Flush();
                Session.Clear();
                var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(Session.Load<EstimatingProject>(estimatingProject.Id));

                Assert.AreEqual(117, target.GetCostTotalForAssetType(assetType));
            }
        }

        #endregion

        [TestMethod]
        public void TestValidationReturnsErrorWhenContractorLaborCostsHasNulls()
        {
            var assetTypes = new[] {
                GetFactory<EquipmentAssetTypeFactory>().Create(),
                GetFactory<FacilityAssetTypeFactory>().Create(),
                GetFactory<HydrantAssetTypeFactory>().Create(),
                GetFactory<MainAssetTypeFactory>().Create(),
                GetFactory<ServiceAssetTypeFactory>().Create(),
                GetFactory<SewerOpeningAssetTypeFactory>().Create(),
                GetFactory<SewerLateralAssetTypeFactory>().Create(),
                GetFactory<SewerMainAssetTypeFactory>().Create(),
                GetFactory<StormCatchAssetTypeFactory>().Create(),
                GetFactory<ValveAssetTypeFactory>().Create()
            };
            //var contractorLaborCost = GetEntityFactory<ContractorLaborCost>().Create(new { Cost = 21m });
            var contractorLaborCost = GetEntityFactory<ContractorLaborCost>().Create();
            contractorLaborCost.Cost = null;
            Session.Save(contractorLaborCost);
            var project = GetEntityFactory<EstimatingProject>().Create();

            foreach (var assetType in assetTypes)
            {
                project.ContractorLaborCosts.Add(new EstimatingProjectContractorLaborCost
                {
                    AssetType = assetType,
                    ContractorLaborCost = contractorLaborCost,
                    Quantity = 2
                });
            }
            Session.Save(project);

            var target = _viewModelFactory.Build<PipelineProjectCostEstimate, EstimatingProject>(project);

            ValidationAssert.ModelStateHasError(target, x => x.ContractorLaborCosts, PipelineProjectCostEstimate.CONTRACTOR_LABOR_COST_ERROR );

        }

        #endregion
    }
}
