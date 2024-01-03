using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CopyLockoutFormTest : MapCallMvcInMemoryDatabaseTestBase<LockoutForm>
    {
        #region Private Members

        private ViewModelTester<CopyLockoutForm, LockoutForm> _vmTester;
        private CopyLockoutForm _viewModel;
        private LockoutForm _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            _user.IsAdmin = true;

            _entity = GetEntityFactory<LockoutForm>().Create(new {
                ContractorLockOutTagOut = false
            });
            _viewModel = _viewModelFactory.Build<CopyLockoutForm, LockoutForm>(_entity);
            _vmTester = new ViewModelTester<CopyLockoutForm, LockoutForm>(_viewModel, _entity);

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now = DateTime.Now);
        }

        #endregion

        [TestMethod]
        public void TestExpectedFieldsAreCopied()
        {
            // sanity checks
            Assert.IsNotNull(_entity.OperatingCenter);
            Assert.IsNotNull(_entity.Facility);
            // address comes from facility
            // equipment type comes from equipment
            Assert.IsNotNull(_entity.Equipment);
            Assert.IsNotNull(_entity.Coordinate);
            Assert.IsNotNull(_entity.ProductionWorkOrder);
            Assert.IsNotNull(_entity.LockoutReason);
            MyAssert.IsNotNullOrWhiteSpace(_entity.ReasonForLockout);
            Assert.IsNotNull(_entity.ContractorLockOutTagOut);
            Assert.IsNotNull(_entity.LockoutDevice);

            _vmTester.MapToEntity();

            Assert.AreEqual(_entity.OperatingCenter.Id, _viewModel.OperatingCenter);
            Assert.AreEqual(_entity.Facility.Id, _viewModel.Facility);
            // address comes from facility
            // equipment type comes from equipment
            Assert.AreEqual(_entity.Equipment.Id, _viewModel.Equipment);
            Assert.AreEqual(_entity.ProductionWorkOrder, _entity.ProductionWorkOrder);
            Assert.AreEqual(_entity.LockoutReason.Id, _viewModel.LockoutReason);
            Assert.AreEqual(_entity.ReasonForLockout, _viewModel.ReasonForLockout);
            Assert.AreEqual(_entity.ContractorLockOutTagOut, _viewModel.ContractorLockOutTagOut);
        }

        [TestMethod]
        public void TestMapNullsOutPropertiesThatShouldNotBeCopied()
        {
            _viewModel.EmployeeAcknowledgedTraining = true;
            _viewModel.IsolationPoint = 1;
            _viewModel.IsolationPointDescription = "Remove me.";
            _viewModel.LocationOfLockoutNotes = "Remove me.";
            _viewModel.LockoutDevice = 1;

            _vmTester.MapToViewModel();

            Assert.IsFalse(_viewModel.EmployeeAcknowledgedTraining);
            Assert.IsNull(_viewModel.IsolationPoint);
            Assert.IsNull(_viewModel.IsolationPointDescription);
            Assert.IsNull(_viewModel.LocationOfLockoutNotes);
            Assert.IsNull(_viewModel.LockoutDevice);
        }

        [TestMethod]
        public void TestMapSetsQuestionsAnswersCorrectly()
        {
            // Add answers
            var questionCategories = GetFactory<LockoutFormQuestionCategoryFactory>().CreateAll();
            foreach (var category in questionCategories)
            {
                var q = GetEntityFactory<LockoutFormQuestion>().Create(new {
                    Category = category,
                    DisplayOrder = 1,
                    IsActive = true,
                    Question = $"{category.Id} Would you like to play a game of questions?"
                });
                // create out of service with false, the rest true, so we have more variation in the testing
                _entity.LockoutFormAnswers.Add(new LockoutFormAnswer {
                    Answer = (category.Id != LockoutFormQuestionCategory.Indices.OUT_OF_SERVICE),
                    LockoutForm = _entity,
                    LockoutFormQuestion = q
                });
            }

            _vmTester.MapToViewModel();

            Assert.AreEqual(questionCategories.Count, _viewModel.CreateLockoutFormAnswers.Count);
            foreach (var answer in _viewModel.CreateLockoutFormAnswers)
            {
                var category = _container.GetInstance<IRepository<LockoutFormQuestion>>()
                                         .Find(answer.LockoutFormQuestion).Category.Id;
                if (LockoutFormQuestionCategory.NEW_CATEGORIES.Contains(category))
                {
                    if (category == LockoutFormQuestionCategory.Indices.OUT_OF_SERVICE)
                    {
                        Assert.IsFalse(answer.Answer.Value);
                    }
                    else
                    {
                        Assert.IsTrue(answer.Answer.Value);
                    }
                }
                else
                {
                    Assert.IsNull(answer.Answer);
                }
            }
        }

        [TestMethod]
        public void TestSetDefaultsSetsDefaults()
        {
            var employeeId = "112233";
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { OperatingCenter = operatingCenter, EmployeeId = employeeId });
            _user.Employee = employee;
            _user.DefaultOperatingCenter = operatingCenter;

            _viewModel.SetDefaults();

            MyAssert.AreClose(_now, _viewModel.OutOfServiceDateTime.Value);
            MyAssert.AreClose(_now, _viewModel.LockoutDateTime.Value);
            Assert.AreEqual(employee.Id, _viewModel.OutOfServiceAuthorizedEmployee);
        }
    }
}