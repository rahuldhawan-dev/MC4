using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class EstimatingProjectTest : InMemoryDatabaseTest<EstimatingProject>
    {
        #region Init/Cleanup

        private DateTime _now;
        private TestDateTimeProvider _dateTimeProvider;

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider());
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider.SetNow(_now = DateTime.Now);
        }

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(EstimatingProjectTypeFactory).Assembly);
        }

        #endregion

        #region Summary Fields

        [TestMethod]
        public void TestContingencyPercentageAsDecimalReturnsContingencyPercentageAsAProperDecimalForMathingWith()
        {
            var target = new EstimatingProject();
            target.ContingencyPercentage = 41;

            Assert.AreEqual(0.41m, target.ContingencyPercentageAsDecimal);
        }

        //[TestMethod]
        //public void TestContingencyPercentageAsDecimalReturnsZeroIfContingencyPercentageIsNullForSomeReason()
        //{
        //    var target = new EstimatingProject();
        //    target.ContingencyPercentage = null;
        //    Assert.AreEqual(0, target.ContingencyPercentageAsDecimal);
        //}

        [TestMethod]
        public void TestOverheadPercentageAsDecimalReturnsContingencyPercentageAsAProperDecimalForMathingWith()
        {
            var target = new EstimatingProject();
            target.OverheadPercentage = 41;

            Assert.AreEqual(0.41m, target.OverheadPercentageAsDecimal);
        }

        //[TestMethod]
        //public void TestOverheadPercentageAsDecimalReturnsZeroIfContingencyPercentageIsNullForSomeReason()
        //{
        //    var target = new EstimatingProject();
        //    target.OverheadPercentage = null;
        //    Assert.AreEqual(0, target.OverheadPercentageAsDecimal);
        //}

        [TestMethod]
        public void TestSummaryFields()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var material = GetEntityFactory<Material>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>().Create(new {
                OperatingCenter = operatingCenter,
                Material = material,
                Cost = 7m
            });
            var target = GetEntityFactory<EstimatingProject>().Create(new {
                ContingencyPercentage = 10,
                OverheadPercentage = 11,
                LumpSum = 12m,
                OperatingCenter = operatingCenter
            });

            GetEntityFactory<EstimatingProjectMaterial>().CreateList(2, new {
                EstimatingProject = target,
                Quantity = 1,
                Material = material
            });

            GetEntityFactory<EstimatingProjectCompanyLaborCost>().CreateList(2, new {
                EstimatingProject = target,
                Quantity = 2
            });

            GetEntityFactory<EstimatingProjectContractorLaborCost>().CreateList(2, new {
                EstimatingProject = target,
                Quantity = 3
            });

            GetEntityFactory<EstimatingProjectPermit>().CreateList(2, new {
                EstimatingProject = target,
                Quantity = 4,
                Cost = 6.66m
            });

            GetEntityFactory<EstimatingProjectOtherCost>().CreateList(2, new {
                Cost = 6.66m,
                Quantity = 5,
                Description = "vOv",
                EstimatingProject = target
            });
            Session.Flush();
            Session.Clear();

            target = Session.Load<EstimatingProject>(target.Id);

            Assert.AreEqual(14, target.TotalMaterialCost);
            Assert.AreEqual(42, target.TotalCompanyLaborCost);
            Assert.AreEqual(39.96m, target.TotalContractorLaborCost);
            Assert.AreEqual(53.28m, target.TotalPermitCost);
            Assert.AreEqual(66.6m, target.TotalOtherCost);
            Assert.AreEqual(215.84m, target.EstimatedConstructionCost);
            Assert.AreEqual(21.584m, target.ContingencyCost);
            Assert.AreEqual(23.7424m, target.OverheadCost);
            Assert.AreEqual(273.1664m, target.TotalEstimatedCost);
        }

        [TestMethod]
        public void TestOverrideLaborCostOverridesContractorLaborCost()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var target = GetEntityFactory<EstimatingProject>().Create(new {
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });
            var cost = GetEntityFactory<ContractorLaborCost>().Create(new {
                Cost = 5.55m
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                EstimatingProject = target,
                ContractorLaborCost = cost,
                Quantity = 1
            });
            GetEntityFactory<ContractorOverrideLaborCost>().Create(new {
                EffectiveDate = _now.AddSeconds(-1),
                Cost = 6.66m,
                ContractorLaborCost = cost,
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });

            Session.Clear();
            target = Session.Load<EstimatingProject>(target.Id);

            Assert.AreEqual(6.66m, target.TotalContractorLaborCost);
        }

        [TestMethod]
        public void TestOverrideLaborCostOverridesWithTheMostCurrentOverrideThatIsToSayTheVeryCurrentestOverride()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var target = GetEntityFactory<EstimatingProject>().Create(new {
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });
            var cost = GetEntityFactory<ContractorLaborCost>().Create(new {
                Cost = 5.55m
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                EstimatingProject = target,
                ContractorLaborCost = cost,
                Quantity = 1
            });
            GetEntityFactory<ContractorOverrideLaborCost>().Create(new {
                EffectiveDate = _now.AddSeconds(-1),
                Cost = 6.66m,
                ContractorLaborCost = cost,
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });
            GetEntityFactory<ContractorOverrideLaborCost>().Create(new {
                EffectiveDate = _now,
                Cost = 7.77m,
                ContractorLaborCost = cost,
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });
            GetEntityFactory<ContractorOverrideLaborCost>().Create(new {
                EffectiveDate = _now.AddSeconds(1),
                Cost = 8.88m,
                ContractorLaborCost = cost,
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });

            Session.Clear();
            target = Session.Load<EstimatingProject>(target.Id);

            Assert.AreEqual(7.77m, target.TotalContractorLaborCost);
        }

        [TestMethod]
        public void TestOverrideLaborCostDoesNotOverrideIfEffectiveDateHasNotYetCome()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var target = GetEntityFactory<EstimatingProject>().Create(new {
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });
            var cost = GetEntityFactory<ContractorLaborCost>().Create(new {
                Cost = 5.55m
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                EstimatingProject = target,
                ContractorLaborCost = cost,
                Quantity = 1
            });
            GetEntityFactory<ContractorOverrideLaborCost>().Create(new {
                EffectiveDate = _now.AddSeconds(1),
                Cost = 6.66m,
                ContractorLaborCost = cost,
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });

            Session.Clear();
            target = Session.Load<EstimatingProject>(target.Id);

            Assert.AreEqual(5.55m, target.TotalContractorLaborCost);
        }

        [TestMethod]
        public void TestOverrideLaborCostDoesNotOverrideIfOperatingCenterDoesNotMatch()
        {
            var operatingCenter1 = GetEntityFactory<OperatingCenter>().Create();
            var operatingCenter2 = GetEntityFactory<OperatingCenter>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var target = GetEntityFactory<EstimatingProject>().Create(new {
                Contractor = contractor,
                OperatingCenter = operatingCenter1
            });
            var cost = GetEntityFactory<ContractorLaborCost>().Create(new {
                Cost = 5.55m
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                EstimatingProject = target,
                ContractorLaborCost = cost,
                Quantity = 1
            });
            GetEntityFactory<ContractorOverrideLaborCost>().Create(new {
                EffectiveDate = _now.AddSeconds(1),
                Cost = 6.66m,
                ContractorLaborCost = cost,
                Contractor = contractor,
                OperatingCenter = operatingCenter2
            });

            Session.Clear();
            target = Session.Load<EstimatingProject>(target.Id);

            Assert.AreEqual(5.55m, target.TotalContractorLaborCost);
        }

        [TestMethod]
        public void TestOverrideLaborCostDoesNotOverrideIfContractorDoesNotMatch()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var contractor1 = GetEntityFactory<Contractor>().Create();
            var contractor2 = GetEntityFactory<Contractor>().Create();
            var target = GetEntityFactory<EstimatingProject>().Create(new {
                Contractor = contractor1,
                OperatingCenter = operatingCenter
            });
            var cost = GetEntityFactory<ContractorLaborCost>().Create(new {
                Cost = 5.55m
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                EstimatingProject = target,
                ContractorLaborCost = cost,
                Quantity = 1
            });
            GetEntityFactory<ContractorOverrideLaborCost>().Create(new {
                EffectiveDate = _now.AddSeconds(1),
                Cost = 6.66m,
                ContractorLaborCost = cost,
                Contractor = contractor2,
                OperatingCenter = operatingCenter
            });

            Session.Clear();
            target = Session.Load<EstimatingProject>(target.Id);

            Assert.AreEqual(5.55m, target.TotalContractorLaborCost);
        }

        #region Materials

        [TestMethod]
        public void TestMaterialSummaryReturnsAccurateMaterialSummary()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var target = GetEntityFactory<EstimatingProject>().Create(new {
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var material1 = GetEntityFactory<Material>().Create();
            var material2 = GetEntityFactory<Material>().Create();
            GetEntityFactory<OperatingCenterStockedMaterial>()
               .Create(new {Material = material1, OperatingCenter = operatingCenter, Cost = 10m});
            GetEntityFactory<OperatingCenterStockedMaterial>()
               .Create(new {Material = material2, OperatingCenter = operatingCenter, Cost = 15m});
            GetEntityFactory<EstimatingProjectMaterial>().Create(new {
                EstimatingProject = target,
                Material = material1,
                Quantity = 1,
                AssetType = hydrantAssetType
            });
            GetEntityFactory<EstimatingProjectMaterial>().Create(new {
                EstimatingProject = target,
                Material = material1,
                Quantity = 4,
                AssetType = valveAssetType
            });
            GetEntityFactory<EstimatingProjectMaterial>().Create(new {
                EstimatingProject = target,
                Material = material2,
                Quantity = 3
            });
            Session.Flush();
            Session.Clear();
            target = Session.Load<EstimatingProject>(target.Id);
            var groupedMaterials = target.GroupedMaterials;

            Assert.AreEqual(2, groupedMaterials.Count);

            Assert.AreEqual(5, groupedMaterials[0].Quantity);
            Assert.AreEqual(50m, groupedMaterials[0].TotalCost);
            Assert.AreEqual(material1.Id, groupedMaterials[0].Material.Id);

            Assert.AreEqual(3, groupedMaterials[1].Quantity);
            Assert.AreEqual(45m, groupedMaterials[1].TotalCost);
            Assert.AreEqual(material2.Id, groupedMaterials[1].Material.Id);
        }

        #endregion

        #region GroupedContractorLaborCosts

        [TestMethod]
        public void TestGroupedContractorLaborCostsAreGroupedProperly()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var contractor2 = GetEntityFactory<Contractor>().Create();
            var target = GetEntityFactory<EstimatingProject>().Create(new {
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var cost1 = GetEntityFactory<ContractorLaborCost>().Create(new {
                Cost = 5.55m
            });
            var cost2 = GetEntityFactory<ContractorLaborCost>().Create(new {
                Cost = 5.55m
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                AssetType = hydrantAssetType,
                EstimatingProject = target,
                ContractorLaborCost = cost1,
                Quantity = 1
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                AssetType = valveAssetType,
                EstimatingProject = target,
                ContractorLaborCost = cost1,
                Quantity = 1
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                AssetType = valveAssetType,
                EstimatingProject = target,
                ContractorLaborCost = cost2,
                Quantity = 1
            });
            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                AssetType = hydrantAssetType,
                EstimatingProject = target,
                ContractorLaborCost = cost2,
                Quantity = 4
            });
            GetEntityFactory<ContractorOverrideLaborCost>().Create(new {
                EffectiveDate = _now.AddSeconds(1),
                Cost = 6.66m,
                ContractorLaborCost = cost1,
                Contractor = contractor2,
                OperatingCenter = operatingCenter
            });
            Session.Clear();

            target = Session.Load<EstimatingProject>(target.Id);

            Assert.AreEqual(4, target.ContractorLaborCosts.Count);
            Assert.AreEqual(2, target.GroupedContractorLaborCosts.Count);
        }

        [TestMethod]
        public void TestGroupedContractorLaborCostsResultsHaveSetterInjection()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var contractor2 = GetEntityFactory<Contractor>().Create();
            var target = GetEntityFactory<EstimatingProject>().Create(new {
                Contractor = contractor,
                OperatingCenter = operatingCenter
            });
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var cost1 = GetEntityFactory<ContractorLaborCost>().Create(new {
                Cost = 5.55m
            });

            GetEntityFactory<EstimatingProjectContractorLaborCost>().Create(new {
                AssetType = hydrantAssetType,
                EstimatingProject = target,
                ContractorLaborCost = cost1,
                Quantity = 1
            });

            Session.Clear();

            target = Session.Load<EstimatingProject>(target.Id);

            // This will throw an exception if the instances didn't have Container.BuildUp called on them.
            MyAssert.DoesNotThrow(() => target.GroupedContractorLaborCosts.Single().TotalCost);
        }

        #endregion

        #endregion

        [TestMethod]
        public void TestToStringReturnsIdProjectNumberProjectName()
        {
            var projectNumber = "1";
            var projectName = "FooBar";
            var target = GetEntityFactory<EstimatingProject>()
               .Create(new {ProjectNumber = projectNumber, ProjectName = projectName});
            Assert.AreEqual(string.Format("{0} - {1} - {2}", target.Id, projectNumber, projectName), target.ToString());
        }
    }
}
