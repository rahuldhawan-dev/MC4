using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EditMeasurementPointEquipmentTypeTest : MeasurementPointEquipmentTypeViewModelTestBase<EditMeasurementPointEquipmentType>
    {
        #region Constants

        private const string CANNOT_CHANGE_ERR =
            "Certain fields on this record cannot be updated, as this Measurement Point is already being used.";

        #endregion

        #region Fields
        
        private Mock<IMeasurementPointEquipmentTypeRepository> _mockRepo;
        
        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeLimitedEditContext()
        {
            // The tests for this viewmodel will run under the assumption that the record we're editing is
            // already in use by a ProductionWorkOrder. This will trigger the validation in the viewmodel.
            _mockRepo = new Mock<IMeasurementPointEquipmentTypeRepository>();
            _container.Inject(_mockRepo.Object);
           
            _mockRepo.Setup(x => x.IsCurrentlyInUse(It.IsAny<int>(), It.IsAny<int>()))
                     .Returns(true);
            _mockRepo.Setup(x => x.Find(It.IsAny<int>()))
                     .Returns(_entity);
        }

        #endregion
        
        #region Validations
        
        [TestMethod]
        public void TestCategoryCannotBeEditedWhenMeasurementPointIsBeingUsed()
        {
            _viewModel.Category = "A";
            
            ValidationAssert.ModelStateHasNonPropertySpecificError(CANNOT_CHANGE_ERR);
        }
        
        [TestMethod]
        public void TestUnitOfMeasureCannotBeEditedWhenMeasurementPointIsBeingUsed()
        {
            var uom = GetEntityFactory<UnitOfMeasure>().Create();

            _viewModel.UnitOfMeasure = uom.Id;
            
            ValidationAssert.ModelStateHasNonPropertySpecificError(CANNOT_CHANGE_ERR);
        }
        
        [TestMethod]
        public void TestPositionCannotBeEditedWhenMeasurementPointIsBeingUsed()
        {
            _viewModel.Position = 9001;
            
            ValidationAssert.ModelStateHasNonPropertySpecificError(CANNOT_CHANGE_ERR);
        }
        
        [TestMethod]
        public void TestMinCannotBeEditedWhenMeasurementPointIsBeingUsed()
        {
            _viewModel.Min = -1.0m;
            
            ValidationAssert.ModelStateHasNonPropertySpecificError(CANNOT_CHANGE_ERR);
        }
        
        [TestMethod]
        public void TestMaxCannotBeEditedWhenMeasurementPointIsBeingUsed()
        {
            _viewModel.Max = 9001.0m;
            
            ValidationAssert.ModelStateHasNonPropertySpecificError(CANNOT_CHANGE_ERR);
        }

        [TestMethod]
        public void TestNonCriticalFieldsCanBeEditedWhenMeasurementPointIsBeingUsed()
        {
            _viewModel.Description = "this is a valid change";
            _viewModel.IsActive = true;

            ValidationAssert.ModelStateIsValid();
        }

        #endregion
    }
}
