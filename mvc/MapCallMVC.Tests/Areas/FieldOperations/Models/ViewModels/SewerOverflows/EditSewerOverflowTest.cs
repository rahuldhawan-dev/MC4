using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerOverflows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.SewerOverflows
{
    [TestClass]
    public class EditSewerOverflowTest : SewerOverflowViewModelTest<EditSewerOverflow>
    {
        #region Tests

        [TestMethod]
        public void TestMapSetsStateFromOperatingCenter()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {
                State = state
            });

            _entity = GetEntityFactory<SewerOverflow>().Create(new {
                OperatingCenter = operatingCenter
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(state.Id, _viewModel.State);
        }

        #endregion
    }
}
