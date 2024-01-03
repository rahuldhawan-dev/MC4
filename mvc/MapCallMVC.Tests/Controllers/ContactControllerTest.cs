using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class ContactControllerTest : MapCallMvcControllerTestBase<ContactController, Contact, ContactRepository>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresLoggedInUserOnly("~/Contact/Show/");
                a.RequiresLoggedInUserOnly("~/Contact/Index/");
                a.RequiresLoggedInUserOnly("~/Contact/Search/");
                a.RequiresLoggedInUserOnly("~/Contact/ByPartialNameMatch/");

                a.RequiresRole("~/Contact/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Contact/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Contact/New/", module, RoleActions.Add);
                a.RequiresRole("~/Contact/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Contact/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Destroy

        [TestMethod]
        public void TestDestroyAddsErrorMessageIfContactCanNotBeDeleted()
        {
            var contact = GetFactory<ContactFactory>().Create();
            GetFactory<TownContactFactory>().Create(new { Contact = contact });

            var result = _target.Destroy(contact.Id);
            MvcAssert.RedirectsToRoute(result, "Contact", "Show", new{ id = contact.Id });
        }

        #endregion

        #region Searching

        [TestMethod]
        public void TestSearchAddsStateRecordsToViewData()
        {
            GetFactory<StateFactory>().Create();
            var expectedStates = Session.Query<State>().ToArray();
            Assert.AreNotEqual(0, expectedStates.Count(), "How can you have a test without any states?");
            Assert.IsNull(_target.ViewData["State"]);
            _target.Search(null);

            var result = (IEnumerable<SelectListItem>)_target.ViewData["State"];

            Assert.AreEqual(expectedStates.Count(), result.Count());

            foreach (var s in expectedStates)
            {
                Assert.IsTrue(result.Any(x => x.Value == s.Id.ToString()));
            }
        }

        #endregion

        #region ByPartialNameMatch

        [TestMethod]
        public void TestByPartialNameMatchReturnsJsonResultWithJsonResults()
        {
            var contact = GetFactory<ContactFactory>().Create();
            var noMatch = GetFactory<ContactFactory>().Create(new { FirstName = "Alf" });

            var result = (AutoCompleteResult)_target.ByPartialNameMatch(contact.FirstName);
            var model = (IEnumerable<dynamic>)result.Data;
            Assert.AreSame(contact, model.Single());
        }

        #endregion

        #endregion
    }
}
