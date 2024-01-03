using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class BillingPartyControllerTest : MapCallMvcControllerTestBase<BillingPartyController, BillingParty>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = BillingPartyController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/BillingParty/Search/", role);
                a.RequiresRole("~/FieldOperations/BillingParty/Show/", role);
                a.RequiresRole("~/FieldOperations/BillingParty/Index/", role);
                a.RequiresRole("~/FieldOperations/BillingParty/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/BillingParty/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/BillingParty/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/BillingParty/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/BillingParty/CreateBillingPartyContact", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/BillingParty/DestroyBillingPartyContact", role, RoleActions.Edit);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<BillingParty>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<BillingParty>().Create(new {Description = "description 1"});
            var search = new SearchBillingParty();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<BillingParty>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditBillingParty, BillingParty>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<BillingParty>(eq.Id).Description);
        }

        #endregion

        #region BillingPartyContact

        [TestMethod]
        public void TestCreateBillingPartyContactRequiresHttpPost()
        {
            MyAssert.MethodHasAttribute<HttpPostAttribute>(_target, "CreateBillingPartyContact", typeof(CreateBillingPartyContact));
        }

        [TestMethod]
        public void TestCreateBillingPartyContactAddsBillingPartyContactToBillingParty()
        {
            var billingParty = GetEntityFactory<BillingParty>().Create();
            var contact = GetEntityFactory<Contact>().Create();
            var contactType = GetEntityFactory<ContactType>().Create();

            var model = new CreateBillingPartyContact(_container) {
                Id = billingParty.Id,
                Contact = contact.Id,
                ContactType = contactType.Id
            };

            Assert.IsFalse(billingParty.BillingPartyContacts.Any());

            _target.CreateBillingPartyContact(model);

            Assert.IsNotNull(billingParty.BillingPartyContacts.Single(x => x.BillingParty == billingParty && x.ContactType == contactType && x.Contact == contact));
        }

        #endregion
    }
}
