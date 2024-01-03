using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateIncidentTest : ViewModelTestBase<Incident, CreateIncident>
    {
        #region Private Members
        
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        
        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IVehicleRepository>().Use<VehicleRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _authServ.Setup(x => x.CurrentUser)
                     .Returns(_user = GetEntityFactory<User>().Create());
        }

        protected override Incident CreateEntity()
        {
            return new Incident {
                Employee = GetFactory<EmployeeFactory>().Create()
            };
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.AccidentCoordinate);
            _vmTester.CanMapBothWays(x => x.AccidentStreetName);
            _vmTester.CanMapBothWays(x => x.AccidentStreetNumber);
            _vmTester.CanMapBothWays(x => x.AccidentTown);
            _vmTester.CanMapBothWays(x => x.IncidentSummary);
            _vmTester.CanMapBothWays(x => x.AnyImmediateCorrectiveActionsApplied);
            _vmTester.CanMapBothWays(x => x.AthleticActivitiesInLastTwelveMonths);
            _vmTester.CanMapBothWays(x => x.CaseManager);
            _vmTester.CanMapBothWays(x => x.CaseNumber);
            _vmTester.CanMapBothWays(x => x.Claimant);
            _vmTester.CanMapBothWays(x => x.ContractorCompany);
            _vmTester.CanMapBothWays(x => x.ContractorName);
            _vmTester.CanMapBothWays(x => x.DateInvestigationWillBeCompleted);
            _vmTester.CanMapBothWays(x => x.DrugAndAlcoholTestingNotes);
            _vmTester.CanMapBothWays(x => x.DrugAndAlcoholTestingDecision);
            _vmTester.CanMapBothWays(x => x.DrugAndAlcoholTestingResult);
            // TODO: this fails and I'm not sure why
            // _vmTester.CanMapBothWays(x => x.Employee);
            _vmTester.CanMapBothWays(x => x.EmployeeType);
            _vmTester.CanMapBothWays(x => x.Facility);
            _vmTester.CanMapBothWays(x => x.FiveWhysCompleted);
            _vmTester.CanMapBothWays(x => x.GeneralLiabilityCode);
            _vmTester.CanMapBothWays(x => x.IncidentCommitteeReportCompletionDate);
            _vmTester.CanMapBothWays(x => x.IncidentCommitteeReportResults);
            _vmTester.CanMapBothWays(x => x.IncidentClassification);
            _vmTester.CanMapBothWays(x => x.IncidentDate);
            _vmTester.CanMapBothWays(x => x.IncidentReportedDate);
            _vmTester.CanMapBothWays(x => x.IncidentType);
            _vmTester.CanMapBothWays(x => x.EventExposureType);
            // TODO: not sure how to test arrays/collections with this
            // _vmTester.CanMapBothWays(x => x.BodyParts);
            _vmTester.CanMapBothWays(x => x.IncidentShift);
            _vmTester.CanMapBothWays(x => x.IsChargeableMotorVehicleAccident);
            _vmTester.CanMapBothWays(x => x.IsInLitigation);
            _vmTester.CanMapBothWays(x => x.IsOSHARecordable);
            _vmTester.CanMapBothWays(x => x.IsSafetyCodeViolation);
            _vmTester.CanMapBothWays(x => x.SeriousInjuryOrFatalityType);
            _vmTester.CanMapBothWays(x => x.IsOvertime);
            _vmTester.CanMapBothWays(x => x.MapCallWorkOrder);
            _vmTester.CanMapBothWays(x => x.MarkoutNumber);
            _vmTester.CanMapBothWays(x => x.MotorVehicleCode);
            _vmTester.CanMapBothWays(x => x.MedicalProviderName);
            _vmTester.CanMapBothWays(x => x.MedicalProviderPhone);
            _vmTester.CanMapBothWays(x => x.MedicalProviderTown);
            _vmTester.CanMapBothWays(x => x.NatureOfPriorInjury);
            _vmTester.CanMapBothWays(x => x.NumberOfHoursOvertimeInPastWeek);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
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
            _vmTester.CanMapBothWays(x => x.QuestionHaveJobOutsideOfAmericanWater);
            _vmTester.CanMapBothWays(x => x.SoughtMedicalAttention);
            _vmTester.CanMapBothWays(x => x.TravelersReport);
            _vmTester.CanMapBothWays(x => x.Vehicle);
            _vmTester.CanMapBothWays(x => x.WitnessName);
            _vmTester.CanMapBothWays(x => x.WitnessPhone);
            _vmTester.CanMapBothWays(x => x.WorkOrderId);
            _vmTester.CanMapBothWays(x => x.AtRiskBehaviorSection);
            _vmTester.CanMapBothWays(x => x.AtRiskBehaviorSubSection);
            _vmTester.CanMapBothWays(x => x.Why1);
            _vmTester.CanMapBothWays(x => x.Why2);
            _vmTester.CanMapBothWays(x => x.Why3);
            _vmTester.CanMapBothWays(x => x.Why4);
            _vmTester.CanMapBothWays(x => x.Why5);
            _vmTester.CanMapBothWays(x => x.WorkersCompensationClaimStatus);
            _vmTester.CanMapBothWays(x => x.ClaimsCarrierId);
            _vmTester.CanMapBothWays(x => x.EmployeeSpokeWithNurse);
            _vmTester.CanMapBothWays(x => x.IncidentNurseRecommendationType);
            _vmTester.CanMapBothWays(x => x.RecommendedMedicalProvider);
            _vmTester.CanMapBothWays(x => x.NonMedicalTreatmentRecommendation);
            _vmTester.CanMapBothWays(x => x.EmployeeAcceptedRecommendationByNurse);
            _vmTester.CanMapBothWays(x => x.ReasonEmployeeDidNotAcceptRecommendationByNurse);
            _vmTester.CanMapBothWays(x => x.NursePhone);
        }

        [TestMethod]
        public void TestMapToEntitySetsSupervisorToEmployeesReportsTo()
        {
            var super = GetFactory<EmployeeFactory>().Create();
            var emp = GetFactory<EmployeeFactory>().Create(new {
                ReportsTo = super
            });
            _viewModel.Employee = emp.Id;
            _viewModel.EmployeeType = EmployeeType.Indices.EMPLOYEE;
            _vmTester.MapToEntity();

            Assert.AreEqual(super.Id, _entity.Supervisor.Id);
            Assert.AreSame(super, _entity.Supervisor);
        }

        [TestMethod]
        public void TestMapToEntitySetsEmployeePersonnelAreaToPersonnelAreaOfIncident()
        {
            var personnel = GetEntityFactory<PersonnelArea>().Create();
            var emp = GetFactory<EmployeeFactory>().Create(new {
                PersonnelArea = personnel
            });
            _viewModel.Employee = emp.Id;
            _viewModel.EmployeeType = 1;
            _vmTester.MapToEntity();

            Assert.AreEqual(personnel.Id, _entity.PersonnelArea.Id);
            Assert.AreSame(personnel, _entity.PersonnelArea);
        }

        [TestMethod]
        public void TestMapToEntitySetsNullSupervisorIfEmployeeReportsToIsNull()
        {
            var emp = GetFactory<EmployeeFactory>().Create();
            emp.ReportsTo = null;

            _viewModel.Employee = emp.Id;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.Supervisor);
        }

        [TestMethod]
        public void TestMapToEntitySetsCreatedByToCurrentUserName()
        {
            _user.UserName = "Cool";
            _vmTester.MapToEntity();
            Assert.AreEqual(_user.UserName, _entity.CreatedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsPositionHistoryToEmployeesCurrentPosition()
        {
            var expectedPosition = new Position();
            var emp = GetFactory<EmployeeFactory>().Create();
            emp.CurrentPosition = expectedPosition;

            _viewModel.Employee = emp.Id;
            _viewModel.EmployeeType = 1;
            _vmTester.MapToEntity();
            Assert.AreSame(expectedPosition, _entity.Position);
        }

        [TestMethod]
        public void TestMapToEntitySetsIncidentStatusToOpen()
        {
            var incidentStatus = GetEntityFactory<IncidentStatus>().Create(new {
                Description = "Open"
            });
            
            _vmTester.MapToEntity();
            
            Assert.AreSame(incidentStatus, _entity.IncidentStatus);
        }

        [TestMethod]
        public void TestMapToEntitySetsWorkersCompensationClaimStatus()
        {
            var WorkersCompensationClaimStatus = GetEntityFactory<WorkersCompensationClaimStatus>().Create();

            _viewModel.WorkersCompensationClaimStatus = WorkersCompensationClaimStatus.Id;
            _vmTester.MapToEntity();

            Assert.AreSame(
                WorkersCompensationClaimStatus,
                _entity.WorkersCompensationClaimStatus);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestRequiredValidation()
        { 
            ValidationAssert.PropertyIsRequired(
                _viewModel,
                x => x.AccidentCoordinate,
                "The Incident Coordinates field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentStreetName);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentStreetNumber);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentState);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentCounty);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AccidentTown);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentSummary);
            ValidationAssert.PropertyIsRequired(
                _viewModel,
                x => x.DrugAndAlcoholTestingDecision,
                "The Decision field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EmployeeType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Facility);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FiveWhysCompleted);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentClassification);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentReportedDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EventExposureType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IncidentShift);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsOvertime);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(
                _viewModel,
                x => x.QuestionEmployeeDoingBeforeIncidentOccurred,
                "This field is required.");
            ValidationAssert.PropertyIsRequired(
                _viewModel,
                x => x.QuestionWhatHappened,
                "This field is required.");
            ValidationAssert.PropertyIsRequired(
                _viewModel,
                x => x.QuestionInjuryOrIllness,
                "This field is required.");
            ValidationAssert.PropertyIsRequired(
                _viewModel,
                x => x.QuestionWhatObjectOrSubstanceDirectlyHarmedEmployee,
                "This field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.WorkersCompensationClaimStatus);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EmployeeSpokeWithNurse);
            
            // RequiredWhen
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.AthleticActivitiesInLastTwelveMonths,
                "some string value",
                x => x.QuestionParticipatedInAthleticActivitiesInLastTwelveMonths,
                true,
                false,
                "This field is required.");
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.ContractorCompany,
                "some string value",
                x => x.EmployeeType,
                EmployeeType.Indices.CONTRACTOR);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.ContractorName,
                "some string value",
                x => x.EmployeeType,
                EmployeeType.Indices.CONTRACTOR);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.DateInvestigationWillBeCompleted,
                DateTime.Now,
                x => x.IsOSHARecordable,
                true);
            // this fails because the override property on CreateIncident doesn't have RequiredWhen, and for whatever
            // reason reflection isn't finding the attribute on the base class' property
            // ValidationAssert.PropertyIsRequiredWhen(
            //     _viewModel,
            //     x => x.Employee,
            //     GetFactory<ActiveEmployeeFactory>().Create().Id,
            //     x => x.EmployeeType,
            //     EmployeeType.Indices.EMPLOYEE);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.MedicalProviderName,
                "some string value",
                x => x.SoughtMedicalAttention,
                true);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.MedicalProviderPhone,
                "some string value",
                x => x.SoughtMedicalAttention,
                true);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.MedicalProviderState,
                1234,
                x => x.SoughtMedicalAttention,
                true);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.MedicalProviderCounty,
                1234,
                x => x.SoughtMedicalAttention,
                true);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.MedicalProviderTown,
                GetEntityFactory<Town>().Create().Id,
                x => x.SoughtMedicalAttention,
                true);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.NatureOfPriorInjury,
                "some string value",
                x => x.QuestionHaveHadSimilarInjuryBefore,
                true);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.NumberOfHoursOvertimeInPastWeek,
                1234m,
                x => x.IsOvertime,
                true);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.OtherEmployers,
                "some string value",
                x => x.QuestionHaveJobOutsideOfAmericanWater,
                true,
                false,
                "This field is required.");
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.PriorInjuryDate,
                DateTime.Now,
                x => x.QuestionHaveHadSimilarInjuryBefore,
                true);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.PriorInjuryMedicalProvider,
                "some string value",
                x => x.QuestionHaveHadSimilarInjuryBefore,
                true,
                false,
                "This field is required.");
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.AtRiskBehaviorSection,
                GetEntityFactory<AtRiskBehaviorSection>().Create().Id,
                x => x.AtRiskBehaviorSubSection,
                GetEntityFactory<AtRiskBehaviorSubSection>().Create().Id);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.Why1,
                "some string value",
                x => x.FiveWhysCompleted,
                true,
                false,
                "This field is required when 'Five Whys Completed' is Yes.");
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.DateSubmitted,
                DateTime.Now,
                x => x.FiveWhysCompleted,
                true,
                false,
                "This field is required when 'Five Whys Completed' is Yes.");
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.ClaimsCarrierId,
                "some string value",
                x => x.WorkersCompensationClaimStatus,
                WorkersCompensationClaimStatus.Indices.OPEN);
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel,
                x => x.ClaimsCarrierId,
                "some string value",
                x => x.WorkersCompensationClaimStatus,
                WorkersCompensationClaimStatus.Indices.CLOSED_ACCEPTED);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.AccidentCoordinate,
                GetEntityFactory<Coordinate>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.AccidentTown,
                GetEntityFactory<Town>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.DrugAndAlcoholTestingDecision,
                GetEntityFactory<IncidentDrugAndAlcoholTestingDecision>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.DrugAndAlcoholTestingResult,
                GetEntityFactory<IncidentDrugAndAlcoholTestingResult>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.Employee,
                GetFactory<ActiveEmployeeFactory>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.EmployeeType,
                GetEntityFactory<EmployeeType>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.Facility,
                GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.GeneralLiabilityCode,
                GetEntityFactory<GeneralLiabilityCode>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.IncidentClassification,
                GetEntityFactory<IncidentClassification>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.IncidentType,
                GetEntityFactory<IncidentType>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.EventExposureType,
                GetEntityFactory<EventExposureType>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.IncidentShift,
                GetEntityFactory<IncidentShift>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.MotorVehicleCode,
                GetEntityFactory<MotorVehicleCode>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.MedicalProviderTown,
                GetEntityFactory<Town>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.OperatingCenter,
                GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.Vehicle,
                GetEntityFactory<Vehicle>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.AtRiskBehaviorSection,
                GetEntityFactory<AtRiskBehaviorSection>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.AtRiskBehaviorSubSection,
                GetEntityFactory<AtRiskBehaviorSubSection>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.WorkersCompensationClaimStatus,
                GetEntityFactory<WorkersCompensationClaimStatus>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.EmployeeSpokeWithNurse,
                GetEntityFactory<EmployeeSpokeWithNurse>().Create());
            ValidationAssert.EntityMustExist(
                _viewModel,
                x => x.IncidentNurseRecommendationType,
                GetEntityFactory<IncidentNurseRecommendationType>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.AccidentStreetName,
                Incident.StringLengths.MAX_ACCIDENT_STREET_NAME);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.AccidentStreetNumber,
                Incident.StringLengths.MAX_ACCIDENT_STREET_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.IncidentSummary,
                Incident.StringLengths.INCIDENT_SUMMARY);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.CaseManager,
                Incident.StringLengths.MAX_CASE_MANAGER);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.CaseNumber,
                Incident.StringLengths.MAX_CASE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.Claimant,
                Incident.StringLengths.MAX_CLAIMANT);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.ContractorCompany,
                Incident.StringLengths.CONTRACTOR_COMPANY);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.ContractorName,
                Incident.StringLengths.CONTRACTOR_NAME);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.MarkoutNumber,
                Incident.StringLengths.MAX_MARKOUT_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.MedicalProviderName,
                Incident.StringLengths.MAX_MED_PROVIDER_NAME);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.MedicalProviderPhone,
                Incident.StringLengths.MAX_MED_PROVIDER_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.PremiseNumber,
                Incident.StringLengths.MAX_PREMISE_NUMBER);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.WitnessName,
                Incident.StringLengths.MAX_WITNESS_NAME);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.WitnessPhone,
                Incident.StringLengths.MAX_WITNESS_PHONE);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.WorkOrderId,
                Incident.StringLengths.MAX_WORKORDER_ID);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.Why1,
                Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.Why2,
                Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.Why3,
                Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.Why4,
                Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.Why5,
                Incident.StringLengths.FIVE_WHYS);
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel,
                x => x.ClaimsCarrierId,
                Incident.StringLengths.MAX_CLAIMS_CARRIER_ID);
        }

        #region Employee

        [TestMethod]
        public void TestEmployeeMustBeActiveEmployee()
        {
            var emp = GetFactory<EmployeeFactory>().Create(new { IsActive = false });
            _viewModel.EmployeeType = EmployeeType.Indices.EMPLOYEE;
            _viewModel.Employee = emp.Id;
            ValidationAssert.ModelStateHasError(
                _viewModel,
                x => x.Employee,
                CreateIncident.VALIDATION_ERROR_INACTIVE_EMPLOYEE);
        }

        #endregion

        #endregion

        #region Constructor

        [TestMethod]
        public void TestTargetCompletionDateIs10DaysLater()
        {
            var expected = new DateTime(0001, 1, 1);
            var expectedTarget = expected.AddDays(10);
            var target = new CreateIncident(_container);

            target.SetDefaults();
            target.Map(_entity);
            target.MapToEntity(_entity);

            Assert.AreEqual(expected, target.IncidentReportedDate.Value);
            Assert.AreEqual(expected, _entity.IncidentReportedDate);
            Assert.AreEqual(expectedTarget, _entity.IncidentCommitteeReportTargetCompletionDate);
        }

        #endregion

        #region SetDefaults

        [TestMethod]
        public void TestSetDefaultsSetsFiveWhysCompletedToFalse()
        {
            _viewModel.FiveWhysCompleted = null;
            _viewModel.SetDefaults();
            Assert.IsFalse(_viewModel.FiveWhysCompleted.Value);
        }

        [TestMethod]
        public void TestSetDefaultsSetsDrugAndAlcoholTestingDecision()
        {
            _viewModel.SetDefaults();
            Assert.AreEqual(IncidentDrugAndAlcoholTestingDecision.Indices.IMMEDIATE_MEDICAL_TREATMENT_NOT_REQUIRED, _viewModel.DrugAndAlcoholTestingDecision);
        }

        #endregion

        #endregion
    }
}
