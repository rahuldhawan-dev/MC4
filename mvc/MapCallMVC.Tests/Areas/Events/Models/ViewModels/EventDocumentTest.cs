using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Events.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Events.Models.ViewModels
{
    public abstract class EventDocumentViewModelTest<TViewModel> : ViewModelTestBase<EventDocument, TViewModel> where TViewModel : EventDocumentsViewModel
    {
        #region Exposed Methods

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.Facility);
            _vmTester.CanMapBothWays(x => x.EventType);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.EventType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
        }

        [TestMethod]
        public void TestNotRequiredFields()
        {
            ValidationAssert.PropertyIsNotRequired(_viewModel, x => x.Facility);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Description, EventDocument.StringLengths.DESCRIPTION);;
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.EventType, GetEntityFactory<EventType>().Create());
        }

        #endregion
    }

    [TestClass]
    public class CreateEventDocumentTest : EventDocumentViewModelTest<CreateEventDocument>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
        }

        #endregion
    }
}
