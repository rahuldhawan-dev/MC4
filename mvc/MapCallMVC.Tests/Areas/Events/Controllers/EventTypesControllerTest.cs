using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Events.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Tests.Areas.Events.Controllers
{
    [TestClass]
    public class EventTypeControllerTest : MapCallMvcControllerTestBase<EventTypeController, EventType, IRepository<EventType>>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetEntityFactory<EventType>().Create(new {
                CreatedBy = "Gus Persons",
                Description = "With teeth"
            });
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.EventsEvents;
                const string path = "~/Events/EventType/";
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
