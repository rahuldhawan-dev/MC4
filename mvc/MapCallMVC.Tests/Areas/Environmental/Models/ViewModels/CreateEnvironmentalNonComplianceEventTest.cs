using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using System;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents;
using System.Collections.Generic;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    public abstract class EnvironmentalNonComplianceEventViewModelTestBase<TModel> : MapCallMvcInMemoryDatabaseTestBase<EnvironmentalNonComplianceEvent>
        where TModel : EnvironmentalNonComplianceEventViewModel
    {
        protected ViewModelTester<TModel, EnvironmentalNonComplianceEvent> _vmTester;
        protected TModel _viewModel;
        protected EnvironmentalNonComplianceEvent _entity;

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<TModel>();
            _entity = new EnvironmentalNonComplianceEvent();
            _vmTester = new ViewModelTester<TModel, EnvironmentalNonComplianceEvent>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            var factoryService = _container.GetInstance<ITestDataFactoryService>();

            _vmTester.CanMapBothWays(x => x.EventDate);
            _vmTester.CanMapBothWays(x => x.DateFinalized);
            _vmTester.CanMapBothWays(x => x.EnforcementDate);
            _vmTester.CanMapBothWays(x => x.NameOfThirdParty);
            _vmTester.CanMapBothWays(x => x.AwarenessDate);
            _vmTester.CanMapBothWays(x => x.SummaryOfEvent);
            _vmTester.CanMapBothWays(x => x.FailureTypeDescription);
            _vmTester.CanMapBothWays(x => x.FineAmount);
            _vmTester.CanMapBothWays(x => x.NameOfEntity);
            _vmTester.CanMapBothWays(x => x.NOVWorkGroupReviewDate);
            _vmTester.CanMapBothWays(x => x.ChiefEnvOfficerApprovalDate);

            _vmTester.CanMapBothWays(x => x.State, factoryService);
            _vmTester.CanMapBothWays(x => x.OperatingCenter, factoryService);
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply, factoryService);
            _vmTester.CanMapBothWays(x => x.WasteWaterSystem, factoryService);
            _vmTester.CanMapBothWays(x => x.Facility, factoryService);
            _vmTester.CanMapBothWays(x => x.IssueType, factoryService);
            _vmTester.CanMapBothWays(x => x.IssueSubType, factoryService);
            _vmTester.CanMapBothWays(x => x.Responsibility, factoryService);
            _vmTester.CanMapBothWays(x => x.FailureType, factoryService);
            _vmTester.CanMapBothWays(x => x.IssueStatus, factoryService);
            _vmTester.CanMapBothWays(x => x.IssuingEntity, factoryService);
            _vmTester.CanMapBothWays(x => x.CountsAgainstTarget, factoryService);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WaterType);

            var pwsid = GetEntityFactory<PublicWaterSupply>().Create();
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PublicWaterSupply, pwsid.Id, x => x.WaterType, WaterType.Indices.WATER, WaterType.Indices.WASTEWATER);

            var ww = GetEntityFactory<WasteWaterSystem>().Create();
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.WasteWaterSystem, ww.Id, x => x.WaterType, WaterType.Indices.WASTEWATER, WaterType.Indices.WATER);

            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.Facility);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IssueType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IssueStatus);

            if (_viewModel is CreateEnvironmentalNonComplianceEvent)
            {
                ValidationAssert.PropertyIsRequired(_viewModel, x => x.EventDate);
                ValidationAssert.PropertyIsRequired(_viewModel, x => x.AwarenessDate);
                ValidationAssert.PropertyIsRequired(_viewModel, x => x.SummaryOfEvent);
            }
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RootCauses);

            var ie = GetEntityFactory<EnvironmentalNonComplianceEventEntityLevel>().Create();
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.IssuingEntity, ie.Id, x => x.IssueStatus, EnvironmentalNonComplianceEventStatus.Indices.CONFIRMED);

            var ft = GetEntityFactory<EnvironmentalNonComplianceEventFailureType>().Create();
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.FailureType, ft.Id, x => x.DateFinalized, DateTime.Now);

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.FailureTypeDescription, "Stuff", x => x.FailureType, ft.Id);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.NameOfEntity, "Stuff", x => x.IssuingEntity, EnvironmentalNonComplianceEventEntityLevel.Indices.OTHER);
        }

        [TestMethod]
        public void TestFineAmountValidationFailsAndThrowsErrorWhenRegexNotMatched()
        {
            _viewModel.FineAmount = (decimal)1.123;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.FineAmount, "Decimal must be no larger than the hundredths place ex. 500.00");
            
        }

        [TestMethod]
        public void TestSetDefaultsSetsResponsibility()
        {
            _viewModel.SetDefaults();

            Assert.AreEqual(EnvironmentalNonComplianceEventResponsibility.Indices.TBD, _viewModel.Responsibility);
        }

        [TestMethod]
        public void TestRootCauseCanMapBothWays()
        {
            var rootCauses = GetEntityFactory<EnvironmentalNonComplianceEventRootCause>().CreateList(4);
            _entity.RootCauses = new List<EnvironmentalNonComplianceEventRootCause>();
            _entity.RootCauses.Add(rootCauses[0]);
            _entity.RootCauses.Add(rootCauses[2]);

            _viewModel.Map(_entity);

            Assert.AreEqual(2, _viewModel.RootCauses.Length);
            Assert.AreEqual(_entity.RootCauses[0].Id, _viewModel.RootCauses[0]);
            Assert.AreEqual(_entity.RootCauses[1].Id, _viewModel.RootCauses[1]);
        }
    }

    [TestClass]
    public class CreateEnvironmentalNonComplianceEventTest : EnvironmentalNonComplianceEventViewModelTestBase<CreateEnvironmentalNonComplianceEvent>
    {
        [TestMethod]
        public void TestMapToEntitySetsIssueYear()
        {
            _vmTester.ViewModel.EventDate = DateTime.Now;
            _vmTester.MapToEntity();

            Assert.AreEqual(_vmTester.Entity.IssueYear, _vmTester.Entity.EventDate.Year);
        }
    }
}
