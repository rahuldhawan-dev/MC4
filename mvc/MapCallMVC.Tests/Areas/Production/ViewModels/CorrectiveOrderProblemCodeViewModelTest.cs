using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class CorrectiveOrderProblemCodeViewModelTest : ViewModelTestBase<CorrectiveOrderProblemCode, CorrectiveOrderProblemCodeViewModel>
    {
        #region Validations

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // N/A
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Code);
            _vmTester.CanMapBothWays(x => x.Description);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Code);
            ValidationAssert.PropertyIsRequired(x => x.Description);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.Code, CorrectiveOrderProblemCode.StringLengths.CODE);
            ValidationAssert.PropertyHasMaxStringLength(x => x.Description, CorrectiveOrderProblemCode.StringLengths.DESCRIPTION);
        }

        #endregion
    }
}
