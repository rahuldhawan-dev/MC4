using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using System;

// NOTE: If a test is being removed because the functionality is moved to an
//       inheriting model, MOVE THE TEST TO THAT TEST CLASS.

// TODO: Make this a base test class that the other test classes can inherit. No time to do this as part of MC-2137.

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class BaseIncidentModelTest : MapCallMvcInMemoryDatabaseTestBase<Incident>
    {
        private ViewModelTester<TestIncidentModel, Incident> _vmTester;
        private TestIncidentModel _viewModel;
        private Incident _entity;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IVehicleRepository>().Use<VehicleRepository>();
            e.For<ICommercialDriversLicenseProgramStatusRepository>().Use<CommercialDriversLicenseProgramStatusRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new TestIncidentModel(_container);
            _entity = GetFactory<IncidentFactory>().Create();
            _vmTester = new ViewModelTester<TestIncidentModel, Incident>(_viewModel, _entity);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.AccidentStreetName);
            _vmTester.CanMapBothWays(x => x.AccidentStreetNumber);
            _vmTester.CanMapBothWays(x => x.AthleticActivitiesInLastTwelveMonths);
            _vmTester.CanMapBothWays(x => x.CaseManager);
            _vmTester.CanMapBothWays(x => x.CaseNumber);
            _vmTester.CanMapBothWays(x => x.Claimant);
            _vmTester.CanMapBothWays(x => x.DrugAndAlcoholTestingNotes);
            _vmTester.CanMapBothWays(x => x.FiveWhysCompleted);
            _vmTester.CanMapBothWays(x => x.IncidentCommitteeReportCompletionDate);
            _vmTester.CanMapBothWays(x => x.IncidentCommitteeReportResults);
            _vmTester.CanMapBothWays(x => x.IncidentDate);
            _vmTester.CanMapBothWays(x => x.IsChargeableMotorVehicleAccident);
            _vmTester.CanMapBothWays(x => x.IsInLitigation);
            _vmTester.CanMapBothWays(x => x.IsOvertime);
            _vmTester.CanMapBothWays(x => x.IsOSHARecordable);
            _vmTester.CanMapBothWays(x => x.IsSafetyCodeViolation);
            _vmTester.CanMapBothWays(x => x.MarkoutNumber);
            _vmTester.CanMapBothWays(x => x.MedicalProviderName);
            _vmTester.CanMapBothWays(x => x.MedicalProviderPhone);
            _vmTester.CanMapBothWays(x => x.NatureOfPriorInjury);
            _vmTester.CanMapBothWays(x => x.OtherEmployers);
            _vmTester.CanMapBothWays(x => x.PoliceReportFiled);
            _vmTester.CanMapBothWays(x => x.PremiseNumber);
            _vmTester.CanMapBothWays(x => x.PriorInjuryDate);
            _vmTester.CanMapBothWays(x => x.PriorInjuryMedicalProvider);
            _vmTester.CanMapBothWays(x => x.QuestionEmployeeDoingBeforeIncidentOccurred);
            _vmTester.CanMapBothWays(x => x.QuestionWhatHappened);
            _vmTester.CanMapBothWays(x => x.QuestionInjuryOrIllness);
            _vmTester.CanMapBothWays(x => x.QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee);
            _vmTester.CanMapBothWays(x => x.QuestionHaveHadSimilarInjuryBefore);
            _vmTester.CanMapBothWays(x => x.QuestionParticipatedInAthleticActivitiesInLastTwelveMonths);
            _vmTester.CanMapBothWays(x => x.SoughtMedicalAttention);
            _vmTester.CanMapBothWays(x => x.WorkOrderId);
            _vmTester.CanMapBothWays(x => x.WitnessName);
            _vmTester.CanMapBothWays(x => x.WitnessPhone);
            _vmTester.CanMapBothWays(x => x.Why1);
            _vmTester.CanMapBothWays(x => x.Why2);
            _vmTester.CanMapBothWays(x => x.Why3);
            _vmTester.CanMapBothWays(x => x.Why4);
            _vmTester.CanMapBothWays(x => x.Why5);
            _vmTester.CanMapBothWays(x => x.DateSubmitted);
            _vmTester.CanMapBothWays(x => x.AccidentCoordinate, GetFactory<CoordinateFactory>().Create());
            _vmTester.CanMapBothWays(x => x.AccidentTown, GetFactory<TownFactory>().Create());
            _vmTester.CanMapBothWays(x => x.AtRiskBehaviorSection, GetEntityFactory<AtRiskBehaviorSection>().Create());
            _vmTester.CanMapBothWays(x => x.AtRiskBehaviorSubSection, GetEntityFactory<AtRiskBehaviorSubSection>().Create());
            _vmTester.CanMapBothWays(x => x.DrugAndAlcoholTestingDecision, GetFactory<IncidentDrugAndAlcoholTestingDecisionFactory>().Create());
            _vmTester.CanMapBothWays(x => x.DrugAndAlcoholTestingResult, GetFactory<IncidentDrugAndAlcoholTestingResultFactory>().Create());
            _vmTester.CanMapBothWays(x => x.EmployeeType, GetFactory<EmployeeTypeContractorFactory>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentClassification, GetFactory<IncidentClassificationFactory>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentShift, GetFactory<IncidentShiftFactory>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentType, GetFactory<IncidentTypeFactory>().Create());
            _vmTester.CanMapBothWays(x => x.MedicalProviderTown, GetFactory<TownFactory>().Create());
            _vmTester.CanMapBothWays(x => x.QuestionHaveJobOutsideOfAmericanWater);
            _vmTester.CanMapBothWays(x => x.EventExposureType, GetEntityFactory<EventExposureType>().Create());
            _vmTester.CanMapBothWays(x => x.Facility, GetFactory<FacilityFactory>().Create());
            _vmTester.CanMapBothWays(x => x.GeneralLiabilityCode, GetFactory<GeneralLiabilityCodeFactory>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentClassification, GetFactory<IncidentClassificationFactory>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentShift, GetFactory<IncidentShiftFactory>().Create());
            _vmTester.CanMapBothWays(x => x.IncidentType, GetFactory<IncidentTypeFactory>().Create());
            _vmTester.CanMapBothWays(x => x.MedicalProviderTown, GetFactory<TownFactory>().Create());
            _vmTester.CanMapBothWays(x => x.MotorVehicleCode, GetFactory<MotorVehicleCodeFactory>().Create());
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetFactory<OperatingCenterFactory>().Create());
            _vmTester.CanMapBothWays(x => x.SeriousInjuryOrFatalityType, GetFactory<SeriousInjuryOrFatalityTypeFactory>().Create());
            _vmTester.CanMapBothWays(x => x.Vehicle, GetFactory<VehicleFactory>().Create());
            _vmTester.CanMapBothWays(x => x.GeneralLiabilityCode, GetFactory<GeneralLiabilityCodeFactory>().Create());
            _vmTester.CanMapBothWays(x => x.DrugAndAlcoholTestingDecision, GetFactory<IncidentDrugAndAlcoholTestingDecisionFactory>().Create());
            _vmTester.CanMapBothWays(x => x.DrugAndAlcoholTestingResult, GetFactory<IncidentDrugAndAlcoholTestingResultFactory>().Create());
            _vmTester.CanMapBothWays(x => x.MapCallWorkOrder, GetFactory<WorkOrderFactory>().Create());
        }

        #endregion

        [TestMethod]
        public void TestAccidentStateGetsMappedToViewModelWhenAccidentTownHasValue()
        {
            var town = GetFactory<TownFactory>().Create();
            _entity.AccidentTown = town;

            _vmTester.MapToViewModel();
            Assert.AreEqual(town.County.State.Id, _viewModel.AccidentState);
        }

        [TestMethod]
        public void TestAccidentStateGetsMappedToViewModelAsNullWhenAccidentTownDoesNotHaveValue()
        {
            _entity.AccidentTown = null;
            _viewModel.AccidentState = 323; // Make sure it gets nulled out
            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.AccidentState);
        }

        [TestMethod]
        public void TestAccidentCountyGetsMappedToViewModelWhenAccidentTownHasValue()
        {
            var town = GetFactory<TownFactory>().Create();
            _entity.AccidentTown = town;

            _vmTester.MapToViewModel();
            Assert.AreEqual(town.County.Id, _viewModel.AccidentCounty);
        }

        [TestMethod]
        public void TestAccidentCountyGetsMappedToViewModelAsNullWhenAccidentTownDoesNotHaveValue()
        {
            _entity.AccidentTown = null;
            _viewModel.AccidentCounty = 323; // Make sure it gets nulled out

            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.AccidentCounty);
        }

        [TestMethod]
        public void TestBodyPartsCanMapBothWays()
        {
            var bodyParts = GetEntityFactory<BodyPart>().CreateList(4);
            _entity.BodyParts.Add(bodyParts[0]);
            _entity.BodyParts.Add(bodyParts[2]);

            _viewModel.Map(_entity);

            Assert.AreEqual(2, _viewModel.BodyParts.Length);
            Assert.AreEqual(_entity.BodyParts[0].Id, _viewModel.BodyParts[0]);
            Assert.AreEqual(_entity.BodyParts[1].Id, _viewModel.BodyParts[1]);
        }

        [TestMethod]
        public void TestIncidentDateSetsTargetCompletionDate()
        {
            var expectedIncidentDate = DateTime.Now;
            var expectedIncidentTargetDate = expectedIncidentDate.AddDays(10);

            _entity.IncidentDate = expectedIncidentDate;
            _viewModel.Map(_entity);
            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(expectedIncidentDate, _viewModel.IncidentDate);
            Assert.AreEqual(_entity.IncidentDate, _viewModel.IncidentDate);
            Assert.AreEqual(_entity.IncidentCommitteeReportTargetCompletionDate, expectedIncidentTargetDate);
        }
        
        [TestMethod]
        public void TestEmployeeMapsToEntityIfUserIsSiteAdmin()
        {
            var emp = GetEntityFactory<Employee>().Create();
            _entity.Employee = emp;
            _viewModel.Employee = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(emp.Id, _viewModel.Employee);

            _entity.Employee = null;
            _vmTester.MapToEntity();
            Assert.AreSame(emp, _entity.Employee);
        }

        [TestMethod]
        public void TestFacilityMapsToEntityIfUserIsSiteAdmin()
        {
            var fac = GetFactory<FacilityFactory>().Create();
            _entity.Facility = fac;
            _viewModel.Facility = null;

            _vmTester.MapToViewModel();
            Assert.AreEqual(fac.Id, _viewModel.Facility);

            _entity.Facility = null;
            _vmTester.MapToEntity();
            Assert.AreSame(fac, _entity.Facility);
        }

        [TestMethod]
        public void TestMedicalProviderStateGetsMappedToViewModelWhenMedicalProviderTownHasValue()
        {
            var town = GetFactory<TownFactory>().Create();
            _entity.MedicalProviderTown = town;

            _vmTester.MapToViewModel();
            Assert.AreEqual(town.County.State.Id, _viewModel.MedicalProviderState);
        }

        [TestMethod]
        public void TestMedicalProviderStateGetsMappedToViewModelAsNullWhenMedicalProviderTownDoesNotHaveValue()
        {
            _entity.MedicalProviderTown = null;
            _viewModel.MedicalProviderState = 323; // Make sure it gets nulled out

            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.MedicalProviderState);
        }

        [TestMethod]
        public void TestMedicalProviderCountyGetsMappedToViewModelWhenMedicalProviderTownHasValue()
        {
            var town = GetFactory<TownFactory>().Create();
            _entity.MedicalProviderTown = town;

            _vmTester.MapToViewModel();
            Assert.AreEqual(town.County.Id, _viewModel.MedicalProviderCounty);
        }

        [TestMethod]
        public void TestMedicalProviderCountyGetsMappedToViewModelAsNullWhenMedicalProviderTownDoesNotHaveValue()
        {
            _entity.MedicalProviderTown = null;
            _viewModel.MedicalProviderCounty = 323; // Make sure it gets nulled out

            _vmTester.MapToViewModel();
            Assert.IsNull(_viewModel.MedicalProviderCounty);
        }

        #region Validation

        [TestMethod]
        public void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.QuestionEmployeeDoingBeforeIncidentOccurred, "This field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.QuestionWhatHappened, "This field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.QuestionInjuryOrIllness, "This field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee, "This field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentSummary, "The IncidentSummary field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsOvertime);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentShift);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentClassification);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Facility);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EventExposureType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DrugAndAlcoholTestingDecision, "The Decision field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentTown);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentStreetNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentStreetName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentState);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentCounty);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentCoordinate, "The Incident Coordinates field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EmployeeType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WorkersCompensationClaimStatus);
        }

        [TestMethod]
        public void TestRequiredWhenValidation()
        {
            var someSection = GetEntityFactory<AtRiskBehaviorSection>().Create();
            var someSubSection = GetFactory<AtRiskBehaviorSubSectionFactory>().Create();
            var empType = GetFactory<EmployeeTypeContractorFactory>().Create();
            var town = GetEntityFactory<Town>().Create();

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PriorInjuryMedicalProvider, "the dr.", x => x.QuestionHaveHadSimilarInjuryBefore, true, false, "This field is required.");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PriorInjuryDate, DateTime.Today, x => x.QuestionHaveHadSimilarInjuryBefore, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.OtherEmployers, "AW", x => x.QuestionHaveJobOutsideOfAmericanWater, true, false, "This field is required.");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.NumberOfHoursOvertimeInPastWeek, (decimal)12.5, x => x.IsOvertime, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.NatureOfPriorInjury, "stuff", x => x.QuestionHaveHadSimilarInjuryBefore, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MedicalProviderName, "The Dr", x => x.SoughtMedicalAttention, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MedicalProviderPhone, "8561234585", x => x.SoughtMedicalAttention, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.MedicalProviderTown, town.Id, x => x.SoughtMedicalAttention, true, false);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AtRiskBehaviorSection, someSection.Id, x => x.AtRiskBehaviorSubSection, someSubSection.Id);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ContractorCompany, "the Dr", x => x.EmployeeType, empType.Id);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ContractorName, "The Doctor", x => x.EmployeeType, empType.Id);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.Employee, _entity.Employee.Id, x => x.EmployeeType, 1);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.AthleticActivitiesInLastTwelveMonths, "stuff", x => x.QuestionParticipatedInAthleticActivitiesInLastTwelveMonths, true, false, "This field is required.");
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.DateSubmitted, DateTime.Today, x => x.FiveWhysCompleted, true, false, "This field is required when 'Five Whys Completed' is Yes.");
        }

        [TestMethod]
        public void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetFactory<OperatingCenterFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Vehicle, GetFactory<VehicleFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.MotorVehicleCode, GetFactory<MotorVehicleCodeFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.MedicalProviderTown, GetFactory<TownFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.IncidentType, GetFactory<IncidentTypeFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.IncidentShift, GetFactory<IncidentShiftFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.IncidentClassification, GetFactory<IncidentClassificationFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.GeneralLiabilityCode, GetFactory<GeneralLiabilityCodeFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Facility, GetFactory<FacilityFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.EventExposureType, GetEntityFactory<EventExposureType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.DrugAndAlcoholTestingResult, GetFactory<IncidentDrugAndAlcoholTestingResultFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.DrugAndAlcoholTestingDecision, GetFactory<IncidentDrugAndAlcoholTestingDecisionFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.AccidentTown, GetEntityFactory<Town>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.AccidentCoordinate, GetFactory<CoordinateFactory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.WorkersCompensationClaimStatus, GetFactory<WorkersCompensationClaimStatusFactory>().Create());
        }

        [TestMethod]
        public void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.WitnessPhone, Incident.StringLengths.MAX_WITNESS_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.WitnessName, Incident.StringLengths.MAX_WITNESS_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.WorkOrderId, Incident.StringLengths.MAX_WORKORDER_ID);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why1, Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why2, Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why3, Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why4, Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Why5, Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.IncidentSummary, Incident.StringLengths.INCIDENT_SUMMARY);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PremiseNumber, Incident.StringLengths.MAX_PREMISE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MedicalProviderPhone, Incident.StringLengths.MAX_MED_PROVIDER_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MedicalProviderName, Incident.StringLengths.MAX_MED_PROVIDER_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.MarkoutNumber, Incident.StringLengths.MAX_MARKOUT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Claimant, Incident.StringLengths.MAX_CLAIMANT);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CaseNumber, Incident.StringLengths.MAX_CASE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.CaseManager, Incident.StringLengths.MAX_CASE_MANAGER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.AccidentStreetNumber, Incident.StringLengths.MAX_ACCIDENT_STREET_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.AccidentStreetName, Incident.StringLengths.MAX_ACCIDENT_STREET_NAME);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ClaimsCarrierId, Incident.StringLengths.MAX_CLAIMS_CARRIER_ID);
        }

        #endregion

        #region IncidentType

        [TestMethod]
        public void TestIncidentTypeModelStateIsValid()
        {
            _viewModel.IncidentType = GetFactory<IncidentTypeFactory>().Create().Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.IncidentType);
        }

        #endregion

        #region DrugAndAlcoholTestingResult

        [TestMethod]
        public void TestDrugAndAlcoholTestingResultIsNotRequiredIfCompletionDateIsNull()
        {
            _viewModel.DrugAndAlcoholTestingResult = null;
            _viewModel.IncidentCommitteeReportCompletionDate = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.DrugAndAlcoholTestingResult);
        }

        [TestMethod]
        public void TestDrugAndAlcoholTestingResultEntityMustExistIfNotNull()
        {
            _viewModel.DrugAndAlcoholTestingResult = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.DrugAndAlcoholTestingResult);
        }

        [TestMethod]
        public void TestDrugAndAlcoholTestingResultIsRequiredIfCompletionDateIsSetAndDecisionRequiresTesting()
        {
            var decision = GetFactory<IncidentDrugAndAlcoholTestingDecisionFactory>().Create(new { Description = "TEST - Do it!" });

            _viewModel.DrugAndAlcoholTestingDecision = decision.Id;
            _viewModel.DrugAndAlcoholTestingResult = null;
            _viewModel.IncidentCommitteeReportCompletionDate = DateTime.Today;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.DrugAndAlcoholTestingResult,
                "A drug and alcohol testing result is required when a decision that requires testing is entered.");
        }

        [TestMethod]
        public void TestDrugAndAlcoholTestingResultIsNotRequiredIfCompletionDateIsSetAndDecisionDoesNotRequireTesting()
        {
            var decision = GetFactory<IncidentDrugAndAlcoholTestingDecisionFactory>().Create(new { Description = "NO TEST - Don't do it!" });

            _viewModel.DrugAndAlcoholTestingDecision = decision.Id;
            _viewModel.DrugAndAlcoholTestingResult = null;
            _viewModel.IncidentCommitteeReportCompletionDate = DateTime.Today;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.DrugAndAlcoholTestingResult);
        }

        #endregion

        #region Test Classes

        private class TestIncidentModel : BaseIncidentModel
        {
            public TestIncidentModel(IContainer container) : base(container) { }
        }

        #endregion
    }
}