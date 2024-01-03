using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class EditEnvironmentalNonComplianceEventActionItemTest : 
        ViewModelTestBase<EnvironmentalNonComplianceEventActionItem, EditEnvironmentalNonComplianceEventActionItem>
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(
                x => x.Type, 
                GetEntityFactory<EnvironmentalNonComplianceEventActionItemType>().Create());

            _vmTester.CanMapBothWays(x => x.ResponsibleOwner, GetEntityFactory<User>().Create());
            _vmTester.CanMapBothWays(x => x.NotListedType);
            _vmTester.CanMapBothWays(x => x.ActionItem);
            _vmTester.CanMapBothWays(x => x.TargetedCompletionDate);
            _vmTester.CanMapBothWays(x => x.DateCompleted);
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(
                _viewModel, 
                x => x.NotListedType, 
                EnvironmentalNonComplianceEventActionItem.StringLengths.NOT_LISTED_TYPE);

            ValidationAssert.PropertyHasMaxStringLength(_viewModel, 
                x => x.ActionItem, 
                EnvironmentalNonComplianceEventActionItem.StringLengths.ACTION_ITEM);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Type);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ResponsibleOwner);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ActionItem);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.TargetedCompletionDate);
        }

        [TestMethod]
        public void TestRequiredWhenValidation()
        {
            ValidationAssert.PropertyIsRequiredWhen(
                _viewModel, 
                x => x.NotListedType, 
                "can i have a cookie now?", 
                x => x.Type, 
                EnvironmentalNonComplianceEventActionItemType.Indices.NOT_LISTED);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(
                _viewModel, 
                x => x.Type, 
                GetEntityFactory<EnvironmentalNonComplianceEventActionItemType>().Create());

            ValidationAssert.EntityMustExist(
                _viewModel, 
                x => x.ResponsibleOwner, 
                GetEntityFactory<User>().Create());
        }

        #endregion

        #endregion
    }
}
