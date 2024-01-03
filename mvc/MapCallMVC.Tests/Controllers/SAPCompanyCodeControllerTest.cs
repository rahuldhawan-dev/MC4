using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class SAPCompanyCodeControllerTest : MapCallMvcControllerTestBase<SAPCompanyCodeController, SAPCompanyCode>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IRepository<SAPCompanyCode>>(Repository);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Needed because controller inherits from EntityLookupControllerBase
            options.ExpectedEditViewName = "~/Views/EntityLookup/Edit.cshtml";
            options.ExpectedNewViewName = "~/Views/EntityLookup/New.cshtml";
            options.ExpectedShowViewName = "~/Views/EntityLookup/Show.cshtml";
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresSiteAdminUser("~/SAPCompanyCode/Index");
                a.RequiresSiteAdminUser("~/SAPCompanyCode/Show");
                a.RequiresSiteAdminUser("~/SAPCompanyCode/New");
                a.RequiresSiteAdminUser("~/SAPCompanyCode/Create");
                a.RequiresSiteAdminUser("~/SAPCompanyCode/Edit");
                a.RequiresSiteAdminUser("~/SAPCompanyCode/Update");
            });
        }
        #endregion
    }
}
