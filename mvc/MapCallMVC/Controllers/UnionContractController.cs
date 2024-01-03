using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class UnionContractController : ControllerBaseWithPersistence<IUnionContractRepository, UnionContract, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesUnion;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<Local>(l => l.Id, l => l.Name);
                    this.AddDropDownData<Union>(x => x.Id, x => x.BargainingUnit);
                    goto case ControllerAction.Edit;
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    break;
            }
        }

        [HttpGet, RequiresRole(ROLE)]
        public CascadingActionResult ByOperatingCenterId(int? opCenter)
        {
            return
                new CascadingActionResult<UnionContract, UnionContractDisplayItem>(
                    Repository.Where(c => c.OperatingCenter.Id == opCenter)
                        .OrderBy(c => c.OperatingCenter)
                        .ThenBy(c => c.Local), "Display", "Id");
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchUnionContract search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchUnionContract search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateUnionContract(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateUnionContract model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditUnionContract>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditUnionContract model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public UnionContractController(ControllerBaseWithPersistenceArguments<IUnionContractRepository, UnionContract, User> args) : base(args) {}
    }
}