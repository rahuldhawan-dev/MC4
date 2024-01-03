using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.Configuration;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Restoration Accrual")]
    public class RestorationAccrualReportController : ControllerBaseWithPersistence<IRestorationRepository, Restoration, User>
    {
        #region Constructor

        public RestorationAccrualReportController(ControllerBaseWithPersistenceArguments<IRestorationRepository, Restoration, User> args) : base(args) { }

        #endregion

        #region Public Methods

        [HttpGet, RequiresRole(RoleModules.FieldServicesWorkManagement)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchRestorationAccrualReport>();
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesWorkManagement)]
        [AuditReport("AccrualReport")]
        public ActionResult Index(SearchRestorationAccrualReport search)
        {
            // There's a footer cell doing calculations, so we need all rows for that to be the correct value.
            search.EnablePaging = false;

            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchRestorationsForAccrualReport(search)
                }));
                formatter.Excel(() => {
                    var results = Repository.SearchRestorationsForAccrualReport(search).Select(x => new {
                        WorkOrder = x.WorkOrder.Id,
                        x.WorkOrder.DateCompleted,
                        x.WorkOrder.AccountCharged,
                        x.WorkOrder.AssetType,
                        x.WorkOrder.WorkDescription,
                        x.WorkOrder.BusinessUnit,
                        x.RestorationType,
                        x.EstimatedRestorationFootage,
                        ActualFootage = x.PartialPavingSquareFootage,
                        x.MeasurementType,
                        TotalAccruedCost = string.Format(CommonStringFormats.CURRENCY, x.TotalAccruedCost),
                        DateInitialCompleted = x.PartialRestorationDate,
                        TotalInitialCost = string.Format(CommonStringFormats.CURRENCY, x.PartialRestorationActualCost),
                        AccrualValue = string.Format(CommonStringFormats.CURRENCY, x.AccrualValue),
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion
    }
}