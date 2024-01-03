using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Engineering.Models.ViewModels.ArcFlash;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Engineering.Models.ViewModels
{
    [TestClass]
    public class ArcFlashStudyViewModelTest : MapCallMvcInMemoryDatabaseTestBase<ArcFlashStudy>
    {
        private ArcFlashStudy _entity;
        private ArcFlashStudyViewModel _viewModel;
        private ViewModelTester<ArcFlashStudyViewModel, ArcFlashStudy> _vmTester;
        private ValidationContext _validationContext;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<ArcFlashStudy>().Create();
            _viewModel = new EditArcFlashStudy(_container);
            _vmTester = new ViewModelTester<ArcFlashStudyViewModel, ArcFlashStudy>(_viewModel, _entity);

            _validationContext = new ValidationContext(_viewModel, null, null);
        }

        #endregion

        [TestMethod]
        public void TestMappings()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 13);
            _vmTester.DoesNotMapToEntity(x => x.Id, 31);

            _vmTester.CanMapBothWays(x => x.PowerCompanyDataReceived);
            _vmTester.CanMapBothWays(x => x.UtilityCompanyDataReceivedDate);
            _vmTester.CanMapBothWays(x => x.AFHAAnalysisPerformed);
            _vmTester.CanMapBothWays(x => x.TransformerKVAFieldConfirmed);
            _vmTester.CanMapBothWays(x => x.DateLabelsApplied);
            _vmTester.CanMapBothWays(x => x.ArcFlashContractor);
            _vmTester.CanMapBothWays(x => x.ArcFlashHazardAnalysisStudyParty);
            _vmTester.CanMapBothWays(x => x.CostToComplete);
            _vmTester.CanMapBothWays(x => x.Priority);
            _vmTester.CanMapBothWays(x => x.UtilityCompanyOther);
            _vmTester.CanMapBothWays(x => x.UtilityAccountNumber);
            _vmTester.CanMapBothWays(x => x.UtilityMeterNumber);
            _vmTester.CanMapBothWays(x => x.PrimaryVoltageKV);
            _vmTester.CanMapBothWays(x => x.TransformerResistancePercentage);
            _vmTester.CanMapBothWays(x => x.TransformerReactancePercentage);
            _vmTester.CanMapBothWays(x => x.PrimaryFuseSize);
            _vmTester.CanMapBothWays(x => x.PrimaryFuseType);
            _vmTester.CanMapBothWays(x => x.PrimaryFuseManufacturer);
            _vmTester.CanMapBothWays(x => x.LineToLineFaultAmps);
            _vmTester.CanMapBothWays(x => x.LineToLineNeutralFaultAmps);
            _vmTester.CanMapBothWays(x => x.ArcFlashNotes);

            var factoryService = _container.GetInstance<ITestDataFactoryService>();
            _vmTester.CanMapBothWays(x => x.UtilityCompany, factoryService);
            _vmTester.CanMapBothWays(x => x.ArcFlashStatus, factoryService);
            _vmTester.CanMapBothWays(x => x.PowerPhase, factoryService);
            _vmTester.CanMapBothWays(x => x.Voltage, factoryService);
            _vmTester.CanMapBothWays(x => x.TransformerKVARating, factoryService);
            _vmTester.CanMapBothWays(x => x.FacilitySize, factoryService);
            _vmTester.CanMapBothWays(x => x.FacilityTransformerWiringType, factoryService);
            _vmTester.CanMapBothWays(x => x.TypeOfArcFlashAnalysis, factoryService);
            _vmTester.CanMapBothWays(x => x.ArcFlashLabelType, factoryService);
        }

        [TestMethod]
        public void TestRequiredFieldsAreRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.PowerPhase);
        }

        [TestMethod]
        public void TestRequiredWhensAreRequiredWhen()
        {
            var pp = GetEntityFactory<PowerPhase>().Create();
            var v = GetEntityFactory<Voltage>().Create();
            var utr = GetEntityFactory<UtilityTransformerKVARating>().Create();

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.UtilityCompanyDataReceivedDate, DateTime.Now, x => x.PowerCompanyDataReceived, true);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.TransformerKVAFieldConfirmed, true, x => x.ArcFlashStatus, ArcFlashStatus.Indices.COMPLETED);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ArcFlashContractor, "Killing Fleas Softly", x => x.ArcFlashStatus, ArcFlashStatus.Indices.COMPLETED);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.CostToComplete, 8.17m, x => x.ArcFlashStatus, ArcFlashStatus.Indices.COMPLETED);

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PowerPhase, pp.Id, x => x.ArcFlashLabelType, ArcFlashLabelType.Indices.CUSTOMLABEL, ArcFlashLabelType.Indices.STANDARDLABEL);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.Voltage, v.Id, x => x.ArcFlashLabelType, ArcFlashLabelType.Indices.CUSTOMLABEL, ArcFlashLabelType.Indices.STANDARDLABEL);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.TransformerKVARating, utr.Id, x => x.ArcFlashLabelType, ArcFlashLabelType.Indices.CUSTOMLABEL, ArcFlashLabelType.Indices.STANDARDLABEL);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PowerCompanyDataReceived, true, x => x.ArcFlashLabelType, ArcFlashLabelType.Indices.CUSTOMLABEL, ArcFlashLabelType.Indices.STANDARDLABEL);
        }
    }

    [TestClass]
    public class EditArcFlashStudyTest : MapCallMvcInMemoryDatabaseTestBase<ArcFlashStudy>
    {
        private ArcFlashStudy _entity;
        private EditArcFlashStudy _viewModel;
        private ViewModelTester<EditArcFlashStudy, ArcFlashStudy> _vmTester;
        private ValidationContext _validationContext;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<ArcFlashStudy>().Create();
            _viewModel = new EditArcFlashStudy(_container);
            _vmTester = new ViewModelTester<EditArcFlashStudy, ArcFlashStudy>(_viewModel, _entity);

            _validationContext = new ValidationContext(_viewModel, null, null);
        }

        #endregion

        [TestMethod]
        public void TestMapSetsState()
        {
            _viewModel.Map(_entity);

            Assert.AreEqual(_entity.Facility.OperatingCenter.State.Id, _viewModel.State);
        }
    }
}
