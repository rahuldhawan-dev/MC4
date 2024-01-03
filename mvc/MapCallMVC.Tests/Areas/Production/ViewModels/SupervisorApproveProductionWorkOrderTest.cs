using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using System;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class SupervisorApproveProductionWorkOrderTest : MapCallMvcInMemoryDatabaseTestBase<ProductionWorkOrder>
    {
        #region Fields

        private ViewModelTester<SupervisorApproveProductionWorkOrder, ProductionWorkOrder> _vmTester;
        private SupervisorApproveProductionWorkOrder _viewModel;
        private ProductionWorkOrder _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _dateTimeProvider = new Mock<IDateTimeProvider>();

            _container.Inject(_dateTimeProvider.Object);
            _container.Inject(_authServ.Object);

            _entity = GetEntityFactory<ProductionWorkOrder>().Create();
            _viewModel = _viewModelFactory.Build<SupervisorApproveProductionWorkOrder, ProductionWorkOrder>(_entity);
            _vmTester = new ViewModelTester<SupervisorApproveProductionWorkOrder, ProductionWorkOrder>(_viewModel, _entity);

            // These need to exist for multiple tests.
            GetFactory<OrderTypeFactory>().CreateAll();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapSetsCorrective()
        {
            _viewModel.Corrective = null;
            _entity.ProductionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {
                OrderType = Session.Load<OrderType>(OrderType.Indices.CORRECTIVE_ACTION_20)
            });

            _vmTester.MapToViewModel();
            Assert.IsTrue(_viewModel.Corrective.Value, "Value must be true for corrective work orders.");

            _viewModel.Corrective = null;
            _entity.ProductionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {
                OrderType = Session.Load<OrderType>(OrderType.Indices.RP_CAPITAL_40)
            });

            _vmTester.MapToViewModel();
            Assert.IsFalse(_viewModel.Corrective.Value, "Value must be false for non-corrective work orders.");
        }

        [TestMethod]
        public void TestMapToEntitySetsApprovedOnAndApprovedBy()
        {
            var expectedDate = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDate);

            _vmTester.MapToEntity();

            MyAssert.AreClose(expectedDate, _entity.ApprovedOn.Value);
            Assert.AreEqual(_user, _entity.ApprovedBy);
        }

        [TestMethod]
        public void TestCauseCodeIsRequiredWhenDescriptionIsCorrective()
        {
            var code = GetEntityFactory<ProductionWorkOrderCauseCode>().Create();

            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.CauseCode, code.Id, x => x.Corrective, true, false, SupervisorApproveProductionWorkOrder.CAUSE_CODE_ERROR_MESSAGE);

            _viewModel.CauseCode = code.Id;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.CauseCode);

            _viewModel.Corrective = false;
            _viewModel.CauseCode = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.CauseCode);
        }

        [TestMethod]
        public void TestValidationFailsIfTheWorkOrderCanNotBeSupervisorApproved()
        {
            _entity.ProductionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new {
                OrderType = Session.Load<OrderType>(OrderType.Indices.CORRECTIVE_ACTION_20)
            });
            _entity.DateCompleted = DateTime.Now;

            Assert.IsTrue(_entity.CanBeSupervisorApproved, "Sanity check");
            _entity.ApprovedOn = DateTime.Now; // Make it so the record is already approved
            Assert.IsFalse(_entity.CanBeSupervisorApproved, "Sanity check");
            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, $"Work Order #{_entity.Id} can not be supervisor approved.");

            _entity.ApprovedOn = null;
            ValidationAssert.ModelStateIsValid(_viewModel);
        }

        #endregion
    }
}