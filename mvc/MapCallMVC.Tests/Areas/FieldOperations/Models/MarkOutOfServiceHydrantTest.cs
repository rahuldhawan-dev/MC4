using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
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

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class MarkOutOfServiceHydrantTest : MapCallMvcInMemoryDatabaseTestBase<Hydrant>
    {
        #region Fields

        private ViewModelTester<MarkOutOfServiceHydrant, Hydrant> _vmTester;
        private MarkOutOfServiceHydrant _viewModel;
        private Hydrant _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IHydrantRepository>().Use<HydrantRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
            e.For<IFireDistrictRepository>().Use<FireDistrictRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IHydrantBillingRepository>().Use<HydrantBillingRepository>();
            e.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new {
                IsAdmin = true
            });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<Hydrant>().Create();
            _viewModel = _viewModelFactory.Build<MarkOutOfServiceHydrant, Hydrant>(_entity);
            _vmTester = new ViewModelTester<MarkOutOfServiceHydrant, Hydrant>(_viewModel, _entity);

            // These need to exist
            GetFactory<PublicHydrantBillingFactory>().Create();
            GetFactory<RetiredAssetStatusFactory>().Create();
            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityCreatesNewOutOfServiceRecord()
        {
            _entity.FireDistrict = null;
            Assert.IsFalse(_entity.OutOfService, "Sanity");
            Assert.AreEqual(0, _entity.OutOfServiceRecords.Count);

            var expectedDateCreated = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(expectedDateCreated);
            _viewModel.OutOfServiceDate = expectedDateCreated.AddDays(1);

            _vmTester.MapToEntity();
            var result = _entity.OutOfServiceRecords.Single();
            Assert.AreSame(_entity, result.Hydrant);
            Assert.AreEqual(_viewModel.OutOfServiceDate, result.OutOfServiceDate);
            Assert.AreSame(_user, result.OutOfServiceByUser);
            Assert.IsNull(result.FireDepartmentContact, "Should be null because hydrant does not have a fire district.");
            Assert.IsNull(result.FireDepartmentFax, "Should be null because hydrant does not have a fire district.");
            Assert.IsNull(result.FireDepartmentPhone, "Should be null because hydrant does not have a fire district.");
        }

        [TestMethod]
        public void TestMapToEntitySetsFireDepartmentInfoOnOutOfServiceRecordFromHydrantFireDistrict()
        {
            var expectedFD = GetFactory<FireDistrictFactory>().Create(new {
                Contact = "Some contact",
                Fax = "Some fax",
                Phone = "Some phone"
            });
            _entity.FireDistrict = expectedFD;

            Assert.IsFalse(_entity.OutOfService, "Sanity");
            Assert.AreEqual(0, _entity.OutOfServiceRecords.Count);

            _viewModel.OutOfServiceDate = DateTime.Now;

            _vmTester.MapToEntity();
            var result = _entity.OutOfServiceRecords.Single();
            Assert.AreEqual("Some contact", result.FireDepartmentContact);
            Assert.AreEqual("Some fax", result.FireDepartmentFax);
            Assert.AreEqual("Some phone", result.FireDepartmentPhone);
        }

        [TestMethod]
        public void TestOutOfServiceDateIsRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OutOfServiceDate);
        }
        

        #endregion
    }
}
