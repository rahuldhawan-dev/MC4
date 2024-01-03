using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class RestorationViewModelTest : MapCallMvcInMemoryDatabaseTestBase<Restoration>
    {
        #region Fields

        private ViewModelTester<RestorationViewModel, Restoration> _vmTester;
        private RestorationViewModel _viewModel;
        private Restoration _entity;
        private DateTime _now;
        private User _currentUser;
        private Mock<IAuthenticationService<User>> _authServ;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
            _authServ = new Mock<IAuthenticationService<User>>();
            e.For<IAuthenticationService<User>>().Use(_authServ.Object);
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IContractorRepository>().Use<ContractorRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<RestorationFactory>().Create();
            _viewModel = _viewModelFactory.Build<RestorationViewModel, Restoration>(_entity);
            _vmTester = new ViewModelTester<RestorationViewModel, Restoration>(_viewModel, _entity);
            _currentUser = GetFactory<AdminUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_currentUser);
        }

        #endregion

        #region Tests

        #region Map

        [TestMethod]
        public void TestMapSetsEstimatedRestorationFootageBasedOnRestorationType()
        {
            _entity.PavingSquareFootage = 32;
            _entity.LinearFeetOfCurb = 44;
            _entity.RestorationType.Description = "This is not a curb!";

            _vmTester.MapToViewModel();
            Assert.AreEqual(32, _viewModel.EstimatedRestorationFootage);

            _entity.RestorationType.Description = "CURB";
            _vmTester.MapToViewModel();
            Assert.AreEqual(44, _viewModel.EstimatedRestorationFootage);

        }

        #endregion

        #region MapToEntity

        [TestMethod]
        public void TestMapToEntityCalculatesStuff()
        {
            var rtc = GetFactory<RestorationTypeCostFactory>().Create(new {
                OperatingCenter = _entity.WorkOrder.OperatingCenter,
                RestorationType = _entity.RestorationType
            });
            _viewModel.EstimatedRestorationFootage = 10;
            _vmTester.MapToEntity();
            Assert.AreEqual(20m, _entity.TotalAccruedCost);
        }

        [TestMethod]
        public void TestMapToEntitySetsResponsePriorityToStandardIfViewModelDoesNotHaveValue()
        {
            // MapToEntity sets the *view model*'s ResponsePriority value before calling
            // the base MapToEntity method that actually finds the entity.
            _viewModel.ResponsePriority = null;
            _vmTester.MapToEntity();
            Assert.AreEqual(RestorationResponsePriority.Indices.STANDARD, _viewModel.ResponsePriority);
        }

        [TestMethod]
        public void TestMapToEntitySetsTheCorrectyEntityPropertyWhenReadingTheViewModelsEstimatedRestorationFootageProperty()
        {
            _viewModel.EstimatedRestorationFootage = 42;
            _vmTester.MapToEntity();
            Assert.AreEqual(42m, _entity.EstimatedRestorationFootage);
            Assert.AreEqual(42m, _entity.PavingSquareFootage);
        }

        [TestMethod]
        public void TestMapToEntitySetsAssignedContractorAtIfTheValueIsNullAndAContractorIsAssigned()
        {
            var contractor = GetFactory<ContractorFactory>().Create();
            _entity.AssignedContractor = null;
            _entity.AssignedContractorAt = null;
            Assert.IsFalse(_entity.HasBeenAssignedToContractor, "Sanity");
            _viewModel.AssignedContractor = contractor.Id;

            _vmTester.MapToEntity();

            Assert.AreSame(contractor, _entity.AssignedContractor);
        }


        [TestMethod]
        public void TestMapToEntitySetsFinalRestorationDateToNowIfCompletedByOthersIsCheckedAndTheDateHasNotBeenSet()
        {
            var expected = DateTime.Today;
            var dp = new Mock<IDateTimeProvider>();
            dp.Setup(x => x.GetCurrentDate()).Returns(expected);
            _container.Inject(dp.Object);

            _viewModel.CompletedByOthers = false;
            _entity.FinalRestorationDate = null;

            _vmTester.MapToEntity();
            Assert.IsNull(_entity.FinalRestorationDate);

            _viewModel.CompletedByOthers = true;
            _vmTester.MapToEntity();
            Assert.AreEqual(expected, _entity.FinalRestorationDate);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestPartialRestorationNotesIsRequiredWhenPartialRestorationDateIsSet()
        {
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.PartialRestorationNotes, "stuff", x => x.PartialRestorationDate, DateTime.Now, null);
        }


        [TestMethod]
        public void TestPartialRestorationNotesIsRequiredWhenPartialPavedSquareFootageIsGreaterThanEstimatedSquareFootage()
        {
            _viewModel.PartialRestorationNotes = null;
            _viewModel.EstimatedRestorationFootage = 13;

            _viewModel.PartialPavingSquareFootage = 12;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PartialRestorationNotes);

            _viewModel.PartialPavingSquareFootage = 13;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PartialRestorationNotes);

            _viewModel.PartialPavingSquareFootage = 14;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.PartialRestorationNotes,
                "Partial restoration notes are required when actual square footage is greater than estimated square footage.");

            _viewModel.PartialRestorationNotes = "Neat";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PartialRestorationNotes);
        }

        [TestMethod]
        public void TestFinalRestorationNotesIsRequiredWhenPartialRestorationDateIsSet()
        {
            _viewModel.FinalRestorationNotes = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FinalRestorationNotes);

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.FinalRestorationNotes, "stuff", x => x.FinalRestorationDate, DateTime.Now, null);

            _viewModel.FinalRestorationNotes = "neat. meat.";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FinalRestorationNotes);
        }

        [TestMethod]
        public void TestFinalRestorationNotesIsRequiredWhenFinalPavedSquareFootageIsGreaterThanEstimatedSquareFootage()
        {
            _viewModel.FinalRestorationNotes = null;
            _viewModel.EstimatedRestorationFootage = 13;

            _viewModel.FinalPavingSquareFootage = 12;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FinalRestorationNotes);

            _viewModel.FinalPavingSquareFootage = 13;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FinalRestorationNotes);

            _viewModel.FinalPavingSquareFootage = 14;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.FinalRestorationNotes,
                "Final restoration notes are required when actual square footage is greater than estimated square footage.");

            _viewModel.FinalRestorationNotes = "Neat";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FinalRestorationNotes);
        }


        [TestMethod]
        public void TestInitialPurchaseOrderNumberIsRequiredWhenAssignedContractor()
        {
            var contractor = GetFactory<ContractorFactory>().Create();

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.InitialPurchaseOrderNumber, "123", x => x.AssignedContractor, contractor.Id, null);
        }

        #endregion

        #region CreateRestoration

        [TestMethod]
        public void TestCreateRestorationSetDefaultsSetsWBSNumberFromWorkOrderAccountCharged()
        {
            var expected = "12345";
            var workorder = GetFactory<WorkOrderFactory>().Create(new { AccountCharged = expected });

            var model = _viewModelFactory.Build<CreateRestoration>();
            model.WorkOrder = workorder.Id;
            model.SetDefaults();

            Assert.AreEqual(expected, model.WBSNumber);
        }


        [TestMethod]
        public void TestSetDefaultsFromPreviousRestorationSetDefaultsFromPreviousRestoration()
        {
            var wo = GetFactory<WorkOrderFactory>().Create();
            var contractor1 = GetFactory<ContractorFactory>().Create();
            var contractor2 = GetFactory<ContractorFactory>().Create();
            var firstRestoration = GetFactory<RestorationFactory>().Create(new { WorkOrder = wo, AssignedContractor = contractor1, WBSNumber = "1", PartialRestorationPurchaseOrderNumber = "11", FinalRestorationPurchaseOrderNumber = "111" });
            var lastRestoration = GetFactory<RestorationFactory>().Create(new { WorkOrder = wo, AssignedContractor = contractor2, WBSNumber = "2", PartialRestorationPurchaseOrderNumber = "22", FinalRestorationPurchaseOrderNumber = "222" });
            wo.Restorations.Add(firstRestoration);
            wo.Restorations.Add(lastRestoration);

            var target = new CreateRestoration(_container);

            target.SetDefaultsFromLastRestoration(wo);

            Assert.AreEqual(contractor2.Id, target.AssignedContractor);
            Assert.AreEqual(lastRestoration.WBSNumber, target.WBSNumber);
            Assert.AreEqual(lastRestoration.PartialRestorationPurchaseOrderNumber, target.PartialRestorationPurchaseOrderNumber);
            Assert.AreEqual(lastRestoration.FinalRestorationPurchaseOrderNumber, target.FinalRestorationPurchaseOrderNumber);
            Assert.AreNotEqual(contractor1.Id, target.AssignedContractor);
        }

        #endregion

        #endregion
    }
}
