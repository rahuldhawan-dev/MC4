using MapCallMVC.Areas.HumanResources.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MapCallMVC.Tests.Areas.HumanResources.Models.ViewModels
{
    [TestClass]
    public class CreateEmployeeHeadCountTest : EmployeeHeadCountViewModelTest<CreateEmployeeHeadCount>
    {
        #region Tests

        [TestMethod]
        public void TestMapToEntitySetsCreatedByToCurrentUserName()
        {
            _user.UserName = "Neato";
            _vmTester.MapToEntity();
            Assert.AreEqual("Neato", _entity.CreatedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsCreatedAtToCurrentDateTime()
        {
            var expectedDate = new DateTime(1984, 4, 24);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedDate, _entity.CreatedAt);
        }

        #endregion
    }
}
