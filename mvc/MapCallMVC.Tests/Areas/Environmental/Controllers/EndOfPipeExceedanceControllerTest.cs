using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class EndOfPipeExceedanceControllerTest : MapCallMvcControllerTestBase<EndOfPipeExceedanceController, EndOfPipeExceedance>
    {
        #region Init/Cleanup

        // Anyone looking at this, if you have an entity that is being filtered by Role you need this for the tests to pass on ControllerBase
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.EnvironmentalGeneral;
            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/EndOfPipeExceedance/Show", role);
                a.RequiresRole("~/Environmental/EndOfPipeExceedance/Search", role);
                a.RequiresRole("~/Environmental/EndOfPipeExceedance/Index", role);
                a.RequiresRole("~/Environmental/EndOfPipeExceedance/New", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/EndOfPipeExceedance/Create", role, RoleActions.Add);
                a.RequiresRole("~/Environmental/EndOfPipeExceedance/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EndOfPipeExceedance/Update", role, RoleActions.Edit);
                a.RequiresRole("~/Environmental/EndOfPipeExceedance/Destroy", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<EndOfPipeExceedance>().Create(new {BriefDescription = "description 0"});
            var entity1 = GetEntityFactory<EndOfPipeExceedance>().Create(new {BriefDescription = "description 1"});
            var search = new SearchEndOfPipeExceedance();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.BriefDescription, "BriefDescription");
                helper.AreEqual(entity1.BriefDescription, "BriefDescription", 1);
            }
        }

        #endregion

        #region Edit/Update
        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var description = "Stuff and Junk";
            var eq = GetFactory<EndOfPipeExceedanceFactory>().Create();

            var result = _target.Update(
                _viewModelFactory.BuildWithOverrides<EditEndOfPipeExceedance, EndOfPipeExceedance>(eq, new {
                    BriefDescription = description
                }));

            Assert.AreEqual(description, Session.Get<EndOfPipeExceedance>(eq.Id).BriefDescription);
        }

        #endregion

        #endregion
    }
}
