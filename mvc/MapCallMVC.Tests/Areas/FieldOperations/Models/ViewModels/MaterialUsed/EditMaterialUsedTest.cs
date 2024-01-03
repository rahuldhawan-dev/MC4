using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.MaterialsUsed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.MaterialUsed
{
    [TestClass]
    public class EditMaterialUsedTest : MaterialUsedViewModelTest<EditMaterialUsed>
    {
        #region Mapping

        [TestMethod]
        public void TestMapSetsOperatingCenter()
        {
            _viewModel.Map(_entity);

            Assert.IsNotNull(_viewModel.OperatingCenter);
            Assert.AreEqual(_entity.WorkOrder.OperatingCenter.Id, _viewModel.OperatingCenter);
        }

        #endregion
    }
}
