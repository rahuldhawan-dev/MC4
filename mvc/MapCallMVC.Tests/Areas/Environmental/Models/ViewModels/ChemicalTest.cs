using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public abstract class CreateChemicalTest<TViewModel> : ViewModelTestBase<Chemical, TViewModel>
        where TViewModel : ChemicalViewModel
    {
        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Name);
            _vmTester.CanMapBothWays(x => x.ChemicalType,GetEntityFactory<ChemicalType>().Create());
            _vmTester.CanMapBothWays(x => x.PartNumber);
            _vmTester.CanMapBothWays(x => x.PricePerPoundWet);
            _vmTester.CanMapBothWays(x => x.WetPoundsPerGal);
            _vmTester.CanMapBothWays(x => x.PackagingQuantities);
            _vmTester.CanMapBothWays(x => x.PackagingType, GetEntityFactory<PackagingType>().Create());
            _vmTester.CanMapBothWays(x => x.PackagingUnits);
            _vmTester.CanMapBothWays(x => x.ChemicalSymbol);
            _vmTester.CanMapBothWays(x => x.Appearance);
            _vmTester.CanMapBothWays(x => x.ChemicalConcentrationLiquid);
            _vmTester.CanMapBothWays(x => x.ConcentrationLbsPerGal);
            _vmTester.CanMapBothWays(x => x.SpecificGravityMin);
            _vmTester.CanMapBothWays(x => x.SpecificGravityMax);
            _vmTester.CanMapBothWays(x => x.RatioResidualProduction);
            _vmTester.CanMapBothWays(x => x.CasNumber);
            _vmTester.CanMapBothWays(x => x.SdsHyperlink);
            _vmTester.CanMapBothWays(x => x.SubNumber);
            _vmTester.CanMapBothWays(x => x.DepartmentOfTransportationNumber);
            _vmTester.CanMapBothWays(x => x.IsPure);
            _vmTester.CanMapBothWays(x => x.TradeSecret);
            _vmTester.CanMapBothWays(x => x.EmergencyPlanningCommunityRightToKnowActOnly);
            _vmTester.CanMapBothWays(x => x.ExtremelyHazardousChemical);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(x => x.Name);
            ValidationAssert.PropertyIsRequired(x => x.PartNumber);
        }

        [TestMethod]
        public void TestMaxLengthOnAllTheStringProperties()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.SdsHyperlink, Chemical.StringLengths.SDS_HYPERLINK);
        }

        [TestMethod]
        public void TestMapToEntityMapsPhysicalHazards()
        {
            var physicalHazards = GetEntityFactory<PhysicalHazard>().CreateList(4);
            var target = _viewModelFactory.Build<CreateChemical, Chemical>(new Chemical {
                PhysicalHazards = new[] { physicalHazards[0], physicalHazards[2] }
            });

            var entity = new Chemical();
            target.MapToEntity(entity);

            Assert.AreEqual(2, entity.PhysicalHazards.Count);
            Assert.AreEqual(physicalHazards[0].Id, entity.PhysicalHazards[0].Id);
            Assert.AreEqual(physicalHazards[2].Id, entity.PhysicalHazards[1].Id);
        }

        [TestMethod]
        public void TestMapMapsPhysicalHazards()
        {
            var physicalHazards = GetEntityFactory<PhysicalHazard>().CreateList(4);
            _entity = GetEntityFactory<Chemical>().Create(new {
                PhysicalHazards = new[] { physicalHazards[0], physicalHazards[2] }
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(2, _viewModel.PhysicalHazards.Length);
            Assert.AreEqual(_entity.PhysicalHazards[0].Id, _viewModel.PhysicalHazards[0]);
            Assert.AreEqual(_entity.PhysicalHazards[1].Id, _viewModel.PhysicalHazards[1]);
        }

        [TestMethod]
        public void TestMapToEntityMapsHealthHazards()
        {
            var healthHazards = GetEntityFactory<HealthHazard>().CreateList(4);
            var target = _viewModelFactory.Build<CreateChemical, Chemical>(new Chemical {
                HealthHazards = new[] { healthHazards[0], healthHazards[2] }
            });

            var entity = new Chemical();
            target.MapToEntity(entity);

            Assert.AreEqual(2, entity.HealthHazards.Count);
            Assert.AreEqual(healthHazards[0].Id, entity.HealthHazards[0].Id);
            Assert.AreEqual(healthHazards[2].Id, entity.HealthHazards[1].Id);
        }

        [TestMethod]
        public void TestMapMapsHealthHazards()
        {
            var healthHazards = GetEntityFactory<HealthHazard>().CreateList(4);
            _entity = GetEntityFactory<Chemical>().Create(new {
                HealthHazards = new[] { healthHazards[0], healthHazards[2] }
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(2, _viewModel.HealthHazards.Length);
            Assert.AreEqual(_entity.HealthHazards[0].Id, _viewModel.HealthHazards[0]);
            Assert.AreEqual(_entity.HealthHazards[1].Id, _viewModel.HealthHazards[1]);
        }

        [TestMethod]
        public void TestChemicalStatesMapBothWays()
        {
            var stateOfMatterTypes = GetEntityFactory<StateOfMatter>().CreateList(3);
            var target = _viewModelFactory.Build<CreateChemical, Chemical>(new Chemical {
                ChemicalStates = new[] { stateOfMatterTypes[0], stateOfMatterTypes[1] }
            });

            var entity = new Chemical();
            target.MapToEntity(entity);

            Assert.AreEqual(2, entity.ChemicalStates.Count);
            Assert.AreEqual(stateOfMatterTypes[0].Id, entity.ChemicalStates[0].Id);
            Assert.AreEqual(stateOfMatterTypes[1].Id, entity.ChemicalStates[1].Id);
            Assert.AreEqual(String.Join(", ", stateOfMatterTypes[0].Description, stateOfMatterTypes[1].Description), entity.DisplayChemicalStates);

            _entity = GetEntityFactory<Chemical>().Create(new {
                ChemicalStates = new[] { stateOfMatterTypes[0], stateOfMatterTypes[1] }
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(2, _viewModel.ChemicalStates.Length);
            Assert.AreEqual(_entity.ChemicalStates[0].Id, _viewModel.ChemicalStates[0]);
            Assert.AreEqual(_entity.ChemicalStates[1].Id, _viewModel.ChemicalStates[1]);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.PackagingType, GetEntityFactory<PackagingType>().Create());
        }

        #endregion
    }

    [TestClass]
    public abstract class EditChemicalTest<TViewModel> : ViewModelTestBase<Chemical, TViewModel>
        where TViewModel : ChemicalViewModel
    {
        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Name);
            _vmTester.CanMapBothWays(x => x.ChemicalType,GetEntityFactory<ChemicalType>().Create());
            _vmTester.CanMapBothWays(x => x.PartNumber);
            _vmTester.CanMapBothWays(x => x.PricePerPoundWet);
            _vmTester.CanMapBothWays(x => x.WetPoundsPerGal);
            _vmTester.CanMapBothWays(x => x.PackagingQuantities);
            _vmTester.CanMapBothWays(x => x.PackagingType, GetEntityFactory<PackagingType>().Create());
            _vmTester.CanMapBothWays(x => x.PackagingUnits);
            _vmTester.CanMapBothWays(x => x.ChemicalSymbol);
            _vmTester.CanMapBothWays(x => x.Appearance);
            _vmTester.CanMapBothWays(x => x.ChemicalConcentrationLiquid);
            _vmTester.CanMapBothWays(x => x.ConcentrationLbsPerGal);
            _vmTester.CanMapBothWays(x => x.SpecificGravityMin);
            _vmTester.CanMapBothWays(x => x.RatioResidualProduction);
            _vmTester.CanMapBothWays(x => x.CasNumber);
            _vmTester.CanMapBothWays(x => x.SdsHyperlink);
            _vmTester.CanMapBothWays(x => x.SubNumber);
            _vmTester.CanMapBothWays(x => x.DepartmentOfTransportationNumber);
            _vmTester.CanMapBothWays(x => x.IsPure);
            _vmTester.CanMapBothWays(x => x.TradeSecret);
            _vmTester.CanMapBothWays(x => x.EmergencyPlanningCommunityRightToKnowActOnly);
            _vmTester.CanMapBothWays(x => x.ExtremelyHazardousChemical);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(x => x.Name);
            ValidationAssert.PropertyIsRequired(x => x.PartNumber);
        }

        [TestMethod]
        public void TestMaxLengthOnAllTheStringProperties()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.SdsHyperlink, Chemical.StringLengths.SDS_HYPERLINK);
        }

        [TestMethod]
        public void TestChemicalStatesMapBothWays()
        {
            var stateOfMatterTypes = GetEntityFactory<StateOfMatter>().CreateList(3);
            var target = _viewModelFactory.Build<CreateChemical, Chemical>(new Chemical {
                ChemicalStates = new[] { stateOfMatterTypes[0], stateOfMatterTypes[1] }
            });

            var entity = new Chemical();
            target.MapToEntity(entity);

            Assert.AreEqual(2, entity.ChemicalStates.Count);
            Assert.AreEqual(stateOfMatterTypes[0].Id, entity.ChemicalStates[0].Id);
            Assert.AreEqual(stateOfMatterTypes[1].Id, entity.ChemicalStates[1].Id);
            Assert.AreEqual(String.Join(", ", stateOfMatterTypes[0].Description, stateOfMatterTypes[1].Description), entity.DisplayChemicalStates);

            _entity = GetEntityFactory<Chemical>().Create(new {
                ChemicalStates = new[] { stateOfMatterTypes[0], stateOfMatterTypes[2] }
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(2, _viewModel.ChemicalStates.Length);
            Assert.AreEqual(_entity.ChemicalStates[0].Id, _viewModel.ChemicalStates[0]);
            Assert.AreEqual(_entity.ChemicalStates[1].Id, _viewModel.ChemicalStates[1]);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.PackagingType, GetEntityFactory<PackagingType>().Create());
        }

        #endregion
    }
}