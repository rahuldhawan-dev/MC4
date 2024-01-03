using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCallMVC.Areas.Production.Models.ViewModels.WellTests;

namespace MapCallMVC.Tests.Areas.Production.ViewModels.WellTests
{
    [TestClass]
    public class CreateWellTestViewModelTest : WellTestViewModelTest<CreateWellTestViewModel>
    {
        #region Tests

        [TestMethod]
        public void TestSetDefaultsSetsEmployeeAndDateOfTest()
        {
            _viewModel.SetDefaults();

            Assert.AreEqual(_user.Employee.Id, _viewModel.Employee);
            Assert.AreEqual(_rightNow, _viewModel.DateOfTest);
        }

        #endregion
    }
}