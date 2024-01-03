using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class InspectorSignOffSheetController : ControllerBaseWithPersistence<IServiceRepository, Service, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public InspectorSignOffSheetController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) { }
        public InspectorSignOffSheetController() : this(null) { }

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