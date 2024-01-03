using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.HumanResources.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HumanResources.Models.ViewModels
{
    public abstract class CovidIssueViewModelTest<TViewModel> : ViewModelTestBase<CovidIssue, TViewModel> where TViewModel : CovidIssueViewModel
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            GetEntityFactory<ReleaseReason>().Create();
            _viewModel.SupervisorsCell = "(123)-123-1234 x123456";
            _viewModel.LocalEmployeeRelationsBusinessPartnerCell = "(123)-123-1234 x123456";
            _viewModel.PersonalEmailAddress =
                "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345679801234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678@r.com";
        }

        #endregion

        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.SupervisorsCell);
            _vmTester.CanMapBothWays(x => x.LocalEmployeeRelationsBusinessPartner);
            _vmTester.CanMapBothWays(x => x.LocalEmployeeRelationsBusinessPartnerCell);
            _vmTester.CanMapBothWays(x => x.RequestType, GetEntityFactory<CovidRequestType>().Create());
            _vmTester.CanMapBothWays(x => x.SubmissionDate);
            _vmTester.CanMapBothWays(x => x.QuestionFromEmail);
            _vmTester.CanMapBothWays(x => x.SubmissionStatus, GetEntityFactory<CovidSubmissionStatus>().Create());
            _vmTester.CanMapBothWays(x => x.HealthDepartmentNotification);
            _vmTester.CanMapBothWays(x => x.OutcomeDescription);
            _vmTester.CanMapBothWays(x => x.QuarantineStatus, GetEntityFactory<CovidQuarantineStatus>().Create());
            _vmTester.CanMapBothWays(x => x.StartDate);
            _vmTester.CanMapBothWays(x => x.EstimatedReleaseDate);
            _vmTester.CanMapBothWays(x => x.ReleaseDate);
            _vmTester.CanMapBothWays(x => x.QuarantineReason);
            _vmTester.CanMapBothWays(x => x.ReleaseReason, GetEntityFactory<ReleaseReason>().Create());
            _vmTester.CanMapBothWays(x => x.PersonalEmailAddress);
            _vmTester.CanMapBothWays(x => x.WorkExposure, GetEntityFactory<CovidAnswerType>().Create());
            _vmTester.CanMapBothWays(x => x.AvoidableCloseContact, GetEntityFactory<CovidAnswerType>().Create());
            _vmTester.CanMapBothWays(x => x.FaceCoveringWorn, GetEntityFactory<CovidAnswerType>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RequestType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SubmissionDate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.QuestionFromEmail);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ReleaseReason, 1, x => x.ReleaseDate, DateTime.Now, null);

            ValidationAssert.PropertyIsRequired(_viewModel, x => x.SubmissionStatus);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.SupervisorsCell);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.LocalEmployeeRelationsBusinessPartner);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.LocalEmployeeRelationsBusinessPartnerCell);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.QuarantineStatus);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.EstimatedReleaseDate);

            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.StartDate);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.ReleaseDate);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.QuarantineReason);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.OutcomeDescription);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.OutcomeCategory);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.PersonalEmailAddress);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.HealthDepartmentNotification);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SupervisorsCell, CovidIssue.StringLengths.SUPERVISORS_CELL, true);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LocalEmployeeRelationsBusinessPartner, CovidIssue.StringLengths.LOCAL_ERBP);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.LocalEmployeeRelationsBusinessPartnerCell, CovidIssue.StringLengths.LOCAL_ERBP_CELL, true);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.PersonalEmailAddress, CovidIssue.StringLengths.PERSONAL_EMAIL_ADDRESS, true);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.OutcomeCategory, GetEntityFactory<CovidOutcomeCategory>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.QuarantineStatus, GetEntityFactory<CovidQuarantineStatus>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.ReleaseReason, GetEntityFactory<ReleaseReason>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.RequestType, GetEntityFactory<CovidRequestType>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.SubmissionStatus, GetEntityFactory<CovidSubmissionStatus>().Create());
        }

        #endregion
    }

    [TestClass]
    public class CreateCovidIssueTest : CovidIssueViewModelTest<CreateCovidIssue>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();
            _vmTester.CanMapBothWays(x => x.Employee, GetEntityFactory<Employee>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.State);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Employee);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();
            ValidationAssert.EntityMustExist(_viewModel, x => x.Employee, GetEntityFactory<Employee>().Create());
        }

        [TestMethod]
        public void TestMapToEntitySetsHumanResourcesManagerToEmployeesCurrentHumanResourcesManager()
        {
            var expected = GetEntityFactory<Employee>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { HumanResourcesManager = expected });
            _viewModel.Employee = employee.Id;
            _entity.HumanResourcesManager = null;

            _vmTester.MapToEntity();

            Assert.AreSame(expected, _entity.HumanResourcesManager);
        }

        [TestMethod]
        public void TestMapToEntitySetsPersonnelAreaToEmployeesCurrentPersonnelArea()
        {
            var expected = GetEntityFactory<PersonnelArea>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { PersonnelArea = expected });
            _viewModel.Employee = employee.Id;
            _entity.PersonnelArea = null;

            _vmTester.MapToEntity();

            Assert.AreSame(expected, _entity.PersonnelArea);
        }

        [TestMethod]
        public void TestValidationFailsIfEmployeeDoesNotHavePersonnelAreaSet()
        {
            var personnelArea = GetEntityFactory<PersonnelArea>().Create();
            var employee = GetEntityFactory<Employee>().Create(new{ PersonnelArea = (PersonnelArea)null });
            _viewModel.Employee = employee.Id;

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Employee, "The employee record must have an associated Personnel Area set.");

            employee.PersonnelArea = personnelArea;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Employee);
        }

        #endregion
    }

    [TestClass]
    public class EditCovidIssueTest : CovidIssueViewModelTest<EditCovidIssue>
    {

        [TestMethod]
        public void TestEmployeeReturnsEmployeeInstance()
        {
            Assert.AreSame(_entity.Employee, _viewModel.Employee);
        }
    }
}
