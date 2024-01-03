using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class RemoveMeasurementPointEquipmentTypeTest : ViewModelTestBase<EquipmentType, RemoveMeasurementPointEquipmentType>
    {
        #region Validations

        [TestMethod]
        public void TestCannotRemoveMeasurementPointWhenInUseByProductionWorkOrder()
        {
            var mockRepo = new Mock<IMeasurementPointEquipmentTypeRepository>();
            _container.Inject(mockRepo.Object);
            mockRepo.Setup(x => x.IsCurrentlyInUse(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(true);
            
            var measurementPoint = GetEntityFactory<MeasurementPointEquipmentType>().Create(new { EquipmentType = _entity });
            _viewModel.MeasurementPointId = measurementPoint.Id;

            ValidationAssert.ModelStateHasNonPropertySpecificError("The Measurement Point with Id '1' is already in use and cannot be deleted.");
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // noop
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop
        }
        
        #endregion
        
        #region Tests
        
        [TestMethod]
        public void TestMapToEntityRemovesMeasurementPointFromEquipmentType()
        {
            var measurementPoint = GetEntityFactory<MeasurementPointEquipmentType>().Create(new { EquipmentType = _entity });
            _entity.MeasurementPoints.Add(measurementPoint);
            _viewModel.MeasurementPointId = measurementPoint.Id;
            
            var actual = _vmTester.MapToEntity();
            
            MyAssert.DoesNotContain(actual.MeasurementPoints, measurementPoint);
        }
        
        #endregion
    }
}
