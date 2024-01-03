using System;
using System.Collections.Generic;
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
    public class CreateMeterChangeOutContractTest : MapCallMvcInMemoryDatabaseTestBase<MeterChangeOutContract>
    {
        #region Fields

        private ViewModelTester<CreateMeterChangeOutContract, MeterChangeOutContract> _vmTester;
        private CreateMeterChangeOutContract _viewModel;
        private MeterChangeOutContract _entity;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private OperatingCenter _opc;
        private Town _town;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            e.For<IUserRepository>().Use<UserRepository>();
            e.For<IContractorRepository>().Use<ContractorRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new
            {
                IsAdmin = true
            });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetEntityFactory<MeterChangeOutContract>().Create();
            _viewModel = _viewModelFactory.Build<CreateMeterChangeOutContract, MeterChangeOutContract>( _entity);
            _vmTester = new ViewModelTester<CreateMeterChangeOutContract, MeterChangeOutContract>(_viewModel, _entity);

            // NOTE: This bypasses having to setup the view model with an actual excel file
            _viewModel.FileUpload = new MMSINC.Metadata.AjaxFileUpload {
                BinaryData = new byte[] {1, 2, 3}
            };
            _viewModel.ExcelRecords = new List<MeterChangeOutContractExcelRecord>(); 

            _opc = GetFactory<OperatingCenterFactory>().Create();
            _opc.OperatingCenterTowns = new List<OperatingCenterTown>();
            _town = GetFactory<TownFactory>().Create();
            _opc.OperatingCenterTowns.Add(new OperatingCenterTown {
                OperatingCenter = _opc,
                Town = _town
            });
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Contractor);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.IsActive);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
        }

        [TestMethod]
        public void TestSetDefaultsIsActiveToTrue()
        {
            _viewModel.IsActive = null;
            Assert.IsNull(_viewModel.IsActive);
            _viewModel.SetDefaults();
            Assert.IsTrue(_viewModel.IsActive.Value);
        }

        [TestMethod]
        public void TestValidationFailsIfImportedRecordsIncludeTownThatIsNotPartOfOperatingCenter()
        {
            var record = new MeterChangeOutContractExcelRecord {
                ServiceCity = _town.ShortName
            };
            _viewModel.ExcelRecords.Add(record);
            ValidationAssert.ModelStateIsValid(_viewModel); // Sanity check.

            record.ServiceCity = "Nope";
            ValidationAssert.ModelStateHasNonPropertySpecificError(_viewModel, "The imported file includes towns that do not belong to the selected operating center: Nope");
        }


        #endregion
    }
}
