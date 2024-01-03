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
    public class SupervisorApproveWorkOrderTest : ViewModelTestBase<WorkOrder, SupervisorApproveWorkOrder>
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
        public override void TestPropertiesCanMapBothWays()
        {
            // noop. All mapping is done manually because they all have different
            // rules for how/when to map.
        }

        [TestMethod]
        public void TestMapSetsAccountCharged()
        {
            _entity.AccountCharged = "sure";
            _vmTester.MapToViewModel();
            Assert.AreEqual("sure", _viewModel.AccountCharged);
        }

        [TestMethod]
        public void TestOperatingCenterHasWorkOrderInvoicingReturnsEntityOperatingCenterHasWorkOrderInvoicingValue()
        {
            // NOTE: Test doesn't require calling Map. ViewModel property does the work here.
            _entity.OperatingCenter.HasWorkOrderInvoicing = true;
            Assert.IsTrue(_viewModel.OperatingCenterHasWorkOrderInvoicing);
            _entity.OperatingCenter.HasWorkOrderInvoicing = false;
            Assert.IsFalse(_viewModel.OperatingCenterHasWorkOrderInvoicing);
        }

        [TestMethod]
        public void TestMapSetsRequiresInvoice()
        {
            _entity.RequiresInvoice = true;
            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.RequiresInvoice);
        }

        [TestMethod]
        public void TestMapToEntitySetsApprovedByToCurrentUser()
        {
            _entity.ApprovedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.ApprovedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsApprovedOnToCurrentDateTime()
        {
            var expected = new DateTime(1984, 4, 24);
            _dtProvider.Setup(x => x.GetCurrentDate()).Returns(expected);
            _vmTester.MapToEntity();
            Assert.AreEqual(expected, _entity.ApprovedOn);
        }

        [TestMethod]
        public void TestMapToEntityDoesNotSetAccountCharged()
        {
            _entity.AccountCharged = "entity value";
            _viewModel.AccountCharged = "view model value";
            _vmTester.MapToEntity();
            Assert.AreEqual("entity value", _entity.AccountCharged);
        }

        [TestMethod]
        public void TestMapToEntitySetsRequiresInvoiceIfOperatingCenterHasWorkOrderInvoicing()
        {
            _entity.RequiresInvoice = false;
            _viewModel.RequiresInvoice = true;

            _entity.OperatingCenter.HasWorkOrderInvoicing = false;
            _vmTester.MapToEntity();
            Assert.IsFalse(_entity.RequiresInvoice);

            _entity.OperatingCenter.HasWorkOrderInvoicing = true;
            _vmTester.MapToEntity();
            Assert.IsTrue(_entity.RequiresInvoice);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsIfWorkOrderIsAlreadyApproved()
        {
            _entity.DateCompleted = DateTime.Now;
            _entity.ApprovedOn = DateTime.Now;
            ValidationAssert.ModelStateHasNonPropertySpecificError("This work order has already been approved.");
            _entity.ApprovedOn = null;
            ValidationAssert.ModelStateIsValid();
        }

        [TestMethod]
        public void TestValidationFailsIfWorkOrderCanBeApprovedIsFalse()
        {
            _entity.DateCompleted = DateTime.Now;
            _entity.CancelledAt = DateTime.Now; // This gets CanBeApproved to return false.
            ValidationAssert.ModelStateHasNonPropertySpecificError("This work order has been cancelled and can not be approved.");
            _entity.CancelledAt = null;
            ValidationAssert.ModelStateIsValid();
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            // noop, no required fields
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
