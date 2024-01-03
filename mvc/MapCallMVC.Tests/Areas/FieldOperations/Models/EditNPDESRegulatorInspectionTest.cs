using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class
        EditNpdesRegulatorInspectionTest : NpdesRegulatorInspectionViewModelTestBase<EditNpdesRegulatorInspection>
    {
        #region Tests

        [TestMethod]
        public void TestMapMapsInspectedBy()
        {
            _entity = GetEntityFactory<NpdesRegulatorInspection>().Create(new {
                InspectedBy = GetEntityFactory<User>().Create(new {
                    UserName = "Wilbur"
                })
            });

            _viewModel.Map(_entity);

            Assert.AreEqual(_entity.InspectedBy.UserName, _viewModel.InspectedBy);
        }

        #endregion
    }
}
