using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class MaterialUsedModelTest<TViewModel> : ViewModelTestBase<MaterialUsed, TViewModel> where TViewModel : MaterialUsedModel
    {
        #region Init/Cleanup

        protected override MaterialUsed CreateEntity()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create();
            return GetEntityFactory<MaterialUsed>().Build(new { WorkOrder = workOrder });
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.Material, GetEntityFactory<Material>().Create());
            ValidationAssert.EntityMustExist(x => x.StockLocation, GetEntityFactory<StockLocation>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.Material);
            _vmTester.CanMapBothWays(x => x.Quantity);
            _vmTester.CanMapBothWays(x => x.NonStockDescription);
            _vmTester.CanMapBothWays(x => x.StockLocation);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Quantity);
            ValidationAssert.PropertyIsRequiredWhen(x => x.Material, GetEntityFactory<Material>().Create().Id, x => x.NonStockDescription, null, "Some invalid value", "Please choose a part number for stock materials.");
            ValidationAssert.PropertyIsRequiredWhen(x => x.NonStockDescription, "some valid non stock value", x => x.Material, null, GetEntityFactory<Material>().Create().Id, "Non-stock materials must have a description.");
            ValidationAssert.PropertyIsRequiredWhen(x => x.StockLocation, GetEntityFactory<StockLocation>().Create().Id, x => x.NonStockDescription, null, "some valid non stock value", "Please choose a stock location for stock materials.");
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMinValueRequirement(x => x.Quantity, 1);
        }

        #endregion
    }
}
