using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    public class EnvironmentalPermitController : ControllerBaseWithPersistence<IRepository<EnvironmentalPermit>, EnvironmentalPermit, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;
        public const string MUST_HAVE_REQUIREMENT_ERROR = "You cannot remove all the requirements when the permit has requirements.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.New:
                    this.AddDropDownData<EnvironmentalPermitRequirementType>("CreateEnvironmentalPermitRequirement.RequirementType");
                    this.AddDropDownData<EnvironmentalPermitRequirementValueUnit>("CreateEnvironmentalPermitRequirement.ValueUnit");
                    this.AddDropDownData<EnvironmentalPermitRequirementValueDefinition>("CreateEnvironmentalPermitRequirement.ValueDefinition");
                    this.AddDropDownData<EnvironmentalPermitRequirementTrackingFrequency>("CreateEnvironmentalPermitRequirement.TrackingFrequency");
                    this.AddDropDownData<EnvironmentalPermitRequirementReportingFrequency>("CreateEnvironmentalPermitRequirement.ReportingFrequency");
                    this.AddDropDownData<CommunicationType>("CreateEnvironmentalPermitRequirement.CommunicationType");
                    break;
                case ControllerAction.Edit:
                case ControllerAction.Show:
                    this.AddDropDownData<EnvironmentalPermitRequirementType>("RequirementType");
                    this.AddDropDownData<EnvironmentalPermitRequirementValueUnit>("ValueUnit");
                    this.AddDropDownData<EnvironmentalPermitRequirementValueDefinition>("ValueDefinition");
                    this.AddDropDownData<EnvironmentalPermitRequirementTrackingFrequency>("TrackingFrequency");
                    this.AddDropDownData<EnvironmentalPermitRequirementReportingFrequency>("ReportingFrequency");
                    this.AddDropDownData<CommunicationType>("CommunicationType");
                    this.AddDropDownData<EnvironmentalPermitFeePaymentMethod>("PaymentMethod");
                    this.AddDropDownData<EnvironmentalPermitFeeType>("EnvironmentalPermitFeeType");
                    this.AddDropDownData<RecurringFrequencyUnit>("PaymentDueFrequencyUnit");
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchEnvironmentalPermit search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, onModelFound: permit => {
                var operatingCenters = permit.OperatingCenters.Select(oc => oc.Id).ToArray();
                this.AddDynamicDropDownData<RepositoryBase<Equipment>, Equipment, EquipmentDisplayItem>(
                    e => e.Id,
                    e => e.Display,
                    filter: e =>
                        permit.Facilities.Any()
                            ? permit.Facilities.Contains(e.Facility)
                            : (permit.OperatingCenters.Any()
                                ? permit.OperatingCenters.Contains(e.OperatingCenter)
                                : permit.State == e.OperatingCenter.State)
                );
                this.AddDynamicDropDownData<RepositoryBase<Facility>,Facility, FacilityDisplayItem>(
                    f => f.Id,
                    f => f.DescriptionWithDepartment,
                    filter: f =>
                        (permit.OperatingCenters.Any()
                            ? operatingCenters.Contains(f.OperatingCenter.Id)
                            : permit.State == f.OperatingCenter.State)
                );
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEnvironmentalPermit search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() =>
                    ActionHelper.DoIndex(search,
                        new ActionHelperDoIndexArgs {RedirectSingleItemToShowView = false}));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search)
                                            .Select(x => new {
                                                 x.Id,
                                                 x.State,
                                                 x.EnvironmentalPermitType,
                                                 x.EnvironmentalPermitStatus,
                                                 x.ProgramInterestNumber,
                                                 Facilities = x.Facilities.ToString(),
                                                 Equipment = x.Equipment.ToString(),
                                                 x.PermitName,
                                                 x.PermitNumber,
                                                 PublicWaterSupply = x.PublicWaterSupply?.Description,
                                                 WasteWaterSystem = x.WasteWaterSystem?.Description,
                                                 x.PermitCrossReferenceNumber,
                                                 x.PermitEffectiveDate,
                                                 x.PermitExpirationDate,
                                                 x.PermitRenewalDate,
                                                 x.Description
                                             });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateEnvironmentalPermit(_container));
        }

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Add)]
        public ActionResult Create(CreateEnvironmentalPermit model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEnvironmentalPermit>(id);
        }

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult Update(EditEnvironmentalPermit model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region AddFacility

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult AddFacility(int id, int facilityId)
        {
            return ActionHelper.DoUpdate(
                ViewModelFactory.BuildWithOverrides<AddEnvironmentalPermitFacility, EnvironmentalPermit>(
                    Repository.Find(id), new {FacilityId = facilityId}));
        }

        #endregion

        #region RemoveFacility

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult RemoveFacility(int id, int facilityId)
        {
            return ActionHelper.DoUpdate(ViewModelFactory.BuildWithOverrides<RemoveEnvironmentalPermitFacility, EnvironmentalPermit>(Repository.Find(id), new {
                FacilityId = facilityId
            }));
        }

        #endregion

        #region AddEquipment

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult AddEquipment(int id, int equipmentId)
        {
            return ActionHelper.DoUpdate(
                ViewModelFactory.BuildWithOverrides<AddEnvironmentalPermitEquipment, EnvironmentalPermit>(Repository.Find(id),
                    new {EquipmentId = equipmentId}));
        }

        #endregion

        #region RemoveEquipment

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult RemoveEquipment(int id, int equipmentId)
        {
            return ActionHelper.DoUpdate(ViewModelFactory.BuildWithOverrides<RemoveEnvironmentalPermitEquipment, EnvironmentalPermit>(Repository.Find(id), new {
                EquipmentId = equipmentId
            }));
        }

        #endregion

        #region AddRequirement

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddEnvironmentalPermitRequirement(CreateEnvironmentalPermitRequirement model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemoveRequirement

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveEnvironmentalPermitRequirement(int id)
        {
            // TODO: Why isn't this done via viewmodel? -Ross 12/27/2019
            var repo = _container.GetInstance<IRepository<EnvironmentalPermitRequirement>>();
            var requirement = repo.Find(id);
            var permit = requirement.EnvironmentalPermit;
            if (permit.RequiresRequirements && permit.Requirements.Count == 1)
            {
                DisplayErrorMessage(MUST_HAVE_REQUIREMENT_ERROR);
            }
            else
            {
                permit.Requirements.Remove(requirement);
                repo.Delete(requirement);
                Repository.Save(permit);
            }

            return RedirectToAction("Show", new {id = permit.Id});
        }

        #endregion

        #region Add/Remove Fees

        [HttpPost, RequiresRole(RoleModules.EnvironmentalGeneral, RoleActions.Edit)]
        public ActionResult RemoveEnvironmentalPermitFee(RemoveEnvironmentalPermitFee model)
        {
            // TODO: Need a specific view model for this.
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => RedirectToAction("Show", new { model.Id }),
                OnError = () => RedirectToAction("Show", new { model.Id })
            });
        }

        #endregion

        public EnvironmentalPermitController(ControllerBaseWithPersistenceArguments<IRepository<EnvironmentalPermit>, EnvironmentalPermit, User> args) : base(args) {}
    }
}
