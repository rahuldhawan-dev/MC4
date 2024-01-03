using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    [DisplayName("Equipment Purposes")]
    public class EquipmentPurposeController : ControllerBaseWithPersistence<IRepository<EquipmentPurpose>, EquipmentPurpose, User>
    {
        #region Constants

        public const string NOT_FOUND = "Equipment Type with the id '{0}' was not found.";
        public const RoleModules ROLE = RoleModules.ProductionEquipment;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDynamicDropDownData<IEquipmentTypeRepository, EquipmentType, EquipmentTypeDisplayItem>(f => f.Id, f => f.Display, dataGetter: r => r.GetWithNoEquipmentPurpose());
                    this.AddDynamicDropDownData<EquipmentType, EquipmentTypeDisplayItem>(f => f.Id, f => f.Display, dataGetter: r => r.GetAllSorted(x => x.Description));
                    this.AddDropDownData<EquipmentCategory>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<EquipmentSubCategory>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<EquipmentLifespan>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchEquipmentPurpose search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEquipmentPurpose search)
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
            return ActionHelper.DoNew(new CreateEquipmentPurpose(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateEquipmentPurpose model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEquipmentPurpose>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEquipmentPurpose model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByEquipmentTypeId

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult ByEquipmentTypeId(int[] equipmentTypeId)
        {
            return new CascadingActionResult<EquipmentPurpose, EquipmentPurposeDisplayItem>(
                Repository.Where(t => t.EquipmentType != null && equipmentTypeId.Contains(t.EquipmentType.Id)), "Display",
                "Id");
        }

        #endregion

        public EquipmentPurposeController(ControllerBaseWithPersistenceArguments<IRepository<EquipmentPurpose>, EquipmentPurpose, User> args) : base(args) {}
    }
}
