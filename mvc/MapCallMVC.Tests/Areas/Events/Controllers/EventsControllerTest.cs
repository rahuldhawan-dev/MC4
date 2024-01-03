using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Events.Controllers;
using MapCallMVC.Areas.Events.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Areas.Events.Controllers
{
    [TestClass]
    public class EventControllerTest : MapCallMvcControllerTestBase<EventController, Event, IRepository<Event>>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetEntityFactory<Event>().Create(new {
                EventCategory = GetEntityFactory<EventCategory>().Create(),
                EventSubcategory = GetEntityFactory<EventSubcategory>().Create(),
            });
            options.InitializeSearchTester = (tester) => {
                // State and OperatingCenter are both readonly string properties on
                // the entity. The search tester blows up because it tries to create
                // a String entity for these. So they need to be made manually for the test.
               // tester.TestPropertyValues[nameof(SearchEvent.State)] = GetEntityFactory<State>().Create().Id;
                tester.TestPropertyValues[nameof(SearchEvent.OperatingCenter)] = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.EventsEvents;
                const string path = "~/Events/Event/";
                a.RequiresRole(path + "Search", role, RoleActions.Read);
                a.RequiresRole(path + "Show", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
                a.RequiresRole(path + "Destroy", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Event>().Create(new { RootCause= "Roots and rags" });
            var entity1 = GetEntityFactory<Event>().Create(new { RootCause = "Rags and roots" });
            var search = new SearchEvent();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.RootCause, "RootCause");
                helper.AreEqual(entity1.RootCause, "RootCause",1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<Event>().Create();
            var expected = "the main was clogged with shoes";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditEvent, Event>(eq, new {
                RootCause = expected
            }));

            Assert.AreEqual(expected, Session.Get<Event>(eq.Id).RootCause);
        }

        #endregion

        #region Lookup Data

        [TestMethod]
        public void TestSetLookUpDataForOperatingCenterSetsCorrectlyOnNew()
        {
            var opc1 = GetEntityFactory<OperatingCenter>().CreateList(5);
            var opc2 = GetEntityFactory<OperatingCenter>().Create(new { IsActive = true });
            GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Events }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.EventsEvents, Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Events }) }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Add }),
                User = _currentUser,
                OperatingCenter = opc2
            });

            _target.SetLookupData(ControllerAction.New);

            var opcs = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            Assert.AreNotEqual(opc1.Count, opcs.Count());
            Assert.AreEqual(1, opcs.Count());
            Assert.IsTrue(opc2.Id.ToString() == opcs.First().Value);
        }

        #endregion
    }
}
