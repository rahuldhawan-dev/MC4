using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateMarkoutDamageTest : MapCallMvcInMemoryDatabaseTestBase<MarkoutDamage>
    {
        #region Fields

        private MarkoutDamage _entity;
        private CreateMarkoutDamage _viewModel;
        private ViewModelTester<CreateMarkoutDamage, MarkoutDamage> _vmTester;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private MarkoutDamageToType _otherMarkoutDamageToType;
        private MarkoutDamageToType _ourMarkoutDamageToType;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IEmployeeRepository>().Use<EmployeeRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _user = new User();
            _authServ.SetupGet(x => x.CurrentUser).Returns(_user);

            _otherMarkoutDamageToType = GetFactory<MarkoutDamageToTypeFactory>().Create(new { Description = MarkoutDamageToType.ImportantDescriptions.OTHERS });
            _ourMarkoutDamageToType = GetFactory<MarkoutDamageToTypeFactory>().Create(new { Description = MarkoutDamageToType.ImportantDescriptions.OURS });

            // This needs to come after the repositories are made or else this fails due to the
            // dynamic requiredwhen validators being unable to find repositories.
            _entity = GetFactory<MarkoutDamageFactory>().Create();
            _viewModel = _viewModelFactory.Build<CreateMarkoutDamage, MarkoutDamage>( _entity);
            _vmTester = new ViewModelTester<CreateMarkoutDamage, MarkoutDamage>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMapToEntitySetsCreatedByToCurrentUser()
        {
            _user.UserName = "some user";
            _entity.CreatedBy = null;

            _vmTester.MapToEntity();

            Assert.AreEqual("some user", _entity.CreatedBy);
        }

        #endregion

        #endregion
    }
}
