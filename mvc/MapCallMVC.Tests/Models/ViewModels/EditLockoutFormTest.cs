using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EditLockoutFormTest : MapCallMvcInMemoryDatabaseTestBase<LockoutForm>
    {
        #region Fields

        private EditLockoutForm _viewModel;
        private LockoutForm _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private ViewModelTester<EditLockoutForm, LockoutForm> _vmTester;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFacilityRepository>().Use<FacilityRepository>();
            e.For<IEquipmentRepository>().Use<EquipmentRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _authServ = new Mock<IAuthenticationService<User>>();
            _user = new User();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _now = DateTime.Now;
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now);

            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(_authServ.Object);

            _entity = GetEntityFactory<LockoutForm>().Create();
            _viewModel = _viewModelFactory.Build<EditLockoutForm, LockoutForm>(_entity);
            _vmTester = new ViewModelTester<EditLockoutForm, LockoutForm>(_viewModel, _entity);
        }

        #endregion

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ReturnedToServiceNotes);
            _vmTester.CanMapBothWays(x => x.ReturnedToServiceDateTime);
        }

        [TestMethod]
        public void TestReturnToServiceAuthorizedEmployeeCanMapBothWays()
        {
            var employee = GetFactory<EmployeeFactory>().Create();
            _entity.ReturnToServiceAuthorizedEmployee = employee;
            _vmTester.MapToViewModel();
            Assert.AreEqual(employee.Id, _viewModel.ReturnToServiceAuthorizedEmployee);

            _entity.ReturnToServiceAuthorizedEmployee = null;
            _vmTester.MapToEntity();
            Assert.AreEqual(employee, _entity.ReturnToServiceAuthorizedEmployee);
        }

        [TestMethod]
        public void TestMapToEntityMapsAnswersToEntity()
        {
            var category = GetFactory<ManagementLockoutFormQuestionCategoryFactory>().Create();
            var question = GetEntityFactory<LockoutFormQuestion>().Create(new {
                Category = category,
                Question = "Que?",
                IsActive = true,
                DisplayOrder = 1
            });
            _entity.LockoutFormAnswers.Add(new LockoutFormAnswer {
                LockoutFormQuestion = question,
                Answer = false,
                Comments = "foo",
                LockoutForm = _entity
            });
            Session.Clear();

            _viewModel.Map(_entity);

            var firstAnswer = _viewModel.EditLockoutFormAnswers.First();
            Assert.AreEqual(question.Id, firstAnswer.LockoutFormQuestion);
            Assert.AreEqual(category.Id, firstAnswer.Category);
            Assert.AreEqual(_entity.Id, firstAnswer.LockoutForm.Value);
            Assert.IsFalse(firstAnswer.Answer.Value);
            Assert.AreEqual("foo", firstAnswer.Comments);
        }

        [TestMethod]
        public void TestMapToEntityUpdatesEditLockoutFormAnswers()
        {
            var category = GetFactory<ManagementLockoutFormQuestionCategoryFactory>().Create();
            var question = GetEntityFactory<LockoutFormQuestion>().Create(new {
                Category = category,
                Question = "Que?",
                IsActive = true,
                DisplayOrder = 1
            });
            var lockoutAnswer = GetEntityFactory<LockoutFormAnswer>().Create(new {
                LockoutForm = _entity,
                LockoutFormQuestion = question,
                Answer = false,
                Comments = "foo"
            });
            _entity.LockoutFormAnswers.Add(lockoutAnswer);
            _viewModel.EditLockoutFormAnswers.Add(
                _viewModelFactory.BuildWithOverrides<EditLockoutFormAnswer, LockoutFormAnswer>(lockoutAnswer,
                    new {Answer = true, Comments = string.Empty}));
            Session.Clear();

            _viewModel.MapToEntity(_entity);

            var firstAnswer = _entity.LockoutFormAnswers.First();
            Assert.AreEqual(question.Id, firstAnswer.LockoutFormQuestion.Id);
            Assert.AreEqual(_entity, firstAnswer.LockoutForm);
            Assert.IsTrue(firstAnswer.Answer.Value);
            Assert.AreEqual(string.Empty, firstAnswer.Comments);
        }

        [TestMethod]
        public void TestManagementQuestionsAreRequired()
        {
            _viewModel.SameAsInstaller = false;
            _viewModel.SupervisorInvolved = 1;
            _viewModel.DateOfContact = new DateTime(2019, 1, 1);
            _viewModel.MethodOfContact = "Phone";
            _viewModel.OutcomeOfContact = "foo";
            _viewModel.LockRemovalMethod = GetEntityFactory<WayToRemoveLocks>().Create().Id;
            _viewModel.AuthorizedManagementPerson = 1;

            var category = GetFactory<ManagementLockoutFormQuestionCategoryFactory>().Create();
            var question = GetEntityFactory<LockoutFormQuestion>().Create(new {
                Category = category,
                Question = "Que?",
                IsActive = true,
                DisplayOrder = 1
            });

            var answer = new LockoutFormAnswer {
                LockoutFormQuestion = question,
                Answer = null
            };

            _viewModel.LockoutFormAnswers.Add(_viewModelFactory.BuildWithOverrides<LockoutFormAnswerViewModel, LockoutFormAnswer>(answer, new { Category = category.Id }));

            ValidationAssert.ModelStateHasError(_viewModel, x => x.LockoutFormAnswers, LockoutFormViewModel.ValidationErrors.MANAGEMENT);
        }

        [TestMethod]
        public void TestReturnToServiceQuestionsAreRequiredWhenReturnedToServiceEntered()
        {
            _viewModel.SameAsInstaller = false;
            _viewModel.SupervisorInvolved = 1;
            _viewModel.DateOfContact = new DateTime(2019, 1, 1);
            _viewModel.MethodOfContact = "Phone";
            _viewModel.OutcomeOfContact = "foo";
            _viewModel.LockRemovalMethod = GetEntityFactory<WayToRemoveLocks>().Create().Id;
            _viewModel.AuthorizedManagementPerson = 1;
            _viewModel.ReturnedToServiceDateTime = DateTime.Now;

            var category = GetFactory<ReturnToServiceLockoutFormQuestionCategoryFactory>().Create();
            var question = GetEntityFactory<LockoutFormQuestion>().Create(new
            {
                Category = category,
                Question = "Que?",
                IsActive = true,
                DisplayOrder = 1
            });

            var answer = new LockoutFormAnswer
            {
                LockoutFormQuestion = question,
                Answer = null
            };

            _viewModel.LockoutFormAnswers.Add(_viewModelFactory.BuildWithOverrides<LockoutFormAnswerViewModel, LockoutFormAnswer>(answer, new { Category = category.Id }));

            ValidationAssert.ModelStateHasError(_viewModel, x => x.LockoutFormAnswers, LockoutFormViewModel.ValidationErrors.OUT_OF_SERVICE);
        }

        [TestMethod]
        public void TestRequiredFieldsEditLockoutForm()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LockoutDateTime, "The Lockout DateTime field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReasonForLockout, "The Reason For Lockout field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.LocationOfLockoutNotes, "The Location of Lockout Notes field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OutOfServiceDateTime, "The Out Of Service DateTime field is required.");
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Equipment);

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ContractorFirstName, "Maria",
                x => x.ContractorLockOutTagOut, true);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ContractorLastName, "XYZ",
                x => x.ContractorLockOutTagOut, true);
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.ContractorPhone, "1234567890",
                x => x.ContractorLockOutTagOut, true);
        }
    }
}