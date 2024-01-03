using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Engineering.Controllers;
using System;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Engineering.Models.ViewModels.AwiaCompliance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Results;
using System.Linq;
using MapCall.Common.Model.Entities.Users;

namespace MapCallMVC.Tests.Areas.Engineering.Controllers
{
    [TestClass()]
    public class AwiaComplianceControllerTest : MapCallMvcControllerTestBase<AwiaComplianceController, AwiaCompliance>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                const string urlPathPart = "~/Engineering/AwiaCompliance";

                a.RequiresRole($"{urlPathPart}/Search/", RoleModules.EngineeringRiskRegister);
                a.RequiresRole($"{urlPathPart}/Show/", RoleModules.EngineeringRiskRegister);
                a.RequiresRole($"{urlPathPart}/Index/", RoleModules.EngineeringRiskRegister);
                a.RequiresRole($"{urlPathPart}/New/", RoleModules.EngineeringRiskRegister, RoleActions.Add);
                a.RequiresRole($"{urlPathPart}/Create/", RoleModules.EngineeringRiskRegister, RoleActions.Add);
                a.RequiresRole($"{urlPathPart}/Edit/", RoleModules.EngineeringRiskRegister, RoleActions.Edit);
                a.RequiresRole($"{urlPathPart}/Update/", RoleModules.EngineeringRiskRegister, RoleActions.Edit);
            });
        }

        #endregion

        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues.Add(nameof(SearchAwiaCompliance.PublicWaterSupplies), GetFactory<PublicWaterSupplyFactory>().Create().Id);
            };
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var date = DateTime.Now.AddDays(-1);

            var entity = GetEntityFactory<AwiaCompliance>().Create();

            _target.Update(_viewModelFactory.BuildWithOverrides<EditAwiaCompliance, AwiaCompliance>(entity, new {
                DateAccepted = date
            }));

            var updatedEntity = Session.Get<AwiaCompliance>(entity.Id);

            Assert.AreEqual(date, updatedEntity.DateAccepted);
        }

        #endregion

        #region Search

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var certificationType = GetEntityFactory<AwiaComplianceCertificationType>().Create();
            var awiaCompliance = GetEntityFactory<AwiaCompliance>().Create(new {
                CertificationType = certificationType
            });

            var searchViewModel = new SearchAwiaCompliance { CertificationType  = new []{ certificationType.Id }};

            var result = _target.Index(searchViewModel);

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, searchViewModel.Count);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcelForExportableProperties()
        {
            var pwsIds = GetEntityFactory<PublicWaterSupply>().CreateList(2);
            var entity0 = GetEntityFactory<AwiaCompliance>().Create(new {
                Id = 96,
                State = GetEntityFactory<State>().Create(),
                OperatingCenter = GetEntityFactory<OperatingCenter>().Create(),
                CertificationType = GetEntityFactory<AwiaComplianceCertificationType>().Create(),
                CreatedBy = GetEntityFactory<User>().Create(),
                CertifiedBy = GetEntityFactory<User>().Create(),
                DateSubmitted = DateTime.Now,
                DateAccepted = DateTime.Now,
                RecertificationDue = DateTime.Now
            });
            var note1 = new Note<AwiaCompliance> {
                Id = 1,
                Entity = entity0,
                LinkedId = entity0.Id,
                Note = GetEntityFactory<Note>().Create(),
                DataType = GetEntityFactory<DataType>().Create()
            };
            entity0.PublicWaterSupplies.AddRange(pwsIds);
            entity0.AwiaComplianceNotes.Add(note1);
            var search = new SearchAwiaCompliance();

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            var notes = string.Join(", ", entity0.AwiaComplianceNotes.Select(x => x.Note?.Text));
            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity0.State, "State");
                helper.AreEqual(entity0.OperatingCenter, "OperatingCenter");
                helper.AreEqual(entity0.PublicWaterSupplies.ToString(), "PWSID");
                helper.AreEqual(entity0.CertificationType, "CertificationType");
                helper.AreEqual(entity0.CreatedBy?.FullName, "EnteredBy");
                helper.AreEqual(entity0.CertifiedBy?.FullName, "CertifiedBy");
                helper.AreEqual(entity0.DateSubmitted, "DateSubmitted");
                helper.AreEqual(entity0.DateAccepted, "DateAccepted");
                helper.AreEqual(entity0.RecertificationDue, "RecertificationDue");
                helper.AreEqual(notes, "Notes");

                Assert.IsNotNull(helper.GetValue<string>("PWSID", 0));
                Assert.IsNotNull(helper.GetValue<string>("Notes", 0));
            }
        }

        #endregion
    }
}
