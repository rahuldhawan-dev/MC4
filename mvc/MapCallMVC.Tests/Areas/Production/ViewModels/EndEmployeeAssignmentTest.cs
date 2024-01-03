using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Utilities;
using Moq;
using StructureMap;
using System;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class EndEmployeeAssignmentTest : MapCallMvcInMemoryDatabaseTestBase<EmployeeAssignment>
    {
        #region Fields

        private ViewModelTester<EndEmployeeAssignment, EmployeeAssignment> _vmTester;
        private EndEmployeeAssignment _viewModel;
        private EmployeeAssignment _entity;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_dateTimeProvider.Object);
            _entity = GetEntityFactory<EmployeeAssignment>().Create();
            _viewModel = _viewModelFactory.Build<EndEmployeeAssignment, EmployeeAssignment>(_entity);
            _vmTester = new ViewModelTester<EndEmployeeAssignment, EmployeeAssignment>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestBasicMapping()
        {
            // DateStarted should only map to the view model
            _vmTester.CanMapToViewModel(x => x.DateStarted, DateTime.Now);
            // This test flukes and fails on this line for some reason. Only if you run all tests
            // or if you call CanMapToViewModel + DoesNotMapToEntity twice in a row on the same
            // property. 
            _vmTester.DoesNotMapToEntity(x => x.DateStarted, DateTime.Now.AddDays(1));
         
            // DateEnded can map both ways
            _vmTester.CanMapBothWays(x => x.DateEnded);

            // Can't really test that Notes doesn't map because EmployeeAssignment
            // does not have a Notes property. The controller uses that property to
            // add a note to the ProductionWorkOrder.
        }

        [TestMethod]
        public void TestProductionWorkOrderMapsToViewModel()
        {
            var prod = GetEntityFactory<ProductionWorkOrder>().Create();
            _entity.ProductionWorkOrder = prod;

            _vmTester.MapToViewModel();
            Assert.AreEqual(prod.Id, _viewModel.ProductionWorkOrder);

            _entity.ProductionWorkOrder = null;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.ProductionWorkOrder, "ProductionWorkOrder property should not be mapped back to the entity.");
        }

        [TestMethod]
        public void TestMapToEntitySetsDateEndedIfTheValueIsNull()
        {
            var expectedIfNull = new DateTime(1984, 4, 24, 4, 0, 4);
            var expectedNotNull = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedIfNull);

            _viewModel.DateEnded = expectedNotNull;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedNotNull, _entity.DateEnded);

            _viewModel.DateEnded = null;
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedIfNull, _entity.DateEnded);
        }

        [TestMethod]
        public void TestMapToEntitySetsIsFinalAssignment()
        {
            var emp = GetEntityFactory<Employee>().Create();
            var employeeAssignment1 = GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Today,
                DateEnded = DateTime.Now,
                AssignedTo = emp
            });
            _entity.ProductionWorkOrder.CurrentAssignments.Add(employeeAssignment1);

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.IsFinalAssignment);

            var emp2 = GetEntityFactory<Employee>().Create();
            var employeeAssignment2 = GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Today,
                AssignedTo = emp2
            });
            _entity.ProductionWorkOrder.CurrentAssignments.Add(employeeAssignment2);

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.IsFinalAssignment);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredProperties()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.DateStarted);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProductionWorkOrder);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Notes);
        }

        [TestMethod]
        public void TestDateEndedMustBeGreaterThanDateStartedUnlessDateEndedIsNull()
        {
            _viewModel.DateStarted = new DateTime(1984, 4, 24);
            _viewModel.DateEnded = new DateTime(1984, 4, 25);
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.DateEnded);

            _viewModel.DateEnded = new DateTime(1984, 4, 24);
            ValidationAssert.ModelStateHasError(_viewModel, x => x.DateEnded, "DateEnded must be greater than DateStarted.");

            _viewModel.DateEnded = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.DateEnded);
        }

        [TestMethod]
        public void TestProductionWorkOrderEntttiyMustExist()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
        }

        [TestMethod]
        public void TestProductionWorkOrderCanNotBeTamperedWith()
        {
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create();
            _entity.ProductionWorkOrder = pwo;
            _viewModel.ProductionWorkOrder = 935312;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ProductionWorkOrder, "ProductionWorkOrder is invalid.");
        }

        [TestMethod]
        public void TestDateStartedCanNotBeTamperedWith()
        {
            var expected = DateTime.Now;
            _entity.DateStarted = expected;
            _viewModel.DateStarted = DateTime.Now.AddDays(1);
            ValidationAssert.ModelStateHasError(_viewModel, x => x.DateStarted, "DateStarted is invalid.");
        }

        #endregion

        #endregion
    }
}
