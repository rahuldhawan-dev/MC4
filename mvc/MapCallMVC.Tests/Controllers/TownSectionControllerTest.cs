using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TownSectionControllerTest : MapCallMvcControllerTestBase<TownSectionController, TownSection>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateTownSection)vm;
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
                a.RequiresLoggedInUserOnly("~/TownSection/ActiveByTownId");
                a.RequiresLoggedInUserOnly("~/TownSection/ByTownId");
                a.RequiresSiteAdminUser("~/TownSection/Search");
                a.RequiresSiteAdminUser("~/TownSection/Show");
                a.RequiresSiteAdminUser("~/TownSection/Index");
                a.RequiresSiteAdminUser("~/TownSection/New");
                a.RequiresSiteAdminUser("~/TownSection/Create");
                a.RequiresSiteAdminUser("~/TownSection/Edit");
                a.RequiresSiteAdminUser("~/TownSection/Update");
            });
        }

        #region Ajaxie Actions

        [TestMethod]
        public void TestByTownId()
        {
            var townA = GetEntityFactory<Town>().Create();
            var townB = GetEntityFactory<Town>().Create();
            var townC = GetEntityFactory<Town>().Create();

            var townSectionAInTownA = GetEntityFactory<TownSection>().Create(new { Town = townA });
            var townSectionBInTownA = GetEntityFactory<TownSection>().Create(new { Town = townA });
            var townSectionCInTownB = GetEntityFactory<TownSection>().Create(new { Town = townB });
            var townSectionDInTownC = GetEntityFactory<TownSection>().Create(new { Town = townC });

            var result = (CascadingActionResult)_target.ByTownId(townA.Id, townB.Id);

            var items = result.GetSelectListItems().ToList();

            Assert.AreEqual(3, items.Count - 1);
            Assert.AreEqual(1, items.Count(x => x.Value == townSectionAInTownA.Id.ToString()));
            Assert.AreEqual(1, items.Count(x => x.Value == townSectionBInTownA.Id.ToString()));
            Assert.AreEqual(1, items.Count(x => x.Value == townSectionCInTownB.Id.ToString()));
            Assert.AreEqual(0, items.Count(x => x.Value == townSectionDInTownC.Id.ToString()));
        }

        [TestMethod]
        public void TestByActiveTownId()
        {
            var townA = GetEntityFactory<Town>().Create();
            var townB = GetEntityFactory<Town>().Create();
            var townC = GetEntityFactory<Town>().Create();

            var townSectionAInTownA = GetEntityFactory<TownSection>().Create(new { Town = townA, Active = true });
            var townSectionDInTownC = GetEntityFactory<TownSection>().Create(new { Town = townC, Active = true });

            GetEntityFactory<TownSection>().Create(new { Town = townA, Active = false });
            GetEntityFactory<TownSection>().Create(new { Town = townB, Active = false });
            
            var result = (CascadingActionResult)_target.ActiveByTownId(townA.Id, townC.Id);

            var items = result.GetSelectListItems().ToList();

            Assert.AreEqual(2, items.Count - 1);
            Assert.AreEqual(1, items.Count(x => x.Value == townSectionAInTownA.Id.ToString()));
            Assert.AreEqual(1, items.Count(x => x.Value == townSectionDInTownC.Id.ToString()));
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<TownSection>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTownSection, TownSection>(eq, new {
                Name = expected
            }));

            Assert.AreEqual(expected, Session.Get<TownSection>(eq.Id).Description);
        }

        #endregion

        #endregion
    }
}
