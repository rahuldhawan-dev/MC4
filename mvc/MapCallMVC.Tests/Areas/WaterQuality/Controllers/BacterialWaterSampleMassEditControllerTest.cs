using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.WaterQuality.Controllers;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class BacterialWaterSampleMassEditControllerTest : MapCallMvcControllerTestBase<BacterialWaterSampleMassEditController, BacterialWaterSample>
    {
        #region Private Members

        private Mock<INotificationService> _notifier;
        private Mock<IRoleService> _roleService;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _roleService = new Mock<IRoleService>();

            _container.Inject(_notifier.Object);
            _container.Inject(_roleService.Object);
        }

        private void SetupTargetForExcel()
        {
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.WaterQualityGeneral;

            Authorization.Assert(a =>
            {
                // All three actions require the Edit role because this is acting like editing for BacterialWaterSampleController.
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleMassEdit/Export/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleMassEdit/New/", role, RoleActions.Edit);
                a.RequiresRole("~/WaterQuality/BacterialWaterSampleMassEdit/Create/", role, RoleActions.Edit);
            });
        }

        #endregion

        #region Create

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // noop: see notes in other test.
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // override needed because:
            // - Create is incorrectly named and should be Update
            // - It's updating numerous records, not just one
            // - It redirects to the Index page when it's done.
            Assert.Inconclusive("Write me");
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            // override because this redirects to New instead of returning a New view for whatever reason.
            Assert.Inconclusive("Write me");
        }

        [TestMethod]
        public void TestCreateFailsValidationIfExcelFileHasMoreThan200Records()
        {
            var model = _viewModelFactory.Build<CreateBacterialWaterSampleMassEdit>();
            model.FileUpload = _container.GetInstance<ExcelAjaxFileUpload<BacterialWaterSampleMassEditExcelItem>>();
            model.FileUpload.Items = new List<BacterialWaterSampleMassEditExcelItem>();
            for (var i = 0; i < 201; i++)
            {
                model.FileUpload.Items.Add(new BacterialWaterSampleMassEditExcelItem(_container));
            }

            var result = _target.Create(model);

            Assert.IsTrue(_target.ModelState["FileUpload"].Errors.Any(x =>
                x.ErrorMessage == "You may only import a maximum of 200 records at one time."));
            MvcAssert.RedirectsToRoute(result, new { Action = "New", Controller = "BacterialWaterSampleMassEdit" });
        }

        [TestMethod]
        public void TestCreateCanUpdate200Records()
        {
            var allRecords = GetFactory<BacterialWaterSampleFactory>().CreateList(200);
            Assert.AreEqual(200, allRecords.Count, "Sanity");

            var model = _viewModelFactory.Build<CreateBacterialWaterSampleMassEdit>();
            model.FileUpload = _container.GetInstance<ExcelAjaxFileUpload<BacterialWaterSampleMassEditExcelItem>>();
            model.FileUpload.Items = new System.Collections.Generic.List<BacterialWaterSampleMassEditExcelItem>();

            for (var i = 0; i < 200; i++)
            {
                var cur = allRecords[i];
                var vm = _viewModelFactory.Build<BacterialWaterSampleMassEditExcelItem, BacterialWaterSample>(cur);
                vm.Temperature = i;
                model.FileUpload.Items.Add(vm);
            }
            
            var result = _target.Create(model);

            for (var i = 0; i < 200; i++)
            {
                // Asserting the view model values were properly mapped to the entities.
                Assert.AreEqual((decimal)i, allRecords[i].Temperature);
            }
        }

        [TestMethod]
        public void TestCreateSendsOutNotificationsForEveryRecord()
        {
            var records = GetFactory<BacterialWaterSampleFactory>().CreateList(2, new {
                ComplianceSample = true,
                EColiConfirm = true 
            });

            var model = _viewModelFactory.Build<CreateBacterialWaterSampleMassEdit>();
            model.FileUpload = _container.GetInstance<ExcelAjaxFileUpload<BacterialWaterSampleMassEditExcelItem>>();
            model.FileUpload.Items = new System.Collections.Generic.List<BacterialWaterSampleMassEditExcelItem>();

            foreach (var rec in records)
            {
                model.FileUpload.Items.Add(_viewModelFactory
                   .Build<BacterialWaterSampleMassEditExcelItem, BacterialWaterSample>(rec));
            }

            var mockedNotifierArgs = new List<NotifierArgs>();
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => mockedNotifierArgs.Add(x));

            var result = _target.Create(model);

            Assert.IsTrue(mockedNotifierArgs.Any(x => x.Data == records[0]));
            Assert.IsTrue(mockedNotifierArgs.Any(x => x.Data == records[1]));
        }

        #endregion

        #region Export

        [TestMethod]
        public void TestExportIncludesAWholeTonOfColumns()
        {
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new{ OperatingCenterCode = "QQ1", OperatingCenterName  = "Q Town"});
            var user = GetFactory<UserFactory>().Create(new{ UserName = "thisdude" });
            var sampleCollectionDate = new DateTime(1984, 4, 24, 4, 0, 4);
            var receivedByLabDTM = new DateTime(1985, 5, 25, 5, 0, 5);
            var coliformSetupDTM = new DateTime(1986, 6, 26, 6, 0, 6);
            var coliformReadDTM = new DateTime(1987, 7, 27, 7, 0, 7);
            var hpcSetupDTM = new DateTime(1988, 8, 28, 8, 0, 8);
            var hpcReadDTM = new DateTime(1989, 9, 29, 9, 0, 9);
            var bacterialSampleType = GetFactory<RoutineBacterialSampleTypeFactory>().Create(new{ Description = "This bacterial sample type"});
            var coliformConfirmMethod = GetEntityFactory<BacterialWaterSampleConfirmMethod>().Create();
            var ecoliformConfirmMethod = GetEntityFactory<BacterialWaterSampleConfirmMethod>().Create();
            var hpcConfirmMethod = GetEntityFactory<BacterialWaterSampleConfirmMethod>().Create();
            var coliformSetupAnalyst = GetFactory<BacterialWaterSampleAnalystFactory>().Create();
            var coliformReadAnalyst = GetFactory<BacterialWaterSampleAnalystFactory>().Create();
            var hpcSetupAnalyst = GetFactory<BacterialWaterSampleAnalystFactory>().Create();
            var hpcReadAnalyst = GetFactory<BacterialWaterSampleAnalystFactory>().Create();
            var bacti1 = GetFactory<BacterialWaterSampleFactory>().Create(new {
                OperatingCenter = opc1,
                BacterialSampleType = bacterialSampleType,
                Location = "Some location",
                CollectedBy = user,
                SampleCollectionDTM = sampleCollectionDate,
                Ph = (decimal)1,
                Temperature = (decimal)2,
                Cl2Free = (decimal)3,
                Cl2Total = (decimal)4,
                Monochloramine = (decimal)5,
                FreeAmmonia = (decimal)6,
                Nitrite = (decimal)7,
                Nitrate = (decimal)8,
                Alkalinity = (decimal)9,
                Iron = (decimal)10,
                Manganese = (decimal)11,
                Turbidity = (decimal)12,
                Conductivity = (decimal)13,
                OrthophosphateAsP = (decimal)14,
                OrthophosphateAsPO4 = (decimal)15,
                SampleNumber = "ABC123",
                ReceivedByLabDTM = receivedByLabDTM,
                ColiformConfirmMethod = coliformConfirmMethod,
                ColiformSetupAnalyst = coliformSetupAnalyst,
                ColiformSetupDTM = coliformSetupDTM,
                ColiformReadAnalyst = coliformReadAnalyst,
                ColiformReadDTM = coliformReadDTM,
                FinalHPC = (decimal)16,
                HPCConfirmMethod = hpcConfirmMethod,
                HPCSetupAnalyst = hpcSetupAnalyst,
                HPCSetupDTM = hpcSetupDTM,
                HPCReadAnalyst = hpcReadAnalyst,
                HPCReadDTM = hpcReadDTM
            });
            bacti1.SampleSite.BactiSite = true;
            Session.Flush();

            var search = new SearchBacterialWaterSample();
            SetupTargetForExcel();

            var result = _target.Export(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                const string sheetName = "Samples";
                Assert.AreEqual(1, helper.GetRowCount(sheetName));

                // NOTE: This test does not include boolean fields. These require specific
                // formatting and are tested separately.

                // Display fields
                helper.AreEqual(bacti1.Id, sheetName, "Id");
                helper.AreEqual("QQ1 - Q Town", sheetName, "OperatingCenter");
                helper.AreEqual(bacti1.SampleSite.ToString(), sheetName, "SampleSite");
                helper.AreEqual("Some location", sheetName, "Location");
                helper.AreEqual(sampleCollectionDate, sheetName, "SampleCollectionDTM");
                helper.AreEqual("thisdude", sheetName, "CollectedBy");
                helper.AreEqual("This bacterial sample type", sheetName, "BacterialSampleType");

                // Editable fields
                helper.AreEqual((decimal)1, sheetName, "Ph");
                helper.AreEqual((decimal)2, sheetName, "Temperature");
                helper.AreEqual((decimal)3, sheetName, "Cl2Free");
                helper.AreEqual((decimal)4, sheetName, "Cl2Total");
                helper.AreEqual((decimal)5, sheetName, "Monochloramine");
                helper.AreEqual((decimal)6, sheetName, "FreeAmmonia");
                helper.AreEqual((decimal)7, sheetName, "Nitrite");
                helper.AreEqual((decimal)8, sheetName, "Nitrate");
                helper.AreEqual((decimal)9, sheetName, "Alkalinity");
                helper.AreEqual((decimal)10, sheetName, "Iron");
                helper.AreEqual((decimal)11, sheetName, "Manganese");
                helper.AreEqual((decimal)12, sheetName, "Turbidity");
                helper.AreEqual((decimal)13, sheetName, "Conductivity");
                helper.AreEqual((decimal)14, sheetName, "OrthophosphateAsP");
                helper.AreEqual((decimal)15, sheetName, "OrthophosphateAsPO4");
                helper.AreEqual("ABC123", sheetName, "SampleNumber");
                helper.AreEqual(receivedByLabDTM, sheetName, "ReceivedByLabDTM");
                helper.AreEqual(coliformConfirmMethod.Id, sheetName, "ColiformConfirmMethod");
                helper.AreEqual(coliformSetupAnalyst.Id, sheetName, "ColiformSetupAnalyst");
                helper.AreEqual(coliformSetupDTM, sheetName, "ColiformSetupDTM");
                helper.AreEqual(coliformReadAnalyst.Id, sheetName, "ColiformReadAnalyst");
                helper.AreEqual(coliformReadDTM, sheetName, "ColiformReadDTM");
                helper.AreEqual((decimal)16, sheetName, "FinalHPC");
                helper.AreEqual(hpcConfirmMethod.Id, sheetName, "HPCConfirmMethod");
                helper.AreEqual(hpcSetupAnalyst.Id, sheetName, "HPCSetupAnalyst");
                helper.AreEqual(hpcSetupDTM, sheetName, "HPCSetupDTM");
                helper.AreEqual(hpcReadAnalyst.Id, sheetName, "HPCReadAnalyst");
                helper.AreEqual(hpcReadDTM, sheetName, "HPCReadDTM");
            }
        }

        [TestMethod]
        public void TestExportDoesProperBooleanFormattingForColiformConfirm()
        {
            // This test exists because the export is still doing TRUE/FALSE and I want
            // this to fail immediately if they change their mind and want Present/Absent instead.
            var sampleSite = GetFactory<SampleSiteFactory>().Create(new{ BactiSite = true });
            var bactiTrueColiform = GetFactory<BacterialWaterSampleFactory>().Create( new{ ColiformConfirm = true, SampleSite = sampleSite });
            var bactiFalseColiform = GetFactory<BacterialWaterSampleFactory>().Create( new{ ColiformConfirm = false, SampleSite = sampleSite });

            var search = new SearchBacterialWaterSample();
            SetupTargetForExcel();

            var result = _target.Export(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                const string sheetName = "Samples";
                helper.AreEqual(true, sheetName, "ColiformConfirm", 0);
                helper.AreEqual(false, sheetName, "ColiformConfirm", 1);
            }
        }

        [TestMethod]
        public void TestExportDoesProperBooleanFormattingForIsSpreader()
        {
            // This test exists because the export is still doing TRUE/FALSE and I want
            // this to fail immediately if they change their mind and want Yes/No instead.
            var sampleSite = GetFactory<SampleSiteFactory>().Create(new { BactiSite = true });
            var bactiTrueSpreader = GetFactory<BacterialWaterSampleFactory>().Create(new { IsSpreader = true, SampleSite = sampleSite });
            var bactiFalseSpreader = GetFactory<BacterialWaterSampleFactory>().Create(new { IsSpreader = false, SampleSite = sampleSite });

            var search = new SearchBacterialWaterSample();
            SetupTargetForExcel();

            var result = _target.Export(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                const string sheetName = "Samples";
                helper.AreEqual(true, sheetName, "IsSpreader", 0);
                helper.AreEqual(false, sheetName, "IsSpreader", 1);
            }
        }

        [TestMethod]
        public void TestExportHandlesNullValuesWithoutExploding()
        {
            // While most BacterialWaterSamples should have some of these values set,
            // there are random records that don't have things set correctly. We want
            // to make sure that the export does not blow up if we happen to hit one
            // of these random nulls.
            var bacti1 = GetFactory<BacterialWaterSampleFactory>().Create();
            bacti1.OperatingCenter = null;
            bacti1.SampleCollectionDTM = null;
            bacti1.CollectedBy = null;
            bacti1.BacterialSampleType = null;
            bacti1.SampleSite.BactiSite = true;
            Session.Flush();

            var search = new SearchBacterialWaterSample();
            SetupTargetForExcel();

            var result = _target.Export(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                const string sheetName = "Samples";
                Assert.AreEqual(1, helper.GetRowCount(sheetName));

                // NOTE: This test does not include boolean fields. These require specific
                // formatting and are tested separately.

                // NOTE 2: This does not include Cl2Free and Cl2Total as they aren't nullable at all.

                // NOTE 3: SampleSite is nullable, but the search will not return any results 
                // without a SampleSite currently. -Ross 3/7/2018

                // Display fields
                helper.AreEqual(bacti1.Id, sheetName, "Id");
                helper.IsNull(sheetName, "OperatingCenter");
               // helper.IsNull(sheetName, "SampleSite");
                helper.IsNull(sheetName, "Location");
                helper.IsNull(sheetName, "SampleCollectionDTM");
                helper.IsNull(sheetName, "CollectedBy");
                helper.IsNull(sheetName, "BacterialSampleType");

                // Editable fields
                helper.IsNull(sheetName, "Ph");
                helper.IsNull(sheetName, "Temperature");
                helper.IsNull(sheetName, "Monochloramine");
                helper.IsNull(sheetName, "FreeAmmonia");
                helper.IsNull(sheetName, "Nitrite");
                helper.IsNull(sheetName, "Nitrate");
                helper.IsNull(sheetName, "Alkalinity");
                helper.IsNull(sheetName, "Iron");
                helper.IsNull(sheetName, "Manganese");
                helper.IsNull(sheetName, "Turbidity");
                helper.IsNull(sheetName, "Conductivity");
                helper.IsNull(sheetName, "OrthophosphateAsP");
                helper.IsNull(sheetName, "OrthophosphateAsPO4");
                helper.IsNull(sheetName, "SampleNumber");
                helper.IsNull(sheetName, "ReceivedByLabDTM");
                helper.IsNull(sheetName, "ColiformConfirmMethod");
                helper.IsNull(sheetName, "ColiformSetupAnalyst");
                helper.IsNull(sheetName, "ColiformSetupDTM");
                helper.IsNull(sheetName, "ColiformReadAnalyst");
                helper.IsNull(sheetName, "ColiformReadDTM");
                helper.IsNull(sheetName, "FinalHPC");
                helper.IsNull(sheetName, "HPCConfirmMethod");
                helper.IsNull(sheetName, "HPCSetupAnalyst");
                helper.IsNull(sheetName, "HPCSetupDTM");
                helper.IsNull(sheetName, "HPCReadAnalyst");
                helper.IsNull(sheetName, "HPCReadDTM");
            }
        }

        [TestMethod]
        public void TestExportIncludesSheetWithAllConfirmationMethods()
        {
            var confirm1 = GetEntityFactory<BacterialWaterSampleConfirmMethod>().Create(new { Description = "Description the first" });
            var confirm2 = GetEntityFactory<BacterialWaterSampleConfirmMethod>().Create(new { Description = "Description the second" });
            var search = new SearchBacterialWaterSample();
            SetupTargetForExcel();

            var result = _target.Export(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                const string sheetName = "Confirm Methods";
                helper.AreEqual("Description the first", sheetName, "Description", 0);
                helper.AreEqual(confirm1.Id, sheetName, "Id", 0);
                helper.AreEqual("Description the second", sheetName, "Description", 1);
                helper.AreEqual(confirm2.Id, sheetName, "Id", 1);
            }
        }

        [TestMethod]
        public void TestExportIncludesAnalystsOnlyForTheOperatingCentersThatAppearInTheResults()
        {
            // NOTE: Analysts are added if they are in an operating center that's included in the results, *not*
            //       because the operating center was searched for in the search model. 

            // Setup operating centers
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "ZZ1", OperatingCenterName = "Z Town" });
            var opc2 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "AA2", OperatingCenterName = "A Town" });
            var opcBad = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "QQ3" });

            // Setup employees
            var emp1 = GetFactory<EmployeeFactory>().Create(new { FirstName = "This", LastName = "One" });
            var emp2 = GetFactory<EmployeeFactory>().Create(new { FirstName = "Another", LastName = "One" });
            var emp3 = GetFactory<EmployeeFactory>().Create(new { FirstName = "B", LastName = "Name" });
            var emp4 = GetFactory<EmployeeFactory>().Create(new { FirstName = "Q", LastName = "Name" });
            var empBad = GetFactory<EmployeeFactory>().Create(new { FirstName = "RQ", LastName = "McPerson" });

            // Setup analysts
            var analyst1 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new { Employee = emp1 });
            analyst1.OperatingCenters.Add(opc1);
            Session.Save(analyst1);
            var analyst2 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new { Employee = emp2 });
            analyst2.OperatingCenters.Add(opc1);
            Session.Save(analyst2);
            var analyst3 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new { Employee = emp3 });
            analyst3.OperatingCenters.Add(opc2);
            Session.Save(analyst3);
            var analyst4 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new { Employee = emp4 });
            analyst4.OperatingCenters.Add(opc2);
            Session.Save(analyst4);
            var analystBad = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new { Employee = empBad });
            analystBad.OperatingCenters.Add(opcBad);
            Session.Save(analystBad);

            // Setup some samples for those operating centers
            var bacti1 = GetFactory<BacterialWaterSampleFactory>().Create(new { OperatingCenter = opc1 });
            var bacti2 = GetFactory<BacterialWaterSampleFactory>().Create(new { OperatingCenter = opc2 });

            // The search model is forcing the generated query to only search for samples where the
            // sample site's BactiSite == true. 
            bacti1.SampleSite.BactiSite = true;
            bacti2.SampleSite.BactiSite = true;

            // Flush to save all the changes made to the test data
            Session.Flush();

            var search = new SearchBacterialWaterSample();
            SetupTargetForExcel();

            var result = _target.Export(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                // NOTE: This should be an alphabetical order by Op Code, then by Employee name
                // | Id | Operating Center | Employee    |
                // |    | AA2              | B Name      |
                // |    | AA2              | Q Name      |
                // |    | ZZ1              | Another One |
                // |    | ZZ1              | This One    |

                const string sheetName = "Analysts";
                helper.AreEqual(analyst3.Id, sheetName, "Id", 0);
                helper.AreEqual("AA2 - A Town", sheetName, "OperatingCenter", 0);
                helper.AreEqual("B Name", sheetName, "Employee", 0);

                helper.AreEqual(analyst4.Id, sheetName, "Id", 1);
                helper.AreEqual("AA2 - A Town", sheetName, "OperatingCenter", 1);
                helper.AreEqual("Q Name", sheetName, "Employee", 1);

                helper.AreEqual(analyst2.Id, sheetName, "Id", 2);
                helper.AreEqual("ZZ1 - Z Town", sheetName, "OperatingCenter", 2);
                helper.AreEqual("Another One", sheetName, "Employee", 2);

                helper.AreEqual(analyst1.Id, sheetName, "Id", 3);
                helper.AreEqual("ZZ1 - Z Town", sheetName, "OperatingCenter", 3);
                helper.AreEqual("This One", sheetName, "Employee", 3);
            }
        }
        
        [TestMethod]
        public void TestExportOnlyIncludesActiveAnalysts()
        {
            // Setup operating centers
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "ZZ1", OperatingCenterName = "Z Town" });

            // Setup employees
            var emp1 = GetFactory<EmployeeFactory>().Create(new { FirstName = "This", LastName = "One" });
            var emp2 = GetFactory<EmployeeFactory>().Create(new { FirstName = "Another", LastName = "One" });

            // Setup analysts
            var analyst1 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new { Employee = emp1 });
            analyst1.OperatingCenters.Add(opc1);
            Session.Save(analyst1);
            var analyst2 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new { Employee = emp2 });
            analyst2.OperatingCenters.Add(opc1);
            analyst2.IsActive = false;
            Session.Save(analyst2);

            // Setup some samples for those operating centers
            var bacti1 = GetFactory<BacterialWaterSampleFactory>().Create(new { OperatingCenter = opc1 });

            // The search model is forcing the generated query to only search for samples where the
            // sample site's BactiSite == true. 
            bacti1.SampleSite.BactiSite = true;

            // Flush to save all the changes made to the test data
            Session.Flush();

            var search = new SearchBacterialWaterSample();
            SetupTargetForExcel();

            var result = _target.Export(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                const string sheetName = "Analysts";

                Assert.AreEqual(1, helper.GetRowCount(sheetName), "There should only be one row.");
                helper.AreEqual(analyst1.Id, sheetName, "Id", 0);
                helper.AreEqual("ZZ1 - Z Town", sheetName, "OperatingCenter", 0);
                helper.AreEqual("This One", sheetName, "Employee", 0);
            }
        }

        [TestMethod]
        public void TestExportIncludesAnalystsThatAreInMultipleOperatingCenters()
        {
            // Setup operating centers
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "ZZ1", OperatingCenterName = "Z Town" });
            var opc2 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "AA2", OperatingCenterName = "A Town" });

            // Setup employees
            var emp1 = GetFactory<EmployeeFactory>().Create(new { FirstName = "This", LastName = "One" });

            // Setup analysts
            var analyst1 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new { Employee = emp1 });
            analyst1.OperatingCenters.Add(opc1);
            analyst1.OperatingCenters.Add(opc2);
            Session.Save(analyst1);

            // Setup some samples for those operating centers
            var bacti1 = GetFactory<BacterialWaterSampleFactory>().Create(new { OperatingCenter = opc1 });
            var bacti2 = GetFactory<BacterialWaterSampleFactory>().Create(new { OperatingCenter = opc2 });

            // The search model is forcing the generated query to only search for samples where the
            // sample site's BactiSite == true. 
            bacti1.SampleSite.BactiSite = true;
            bacti2.SampleSite.BactiSite = true;

            // Flush to save all the changes made to the test data
            Session.Flush();

            var search = new SearchBacterialWaterSample();
            SetupTargetForExcel();

            var result = _target.Export(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                const string sheetName = "Analysts";

                Assert.AreEqual(2, helper.GetRowCount(sheetName), "There should only be two rows, one for each operating center.");
                helper.AreEqual(analyst1.Id, sheetName, "Id", 0);
                helper.AreEqual("AA2 - A Town", sheetName, "OperatingCenter", 0);
                helper.AreEqual("This One", sheetName, "Employee", 0);
                helper.AreEqual(analyst1.Id, sheetName, "Id", 1);
                helper.AreEqual("ZZ1 - Z Town", sheetName, "OperatingCenter", 1);
                helper.AreEqual("This One", sheetName, "Employee", 1);
            }
        }


        [TestMethod]
        public void TestExportDoesNotChokeOnExportingAnalystsWhenABacterialWaterSampleDoesNotHaveAnOperatingCenter()
        {
            // Setup operating centers
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "ZZ1", OperatingCenterName = "Z Town" });

            // Setup employees
            var emp1 = GetFactory<EmployeeFactory>().Create(new { FirstName = "This", LastName = "One" });

            // Setup analysts
            var analyst1 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new { Employee = emp1 });
            analyst1.OperatingCenters.Add(opc1);
            Session.Save(analyst1);

            // Setup some samples for those operating centers
            var bacti1 = GetFactory<BacterialWaterSampleFactory>().Create(new { OperatingCenter = opc1 });
            var bacti2 = GetFactory<BacterialWaterSampleFactory>().Create();
            bacti2.OperatingCenter = null;

            // The search model is forcing the generated query to only search for samples where the
            // sample site's BactiSite == true. 
            bacti1.SampleSite.BactiSite = true;
            bacti2.SampleSite.BactiSite = true;

            // Flush to save all the changes made to the test data
            Session.Flush();

            var search = new SearchBacterialWaterSample();
            SetupTargetForExcel();

            var result = _target.Export(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                const string sheetName = "Analysts";

                helper.AreEqual(analyst1.Id, sheetName, "Id", 0);
                helper.AreEqual("ZZ1 - Z Town", sheetName, "OperatingCenter", 0);
                helper.AreEqual("This One", sheetName, "Employee", 0);
            }
        }


        #endregion
    }
}
