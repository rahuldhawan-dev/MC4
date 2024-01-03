using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Contractors.Controllers;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Contractors.Controllers
{
    [TestClass]
    public class ContractorUserControllerTest : MapCallMvcControllerTestBase<ContractorUserController, ContractorUser>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateContractorUser)vm;
                model.Password = "some password that needs to be set for tests to not crash#ABC1234";
                model.PasswordAnswer = "same reason as above";
                model.ConfirmPassword = model.Password;
                model.Email = "some@email.com";
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ContractorsGeneral;
            Authorization.Assert((a) => {
                a.RequiresRole("~/Contractors/ContractorUser/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/ContractorUser/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/ContractorUser/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/ContractorUser/New/", module, RoleActions.Add);
                a.RequiresRole("~/Contractors/ContractorUser/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Contractors/ContractorUser/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Contractors/ContractorUser/Update/", module, RoleActions.Edit);
            });
        }
    }
}
