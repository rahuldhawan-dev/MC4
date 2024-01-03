using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class HydrantWorkOrdersByDescriptionController : ControllerBaseWithPersistence<IHydrantRepository, Hydrant, User>
    {
        #region Constructors

        public HydrantWorkOrdersByDescriptionController(ControllerBaseWithPersistenceArguments<IHydrantRepository, Hydrant, User> args) : base(args) { }

        #endregion

        #region Public Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            this.AddOperatingCenterDropDownData();
            this.AddDropDownData<WorkDescription>("HasWorkOrderWithWorkDescriptions", x => x.GetAllSorted().Where(y => y.AssetType != null && y.AssetType.Id == AssetType.Indices.HYDRANT), x => x.Id, x => x.Description);
            this.AddDropDownData<AssetStatus>();
            this.AddDropDownData<HydrantBilling>();
            this.AddDropDownData<HydrantManufacturer>();
        }

        [HttpGet]
        public ActionResult Search(SearchHydrantWorkOrdersByDescription search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet]
        public ActionResult Index(SearchHydrantWorkOrdersByDescription search)
        {
            // no paging for this report
            search.EnablePaging = false;
            var args = new MMSINC.Utilities.ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => {
                    // TODO: Why are the results being set on the search model in here
                    // instead of in the repository method? It looks like the repo should already
                    // be doing that since it's calling the base Search method, so what's this for?
                    var results = Repository.GetHydrantWorkOrdersByDescription(search).ToList();
                    search.Count = results.Count();
                    search.Results = results;
                }
            };
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search, args));
                formatter.Excel(() => ActionHelper.DoExcel(search, args));
            });
        }

        #endregion
    }
}