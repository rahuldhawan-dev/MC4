using System;
using System.ComponentModel;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.SewerMainCleanings;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    [DisplayName("Sewer Main Inspections / Cleaning")]
    public class SewerMainCleaningController : SapSyncronizedControllerBaseWithPersisence<ISewerMainCleaningRepository, SewerMainCleaning, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Private Methods

        protected override void UpdateEntityForSap(SewerMainCleaning entity)
        {
            var sapEquipment = _container.GetInstance<ISAPInspectionRepository>().Save(new SAPInspection(entity));
            if (sapEquipment.SAPNotificationNumber != null)
                entity.SAPNotificationNumber = sapEquipment.SAPNotificationNumber;
            entity.SAPErrorCode = sapEquipment.SAPErrorCode;
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            Action addAddEditLookups = () => {
                this.AddDropDownData<OpeningCondition>("Opening1Condition");
                this.AddDropDownData<OpeningCondition>("Opening2Condition");
                this.AddDropDownData<OpeningFrameAndCover>("Opening1FrameAndCover");
                this.AddDropDownData<OpeningFrameAndCover>("Opening2FrameAndCover");
            };
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
                    this.AddDropDownData<MainCondition>();
                    this.AddDropDownData<CauseOfBlockage>();
                    break;
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, "OperatingCenter", x => x.IsActive);
                    addAddEditLookups();
                    break;
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    addAddEditLookups();
                    break;
                case ControllerAction.Show:
                    this.AddDropDownData<SewerTerminationType>();
                    this.AddDropDownData<RecurringFrequencyUnit>("InspectionFrequencyUnit");
                    this.AddDropDownData<SewerOpening>("ConnectedOpening", x => x.GetAllSorted(y => y.OpeningSuffix), x => x.Id, x => x.OpeningNumber);
                    break;
            }
        }
        
        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchSewerMainCleaning search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, onModelFound: DisplaySapErrorIfApplicable);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSewerMainCleaning search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(c => new {
                        c.Id,
                        c.OperatingCenter,
                        c.Town,
                        c.Date,
                        c.CreatedBy,
                        c.InspectedDate,
                        c.InspectionType,
                        c.InspectionGrade,
                        c.Street,
                        c.CrossStreet,
                        c.CrossStreet2,
                        c.Opening1,
                        c.Opening2,
                        c.Opening2IsATerminus,
                        c.FootageOfMainInspected,
                        c.BlockageFound,
                        c.CauseOfBlockage,
                        c.Overflow,
                        c.SewerOverflow,
                        Notes = c.TableNotes,
                        c.Opening1Condition,
                        c.Opening1FrameAndCover,
                        c.Opening1Catchbasin,
                        c.OverflowOpening1,
                        c.Opening2Condition,
                        c.Opening2FrameAndCover,
                        c.Opening2Catchbasin,
                        c.OverflowOpening2,
                        c.GallonsOfWaterUsed,
                        c.HydrantUsed,
                        CreatedOn = c.CreatedAt,
                        c.SAPErrorCode,
                        c.SAPNotificationNumber,
                        c.NeedsToSync,
                        c.LastSyncedAt
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateSewerMainCleaning(_container));
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult NewFromSewerOpening(int id)
        {
            var sewerOpeningRepository = _container.GetInstance<IRepository<SewerOpening>>();
            var sewerOpeningmodel = sewerOpeningRepository.Find(id);

            if (sewerOpeningmodel == null)
            {
                return HttpNotFound($"Sewer Opening with #{id} could not be found");
            }

            var model = new CreateSewerMainCleaning(_container);
            model.SetValuesFromNewSewerOpening(sewerOpeningmodel);
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateSewerMainCleaning model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }
                        
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSewerMainCleaning>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditSewerMainCleaning model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    if (model.SendToSAP)
                    {
                        UpdateSAP(model.Id, ROLE);
                    }

                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }
		
        #endregion

        #region Delete

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id)
        {
            return ActionHelper.DoDestroy(id);
        }

        #endregion

        #region Constructors

        public SewerMainCleaningController(ControllerBaseWithPersistenceArguments<ISewerMainCleaningRepository, SewerMainCleaning, User> args) : base(args) {}

		#endregion
    }
}