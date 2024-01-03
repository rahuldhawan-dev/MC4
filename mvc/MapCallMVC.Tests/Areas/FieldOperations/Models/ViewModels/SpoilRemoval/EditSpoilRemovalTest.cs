using MapCall.Common.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using SpoilRemovalEntity = MapCall.Common.Model.Entities.SpoilRemoval;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.SpoilRemoval
{
    [TestClass]
    public class EditSpoilRemovalTest : SpoilRemovalViewModelTest<EditSpoilRemoval>
    {
        #region Mapping

        [TestMethod]
        public void TestMapSetsOperatingCenter()
        {
            _viewModel.Map(_entity);

            Assert.IsNotNull(_viewModel.OperatingCenter);
            Assert.AreEqual(_entity.RemovedFrom.OperatingCenter.Id, _viewModel.OperatingCenter);
        }

        #endregion

        [TestMethod]
        public void Test_DisplaySpoilRemoval_ReturnsOriginalSpoilRemoval()
        {
            var entity = GetEntityFactory<SpoilRemovalEntity>().Create();
            var vm = _viewModelFactory.Build<EditSpoilRemoval, SpoilRemovalEntity>(entity);
            Assert.AreSame(entity, vm.SpoilRemovalDisplay);
        }
    }
}
