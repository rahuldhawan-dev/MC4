using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Operations.Controllers
{
    public class AtRiskBehaviorSubSectionController : ControllerBaseWithPersistence<AtRiskBehaviorSubSection, User>
    {
        #region Consts

        private const RoleModules ROLE_MODULE = RoleModules.OperationsIncidents;

        #endregion

        #region Consructor
        
        public AtRiskBehaviorSubSectionController(ControllerBaseWithPersistenceArguments<IRepository<AtRiskBehaviorSubSection>, AtRiskBehaviorSubSection, User> args) : base(args) {}
        
        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(ROLE_MODULE)]
        public ActionResult ByAtRiskBehaviorSectionId(int id)
        {
            var result = Repository.Where(x => x.Section.Id == id);
            return new CascadingActionResult(result, "Description", "Id");
        }

        #endregion
    }
}