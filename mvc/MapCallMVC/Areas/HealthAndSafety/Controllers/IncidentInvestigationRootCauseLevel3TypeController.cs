using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class IncidentInvestigationRootCauseLevel3TypeController : ControllerBaseWithPersistence<IncidentInvestigationRootCauseLevel3Type, User>
    {
        #region Constants

        public const RoleModules ROLE = IncidentInvestigationController.ROLE_MODULE;

        #endregion

        #region Constructor

        public IncidentInvestigationRootCauseLevel3TypeController(ControllerBaseWithPersistenceArguments<IRepository<IncidentInvestigationRootCauseLevel3Type>, IncidentInvestigationRootCauseLevel3Type, User> args) : base(args) { }

        #endregion

        #region Cascades

        [HttpGet]
        public CascadingActionResult ByLevel2(int id)
        {
            return new CascadingActionResult(Repository.Where(x => x.IncidentInvestigationRootCauseLevel2Type.Id == id), "Description", "Id");
        }

        #endregion
    }
}
