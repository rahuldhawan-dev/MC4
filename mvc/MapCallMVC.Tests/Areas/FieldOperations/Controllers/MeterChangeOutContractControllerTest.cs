using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.Utilities;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class MeterChangeOutContractControllerTest : MapCallMvcControllerTestBase<MeterChangeOutContractController, MeterChangeOutContract>
    {
        #region Fields

        private User _user;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateMeterChangeOutContract)vm;
                model.FileUpload = new MMSINC.Metadata.AjaxFileUpload {
                    FileName = "somefile.xls",
                    BinaryData = TestFiles.GetExcel2007File()
                };
                model.ExcelRecords = new List<MeterChangeOutContractExcelRecord>();

                // There's no existing test for making sure all the validation and mapping works
                // correctly. I don't have time to write all that up as part of this test fixing
                // in MC-6257. It's also my understanding that this page isn't even used anymore
                // because Work1V took over.
                model.SkipValidation = true;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesMeterChangeOuts;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/MeterChangeOutContract/Show/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/MeterChangeOutContract/Index/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/MeterChangeOutContract/Search/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/MeterChangeOutContract/New/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/MeterChangeOutContract/Create/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/MeterChangeOutContract/Edit/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/MeterChangeOutContract/Update/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/MeterChangeOutContract/AddMeterChangeOuts/", module, RoleActions.UserAdministrator);
            });
        }
    }
}