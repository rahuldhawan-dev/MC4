using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites;
using MapCall.Common.Utility.Notifications;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class SampleSiteController : ControllerBaseWithPersistence<ISampleSiteRepository, SampleSite, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        public const string 
            CHEMICAL_SAMPLE_SITE_CREATED_PURPOSE = "Chemical Sample Site Created",
            CHEMICAL_SAMPLE_SITE_STATUS_CHANGED_PURPOSE = "Chemical Sample Site Status Changed";

        #endregion

        #region Constructors

        public SampleSiteController(ControllerBaseWithPersistenceArguments<ISampleSiteRepository, SampleSite, User> args) : base(args) { }

        #endregion

        #region Private Methods

        private void SendSampleSiteNotification(SampleSite site, string purpose)
        {
            site.RecordUrl = GetUrlForModel(site, "Show", "SampleSite", "WaterQuality");

            if (site.Premise?.MostRecentService != null)
            {
                site.Premise.MostRecentService.Service.RecordUrl = GetUrlForModel(site.Premise.MostRecentService.Service, "Show", "Service", "FieldOperations");
            }

            this.SendNotification(site.OperatingCenter.Id, ROLE, purpose, site);
        }

        private void SendChemicalSampleSiteCreated(SampleSiteViewModel sampleSiteViewModel)
        {
            var sampleSiteEntity = Repository.Find(sampleSiteViewModel.Id);

            sampleSiteEntity.RecordUrl = GetUrlForModel(sampleSiteEntity, "Show", "SampleSite", "WaterQuality");

            var args = new NotifierArgs {
                OperatingCenterId = sampleSiteEntity.OperatingCenter.Id,
                Module = ROLE,
                Purpose = CHEMICAL_SAMPLE_SITE_CREATED_PURPOSE,
                Data = sampleSiteEntity,
                Address = AuthenticationService.CurrentUser.Email
            };

            _container.GetInstance<INotificationService>().Notify(args);
        }

        private void SendChemicalSampleSiteStatusChanged(SampleSiteViewModel sampleSiteViewModel)
        {
            var sampleSiteEntity = Repository.Find(sampleSiteViewModel.Id);

            sampleSiteEntity.RecordUrl = GetUrlForModel(sampleSiteEntity, "Show", "SampleSite", "WaterQuality");

            var args = new NotifierArgs {
                OperatingCenterId = sampleSiteEntity.OperatingCenter.Id,
                Module = ROLE,
                Purpose = CHEMICAL_SAMPLE_SITE_STATUS_CHANGED_PURPOSE,
                Data = sampleSiteEntity,
                Address = AuthenticationService.CurrentUser.Email
            };

            _container.GetInstance<INotificationService>().Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            this.AddDropDownData<SampleSiteValidationStatus>(x => x.GetAllSorted(y => y.Id));

            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                    var addressLocationTypeRep = _container.GetInstance<IRepository<SampleSiteAddressLocationType>>();
                    var addressLocationTypes = addressLocationTypeRep.GetAllSorted(x => x.Id);
                    this.AddDropDownData("SampleSiteAddressLocationType", addressLocationTypes, x => x.Id, x => x.Description);
                    break;

                case ControllerAction.Search:
                    this.AddDropDownData<ServiceMaterial>("CustomerSideMaterial");
                    this.AddDropDownData<ServiceMaterial>("CustomerPlumbingMaterial");
                    this.AddDropDownData<ServiceMaterial>();
                    this.AddDynamicDropDownData<SamplePlan, SamplePlanDisplayItem>();
                    break;
            }

            if (action != ControllerAction.Show)
            {
                this.AddDropDownData<SampleSiteAvailability>();
                this.AddDropDownData<SampleSiteStatus>("Status");
                this.AddDropDownData<ServiceMaterial>("CustomerPlumbingMaterial");
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchSampleSite>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id, null, onModelFound: (model) => {
                    this.AddDynamicDropDownData<SamplePlan, SamplePlanDisplayItem>(filter: y => y.PWSID == model.PublicWaterSupply);
                    this.AddDropDownData<SampleSiteBracketSiteLocationType>("LocationType");

                    // SampleSite.PublicWaterSupply is nullable. If it's null, a bracket site can't be added, so we're not
                    // gonna populate the dropdown here.
                    if (model.PublicWaterSupply != null)
                    {
                        var bracketSites = Repository.GetByPublicWaterSupply(model.PublicWaterSupply.Id).Items.OrderBy(y => y.LongDescription).ToList();

                        // Don't include the current SampleSite. Can't have a bracket site be itself.
                        // Also, these aren't entity references so we need to find it by id
                        var displayOnlyModel = bracketSites.SingleOrDefault(y => y.Id == model.Id);
                        bracketSites.Remove(displayOnlyModel);
                        this.AddDropDownData("BracketSite", bracketSites, y => y.Id, y => y.ToString());
                    }
                }));
                x.Fragment(() => ActionHelper.DoShow(id, new ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
                x.Map(() => {
                    var model = Repository.Find(id);
                    return model == null
                        ? (ActionResult)HttpNotFound()
                        : _container.With((IEnumerable<IThingWithCoordinate>)new[] { model })
                                    .GetInstance<MapResultWithCoordinates>();
                });
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSampleSite search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    RedirectSingleItemToShowView = true
                }));

                // Where is this fragment being used?
                formatter.Fragment(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    IsPartial = true,
                    ViewName = "_Index",
                    OnNoResults = () => PartialView("_NoResults")
                }));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateSampleSite(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateSampleSite model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.SendChemicalSiteCreatedNotificationOnSave)
                    {
                        SendChemicalSampleSiteCreated(model);
                    }

                    return null; // defer to default
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSampleSite>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditSampleSite model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.SendChemicalSiteStatusChangedNotificationOnSave)
                    {
                        SendChemicalSampleSiteStatusChanged(model);
                    }

                    return null;
                }
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddSamplePlan(AddSampleSiteSamplePlan model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveSamplePlan(RemoveSampleSiteSamplePlan model)
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

        #region Add/Remove BracketSites

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddBracketSite(AddSampleSiteBracketSite model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveBracketSite(RemoveSampleSiteBracketSite model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Cascading / Json Endpoints

        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            var sampleSites = Repository.Where(x => x.OperatingCenter.Id == operatingCenterId)
                                        .OrderBy(x => x.CommonSiteName);

            return new CascadingActionResult<SampleSite, SampleSiteDisplayItem>(sampleSites);
        }

        [HttpGet]
        public ActionResult ByOperatingCenterIds(int[] operatingCenterIds)
        {
            var sampleSites = Repository.Where(x => operatingCenterIds.Contains(x.OperatingCenter.Id))
                                        .OrderBy(x => x.CommonSiteName);

            return new CascadingActionResult<SampleSite, SampleSiteDisplayItem>(sampleSites);
        }

        [HttpGet]
        public ActionResult ActiveBactiSitesByOperatingCenterId(int operatingCenterId)
        {
            var sampleSites = Repository.Where(x => x.Status.Description == "Active" && 
                                                    x.BactiSite && 
                                                    x.OperatingCenter.Id == operatingCenterId)
                                        .OrderBy(x => x.CommonSiteName);

            return new CascadingActionResult<SampleSite, SampleSiteDisplayItem>(sampleSites);
        }

        [HttpGet]
        public ActionResult ByOperatingCenterIdWithMatrices(int operatingCenterId)
        {
            var sampleSites = Repository.Where(x => x.OperatingCenter.Id == operatingCenterId && 
                                                    x.SampleIdMatrices.Count > 0)
                                        .OrderBy(x => x.CommonSiteName);

            return new CascadingActionResult<SampleSite, SampleSiteDisplayItem>(sampleSites);
        }

        [HttpGet]
        public ActionResult BySampleMatrixId(int sampleMatrixId)
        {
            var sampleSites = Repository.Where(x => x.SampleIdMatrices.Any(y => y.Id == sampleMatrixId))
                                        .OrderBy(x => x.CommonSiteName);

            return new CascadingActionResult<SampleSite, SampleSiteDisplayItem>(sampleSites);
        }

        [HttpGet]
        public ActionResult GetPrimaryEligibleSampleSitesByStateId(int stateId)
        {
            var sampleSites = Repository.Where(x => SampleSiteLocationType.ELIGIBLE_FOR_PARENT_SITE.Contains(x.LocationType.Id) &&
                                                    x.State.Id == stateId)
                                        .OrderBy(x => x.CommonSiteName);

            return new CascadingActionResult<SampleSite, SampleSiteDisplayItem>(sampleSites);
        }

        [HttpGet]
        public ActionResult GetPrimarySampleSitesByStateId(int stateId)
        {
            var sampleSites = Repository.Where(x => x.LocationType.Id == SampleSiteLocationType.Indices.PRIMARY &&
                                                    x.State.Id == stateId)
                                        .OrderBy(x => x.CommonSiteName);

            return new CascadingActionResult<SampleSite, SampleSiteDisplayItem>(sampleSites);
        }

        [HttpGet]
        public ActionResult GetSampleSitesByPremiseNumber(string premiseNumber)
        {
            var sampleSites = Repository.GetByPremiseNumber(premiseNumber)
                                        .Select(x => new {
                                             x.Id,
                                             x.CommonSiteName,
                                             OperatingCenter = x.OperatingCenter.Description,
                                             PublicWaterSupply = x.PublicWaterSupply.Description
                                         })
                                        .OrderBy(x => x.CommonSiteName);

            return Json(sampleSites, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
