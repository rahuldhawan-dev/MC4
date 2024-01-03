using System.ComponentModel;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCallMVC.ClassExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    [DisplayName("Facility Inspections")]
    public class ApcInspectionItemController : ControllerBaseWithPersistence<ApcInspectionItem, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Show:
                    this.AddDropDownData<ApcInspectionItemType>("Type");
                    break;

                case ControllerAction.New:
                    this.AddOperatingCenterDropDownData(x => x.IsActive);
                    break;
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchApcInspectionItem search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchApcInspectionItem search)
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
            return ActionHelper.DoNew(new CreateApcInspectionItem(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateApcInspectionItem model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnError = () => {
                    model.SetDefaults();
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditApcInspectionItem>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditApcInspectionItem model)
        {
            return ActionHelper.DoUpdate(model);
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Constructors

        public ApcInspectionItemController(ControllerBaseWithPersistenceArguments<IRepository<ApcInspectionItem>, ApcInspectionItem, User> args) : base(args) {}

        #endregion
    }
}