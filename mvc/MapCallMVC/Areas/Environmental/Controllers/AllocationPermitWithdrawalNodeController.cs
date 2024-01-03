using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    [DisplayName("Allocation Withdrawal Nodes")]
    public class AllocationPermitWithdrawalNodeController : ControllerBaseWithPersistence<IRepository<AllocationPermitWithdrawalNode>, AllocationPermitWithdrawalNode, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<AllocationPermit>(ap => ap.GetAllSorted(x => x.Id), ap => ap.Id, ap => ap.Id);
                    this.AddDropDownData<AllocationCategory>(ac => ac.GetAllSorted(x => x.Description), ac => ac.Id, ac => ac.Description);
                    break;

                case ControllerAction.Edit:
                case ControllerAction.New:
                    this.AddDynamicDropDownData<Facility, FacilityDisplayItem>();
                    this.AddDropDownData<AllocationPermit>(ap => ap.GetAllSorted(x => x.Id), ap => ap.Id, ap => ap.Id);
                    this.AddDropDownData<AllocationCategory>(ac => ac.GetAllSorted(x => x.Description), ac => ac.Id, ac => ac.Description);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchAllocationPermitWithdrawalNode search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo((f) => {
                f.View(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound() as ActionResult;
                    var operatingCenter = model.Facility?.OperatingCenter;
                    this.AddDropDownData<IRepository<AllocationPermit>, AllocationPermitDisplay>(
                        "AllocationPermit",
                        ap => ap.Where(y => y.OperatingCenter == operatingCenter).Select(y =>
                            new AllocationPermitDisplay {
                                Id = y.Id,
                                OperatingCenter =
                                    y.OperatingCenter == null ? null : y.OperatingCenter.OperatingCenterCode,
                                PermitType = y.PermitType.ToString()
                            }), ap => ap.Id, ap => ap.Display);
                    if (model.Facility != null)
                        this.AddDynamicDropDownData<IEquipmentRepository, Equipment, EquipmentDisplayItem>(
                            e => e.Id, e => e.Display,
                            dataGetter: e => e.GetByFacilityId(model.Facility.Id).OrderBy(z => z.Description));
                    return ActionHelper.DoShow(id);
                });
                f.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true,
                    NotFound = string.Empty // Why do we show an empty string for this?
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchAllocationPermitWithdrawalNode search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateAllocationPermitWithdrawalNode(_container));
        }

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Add)]
        public ActionResult Create(CreateAllocationPermitWithdrawalNode model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditAllocationPermitWithdrawalNode>(id);
        }

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult Update(EditAllocationPermitWithdrawalNode model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddAllocationPermit(AddAllocationPermitAllocationPermitWithdrawalNode model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveAllocationPermit(RemoveAllocationPermitAllocationPermitWithdrawalNode model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #region AddEquipment

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult AddEquipment(AddAllocationPermitWithdrawalNodeEquipment model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemoveEquipment

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult RemoveEquipment(RemoveAllocationPermitWithdrawalNodeEquipment model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        public AllocationPermitWithdrawalNodeController(ControllerBaseWithPersistenceArguments<IRepository<AllocationPermitWithdrawalNode>, AllocationPermitWithdrawalNode, User> args) : base(args) {}
    }
}