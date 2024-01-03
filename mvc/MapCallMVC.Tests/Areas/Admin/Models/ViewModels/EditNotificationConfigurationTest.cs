using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Admin.Models.ViewModels.NotificationConfigurations;

namespace MapCallMVC.Tests.Areas.Admin.Models.ViewModels
{
    [TestClass]
    public class EditNotificationConfigurationTest : ViewModelTestBase<NotificationConfiguration, EditNotificationConfiguration>
    {
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Contact, GetEntityFactory<Contact>().Create());
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation() { }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Contact);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Contact, GetEntityFactory<Contact>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
        }

        [TestMethod]
        public void TestMapToEntityShouldSetOperatingCenterToNullWhenAppliesToAllOperatingCentersIsTrue()
        {
            var expectedOperatingCenter = GetEntityFactory<OperatingCenter>().Create();
            _viewModel.OperatingCenter = expectedOperatingCenter.Id;
            _viewModel.AppliesToAllOperatingCenters = true;

            _vmTester.MapToEntity();

            Assert.IsNull(_entity.OperatingCenter);

            _viewModel.AppliesToAllOperatingCenters = false;
            _vmTester.MapToEntity();
            Assert.AreSame(expectedOperatingCenter, _entity.OperatingCenter);
        }
    }
}