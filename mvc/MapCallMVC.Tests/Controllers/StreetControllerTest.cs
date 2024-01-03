using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using MapCallMVC.Models.ViewModels.Streets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class StreetControllerTest : MapCallMvcControllerTestBase<StreetController, Street>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ITownRepository>().Use<TownRepository>();
            e.For<IStreetRepository>().Use<StreetRepository>();
        }
        
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateStreet)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.County = GetEntityFactory<County>().Create().Id;
            };
        }
		
        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Street/ByTownId/");
                a.RequiresLoggedInUserOnly("~/Street/GetActiveByTownId/");
                a.RequiresLoggedInUserOnly("~/Street/GetActiveByTownIdAndPartialStreetName/");
                a.RequiresLoggedInUserOnly("~/Street/GetByTownIdAndPartialStreetName/");
                a.RequiresRole("~/Street/Index/", StreetController.ROLE);
                a.RequiresRole("~/Street/Search/", StreetController.ROLE);
                a.RequiresRole("~/Street/Show/", StreetController.ROLE);
                a.RequiresRole("~/Street/New/", StreetController.ROLE, RoleActions.Add);
                a.RequiresRole("~/Street/Create/", StreetController.ROLE, RoleActions.Add);
                a.RequiresRole("~/Street/Edit/", StreetController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/Street/Update/", StreetController.ROLE, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/Street/Destroy");
            });
        }

        #region Ajaxie Actions

        [TestMethod]
        public void TestActiveByTownIdReturnsActiveStreetsByTown()
        {
            var town1 = GetFactory<TownFactory>().Create();
            var town2 = GetFactory<TownFactory>().Create();
            var street = GetFactory<StreetFactory>().Create(new { Town = town1 });
            var street2 = GetFactory<StreetFactory>().Create(new { Town = town2 });
            var street3 = GetFactory<StreetFactory>().Create(new { Town = town1, IsActive = true });
            
            var result = (CascadingActionResult)_target.GetActiveByTownId(town1.Id);

            var actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1);
        }

        [TestMethod]
        public void TestByTownIdReturnsStreetsByTownId()
        {
            var townA = GetEntityFactory<Town>().Create();
            var townB = GetEntityFactory<Town>().Create();
            var townC = GetEntityFactory<Town>().Create();

            var streetAInTownA = GetEntityFactory<Street>().Create(new { Town = townA });
            var streetBInTownA = GetEntityFactory<Street>().Create(new { Town = townA });
            var streetCInTownB = GetEntityFactory<Street>().Create(new { Town = townB });
            var streetDInTownC = GetEntityFactory<Street>().Create(new { Town = townC });

            var result = (CascadingActionResult)_target.ByTownId(townA.Id, townB.Id);

            var items = result.GetSelectListItems().ToList();

            Assert.AreEqual(3, items.Count - 1);
            Assert.AreEqual(1, items.Count(x => x.Value == streetAInTownA.Id.ToString()));
            Assert.AreEqual(1, items.Count(x => x.Value == streetBInTownA.Id.ToString()));
            Assert.AreEqual(1, items.Count(x => x.Value == streetCInTownB.Id.ToString()));
            Assert.AreEqual(0, items.Count(x => x.Value == streetDInTownC.Id.ToString()));
        }

        [TestMethod]
        public void TestGetActiveByTownIdAndPartialStreetNameReturnsWhen3OrMoreCharactersAreEntered()
        {
            var town = GetEntityFactory<Town>().Create();
            var town2 = GetEntityFactory<Town>().Create();
            var badTown = GetEntityFactory<Town>().Create();
            var north = GetEntityFactory<StreetPrefix>().Create(new { Description = "N" });
            var south = GetEntityFactory<StreetPrefix>().Create(new { Description = "S" });
            var east = GetEntityFactory<StreetPrefix>().Create(new { Description = "E" });
            var west = GetEntityFactory<StreetPrefix>().Create(new { Description = "W" });
            var avenue = GetEntityFactory<StreetSuffix>().Create(new { Description = "AVE" });
            var streetValidBecauseNameMatch = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "foo", Prefix = north, Suffix = avenue, IsActive = true });
            var streetValidBecauseNameMatch2 = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "food", Prefix = south, Suffix = avenue, IsActive = true });
            var streetValidBecauseNameMatch3 = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "snafoo", Prefix = east, Suffix = avenue, IsActive = true });
            var streetValidBecauseNameMatchInOtherSearchedTown = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "snafoo", Prefix = east, Suffix = avenue, IsActive = true });
            var streetNotValidBecauseNameMatchInTownThatIsNotSearchedFor = GetEntityFactory<Street>()
               .Create(new { Town = badTown, Name = "snafoo", Prefix = east, Suffix = avenue, IsActive = true });
            var streetNotValidBecauseNameDoesNotMatch = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "fotown", Prefix = west, Suffix = avenue, IsActive = true });
            var streetNotValidBecauseInactive = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "superfoo", Prefix = west, Suffix = avenue, IsActive = false });
            
            var result = (AutoCompleteResult)_target.GetActiveByTownIdAndPartialStreetName("foo", new[] { town.Id, town2.Id });
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(4, data.Count());
            Assert.IsTrue(data.Contains(streetValidBecauseNameMatch));
            Assert.IsTrue(data.Contains(streetValidBecauseNameMatch2));
            Assert.IsTrue(data.Contains(streetValidBecauseNameMatch3));
            Assert.IsTrue(data.Contains(streetValidBecauseNameMatchInOtherSearchedTown));
        }
        
        [TestMethod]
        public void TestGetActiveByTownIdAndPartialStreetNameReturnsWhen3OrMoreCharactersAreNotEntered()
        {
            var town = GetEntityFactory<Town>().Create();
            var validMatchingStreet = GetEntityFactory<Street>().Create(new { Town = town, Name = "foo", IsActive = true });
            var result = (AutoCompleteResult)_target.GetActiveByTownIdAndPartialStreetName("fo", new[] { town.Id });
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(0, data.Count());
        }

        [TestMethod]
        public void TestGetActiveByTownIdAndPartialStreetNameReturnsWhenNoCharactersAreEntered()
        {
            var town = GetEntityFactory<Town>().Create();
            var validMatchingStreet = GetEntityFactory<Street>().Create(new { Town = town, Name = "foo", IsActive = true });
            var result = (AutoCompleteResult)_target.GetActiveByTownIdAndPartialStreetName("", new[] { town.Id });
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(0, data.Count());
        }

        [TestMethod]
        public void TestGetByTownIdAndPartialStreetNameReturnsWhen3OrMoreCharactersAreEntered()
        {
            var town = GetEntityFactory<Town>().Create();
            var town2 = GetEntityFactory<Town>().Create();
            var badTown = GetEntityFactory<Town>().Create();
            var north = GetEntityFactory<StreetPrefix>().Create(new { Description = "N" });
            var south = GetEntityFactory<StreetPrefix>().Create(new { Description = "S" });
            var east = GetEntityFactory<StreetPrefix>().Create(new { Description = "E" });
            var west = GetEntityFactory<StreetPrefix>().Create(new { Description = "W" });
            var avenue = GetEntityFactory<StreetSuffix>().Create(new { Description = "AVE" });
            var streetValidBecauseNameMatch = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "foo", Prefix = north, Suffix = avenue, IsActive = true });
            var streetValidBecauseNameMatch2 = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "food", Prefix = south, Suffix = avenue, IsActive = true });
            var streetValidBecauseNameMatch3 = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "snafoo", Prefix = east, Suffix = avenue, IsActive = true });
            var streetValidBecauseNameMatchInOtherSearchedTown = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "snafoo", Prefix = east, Suffix = avenue, IsActive = true });
            var streetNotValidBecauseNameMatchInTownThatIsNotSearchedFor = GetEntityFactory<Street>()
               .Create(new { Town = badTown, Name = "snafoo", Prefix = east, Suffix = avenue, IsActive = true });
            var streetNotValidBecauseNameDoesNotMatch = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "fotown", Prefix = west, Suffix = avenue, IsActive = true });
            var streetValidEvenThoughItsInactive = GetEntityFactory<Street>()
               .Create(new { Town = town, Name = "superfoo", Prefix = west, Suffix = avenue, IsActive = false });
            
            var result = (AutoCompleteResult)_target.GetByTownIdAndPartialStreetName("foo", new[] { town.Id });
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(5, data.Count());
            Assert.IsTrue(data.Contains(streetValidBecauseNameMatch));
            Assert.IsTrue(data.Contains(streetValidBecauseNameMatch2));
            Assert.IsTrue(data.Contains(streetValidBecauseNameMatch3));
            Assert.IsTrue(data.Contains(streetValidBecauseNameMatchInOtherSearchedTown));
            Assert.IsTrue(data.Contains(streetValidEvenThoughItsInactive));
        }
        
        [TestMethod]
        public void TestGetByTownIdAndPartialStreetNameReturnsWhen3OrMoreCharactersAreNotEntered()
        {
            var town = GetEntityFactory<Town>().Create();
            var validMatchingStreet = GetEntityFactory<Street>().Create(new { Town = town, Name = "foo", IsActive = true });
            var result = (AutoCompleteResult)_target.GetByTownIdAndPartialStreetName("fo", new[] { town.Id });
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(0, data.Count());
        }

        [TestMethod]
        public void TestGetByTownIdAndPartialStreetNameReturnsWhenNoCharactersAreEntered()
        {
            var town = GetEntityFactory<Town>().Create();
            var validMatchingStreet = GetEntityFactory<Street>().Create(new { Town = town, Name = "foo", IsActive = true });
            var result = (AutoCompleteResult)_target.GetByTownIdAndPartialStreetName("", new[] { town.Id });
            var data = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(0, data.Count());
        }
        
        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Street>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<Street>().Create(new {Description = "description 1"});
            var search = new SearchStreet();
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
            var eq = GetEntityFactory<Street>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditStreet, Street>(eq, new {
                Name = expected
            }));

            Assert.AreEqual(expected, Session.Get<Street>(eq.Id).Name);
        }

        #endregion

        #endregion
    }
}