using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Controllers;
using System.Web.Mvc;
using MMSINC;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class IncidentInvestigationRootCauseLevel2TypeController : ControllerBaseWithPersistence<IncidentInvestigationRootCauseLevel2Type, User>
    {
        #region Constants

        public const RoleModules ROLE = IncidentInvestigationController.ROLE_MODULE;

        #endregion

        #region Constructor

        public IncidentInvestigationRootCauseLevel2TypeController(ControllerBaseWithPersistenceArguments<IRepository<IncidentInvestigationRootCauseLevel2Type>, IncidentInvestigationRootCauseLevel2Type, User> args) : base(args) { }

        #endregion

        #region Cascades

        [HttpGet]
        public CascadingActionResult ByLevel1(int id)
        {
            return new CascadingActionResult(Repository.Where(x => x.IncidentInvestigationRootCauseLevel1Type.Id == id), "Description", "Id");
        }

        #endregion
    }
}
