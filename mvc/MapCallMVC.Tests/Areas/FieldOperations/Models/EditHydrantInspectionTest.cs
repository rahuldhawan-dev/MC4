using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class EditHydrantInspectionTest : MapCallMvcInMemoryDatabaseTestBase<HydrantInspection>
    {
        #region Fields

        private ViewModelTester<EditHydrantInspection, HydrantInspection> _vmTester;
        private EditHydrantInspection _viewModel;
        private HydrantInspection _entity;
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
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IHydrantRepository>().Use<HydrantRepository>();
            e.For<IValveRepository>().Use<ValveRepository>();
            e.For<IHydrantInspectionRepository>().Use<HydrantInspectionRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<HydrantInspection>().Create();
            _viewModel = _viewModelFactory.Build<EditHydrantInspection, HydrantInspection>( _entity);
            _vmTester = new ViewModelTester<EditHydrantInspection, HydrantInspection>(_viewModel, _entity);

            GetEntityFactory<HydrantInspectionType>().Create(new { Description = "FLUSH" });
            GetEntityFactory<HydrantInspectionType>().Create(new { Description = "INSPECT/FLUSH" });
            GetEntityFactory<HydrantInspectionType>().Create(new { Description = "WATER QUALITY" });

        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMapToEntityDoesNotOverwriteDateAddedValue()
        {
            var expected = new DateTime(1984, 4, 24);
            _entity.CreatedAt = expected;
            _vmTester.MapToEntity();
            Assert.AreEqual(expected, _entity.CreatedAt);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var hydrant = GetEntityFactory<Hydrant>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Hydrant = hydrant.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPTrueWhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
            {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var hydrant = GetEntityFactory<Hydrant>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Hydrant = hydrant.Id;

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledTrueAndContractedOps()
        {
            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                    .Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
            {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var hydrant = GetEntityFactory<Hydrant>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Hydrant = hydrant.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsOperatingCenterFromHydrant()
        {
            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                    .Create(new { SAPEnabled = true, IsContractedOperations = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
            {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var hydrant = GetEntityFactory<Hydrant>().Create(new { Town = town, OperatingCenter = opc1 });
            _viewModel.Hydrant = hydrant.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(opc1, _entity.OperatingCenter);
        }


        #endregion
    }
}
