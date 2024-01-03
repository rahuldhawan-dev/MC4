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
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class CreateHydrantInspectionTest : MapCallMvcInMemoryDatabaseTestBase<HydrantInspection>
    {
        #region Defaults

        [TestMethod]
        public void TestSetDefaultsSetsDefaults()
        {
            var now = DateTime.Now;
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(now);
            var hydrantTagStatus = GetEntityFactory<HydrantTagStatus>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create();
            var viewModel = _viewModelFactory.Build<CreateHydrantInspection>();
            viewModel.Hydrant = hydrant.Id;

            viewModel.SetDefaults();

            Assert.AreEqual(now, viewModel.DateInspected.Value);
            Assert.AreEqual(hydrant.Town.State.Id, viewModel.State);
            Assert.IsNull(viewModel.HydrantTagStatus);

            hydrant.HydrantTagStatus = hydrantTagStatus;

            viewModel.SetDefaults();

            Assert.AreEqual(hydrant.HydrantTagStatus.Id, viewModel.HydrantTagStatus);
        }

        #endregion

        #region Fields

        private ViewModelTester<CreateHydrantInspection, HydrantInspection> _vmTester;
        private CreateHydrantInspection _viewModel;
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
            _user = GetFactory<UserFactory>().Create(new {IsAdmin = true});
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<HydrantInspection>().Create();
            _viewModel = _viewModelFactory.Build<CreateHydrantInspection, HydrantInspection>(_entity);
            _vmTester = new ViewModelTester<CreateHydrantInspection, HydrantInspection>(_viewModel, _entity);

            GetEntityFactory<HydrantInspectionType>().Create(new {Description = "FLUSH"});
            GetEntityFactory<HydrantInspectionType>().Create(new {Description = "INSPECT/FLUSH"});
            GetEntityFactory<HydrantInspectionType>().Create(new {Description = "WATER QUALITY"});
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestHydrantCanMapBothWays()
        {
            var opCntr1 = GetEntityFactory<OperatingCenter>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opCntr1});

            _entity.Hydrant = hydrant;
            _vmTester.MapToViewModel();

            Assert.AreEqual(hydrant.Id, _viewModel.Hydrant);

            _entity.Hydrant = null;
            _vmTester.MapToEntity();

            Assert.AreSame(hydrant, _entity.Hydrant);
        }

        [TestMethod]
        public void TestMapToEntitySetsInspectedBy()
        {
            // need to evict because MapToEntity causes a flush 
            // which causes an nhibernate error because nhibernate
            // is stupid.
            Session.Evict(_entity);
            _entity.InspectedBy = null;
            _vmTester.MapToEntity();
            Assert.AreSame(_user, _entity.InspectedBy);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledFalse()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = false});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var hydrant = GetEntityFactory<Hydrant>().Create(new {Town = town, OperatingCenter = opc1});
            _viewModel.Hydrant = hydrant.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPTrueWhenOperatingCenterSAPEnabledTrue()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var hydrant = GetEntityFactory<Hydrant>().Create(new {Town = town, OperatingCenter = opc1});
            _viewModel.Hydrant = hydrant.Id;

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsSendToSAPFalseWhenOperatingCenterSAPEnabledTrueAndContractedOps()
        {
            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                   .Create(new {SAPEnabled = true, IsContractedOperations = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var hydrant = GetEntityFactory<Hydrant>().Create(new {Town = town, OperatingCenter = opc1});
            _viewModel.Hydrant = hydrant.Id;

            _vmTester.MapToEntity();

            Assert.IsFalse(_viewModel.SendToSAP);
        }

        [TestMethod]
        public void TestMapToEntitySetsOperatingCenterFromHydrant()
        {
            var opc1 =
                GetFactory<UniqueOperatingCenterFactory>()
                   .Create(new {SAPEnabled = true, IsContractedOperations = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = town,
                OperatingCenter = opc1,
                Abbreviation = "ZZ"
            });
            var hydrant = GetEntityFactory<Hydrant>().Create(new {Town = town, OperatingCenter = opc1});
            _viewModel.Hydrant = hydrant.Id;

            _vmTester.MapToEntity();

            Assert.AreEqual(opc1, _entity.OperatingCenter);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestRequiredFieldsAreRequired()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Hydrant);
            var ht = GetEntityFactory<HydrantTagStatus>().Create();
            ValidationAssert.PropertyIsRequiredWhen(_viewModel, x => x.HydrantTagStatus, ht.Id, x => x.State, 1, 2);
        }

        [TestMethod]
        public void TestHydrantEntityMustExist()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.Hydrant, GetFactory<HydrantFactory>().Create());
        }

        [TestMethod]
        public void TestResidualChlorineMustBeGreaterThanOrEqualToZeroAndLessThanOrEqualToNinePointNineNine()
        {
            _viewModel.ResidualChlorine = -1;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ResidualChlorine,
                "Residual chlorine must be between 0 and 9.99");

            _viewModel.ResidualChlorine = 0;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ResidualChlorine);

            _viewModel.ResidualChlorine = 10.1m;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.ResidualChlorine,
                "Residual chlorine must be between 0 and 9.99");

            _viewModel.ResidualChlorine = 9.99m;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ResidualChlorine);
        }

        [TestMethod]
        public void TestPreResidualChlorineMustBeGreaterThanOrEqualToZeroAndLessThanOrEqualToNinePointNineNine()
        {
            _viewModel.PreResidualChlorine = -1;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.PreResidualChlorine,
                "Residual chlorine must be between 0 and 9.99");

            _viewModel.PreResidualChlorine = 0;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PreResidualChlorine);

            _viewModel.PreResidualChlorine = 10.1m;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.PreResidualChlorine,
                "Residual chlorine must be between 0 and 9.99");

            _viewModel.PreResidualChlorine = 9.99m;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PreResidualChlorine);
        }

        [TestMethod]
        public void TestTotalChlorineMustBeGreaterThanOrEqualToZeroAndLessThanOrEqualToFour()
        {
            _viewModel.TotalChlorine = -1;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.TotalChlorine,
                "Total chlorine must be between 0 and 4.");

            _viewModel.TotalChlorine = 0;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.TotalChlorine);

            _viewModel.TotalChlorine = 4.1m;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.TotalChlorine,
                "Total chlorine must be between 0 and 4.");

            _viewModel.TotalChlorine = 4;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.TotalChlorine);
        }

        [TestMethod]
        public void TestPreTotalChlorineMustBeGreaterThanOrEqualToZeroAndLessThanOrEqualToFour()
        {
            _viewModel.PreTotalChlorine = -1;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.PreTotalChlorine,
                "Total chlorine must be between 0 and 4.");

            _viewModel.PreTotalChlorine = 0;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PreTotalChlorine);

            _viewModel.PreTotalChlorine = 4.1m;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.PreTotalChlorine,
                "Total chlorine must be between 0 and 4.");

            _viewModel.PreTotalChlorine = 4;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.PreTotalChlorine);
        }

        [TestMethod]
        public void TestValidationFailsIfHydrantIsNotInspectable()
        {
            _entity.Hydrant.Status.Id = AssetStatus.Indices.CANCELLED;
            Assert.IsFalse(_entity.Hydrant.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Hydrant,
                "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Hydrant.Status.Id = AssetStatus.Indices.INACTIVE;
            Assert.IsFalse(_entity.Hydrant.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Hydrant,
                "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Hydrant.Status.Id = AssetStatus.Indices.RETIRED;
            Assert.IsFalse(_entity.Hydrant.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Hydrant,
                "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Hydrant.Status.Id = AssetStatus.Indices.REMOVED;
            Assert.IsFalse(_entity.Hydrant.IsInspectable, "Sanity");

            ValidationAssert.ModelStateHasError(_viewModel, x => x.Hydrant,
                "New inspection records can not be created for assets that are cancelled, inactive, retired, or removed.");

            _entity.Hydrant.Status.Id = AssetStatus.Indices.ACTIVE;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.Hydrant);
        }

        #endregion
    }
}
