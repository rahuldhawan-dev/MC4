using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.SAP.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MapCall.SAP.Model.Repositories;

namespace MapCallMVC.Areas.SAP.Controllers
{
    /// <summary>
    /// Wait a moment? Why do we have an SAPFunctionalLocation controller and our own?
    /// At this point there isn't a single source of
    /// </summary>
    public class SAPFunctionalLocationController : ControllerBaseWithPersistence<Equipment, User>
    {
        #region Constructors

        public SAPFunctionalLocationController(ControllerBaseWithPersistenceArguments<IRepository<Equipment>, Equipment, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<FunctionalLocationCategory>(x => x.GetAllSorted(), x => x.SAPCode, x => x.Description);
                this.AddDropDownData<TechnicalObjectType>(x => x.GetAllSorted(), x => x.SAPCode, x => x.Description);
            }
        }

        #endregion

        #region Search/Show/Index

        [HttpGet]
        public ActionResult Index(SearchSapFunctionalLocation search)
        {
            var operatingCenter = _container.GetInstance<IOperatingCenterRepository>().Find(search.OperatingCenter);
            var repo = _container.GetInstance<ISAPFunctionalLocationRepository>();
            //search.PlanningPlant = operatingCenter.ProductionPlanningPlant.Code;
            var results = repo.Search(search.ToSearchSapNotification());

            return this.RespondTo((formatter) =>
            {
                formatter.Json(() =>
                {
                    return Json(new
                    {
                        Data = results.Select(x => new
                        {
                            x.FunctionalLocation,
                            x.FunctionalLocationDescription,
                            x.SAPErrorCode
                        })
                    }, JsonRequestBehavior.AllowGet);
                });
            });
        }

        [HttpGet]
        [OutputCache(Location = OutputCacheLocation.None, Duration = 0, NoStore = true)]
        public PartialViewResult Find(int? operatingCenterId, int? facilityId)
        {
            var search = new SearchSapFunctionalLocation();
            search.SearchUrl = Url.Action("Index", "SAPFunctionalLocation", new {area = "SAP"}) + "/Index.json";

            if (operatingCenterId.HasValue)
            {
                this.AddDynamicDropDownData<PlanningPlant, PlanningPlantDisplayItem>(x => x.Code, x => x.Display,
                    filter: z => z.OperatingCenter.Id == operatingCenterId.Value);
            }
            if (facilityId.HasValue)
            {
                var facility = _container.GetInstance<IFacilityRepository>().Find(facilityId.Value);
                if (!string.IsNullOrWhiteSpace(facility.FunctionalLocation))
                    search.FunctionalLocation = facility.FunctionalLocation + "*";
                if (facility.PlanningPlant != null)
                    search.PlanningPlant = facility.PlanningPlant.Code;
            }

            SetLookupData(ControllerAction.Search);

            return PartialView("_Find", search);
        }
        #endregion
    }
}