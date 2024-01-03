using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{

    // TODO: The two reports using this base class need to be updated to use the new one -> ServiceReportController
    // NOTE: If the required role needs to be moved to the inheriting class then the RoleAuthorizationFilter will need to be fixed.
    [DisplayName("Services Renewed w/Footage")]
    public class ServicesRenewedController : ControllerBaseWithPersistence<IServiceRepository, Service, User>
    {
        #region Constants

        public const string NO_RESULTS = "No results matched your query.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchServicesRenewed>();
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesAssets)]
        public ActionResult Index(SearchServicesRenewed search)
        {
            // validate the search was entered correctly
            if (!ModelState.IsValid)
            {
                DisplayModelStateErrors();
                return DoRedirectionToAction("Search", search);
            }

            // TODO: ActionHelper can't handle searches where the return model 
            // is different from the search type. 
            var results = Repository.GetServicesRenewed(search);
            return this.RespondTo(f => {
                f.View(() => {
                    if (!results.Any())
                    {
                        DisplayErrorMessage(NO_RESULTS);
                        return DoRedirectionToAction("Search", search);
                    }

                    return View(results);
                });
                f.Excel(() => {
                    // This smells. This is not returning a real excel file by any means. -Ross 12/18/2019
                    Response.AddHeader("Content-Type", "application/vnd.ms-excel");
                    return PartialView("_Index", results);
                });
            });
        }

        #endregion

        public ServicesRenewedController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}
    }
}
