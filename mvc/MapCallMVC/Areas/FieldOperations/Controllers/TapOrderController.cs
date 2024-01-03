using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class TapOrderController : ControllerBaseWithPersistence<IServiceRepository, Service, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public TapOrderController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}
        public TapOrderController() : this(null) { }

        #endregion

        #region Search/Index/Show
        
        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Pdf(() => ActionHelper.DoPdf(id, "Show"));
            });
        }
        
        #endregion
    }
}