using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Contractors.Controllers;
using MapCallMVC.Areas.Contractors.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Contractors.Controllers
{
    [TestClass]
    public class ContractorControllerTest : MapCallMvcControllerTestBase<ContractorController, Contractor>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.ContractorsGeneral;
            Authorization.Assert((a) => {
                a.RequiresRole("~/Contractors/Contractor/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/Contractor/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/Contractor/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Contractors/Contractor/New/", module, RoleActions.Add);
                a.RequiresRole("~/Contractors/Contractor/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Contractors/Contractor/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Contractors/Contractor/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Contractor/CreateContractorContact/", module, RoleActions.Edit);
                a.RequiresRole("~/Contractor/DestroyContractorContact/", module, RoleActions.Edit);
                a.RequiresRole("~/Contractors/Contractor/Destroy/", module, RoleActions.Delete);

                a.RequiresLoggedInUserOnly("~/Contractor/GetFrameworkContractorsByOperatingCenter/");
                a.RequiresLoggedInUserOnly("~/Contractor/ByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/Contractor/ActiveContractorsByOperatingCenterId/");
            });
        }

        #region ContractorContact

        [TestMethod]
        public void TestCreateContractorContactAddsContractorContactToContractor()
        {
            var contractor = GetEntityFactory<Contractor>().Create();
            var contact = GetEntityFactory<Contact>().Create();
            var contactType = GetEntityFactory<ContactType>().Create();

            var model = new CreateContractorContact(_container) {
                Id = contractor.Id,
                Contact = contact.Id,
                ContactType = contactType.Id
            };

            Assert.IsFalse(contractor.Contacts.Any());

            _target.CreateContractorContact(model);

            Assert.IsNotNull(contractor.Contacts.Single(x => x.Contractor == contractor && x.ContactType == contactType && x.Contact == contact));
        }

        #endregion
    }
}
