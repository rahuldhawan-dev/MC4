using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCall.Common.Validation;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class BacterialWaterSampleController : ControllerBaseWithPersistence<IBacterialWaterSampleRepository, BacterialWaterSample, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;
        public const string NOTIFICATION_PURPOSE = "Bacti Input Trigger";
        public const decimal FREE_AMMONIA_THRESHOLD = 0.2m,
                             NITRITE_THRESHOLD = 0.05m,
                             CL2_TOTAL_THRESHOLD = 0.2m,
                             CL2_FREE_THRESHOLD  = 0.2m,
                             PH_THRESHOLD = 6.5m;

        #endregion

        #region Private Methods


        /// <summary>
        /// 
        /// 
        /// 
        /// IF YOU EDIT THIS, PLEASE UPDATE THE _ACTIONBARHELP.cshtml
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="entity"></param>
        internal static void SendCreationsMostBodaciousNotification(IContainer container, BacterialWaterSample entity)
        {
            // NOTE: This is called from BacterialWaterSampleMassEditController, too!
            
            // MC-547: Total and Free Chlorine reporting is based on PWSID.
            // Total Chlorine should be sent if there is no SampleSite or PWSID selected
            // Free chlorine only goes out if the property is true.

            var shouldSendTotalChlorine = (entity.SampleSite?.PublicWaterSupply == null ||
                                           entity.SampleSite.PublicWaterSupply.TotalChlorineReported);
            var shouldSendFreeChlorine = (entity.SampleSite?.PublicWaterSupply?.FreeChlorineReported).GetValueOrDefault();

            bool sendNotification =
                (entity.Cl2Total < CL2_TOTAL_THRESHOLD && shouldSendTotalChlorine)
                || entity.Cl2Free < CL2_FREE_THRESHOLD && shouldSendFreeChlorine
                || entity.Nitrite > NITRITE_THRESHOLD
                || entity.FreeAmmonia > FREE_AMMONIA_THRESHOLD
                || entity.Ph < PH_THRESHOLD
                || entity.ColiformConfirm == true
                || entity.EColiConfirm == true;

            if (!entity.ComplianceSample)
            {
                sendNotification = false;
            }

            if (sendNotification)
            {
                var notifier = container.GetInstance<INotificationService>();
                var args = new NotifierArgs
                {
                    OperatingCenterId = (entity.OperatingCenter != null) ? entity.OperatingCenter.Id : 0,
                    Module = RoleModules.WaterQualityGeneral,
                    Purpose = NOTIFICATION_PURPOSE,
                    Data = entity
                };
                notifier.Notify(args);
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            var towns = _container.GetInstance<ITownRepository>().GetAllSorted();

            Action addGenericLookups = () =>
            {
                this.AddOperatingCenterDropDownData();
                // this.AddDropDownData<NonSheenColonyCountOperator>();
                //  this.AddDropDownData<SheenColonyCountOperator>();
                //  this.AddDropDownData<BacterialSampleType>();
                this.AddDropDownData<Town>(towns, x => x.Id, x => x.ToString());
                // this.AddDropDownData<Town>("SampleTown", towns, x => x.Id, x => x.ToString());
                this.AddDynamicDropDownData<EstimatingProject, EstimatingProjectDisplayItem>(x => x.Id, x => x.Display);
            };
            switch (action)
            {
                case ControllerAction.Edit:
                    addGenericLookups();
                    break;

                case ControllerAction.New:
                    addGenericLookups();
                    break;

                case ControllerAction.Search:
                    // Do not call addGenericLookups here. Only the Town dropdown data is used here. The rest aren't
                    // dropdowns for search and can slow things down.
                    this.AddDropDownData<Town>(towns, x => x.Id, x => x.ToString());

                    this.AddDropDownData<SampleSiteStatus>();
                    // bug 3987 asked for this specifically for searching.
                    // WO0000000195491: wants this ordered by PWSID 
                    var pwsids = _container.GetInstance<IPublicWaterSupplyRepository>().GetAllFilteredByWaterQualityGeneralRole().OrderBy(x => x.Identifier);
                    this.AddDropDownData(pwsids, x => x.Id, x => x.Description);

                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchBacterialWaterSample());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x =>
            {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
                x.Map(() =>
                {
                    var model = Repository.Find(id);
                    return model == null
                        ? (ActionResult)HttpNotFound()
                        : new MapResultWithCoordinates(AuthenticationService, _container.GetInstance<IIconSetRepository>(),
                            new[] {model});
                });
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchBacterialWaterSample search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(int? id)
        {
            return ActionHelper.DoNew(new CreateBacterialWaterSample(_container) { SampleSite = id });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateBacterialWaterSample model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendCreationsMostBodaciousNotification(_container, Repository.Find(model.Id));
                    return null;
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditBacterialWaterSample>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditBacterialWaterSample model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    SendCreationsMostBodaciousNotification(_container, Repository.Find(model.Id));
                    return null;
                }
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region RecentByPwsid

        [HttpGet, RequiresRole(ROLE)] // TODO: This should be renamed. The id parameter is not PWSID, it's a BacterialWaterSample Id.
        public ActionResult RecentByPwsid(int id)
        {
            return PartialView("_IndexRecent", Repository.GetRecentByPWSIDOfBacterialWaterSample(id));
        }

        #endregion

        #region GetBySampleSiteIdWithBracketSites

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult GetBySampleSiteIdWithBracketSites(int sampleSiteId)
        {
            return new CascadingActionResult(Repository.GetBySampleSiteIdWithBracketSites(sampleSiteId)) { SortItemsByTextField = false, TextField = "Id", ValueField = "Id" };
        }

        #endregion

        #region Inline Editing

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult InlineEdit(int id)
        {
            this.AddDropDownData<BacterialSampleType>();
            this.AddDropDownData<BacterialWaterSampleConfirmMethod>("ColiformConfirmMethod");
            this.AddDropDownData<BacterialWaterSampleConfirmMethod>("HPCConfirmMethod");

            return ActionHelper.DoEdit(id, new MMSINC.Utilities.ActionHelperDoEditArgs<BacterialWaterSample, InlineEditBacterialWaterSample> {
                ViewName = "_InlineEdit",
                IsPartial = true,
                SkipLookupData = true
            });
        }

        [HttpPost, RequiresSecureForm(false), RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult InlineUpdate(InlineEditBacterialWaterSample model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnErrorView = "_InlineEdit",
                OnSuccess = () => { return InlineShow(model.Id); }
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult InlineShow(int id)
        {
            return ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                ViewName = "_InlineShow",
                IsPartial = true
            });
        }

        #endregion

        #region Validation

        // NOTE: The parameter names MUST be the same as the model properties they're coming from or else they won't modelbind.
        [HttpGet, NoCache, RequiresRole(ROLE)] 
        public ActionResult ValidateTotalChlorine(decimal? cl2Total, int? sampleSite)
        {
            // This action exists only for client-side validation for the main Create/Edit screens of BacterialWaterSamples.
            var result = _container.GetInstance<BacterialWaterSampleValidationHelper>().ValidateTotalChlorineForSampleSite(cl2Total, sampleSite);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // NOTE: The parameter names MUST be the same as the model properties they're coming from or else they won't modelbind.
        [HttpGet, NoCache, RequiresRole(ROLE)] 
        public ActionResult ValidateFreeChlorine(decimal? cl2Free, int? sampleSite)
        {
            // This action exists only for client-side validation for the main Create/Edit screens of BacterialWaterSamples.
            var result = _container.GetInstance<BacterialWaterSampleValidationHelper>().ValidateFreeChlorineForSampleSite(cl2Free, sampleSite);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public BacterialWaterSampleController(ControllerBaseWithPersistenceArguments<IBacterialWaterSampleRepository, BacterialWaterSample, User> args) : base(args) { }
    }
}