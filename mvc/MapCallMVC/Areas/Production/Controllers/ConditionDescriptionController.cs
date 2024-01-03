using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Controllers;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    [ActionHelperViewVirtualPathFormat("~/Areas/Production/Views/ConditionDescription/{0}.cshtml")]
    public class ConditionDescriptionController : EntityLookupControllerBase<IRepository<ConditionDescription>, ConditionDescription>
    {
        #region Private Members

        private readonly IRepository<ConditionDescription> _conditionDescriptionRepo;

        #endregion

        #region Exposed Methods

        public override RoleModules GetDynamicRoleModuleForAction(string action)
        {
            return RoleModules.ProductionDataAdministration;
        }

        #endregion

        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionDataAdministration;

        #endregion

        #region Constructor

        public ConditionDescriptionController(
            ControllerBaseWithPersistenceArguments<IRepository<ConditionDescription>, ConditionDescription, User> args,
            IRepository<ConditionDescription> conditionDescriptionRepo) : base(args)
        {
            _conditionDescriptionRepo = conditionDescriptionRepo;
        }
        #endregion

        #region Methods

        [HttpGet]
        [RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult GetByConditionTypeId(int conditionTypeId)
        {            
            return new CascadingActionResult(_conditionDescriptionRepo.Where(x => x.ConditionType.Id == conditionTypeId), "Description", "Id");
        }

        #endregion
    }
}