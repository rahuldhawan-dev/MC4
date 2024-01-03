using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Events.Controllers;
using MapCallMVC.Areas.Events.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Areas.Events.Controllers
{
    [TestClass]
    public class EventDocumentControllerTest : MapCallMvcControllerTestBase<EventDocumentController, EventDocument, IRepository<EventDocument>>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetEntityFactory<EventDocument>().Create(new {
                OperatingCenter = GetEntityFactory<OperatingCenter>().Create(),
                Facility = GetEntityFactory<Facility>().Create(new{Id = 1, FacilityName = "Sawn"}),
                EventType = GetEntityFactory<EventType>().Create(new {Description = "stuff"}),
                Description = "stuff",
            });
            options.InitializeSearchTester = (tester) => {
                // State and OperatingCenter are both readonly string properties on
                // the entity. The search tester blows up because it tries to create
                // a String entity for these. So they need to be made manually for the test.
                // tester.TestPropertyValues[nameof(SearchEvent.State)] = GetEntityFactory<State>().Create().Id;
                tester.TestPropertyValues[nameof(SearchEventDocument.OperatingCenter)] = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }


        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.EventsEvents;
                const string path = "~/Events/EventDocument/";
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
     
        #endregion
    }
}
