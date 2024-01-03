using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderStockToIssue;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.WorkOrderStockToIssue
{
    [TestClass]
    public class ApproveWorkOrderStockToIssueTest : ViewModelTestBase<WorkOrder, ApproveWorkOrderStockToIssue>
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
        public void TestMapToEntitySetsMaterialsApprovedOnToNow()
        {
            var expected = DateTime.Now;
            _dtProvider.Setup(x => x.GetCurrentDate()).Returns(expected);
            _entity.MaterialsApprovedOn = null;
            _vmTester.MapToEntity();
            Assert.AreEqual(expected, _entity.MaterialsApprovedOn);
        }

        [TestMethod]
        public void TestMapToEntitySetsMaterialsApprovedByToCurrentUser()
        {
            _entity.MaterialsApprovedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.MaterialsApprovedBy);
        }

        [TestMethod]
        public void TestMapSetsMaterialPostingDateToCurrentDate()
        {
            DateTime date = DateTime.Now;
            _viewModel.MaterialPostingDate = null;
            _viewModel.Map(_entity);
            MyAssert.AreClose(date, _viewModel.MaterialPostingDate.Value);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidationFailsIfMaterialsApprovedOnIsNotNull()
        {
            _viewModel.MaterialPostingDate = DateTime.Now;
            _entity.MaterialsApprovedOn = DateTime.Now;
            ValidationAssert.ModelStateHasNonPropertySpecificError("Materials have already been approved for this work order.");

            _entity.MaterialsApprovedOn = null;
            ValidationAssert.ModelStateIsValid();
        }

        #endregion

        #endregion
        
        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            // noop
        }
        
        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.MaterialPostingDate);
        }
        
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            // noop
        }
        
        [TestMethod]
        public override void TestStringLengthValidation()
        {
             // noop
        }
    }
}
