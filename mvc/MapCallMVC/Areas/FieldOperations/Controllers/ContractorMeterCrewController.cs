using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class ContractorMeterCrewController : ControllerBaseWithPersistence<ContractorMeterCrew, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesMeterChangeOuts;

        #endregion

        #region Constructor

        public ContractorMeterCrewController(ControllerBaseWithPersistenceArguments<IRepository<ContractorMeterCrew>, ContractorMeterCrew, User> args) : base(args) { }

        #endregion

        #region Actions

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchContractorMeterCrew());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchContractorMeterCrew search)
        {
            return ActionHelper.DoIndex(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateContractorMeterCrew(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateContractorMeterCrew model)
        {
            return ActionHelper.DoCreate(model);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditContractorMeterCrew>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditContractorMeterCrew model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

    }
}