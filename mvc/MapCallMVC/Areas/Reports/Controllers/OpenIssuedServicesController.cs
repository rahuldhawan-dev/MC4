using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class OpenIssuedServicesController : ControllerBaseWithPersistence<IServiceRepository, Service, User>
    {
        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddOperatingCenterDropDownData();
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchOpenIssuedServicesReport>();
        }

        [HttpGet]
        public ActionResult Index(SearchOpenIssuedServicesReport model)
        {
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => { Repository.GetOpenIssuedServices(model); }
            };
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(model, args));
                f.Excel(() => ActionHelper.DoExcel(model, args));
            });
        }

        #endregion

        public OpenIssuedServicesController(ControllerBaseWithPersistenceArguments<IServiceRepository, Service, User> args) : base(args) {}
    }
}