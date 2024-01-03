using System.IO;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.WaterQuality.Models.ViewModels
{
    [TestClass]
    public class CreateBacterialWaterSampleMassEditTest : MapCallMvcInMemoryDatabaseTestBase<BacterialWaterSample>
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private Mock<IUserRepository> _userRepo;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            _userRepo = e.For<IUserRepository>().Mock();
            e.For<IPublicWaterSupplyRepository>().Use<PublicWaterSupplyRepository>();
            e.For<IDateTimeProvider>().Mock();
            e.For<IBacterialWaterSampleRepository>().Use<BacterialWaterSampleRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create(new { IsAdmin = true });
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestValidateFailsIfThereAreDuplicateIds()
        {
            var model = _viewModelFactory.Build<CreateBacterialWaterSampleMassEdit>();
            model.FileUpload = _container.GetInstance<MMSINC.Data.ExcelAjaxFileUpload<BacterialWaterSampleMassEditExcelItem>>();
            model.FileUpload.Items = new System.Collections.Generic.List<BacterialWaterSampleMassEditExcelItem>();
            model.FileUpload.SkipExcelImporting = true;

            model.FileUpload.Items.Add(new BacterialWaterSampleMassEditExcelItem(_container) {Id = 1});
            model.FileUpload.Items.Add(new BacterialWaterSampleMassEditExcelItem(_container) {Id = 1});
            model.FileUpload.Items.Add(new BacterialWaterSampleMassEditExcelItem(_container) {Id = 2});
            model.FileUpload.Items.Add(new BacterialWaterSampleMassEditExcelItem(_container) {Id = 3});
            model.FileUpload.Items.Add(new BacterialWaterSampleMassEditExcelItem(_container) {Id = 3});

            ValidationAssert.ModelStateHasNonPropertySpecificError(model, "The following record ids appear more than once in the file: 1, 3.");
        }

        [TestMethod]
        public void TestValidationFailsIfRecordDoesNotExist()
        {
            var existingEntity = GetFactory<BacterialWaterSampleFactory>().Create();

            var model = _viewModelFactory.Build<CreateBacterialWaterSampleMassEdit>();
            model.FileUpload = _container.GetInstance<MMSINC.Data.ExcelAjaxFileUpload<BacterialWaterSampleMassEditExcelItem>>();
            model.FileUpload.Items = new System.Collections.Generic.List<BacterialWaterSampleMassEditExcelItem>();
            model.FileUpload.SkipExcelImporting = true;

            model.FileUpload.Items.Add(new BacterialWaterSampleMassEditExcelItem(_container) { Id = existingEntity.Id });
            model.FileUpload.Items.Add(new BacterialWaterSampleMassEditExcelItem(_container) { Id = existingEntity.Id + 1 });

            var expectedErrorId = existingEntity.Id + 1;
            ValidationAssert.ModelStateHasNonPropertySpecificError(model, $"The following ids do not match an existing record: {expectedErrorId}.");
        }

        [TestMethod]
        public void TestPropertyLevelValidationOccursOnIndividualRecords()
        {
            var existingEntity = GetFactory<BacterialWaterSampleFactory>().Create();
            var vm = _viewModelFactory.Build<BacterialWaterSampleMassEditExcelItem, BacterialWaterSample>(existingEntity);
            vm.Cl2Total = -1;

            var model = _viewModelFactory.Build<CreateBacterialWaterSampleMassEdit>();
            model.FileUpload = _container.GetInstance<MMSINC.Data.ExcelAjaxFileUpload<BacterialWaterSampleMassEditExcelItem>>();
            model.FileUpload.FileUpload = new MMSINC.Metadata.AjaxFileUpload();

            using (var ms = new MemoryStream())
            {
                var exporter = new ExcelExport();
                exporter.AddSheet(new[] { vm }, new ExcelExportSheetArgs {SheetName = "Sheet" });
                exporter.ExportTo(ms);
                model.FileUpload.FileUpload.BinaryData = ms.ToArray();
            }

            ValidationAssert.ModelStateHasNonPropertySpecificError(model, "Row[2]: The field Cl2Total must be between 0.000 and 99.999.");
        }

        #endregion
    }
}
