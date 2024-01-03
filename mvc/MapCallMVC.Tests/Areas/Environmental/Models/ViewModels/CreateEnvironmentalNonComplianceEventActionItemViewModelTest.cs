using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.Common.Testing;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels
{
    [TestClass]
    public class CreateEnvironmentalNonComplianceEventActionItemViewModelTest : 
        ViewModelTestBase<EnvironmentalNonComplianceEvent, CreateEnvironmentalNonComplianceEventActionItem>
    {
        #region Tests

        #region Mapping

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // This thing is manually mapped
        }

        #endregion

        #region Validation

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.NotListedType, 
                EnvironmentalNonComplianceEventActionItem.StringLengths.NOT_LISTED_TYPE);

            ValidationAssert.PropertyHasMaxStringLength(x => x.ActionItem, 
                EnvironmentalNonComplianceEventActionItem.StringLengths.ACTION_ITEM);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Type);
            ValidationAssert.PropertyIsRequired(x => x.ResponsibleOwner);
            ValidationAssert.PropertyIsRequired(x => x.ActionItem);
            ValidationAssert.PropertyIsRequired(x => x.TargetedCompletionDate);
        }

        [TestMethod]
        public void TestRequiredWhenValidation()
        {
            ValidationAssert.PropertyIsRequiredWhen(x => x.NotListedType, 
                "can i have a cookie now?", 
                x => x.Type, 
                EnvironmentalNonComplianceEventActionItemType.Indices.NOT_LISTED);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // No properties marked with entity must exist
        }

        #endregion

        #endregion
    }
}
