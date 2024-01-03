using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class LockoutDeviceTest : MapCallMvcInMemoryDatabaseTestBase<LockoutDevice>
    {
        #region Fields

        private LockoutDeviceViewModel _viewModel;
        private LockoutDevice _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private ViewModelTester<LockoutDeviceViewModel, LockoutDevice> _vmTester;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IUserRepository>().Use<UserRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<LockoutDevice>().Create();
            _viewModel = _viewModelFactory.Build<LockoutDeviceViewModel, LockoutDevice>(_entity);
            _vmTester = new ViewModelTester<LockoutDeviceViewModel, LockoutDevice>(_viewModel, _entity);

            _user = new User { FullName = "Gomer Pile"};
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }
        
        #endregion

        #region Mapping

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.SerialNumber);
            _vmTester.CanMapBothWays(x => x.Description);
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            _entity.OperatingCenter = opc;
            _vmTester.MapToViewModel();
            Assert.AreEqual(opc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();
            Assert.AreSame(opc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestLockoutDeviceColorCanMapBothWays()
        {
            var color = GetEntityFactory<LockoutDeviceColor>().Create();
            _entity.LockoutDeviceColor = color;
            _vmTester.MapToViewModel();
            Assert.AreEqual(color.Id, _viewModel.LockoutDeviceColor);

            _entity.LockoutDeviceColor = null;
            _vmTester.MapToEntity();
            Assert.AreSame(color, _entity.LockoutDeviceColor);
        }

        #endregion
    }

    public abstract class LockoutDeviceViewModelTest<TViewModel> : ViewModelTestBase<LockoutDevice, TViewModel> where TViewModel : LockoutDeviceViewModel
    {
        #region Fields
        
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        
        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            GetEntityFactory<LockoutDevice>().Create();
        }

        #endregion

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            _vmTester.CanMapBothWays(x => x.SerialNumber);
            _vmTester.CanMapBothWays(x => x.LockoutDeviceColor, GetEntityFactory<LockoutDeviceColor>().Create());
            _vmTester.CanMapBothWays(x => x.Description);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.LockoutDeviceColor, GetEntityFactory<LockoutDeviceColor>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Description);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.Description, LockoutDevice.StringLengths.DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(_viewModel, x => x.SerialNumber, LockoutDevice.StringLengths.DESCRIPTION);
        }
    }

    [TestClass]
    public class CreateLockoutDeviceTest : LockoutDeviceViewModelTest<CreateLockoutDevice>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();
            _vmTester.CanMapBothWays(x => x.Person, GetEntityFactory<User>().Create());
        }

        [TestMethod]
        public void TestPersonEntityMustExist()
        {
            var person = GetFactory<UserFactory>().Create();
            ValidationAssert.EntityMustExist(_viewModel, x => x.Person, person);
        }

        [TestMethod]
        public void TestRequiredValidationPerson()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Person);
        }

        #endregion
    }

    [TestClass]
    public class EditLockoutDeviceTest : LockoutDeviceViewModelTest<EditLockoutDevice>
    {
        
    }
}
