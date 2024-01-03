using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    [TestClass]
    public class RemovedContractorTest : ViewModelTestBase<WorkOrder, RemoveContractor>
    {
        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // no properties to test
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // no properties to test
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            // no properties to test
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no properties to test
        }

        [TestMethod]
        public void TestMapToEntityRemovesAssignedContractor()
        {
            _entity.AssignedContractor = GetFactory<ContractorFactory>().Create();

            _vmTester.MapToEntity();

            Assert.IsNull(_entity.AssignedContractor);
        }

        #endregion
    }
}
