using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class PremiseTypeController : ControllerBaseWithPersistence<IRepository<PremiseType>, PremiseType, User>
    {
        #region Constants

        public const string NOT_FOUND = "Premise Type with the id '{0}' was not found.";

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups)]
        public ActionResult Index(SearchPremiseType search)
        {
            return ActionHelper.DoIndex(search);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreatePremiseType(_container));
        }

        [HttpPost, RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Add)]
        public ActionResult Create(CreatePremiseType model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditPremiseType>(id);
        }

        [HttpPost, RequiresRole(RoleModules.FieldServicesDataLookups, RoleActions.Edit)]
        public ActionResult Update(EditPremiseType model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public PremiseTypeController(ControllerBaseWithPersistenceArguments<IRepository<PremiseType>, PremiseType, User> args) : base(args) {}
    }
}
