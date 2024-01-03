using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class WaterLossManagementController : ControllerBaseWithPersistence<WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;
        public const string START_ERROR = "The results below are not inclusive of the entire first month as the Start Date searched is after the first of the month.",
                            END_ERROR = "The results below are not inclusive of the entire last month as the End date searched is not the last day of the month.", 
                            EXACT_ERROR = "Results are only being shown for a specific date, not an entire month.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData("OperatingCenter");
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchWaterLoss>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchWaterLoss model)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(model, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => {
                        Repository.GetWaterLossReport(model);
                        CheckForErrors(model);
                    }
                }));
                f.Pdf(() => {
                    Repository.GetWaterLossReport(model);
                    return new PdfResult(_container.GetInstance<IHtmlToPdfConverter>(), "Pdf", model);
                });
            });
        }

        // TODO: This is all a way to display errors when they really wanted a MonthRange search
        // Refactor these validations to be part of the DateRange class? or replace it entirely with a MonthRange
        private void CheckForErrors(SearchWaterLoss model)
        {
            // if we have a between search
            if (model.Date.Operator == RangeOperator.Between)
            {
                if (model.Date.Start.HasValue &&
                    model.Date.Start != model.Date.Start.Value.GetBeginningOfMonth())
                {
                    DisplayErrorMessage(START_ERROR);
                }

                if (model.Date.End.HasValue && model.Date.End.Value.BeginningOfDay() !=
                    model.Date.End.Value.GetEndOfMonth().BeginningOfDay())
                {
                    DisplayErrorMessage(END_ERROR);
                }
            }

            // greater than search 
            if (model.Date.Operator == RangeOperator.GreaterThan ||
                model.Date.Operator == RangeOperator.GreaterThanOrEqualTo)
            {
                if (model.Date.End.HasValue &&
                    model.Date.End != model.Date.End.Value.GetBeginningOfMonth())
                {
                    DisplayErrorMessage(START_ERROR);
                }
            }

            if (model.Date.Operator == RangeOperator.LessThan || model.Date.Operator == RangeOperator.LessThanOrEqualTo)
            {
                if (model.Date.End.HasValue && model.Date.End.Value.BeginningOfDay() !=
                    model.Date.End.Value.GetEndOfMonth().BeginningOfDay())
                {
                    DisplayErrorMessage(END_ERROR);
                }
            }

            if (model.Date.Operator == RangeOperator.Equal)
            {
                DisplayErrorMessage(EXACT_ERROR);
            }
        }

        #endregion

        public WaterLossManagementController(
            ControllerBaseWithPersistenceArguments<IRepository<WorkOrder>, WorkOrder, User> args) : base(args) { }
    }
}