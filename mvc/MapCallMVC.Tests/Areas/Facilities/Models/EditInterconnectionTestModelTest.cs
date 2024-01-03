using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Facilities.Models
{
    public abstract class InterconnectionTestViewModelTest<TViewModel>
        : ViewModelTestBase<InterconnectionTest, TViewModel>
        where TViewModel : InterconnectionTestViewModel
    {
        #region Tests

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.State);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(x => x.Facility);
            ValidationAssert.PropertyIsRequired(x => x.InspectionDate);
            ValidationAssert.PropertyIsRequired(x => x.InterconnectionInspectionRating);
            ValidationAssert.PropertyIsRequired(x => x.Employee);
            ValidationAssert.PropertyIsNotRequired(x => x.WorkOrder);
            ValidationAssert.PropertyIsNotRequired(x => x.MaxFlowMGDAchieved);
            ValidationAssert.PropertyIsNotRequired(x => x.AllValvesOperational);
            ValidationAssert.PropertyIsNotRequired(x => x.Contractor);
            ValidationAssert.PropertyIsNotRequired(x => x.InspectionComments);
            ValidationAssert.PropertyIsNotRequired(x => x.RepresentativeOnSite);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.State, GetEntityFactory<State>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.Facility, GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(x => x.InterconnectionInspectionRating, GetEntityFactory<InterconnectionInspectionRating>().Create());
            ValidationAssert.EntityMustExist(x => x.Employee, GetEntityFactory<Employee>().Create());
            ValidationAssert.EntityMustExist(x => x.Contractor, GetEntityFactory<Contractor>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Facility, GetEntityFactory<Facility>().Create());
            _vmTester.CanMapBothWays(x => x.InterconnectionInspectionRating, GetEntityFactory<InterconnectionInspectionRating>().Create());
            _vmTester.CanMapBothWays(x => x.Employee, GetEntityFactory<Employee>().Create());
            _vmTester.CanMapBothWays(x => x.Contractor, GetEntityFactory<Contractor>().Create());

            _vmTester.CanMapBothWays(x => x.InspectionDate);
            _vmTester.CanMapBothWays(x => x.MaxFlowMGDAchieved);
            _vmTester.CanMapBothWays(x => x.AllValvesOperational);
            _vmTester.CanMapBothWays(x => x.InspectionComments);
            _vmTester.CanMapBothWays(x => x.RepresentativeOnSite);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.InspectionComments,
                InterconnectionTest.StringLengths.INSPECTION_COMMENTS);
            ValidationAssert.PropertyHasMaxStringLength(x => x.RepresentativeOnSite,
                InterconnectionTest.StringLengths.REPRESENTATIVE_ON_SITE);
        }

        #endregion
    }

    [TestClass]
    public class EditInterconnectionTestModelTest : InterconnectionTestViewModelTest<EditInterconnectionTest>
    {
        #region Tests

        [TestMethod]
        public void TestMapSetsStateAndOperatingCenter()
        {
            var state = GetEntityFactory<State>().Create();
            var opc = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var facility = GetEntityFactory<Facility>()
               .Create(new { OperatingCenter = opc });
            _entity.Facility = facility;

            _viewModel.Map(_entity);

            Assert.AreEqual(_entity.Facility.OperatingCenter.State.Id, _viewModel.State);
            Assert.AreEqual(_entity.Facility.OperatingCenter.Id, _viewModel.OperatingCenter);
        }

        #endregion
    }
}
