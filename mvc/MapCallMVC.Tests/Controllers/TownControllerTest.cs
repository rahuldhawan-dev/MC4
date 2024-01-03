using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using StructureMap;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Results;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TownControllerTest : MapCallMvcControllerTestBase<TownController, Town>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
            e.For<IFireDistrictRepository>().Use<FireDistrictRepository>();
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IContactRepository>().Use<ContactRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // Factory Create method has additional logic that doesn't exist for Build
            // for whatever reason.
            options.CreateValidEntity = () => GetEntityFactory<Town>().Create();
            options.IndexRedirectsToShowForSingleResult = true;
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                a.RequiresRole("~/Town/Edit/", RoleModules.GeneralTowns, RoleActions.Edit);
                a.RequiresRole("~/Town/Update/", RoleModules.GeneralTowns, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/Town/New/");
                a.RequiresSiteAdminUser("~/Town/Create/");
                a.RequiresRole("~/Town/CreateTownContact/", RoleModules.GeneralTowns, RoleActions.Edit);
                a.RequiresRole("~/Town/DestroyTownContact/", RoleModules.GeneralTowns, RoleActions.Edit);
                a.RequiresRole("~/Town/AddPublicWaterSupply/", RoleModules.GeneralTowns, RoleActions.Edit);
                a.RequiresRole("~/Town/RemovePublicWaterSupply/", RoleModules.GeneralTowns, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/Town/AddWasteWaterSystem/");
                a.RequiresSiteAdminUser("~/Town/RemoveWasteWaterSystem/");
                a.RequiresSiteAdminUser("~/Town/AddGradient/");
                a.RequiresSiteAdminUser("~/Town/RemoveGradient/");

                a.RequiresLoggedInUserOnly("~/Town/Index/");
                a.RequiresLoggedInUserOnly("~/Town/Search/");
                a.RequiresLoggedInUserOnly("~/Town/Show/");
                a.RequiresLoggedInUserOnly("~/Town/GetState/");
                a.RequiresLoggedInUserOnly("~/Town/GetTownCriticalMainBreakNotes/");
         
                // Requiring a role/site admin will blow up cascading dropdowns.
                a.RequiresLoggedInUserOnly("~/Town/ByStateId/");
                a.RequiresLoggedInUserOnly("~/Town/ByStateIdWithCounty/");
                a.RequiresLoggedInUserOnly("~/Town/ByCountyId/");
                a.RequiresLoggedInUserOnly("~/Town/ByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/Town/ByOperatingCenterIds/");
                a.RequiresLoggedInUserOnly("~/Town/WithFacilitiesByStateId/");
                a.RequiresLoggedInUserOnly("~/Town/ByOperatingCenterId/");
            });
        }

        #endregion

        #region ByCountyId(int)

        [TestMethod]
        public void TestByCountyIdReturnsTownsForCounty()
        {
            var invalidTown = GetFactory<TownFactory>().Build();
            var county = GetFactory<CountyFactory>().Create();
            var towns = GetFactory<TownFactory>().CreateList(3, new { County = county });

            var result = (CascadingActionResult)_target.ByCountyId(county.Id);

            var actual = result.GetSelectListItems();

            Assert.AreEqual(towns.Count(), actual.Count() - 1); // -1 accounts for the select here
        }

        #endregion

        #region ByOperatingCenterId

        [TestMethod]
        public void TestByOperatingCenterIdReturnsTownsForOperatingCenter()
        {
            var opcntr1 = GetFactory<OperatingCenterFactory>().Create();
            var opcntr2 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4" });
            var validTowns = GetFactory<TownFactory>().CreateList(2);
            var invalid = GetFactory<TownFactory>().Create();
            validTowns[0].OperatingCentersTowns.Add(new OperatingCenterTown{OperatingCenter=opcntr1,Town = validTowns[0],Abbreviation="XX"});
            validTowns[1].OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opcntr1, Town = validTowns[1], Abbreviation = "XX" });
            invalid.OperatingCentersTowns.Add(new OperatingCenterTown {OperatingCenter = opcntr2, Town=invalid, Abbreviation = "XX"});
            
            Session.Flush();

            var result = (CascadingActionResult)_target.ByOperatingCenterId(opcntr1.Id);

            var actual = result.GetSelectListItems();

            Assert.AreEqual(validTowns.Count(), actual.Count() - 1); // --select here--
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(invalid.Id.ToString(), selectListItem.Value);
            }
        }

        #endregion

        #region Show(int)

        [TestMethod]
        public void TestShowSetsUpAListOfOperatingCentersAsDropDownData()
        {
            var expected = GetFactory<OperatingCenterFactory>().CreateList(3);
            var town = GetFactory<TownFactory>().Create();

            _target.Show(town.Id);

            _target.AssertHasDropDownData(expected, oc => oc.Id, oc => oc.ToString());
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcels()
        {
            var t1 = GetEntityFactory<Town>().Create();
            var t2 = GetEntityFactory<Town>().Create();
            var search = new SearchTown();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(t1.Id, "Id");
                helper.AreEqual(t2.Id, "Id", 1);
                helper.AreEqual(t1.ShortName, "ShortName");
                helper.AreEqual(t2.ShortName, "ShortName", 1);
            }
        }

        #endregion

        #region Edit(int, EditTown)

        [TestMethod]
        public void TestEditSetsUpAListOfStatesAsDropDownData()
        {
            var expected = GetFactory<StateFactory>().CreateList(3);
            var town = GetFactory<TownFactory>().Create();

            _target.Edit(town.Id);

            _target.AssertHasDropDownData(expected, s => s.Id, s => s.Name);
        }

        #endregion

        #region AddPublicWaterSupply

        [TestMethod]
        public void TestAddPublicWaterSupplyAddsPublicWaterSupplyToTown()
        {
            var town = GetFactory<TownFactory>().Create();
            var pws = GetFactory<PublicWaterSupplyFactory>().Create();

            MyAssert.CausesIncrease(
                () => _target.AddPublicWaterSupply(new AddPublicWaterSupplyTown(_container) { Id = town.Id, PublicWaterSupplyId = pws.Id }),
                () => Session.Get<Town>(town.Id).PublicWaterSupplies.Count());
        }

        [TestMethod]
        public void TestRemovePublicWaterSupplyRemovesPublicWaterSupply()
        {
            var town = GetFactory<TownFactory>().Create();
            var pws = GetFactory<PublicWaterSupplyFactory>().Create();
            town.PublicWaterSupplies.Add(pws);
            Session.Save(town);

            MyAssert.CausesDecrease(
                () => _target.RemovePublicWaterSupply(new RemovePublicWaterSupplyTown(_container) { Id = town.Id, PublicWaterSupplyId = pws.Id}),
                () => Session.Get<Town>(town.Id).PublicWaterSupplies.Count());
        }

        #endregion

        #region Update(EditTown)

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var town = GetFactory<TownFactory>().Create();

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTown, Town>(town, new {
                FullName = "FunkyTown"
            }));

            Assert.AreEqual("FunkyTown", Session.Get<Town>(town.Id).FullName);
        }

        #endregion

        #region CreateTownContact

        [TestMethod]
        public void TestCreateTownContactRequiresHttpPost()
        {
            MyAssert.MethodHasAttribute<HttpPostAttribute>(_target, "CreateTownContact", typeof(int), typeof(CreateTownContact));
        }

        [TestMethod]
        public void TestCreateTownContactAddsTownContactToTownsTownContacts()
        {
            var town = GetFactory<TownFactory>().Create();
            var contact = GetFactory<ContactFactory>().Create();
            var contactType = GetFactory<ContactTypeFactory>().Create();

            var model = new CreateTownContact
           (_container) {
                Id = town.Id,
                Contact = contact.Id,
                ContactType = contactType.Id
            };

            Assert.IsFalse(town.TownContacts.Any());

            _target.CreateTownContact(town.Id, model);

            Assert.IsNotNull(town.TownContacts.Single(x => x.Town == town && x.ContactType == contactType && x.Contact == contact));
        }

        #endregion

        #region DestroyTownContact

        [TestMethod]
        public void TestDestroyTownContactRequiresHttpDelete()
        {
            MyAssert.MethodHasAttribute<HttpDeleteAttribute>(_target, "DestroyTownContact", typeof(int), typeof(DestroyTownContact));
        }

        [TestMethod]
        public void TestDestroyTownContactDestroysTheTownContact()
        {
            var existingTownContact = GetFactory<TownContactFactory>().Create();
            var town = existingTownContact.Town;
            var model = new DestroyTownContact(_container)
            {
                Id = existingTownContact.Town.Id,
                TownContactId = existingTownContact.Id
            };

            // This test fails if you don't flush before saving for some reason.
            Session.Flush();
            Assert.IsTrue(town.TownContacts.Any());

            _target.DestroyTownContact(model.Id, model);

            Session.Flush();
            Session.Evict(town);
            town = Session.Query<Town>().Single(x => x.Id == town.Id);

            Assert.IsFalse(town.TownContacts.Any());
            Assert.IsNull(Session.Query<TownContact>().SingleOrDefault(x => x.Id == existingTownContact.Id));
        }

        #endregion
    }
}