using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class EditSmartCoverAlertTest : MapCallMvcInMemoryDatabaseTestBase<SmartCoverAlert>
    {
        #region Fields

        private ViewModelTester<EditSmartCoverAlert, SmartCoverAlert> _vmTester;
        private EditSmartCoverAlert _viewModel;
        private SmartCoverAlert _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetEntityFactory<SmartCoverAlert>().Create();
            _viewModel = _viewModelFactory.Build<EditSmartCoverAlert>();
            _vmTester = new ViewModelTester<EditSmartCoverAlert, SmartCoverAlert>(_viewModel, _entity);
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.AcknowledgedOn);
        }

        [TestMethod]
        public void TestMapToEntitySetValues()
        {
            _entity.AcknowledgedOn = null;
            _entity.Acknowledged = false;
            _entity.AcknowledgedBy = null;
            _entity.NeedsToSync = false;

            _viewModel.AcknowledgedOn = null;

            _viewModel.MapToEntity(_entity);

            Assert.IsTrue(_entity.Acknowledged);
            Assert.IsNotNull(_entity.AcknowledgedBy);
            Assert.AreEqual(_user.Id, _entity.AcknowledgedBy.Id);
            Assert.IsTrue(_entity.NeedsToSync);
            Assert.IsNotNull(_entity.AcknowledgedOn);
        }

        [TestMethod]
        public void TestMapSetsProperties()
        {
            _viewModel.Acknowledged = false;
            _viewModel.User = null;

            _viewModel.Map(_entity);

            Assert.IsTrue(_viewModel.Acknowledged);
            Assert.IsNotNull(_viewModel.User);
            Assert.AreEqual(_user.Id, _viewModel.User.Id);
        }

        #endregion
    }
}
