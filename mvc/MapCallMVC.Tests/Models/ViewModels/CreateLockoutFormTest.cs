using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using System.Linq;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateLockoutFormTest : MapCallMvcInMemoryDatabaseTestBase<LockoutForm>
    {
        #region Fields

        private CreateLockoutForm _viewModel;
        private LockoutForm _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private ViewModelTester<CreateLockoutForm, LockoutForm> _vmTester;
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
            _viewModel = _viewModelFactory.Build<CreateLockoutForm, LockoutForm>(_entity);
            _vmTester = new ViewModelTester<CreateLockoutForm, LockoutForm>(_viewModel, _entity);
        }

        #endregion
        
        [TestMethod]
        public void TestLockoutFormSetDefaultsSetsDefaults()
        {
            var employeeId = "112233";
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { OperatingCenter = operatingCenter, EmployeeId = employeeId });
            _user.Employee = employee;
            _user.DefaultOperatingCenter = operatingCenter;
            var questionCategories = GetFactory<LockoutFormQuestionCategoryFactory>().CreateAll();

            foreach (var category in questionCategories)
            {
                GetEntityFactory<LockoutFormQuestion>().Create(new {
                    Category = category,
                    DisplayOrder = 1,
                    IsActive = true,
                    Question = "Would you like to play a game of questions?"
                });
            }
            // we want to make it null in this case to see that we're only setting it when it's not set
            _viewModel.OperatingCenter = null;

            _viewModel.SetDefaults();

            Assert.AreEqual(_now, _viewModel.OutOfServiceDateTime);
            Assert.AreEqual(_now, _viewModel.LockoutDateTime);
            Assert.AreEqual(employee.Id, _viewModel.OutOfServiceAuthorizedEmployee);
            Assert.AreEqual(operatingCenter.Id, _viewModel.OperatingCenter);
            Assert.AreEqual(2, _viewModel.CreateLockoutFormAnswers.Count);
            Assert.AreEqual(_entity.Id, _viewModel.CreateLockoutFormAnswers.First().LockoutForm);
            foreach (var category in questionCategories)
            {
                if (LockoutFormQuestionCategory.NEW_CATEGORIES.Contains(category.Id))
                {
                    Assert.IsTrue(_viewModel.CreateLockoutFormAnswers.Any(x => x.Category == category.Id));
                }
                else
                {
                    Assert.IsFalse(_viewModel.CreateLockoutFormAnswers.Any(x => x.Category == category.Id));
                }
            }
        }

        [TestMethod]
        public void TestSetsDefaultsDoesNotOverwriteOperatingCenter()
        {
            var employeeId = "112233";
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterOther = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { OperatingCenter = operatingCenter, EmployeeId = employeeId });
            _user.Employee = employee;
            _user.DefaultOperatingCenter = operatingCenter;
            _viewModel.OperatingCenter = operatingCenterOther.Id;

            _viewModel.SetDefaults();
            
            Assert.AreEqual(operatingCenterOther.Id, _viewModel.OperatingCenter);
        }

        [TestMethod]
        public void TestMapToEntityMapsLockoutFormAnswers()
        {
            var category = GetFactory<ManagementLockoutFormQuestionCategoryFactory>().Create();
            var question = GetEntityFactory<LockoutFormQuestion>().Create(new
            {
                Category = category,
                Question = "Que?",
                IsActive = true,
                DisplayOrder = 1
            });
            _viewModel.CreateLockoutFormAnswers.Add(_viewModelFactory.BuildWithOverrides<CreateLockoutFormAnswer, LockoutFormAnswer>(null, new
            {
                LockoutForm = _entity.Id,
                LockoutFormQuestion = question.Id,
                Answer = false,
                Comments = "foo",
                LockoutFormQuestionDisplay = question,
                Category = category.Id
            }));
            _viewModel.MapToEntity(_entity);

            var firstAnswer = _entity.LockoutFormAnswers.First();
            Assert.AreEqual(question, firstAnswer.LockoutFormQuestion);
            Assert.AreEqual(_entity, firstAnswer.LockoutForm);
            Assert.AreEqual("foo", firstAnswer.Comments);
        }

        [TestMethod]
        public void TestMapToEntityMapsAddsAnyMissingActiveLockoutFormAnswers()
        {
            var questionCategories = GetFactory<LockoutFormQuestionCategoryFactory>().CreateAll();

            foreach (var category in questionCategories)
            {
                GetEntityFactory<LockoutFormQuestion>().Create(new
                {
                    Category = category,
                    DisplayOrder = 1,
                    IsActive = true,
                    Question = "Would you like to play a game of questions?"
                });
            }
            var newQuestions = _container.GetInstance<IRepository<LockoutFormQuestion>>().GetActiveQuestionsForCreate();
            foreach(var question in newQuestions)
            {
                _viewModel.CreateLockoutFormAnswers.Add(
                    _viewModelFactory.BuildWithOverrides<CreateLockoutFormAnswer, LockoutFormAnswer>(null, new {
                        LockoutForm = _entity.Id,
                        LockoutFormQuestion = question.Id,
                        Answer = false,
                        Comments = "foo",
                        LockoutFormQuestionDisplay = question,
                        Category = question.Category.Id
                    }));
            }

            _viewModel.MapToEntity(_entity);

            Assert.AreEqual(questionCategories.Count, _viewModel.CreateLockoutFormAnswers.Count);
        }

        [TestMethod]
        public void TestMapToEntityUpdatesSatisifedRequirementOnLockOutFormProductionOrderPreRequisite()
        {
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            var productionPreReq = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var productionWorkOrderPreReq =
                GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {
                    ProductionWorkOrder = productionWorkOrder,
                    ProductionPrerequisite = productionPreReq
                });
            var lockoutForm = GetEntityFactory<LockoutForm>().Create(new {ProductionWorkOrder = productionWorkOrder});

            productionWorkOrder.ProductionWorkOrderProductionPrerequisites.Add(productionWorkOrderPreReq);

            var target = _viewModelFactory.Build<CreateLockoutForm, LockoutForm>(lockoutForm);

            target.MapToEntity(lockoutForm);

            Assert.IsNotNull(productionWorkOrderPreReq.SatisfiedOn);
            Assert.AreEqual(productionWorkOrderPreReq.SatisfiedOn, _dateTimeProvider.Object.GetCurrentDate());
        }

        [TestMethod]
        public void TestMapToEntityDoesNotUpdatePreReqIfProductionWorkOrderIsNull()
        {
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            var productionPreReq = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var productionWorkOrderPreReq =
                GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {
                    ProductionWorkOrder = productionWorkOrder,
                    ProductionPrerequisite = productionPreReq
                });
            var lockoutForm = GetEntityFactory<LockoutForm>().Create();

            productionWorkOrder.ProductionWorkOrderProductionPrerequisites.Add(productionWorkOrderPreReq);

            lockoutForm.ProductionWorkOrder = null;

            var target = _viewModelFactory.Build<CreateLockoutForm, LockoutForm>(lockoutForm);

            target.MapToEntity(lockoutForm);

            Assert.IsNull(productionWorkOrderPreReq.SatisfiedOn);
            Assert.AreNotEqual(productionWorkOrderPreReq.SatisfiedOn, _dateTimeProvider.Object.GetCurrentDate());
        }

        [TestMethod]
        public void TestRequiredFieldsCreateLockoutForm()
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

        [TestMethod]
        public void TestSetValuesFromProductionOrderSetsValuesFromProductionOrder()
        {
            var order = GetEntityFactory<ProductionWorkOrder>().Create();
            var pwoe = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = order,
                IsParent = true
            });
            order.Equipments.Add(pwoe);

            _viewModel.SetValuesFromProductionWorkOrder(order);

            Assert.AreEqual(order.Id, _viewModel.ProductionWorkOrder);
            Assert.AreEqual(order.OperatingCenter.Id, _viewModel.OperatingCenter);
            Assert.AreEqual(order.Facility.Id, _viewModel.Facility);
            Assert.AreEqual(order.EquipmentType.Id, _viewModel.EquipmentType);
            Assert.AreEqual(order.Equipment.Id, _viewModel.Equipment);
        }
    }
}
