using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Utilities;
using Moq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    [TestClass]
    public class EditTrafficControlTest : ViewModelTestBase<WorkOrder, EditTrafficControl>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        public User _user;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void TestInitialize()
        {
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _authServ = new Mock<IAuthenticationService<User>>();
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // no properties to validate
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.NumberOfOfficersRequired);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            // no required properties to validate
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no properties to validate string length
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMapToEntityAppendsNotes()
        {
            var today = DateTime.Now;
            var fullName = "Smith";
            var notes1 = "Testing Notes";
            var notes2 = "Additional Notes";

            _entity.Notes = null;
            _viewModel.AppendNotes = notes1;

            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(today);
            _user = GetFactory<AdminUserFactory>().Create(new { FullName = fullName });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _vmTester.MapToEntity();

            var expectedOutput1 = string.Format("{0} {1} {2}", fullName,
                today.ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS), notes1);
            Assert.AreEqual(expectedOutput1, _entity.Notes);

            var expectedOutput2 = string.Format("{0} {1} {2}", fullName,
                today.ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS), notes2);
            _viewModel.AppendNotes = notes2;

            _vmTester.MapToEntity();

            Assert.AreEqual(expectedOutput1 + Environment.NewLine + expectedOutput2, _entity.Notes);
        }

        [TestMethod]
        public void TestMapSetsWorkOrder()
        {
            _viewModel.Map(_entity);

            Assert.AreEqual(_entity, _viewModel.WorkOrder);
        }

        #endregion
    }
}
