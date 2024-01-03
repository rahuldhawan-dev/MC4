﻿using System;
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
    public class CopyHydrantTest : MapCallMvcInMemoryDatabaseTestBase<Hydrant>
    {
        #region Private Members

        private ViewModelTester<CopyHydrant, Hydrant> _vmTester;
        private CopyHydrant _viewModel;
        private Hydrant _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private AssetStatus _activeStatus;
        private AssetStatus _retiredStatus;
        private AssetStatus _cancelledStatus;
        private AssetStatus _removedStatus;
        private HydrantBilling _publicBilling;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IHydrantRepository>().Use<HydrantRepository>();
            e.For<IAssetStatusRepository>().Use<AssetStatusRepository>();
            e.For<IDateTimeProvider>().Mock();
            e.For<IAbbreviationTypeRepository>().Use<AbbreviationTypeRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            _user.IsAdmin = true;

            _entity = GetEntityFactory<Hydrant>().Create();
            _viewModel = _viewModelFactory.Build<CopyHydrant, Hydrant>(_entity);
            _vmTester = new ViewModelTester<CopyHydrant, Hydrant>(_viewModel, _entity);

            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();
            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
        }

        #endregion

        [TestMethod]
        public void TestSendNotificationIsSetAppropriatelyBasedOnStatus()
        {
            var statuses = new[] {_activeStatus, _retiredStatus, _cancelledStatus, _removedStatus};

            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = status;
                _viewModel.Town = town.Id;
                _viewModel.OperatingCenter = opc1.Id;
                _viewModel.Status = status.Id;

                _vmTester.MapToEntity();

                if (status.Id == AssetStatus.Indices.ACTIVE)
                    Assert.IsTrue(_viewModel.SendNotificationOnSave);
                else
                    Assert.IsFalse(_viewModel.SendNotificationOnSave);

            }
        }

        [TestMethod]
        public void TestStatusIsNotCopiedFromOriginalRecordSoThatOtherRequiredFieldsCanBeSetProperly()
        {
            var statuses = new[] {_activeStatus, _retiredStatus, _cancelledStatus, _removedStatus};
            foreach (var status in statuses)
            {
                var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
                _entity.Town = town;
                _entity.OperatingCenter = opc1;
                _entity.Status = status;
                _viewModel.Town = town.Id;
                _viewModel.OperatingCenter = opc1.Id;

                var entity = _vmTester.MapToEntity();

                Assert.AreEqual(entity.Status.Id, AssetStatus.Indices.PENDING);
            }
        }

        [TestMethod]
        public void TestMapSetsDateInstalledToNull()
        {
            _entity.DateInstalled = DateTime.Now;

            _viewModel.Map(_entity);

            Assert.IsNull(_viewModel.DateInstalled);
        }
    }
}