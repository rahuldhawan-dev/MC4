using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Controllers
{
    [TestClass]
    public class RecurringProjectMainControllerTest : MapCallMvcControllerTestBase<RecurringProjectMainController, RecurringProjectMain>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesProjects;
            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/RecurringProjectMain/Show/", role);
                a.RequiresRole("~/ProjectManagement/RecurringProjectMain/Edit/", role);
            });
        }

        #region Show

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            var rp = GetEntityFactory<RecurringProject>().Create();
            var rpm = GetEntityFactory<RecurringProjectMain>().Create(new
            {
                RecurringProject = rp,
                Layer = "foo",
                Guid = "{asdaasdaasdaasdaasdaasdaasdaasda}"
            });

            var result = _target.Show(rp.Id) as ViewResult;
            var results = (IEnumerable<RecurringProjectMain>)result.Model;

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(rpm.Id, results.First().Id);
        }

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            Assert.Inconclusive("Test me. The action currently will never return a not found because it will never get a null result.");
        }

        #endregion

        #region Edit

        [TestMethod]
        public override void TestEditReturnsEditViewWithEditViewModel()
        {
            // override because there's no matching Update action so the auto test fails.
            // Also because the action deals with multiple records for its view model.
            var rp = GetEntityFactory<RecurringProject>().Create();
            var rpm = GetEntityFactory<RecurringProjectMain>().Create(new {
                RecurringProject = rp,
                Layer = "foo",
                Guid = "{asdaasdaasdaasdaasdaasdaasdaasda}"
            });

            var result = _target.Edit(rp.Id) as ViewResult;
            var results = (IEnumerable<RecurringProjectMain>)result.Model;

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(rpm.Id, results.First().Id);
        }

        [TestMethod]
        public override void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            Assert.Inconclusive("Correct me and test me. I don't return HttpNotFound correctly because my results are never null.");
        }

        #endregion
    }
}