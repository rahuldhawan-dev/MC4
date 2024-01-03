using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    [DisplayName("Auto/General Liability Claim")]
    public class GeneralLiabilityClaimController : ControllerBaseWithPersistence<IGeneralLiabilityClaimRepository, GeneralLiabilityClaim, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;
        public const string NOTIFICATION_PURPOSE = "General Liability Claim";

        #endregion

        #region Private Methods

        private void SendCreationsMostBodaciousNotification(GeneralLiabilityClaim model)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var args = new NotifierArgs
            {
                OperatingCenterId = model.OperatingCenter.Id,
                Module = ROLE,
                Purpose = NOTIFICATION_PURPOSE,
                Data = model
            };

            var pdfResult = new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", model);
            var pdf = pdfResult.RenderPdfToBytes(ControllerContext);
            args.AddAttachment(String.Format("GeneralLiabilityClaim{0}.pdf", model.Id), pdf);
            notifier.Notify(args);
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            Action allActions = () => {
                this.AddDropDownData<LiabilityType>();
                this.AddDropDownData<GeneralLiabilityClaimType>();
                this.AddDropDownData<ClaimsRepresentative>();
            };

            switch (action)
            {
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive);
                    allActions();
                    break;
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    allActions();
                    break;
                case ControllerAction.Search:
                    allActions();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchGeneralLiabilityClaim search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
                x.Map(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    return _container.With((IEnumerable<IThingWithCoordinate>)new[] {model}).GetInstance<MapResultWithCoordinates>();
                });
                x.Pdf(() => ActionHelper.DoPdf(id));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchGeneralLiabilityClaim search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));

                // TODO: Where is this used? This looks like something leftover that got moved to the API project. -Ross 12/17/2019
                formatter.Json(() => {
                    if (search.IncidentDateTime == null || search.IncidentDateTime.Operator != RangeOperator.Between ||
                        search.IncidentDateTime.End == null || search.IncidentDateTime.End.Value
                                                                     .Subtract(search.IncidentDateTime.Start.Value)
                                                                     .TotalDays > 30)
                    {
                        throw new InvalidOperationException(
                            "IncidentDateTime must be a 'between' search of a month or less.");
                    }
                    search.EnablePaging = false;
                    var results = Repository.Search(search);
                    return Json(new {
                        Data = results.Select(c => new {
                            c.Id,
                            OperatingCenter = c.OperatingCenter.ToString(),
                            CompanyContact = c.CompanyContact.ToString(),
                            ClaimsRepresentative = c.ClaimsRepresentative.ToString(),
                            c.ClaimNumber,
                            c.Name,
                            LiabilityType = c.LiabilityType.ToString(),

                            c.Description,
                            c.PhhContacted,
                            c.IncidentDateTime,
                            c.PoliceCalled,
                            c.IncidentNotificationDate,
                            c.FiveWhysCompleted,
                            c.DateSubmitted
                        })
                    }, JsonRequestBehavior.AllowGet);
                });
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateGeneralLiabilityClaim(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateGeneralLiabilityClaim model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    SendCreationsMostBodaciousNotification(entity);
                    return null; // defer to default
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditGeneralLiabilityClaim>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditGeneralLiabilityClaim model)
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

        public GeneralLiabilityClaimController(ControllerBaseWithPersistenceArguments<IGeneralLiabilityClaimRepository, GeneralLiabilityClaim, User> args) : base(args) {}
    }
}
