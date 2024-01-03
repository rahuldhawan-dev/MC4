using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Events.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Events.Models.ViewModels
{
    public abstract class EventViewModelTest<TViewModel> : ViewModelTestBase<Event, TViewModel> where TViewModel : EventViewModel
    {
        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.EventCategory);
            _vmTester.CanMapBothWays(x => x.EventSubcategory);
            _vmTester.CanMapBothWays(x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EventCategory);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EventSubcategory);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.EventSummary);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.IsActive);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.RootCause);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.ResponseActions);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.EstimatedDurationHours);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.NumberCustomersImpacted);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.StartDate);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.EndDate);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.Owners);
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.Coordinate);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.EventSummary, Event.StringLengths.EVENT_SUMMARY);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.RootCause, Event.StringLengths.ROOT_CAUSE);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.ResponseActions, Event.StringLengths.RESPONSE_ACTIONS);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.EventCategory, GetEntityFactory<EventCategory>().Create());
        }

        #endregion
    }

    [TestClass]
    public class CreateEventTest : EventViewModelTest<CreateEvent>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.EventCategory, GetEntityFactory<EventCategory>().Create());
            _vmTester.CanMapBothWays(x => x.EventSubcategory, GetEntityFactory<EventSubcategory>().Create());
            _vmTester.CanMapBothWays(x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EventCategory);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EventSubcategory);
        }

        #endregion
    }
}
