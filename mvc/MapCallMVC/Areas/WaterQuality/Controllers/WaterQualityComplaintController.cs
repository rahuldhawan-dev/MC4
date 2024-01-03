using System.Linq;
using System.Text;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class WaterQualityComplaintController : ControllerBaseWithPersistence<IWaterQualityComplaintRepository, WaterQualityComplaint, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.WaterQualityGeneral;

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<WaterQualityComplaintCustomerSatisfaction>("CustomerSatisfaction");
                    this.AddDropDownData<WaterQualityComplaintType>("Type");
                    break;

                case ControllerAction.Show:
                    this.AddDropDownData<WaterConstituent>();
                    break;
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchWaterQualityComplaint>();
        }

        private SAPNotificationCollection GetSAPNotification(string sapNotificationNumber)
        {
            var sapSearchModel = new SAPNotification {
                CreateWorkOrderNotificationNumber = sapNotificationNumber
            };

            // NOTE: Using SearchWorkOrder is the only way to actually search for a specific
            //       notification number. Using sapRepo.Search() ignores the SAPNotificationNumber
            //        for some reason and returns more results than necessary. -Ross 12/21/2017

            var sapRepo = _container.GetInstance<ISAPNotificationRepository>();

            // SearchWorkOrder should always be returing a single result, even if there
            // isn't a valid result. The first result will include the SAPErrorCode if 
            // there's an error/no results. 

            var result = sapRepo.SearchWorkOrder(sapSearchModel);

            // NOTE: We need to mock out the result for regression test purposes, but we can keep
            //       the actual SAP call in here to make sure that isn't throwing an error.
#if DEBUG
            if (MMSINC.MvcApplication.IsInTestMode)
            {
                if (MMSINC.MvcApplication.RegressionTestFlags.Contains("show valid sap notification"))
                {
                    result.Items.Clear();
                    result.Items.Add(new SAPNotification {
                        SAPErrorCode = "Success"
                    });
                }
                else if (MMSINC.MvcApplication.RegressionTestFlags.Contains("show not found sap notification"))
                {
                    result.Items.Clear();
                    result.Items.Add(new SAPNotification {
                        SAPErrorCode = "No Records found in SAP for given selection"
                    });
                }
            }
#endif 
            return result;
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => {
                    return ActionHelper.DoShow(id, null, onModelFound: (model) => {
                        // Not every record has an OperatingCenter and not every record has san OrcomOrderNumber/SAPNotificationNumber set.
                        if (model.OperatingCenter != null && model.OperatingCenter.SAPEnabled && !string.IsNullOrWhiteSpace(model.OrcomOrderNumber))
                        {
                            var sapResult = GetSAPNotification(model.OrcomOrderNumber);
                            var sapNotification = sapResult.Single();

                            switch (sapResult.Result)
                            {
                                case SAPNotificationCollectionResult.NoResults:
                                    // We want to display that the complaint doesn't have an SAP record, but it's
                                    // not necessarily an error because we won't get a result back if they complete
                                    // or cancel the SAP notification.
                                    DisplayNotification(sapNotification.SAPErrorCode);
                                    break;

                                case SAPNotificationCollectionResult.Success:
                                    ViewData["SAPNotification"] = sapNotification;

                                    var sb = new StringBuilder();
                                    foreach (var note in model.ComplaintNotes.OrderBy(x => x.Note.CreatedAt))
                                    {
                                        sb.AppendLine($"{note.Note.CreatedAt} - {note.Note.CreatedBy} : {note.Note.Text}");
                                    }

                                    ViewData["ReadOnlyNotes"] = sb.ToString();
                                    break;

                                default: // For Error and any unsupported enum values that may get added.
                                    DisplayErrorMessage(sapNotification.SAPErrorCode);
                                    break;
                            }
                        }

                        if (long.TryParse(model.OrcomOrderNumber, out var sapNotificationNumber))
                        {
                            var workOrder = _container.GetInstance<IGeneralWorkOrderRepository>()
                                                      .Where(x => x.SAPNotificationNumber == sapNotificationNumber)
                                                      .FirstOrDefault();

                            if (workOrder != null)
                            {
                                ViewData[WaterQualityComplaint.DisplayNames.WORK_ORDER_ID] = workOrder.Id;
                            }
                        }
                    });
                });
                formatter.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    ViewName = "_ShowPopup",
                    IsPartial = true 
                }));
            });
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWaterQualityComplaint search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(x => new {
                        x.Id,
                        x.OrcomOrderType,
                        x.PublicWaterSupply,
                        x.Type,
                        x.State,
                        x.Town,
                        x.TownSection,
                        x.Coordinate,
                        x.ProblemArea,
                        x.Source,
                        x.ProbableCause,
                        x.ActionTaken,
                        x.CustomerExpectation,
                        x.CustomerSatisfaction,
                        x.RootCause,
                        x.MainSize,
                        x.OperatingCenter,
                        x.InitialLocalResponseType,
                        x.InitialLocalContact,
                        x.OrcomOrderNumber,
                        x.DateComplaintReceived,
                        x.InitialLocalResponseDate,
                        ComplaintNotes = string.Join("\r\n", x.ComplaintNotes.Select(n => n.Note.Text)),
                        x.NotificationCreatedBy,
                        x.NotificationCompletionDate,
                        x.NotificationCompletedBy,
                        x.CustomerName,
                        x.HomePhoneNumber,
                        x.Ext,
                        x.StreetNumber,
                        x.StreetName,
                        x.ApartmentNumber,
                        x.ZipCode,
                        x.PremiseNumber,
                        x.ServiceNumber,
                        x.AccountNumber,
                        x.ComplaintDescription,
                        x.ComplaintStartDate,
                        x.SiteVisitRequired,
                        x.SiteVisitBy,
                        x.SiteComments,
                        x.WaterFilterOnComplaintSource,
                        x.CrossConnectionDetected,
                        x.MaterialOfService,
                        x.CustomerAnticipatedFollowupDate,
                        x.ActualCustomerFollowupDate,
                        x.CustomerSatisfactionFollowupLetter,
                        x.CustomerSatisfactionFollowupCall,
                        x.CustomerSatisfactionFollowupComments,
                        x.RootCauseIdentified,
                        x.FollowUpPostCardSent,
                        x.IsClosed,
                        x.Imported,
                        x.HasCoordinate,
                        x.TableName
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
            return ActionHelper.DoNew(new CreateWaterQualityComplaint(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateWaterQualityComplaint model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditWaterQualityComplaint>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditWaterQualityComplaint model)
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

        public WaterQualityComplaintController(ControllerBaseWithPersistenceArguments<IWaterQualityComplaintRepository, WaterQualityComplaint, User> args) : base(args) { }

        #endregion

        #region Sample Results

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddWaterQualityComplaintSampleResult(AddWaterQualityComplaintSampleResult model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region RemoveWaterQualityComplaintWaterQualityComplaintSampleResult

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult RemoveWaterQualityComplaintSampleResult(RemoveWaterQualityComplaintSampleResult model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion
    }
}