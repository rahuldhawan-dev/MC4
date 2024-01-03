using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval
{
    [TestClass]
    public class SupervisorRejectWorkOrderTest : ViewModelTestBase<WorkOrder, SupervisorRejectWorkOrder>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private Mock<IDateTimeProvider> _dtProvider;
        private User _user;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _user = GetEntityFactory<User>().Create(new { UserName = "some user "});
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dtProvider = e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMapToEntitySetsCompletedByToNull()
        {
            _entity.CompletedBy = new User();
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.CompletedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsDateCompletedToNull()
        {
            _entity.DateCompleted = DateTime.Now;
            _vmTester.MapToEntity();
            Assert.IsNull(_entity.DateCompleted);
        }

        [TestMethod]
        public void TestMapToEntitySetsDateRejectedToCurrentDateTime()
        {
            var expectedDate = new DateTime(1984, 4, 24, 4, 0, 4);
            _dtProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _vmTester.MapToEntity();
            Assert.AreEqual(expectedDate, _entity.DateRejected);
        }

        [TestMethod]
        public void TestMapToEntityAppendsNewRejectionReasonToExistingNotes()
        {
            var expectedDate = new DateTime(1984, 4, 24, 4, 4, 0);
            _dtProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);
            _entity.Notes = "Some existing notes.";
            _viewModel.RejectionReason = "The rejection reason.";
            _user.UserName = "Test User";

            var expected = $"Some existing notes. {Environment.NewLine} 4/24/1984 4:04:00 AM Test User: The rejection reason.";
            _vmTester.MapToEntity();
            Assert.AreEqual(expected, _entity.Notes);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // noop, no mapping occurs to the view model. Only manual mapping to the entity.
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsIfWorkOrderIsApproved()
        {
            _viewModel.RejectionReason = "This reason is needed for the ModelStateIsValid check";

            _entity.ApprovedOn = DateTime.Now;
            ValidationAssert.ModelStateHasNonPropertySpecificError("Can not reject a work order that has already been approved.");

            _entity.ApprovedOn = null;
            ValidationAssert.ModelStateIsValid();
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.RejectionReason);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop, no EntityMustExist props on this model.
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // noop, no StringLength props on this model.
        }

        #endregion

        #endregion
    }
}
