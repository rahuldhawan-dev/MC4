using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class ServiceQualityAssuranceReportController : ControllerBaseWithPersistence<IServiceRepository, Service, User>
    {
        #region Consts

        private const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructor

        public ServiceQualityAssuranceReportController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) { }

        #endregion

        #region Actions

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchServiceQAReport>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchServiceQAReport search)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetServicesQualityAssuranceReport(search)
                }));

                // Using DoExcel here doesn't really matter because we're completely overriding the
                // part where this ever creates an ExcelResult. But for consistency we should still use it.
                x.Excel(() => ActionHelper.DoExcel(search, new ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.GetServicesQualityAssuranceReport(search),
                    OnSuccess = () => {
                        Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                        return PartialView("Excel", search);
                    }
                }));
            });
        }

        #endregion
    }
}
