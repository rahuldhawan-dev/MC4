using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class EditGeneralWorkOrderTest : ViewModelTestBase<WorkOrder, EditGeneralWorkOrder>
    {
        #region Private Members

        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        
        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {

            _user = GetFactory<AdminUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.AccountCharged);
            _vmTester.CanMapBothWays(x => x.ApprovedBy);
            _vmTester.CanMapBothWays(x => x.ApprovedOn);
            _vmTester.CanMapBothWays(x => x.DigitalAsBuiltRequired);
            _vmTester.CanMapBothWays(x => x.MaterialPostingDate);
            _vmTester.CanMapBothWays(x => x.MaterialsApprovedBy);
            _vmTester.CanMapBothWays(x => x.MaterialsApprovedOn);
            _vmTester.CanMapBothWays(x => x.MaterialsDocID);
            _vmTester.CanMapBothWays(x => x.PremiseNumber);
            _vmTester.CanMapBothWays(x => x.SAPErrorCode);
            _vmTester.CanMapBothWays(x => x.SAPNotificationNumber);
            _vmTester.CanMapBothWays(x => x.SAPWorkOrderNumber);
            _vmTester.CanMapBothWays(x => x.WorkDescription);
        }
 
        [TestMethod]
        public override void TestRequiredValidation() { }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert
               .EntityMustExist<User>(x => x.MaterialsApprovedBy)
               .EntityMustExist<WorkDescription>(x => x.WorkDescription)
               .EntityMustExist<User>(x => x.ApprovedBy);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert
               .PropertyHasMaxStringLength(x => x.PremiseNumber, WorkOrder.StringLengths.PREMISE_NUMBER)
               .PropertyHasMaxStringLength(x => x.AccountCharged, WorkOrder.StringLengths.ACCOUNT_CHARGED)
               .PropertyHasMaxStringLength(x => x.MaterialsDocID, WorkOrder.StringLengths.MATERIALS_DOC_ID);
        }

        [TestMethod]
        public void Test_Map_SetsSAPWorkOrdersEnabled()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {
                SAPEnabled = true, SAPWorkOrdersEnabled = true, OperatingCenterCode = "NJ4"
            });
            var workOrder = GetEntityFactory<WorkOrder>().Create(new {OperatingCenter = operatingCenter});
            _viewModel = _viewModelFactory.Build<EditGeneralWorkOrder, WorkOrder>(workOrder);

            _viewModel.Map(workOrder);

            Assert.IsTrue(_viewModel.SAPWorkOrdersEnabled);
        }

        [TestMethod]
        public void Test_Map_SetsSAPWorkOrdersEnabledToFalse()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {
                SAPEnabled = true,
                SAPWorkOrdersEnabled = false,
                OperatingCenterCode = "NJ4"
            });
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = operatingCenter });
            _viewModel = _viewModelFactory.Build<EditGeneralWorkOrder, WorkOrder>(workOrder);

            _viewModel.Map(workOrder);

            Assert.IsFalse(_viewModel.SAPWorkOrdersEnabled);
        }

        [TestMethod]
        public void Test_MapToEntity_SetsDigitalAsBuiltRequired_IfWorkDescriptionSaysTo()
        {
            var workDescription = GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create();

            _viewModel.WorkDescription = workDescription.Id;

            var result = _viewModel.MapToEntity(_entity);
            
            Assert.IsTrue(result.DigitalAsBuiltRequired);
        }

        [TestMethod]
        public void Test_MapToEntity_DoesNotSetDigitalAsBuiltRequired_IfWorkDescriptionDoesNotSayTo()
        {
            var workDescription = GetFactory<HydrantLandscapingWorkDescriptionFactory>().Create();

            _viewModel.WorkDescription = workDescription.Id;

            var result = _viewModel.MapToEntity(_entity);
            
            Assert.IsFalse(result.DigitalAsBuiltRequired);
        }

        [TestMethod]
        public void Test_MapToEntity_DoesNotThrowException_WhenWorkDescriptionIsNull()
        {
            // functional tests failed because of this issue
            _viewModel.WorkDescription = null;
            
            MyAssert.DoesNotThrow(() => _viewModel.MapToEntity(_entity));
        }
    }
}