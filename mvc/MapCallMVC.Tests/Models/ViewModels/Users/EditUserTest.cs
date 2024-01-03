using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels.Users
{
    [TestClass]
    public class EditUserTest : BaseUserViewModelTest<EditUser>
    {
        #region Tests
        
        [TestMethod]
        public void TestMapSetsEmployeeNumberFromUserEmployeeRecord()
        {
            var employee = GetEntityFactory<Employee>().Create(new { EmployeeId = "44444444" });
            _entity.Employee = employee;
            _vmTester.MapToViewModel();
            Assert.AreEqual("44444444", _viewModel.EmployeeNumber);
        }

        #endregion
    }
}
