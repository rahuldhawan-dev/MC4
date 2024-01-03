using System.ComponentModel;
using System.Web.Mvc;
using System.Web.UI;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;

namespace MapCallMVC.Areas.SAP.Controllers
{
    public class WBSElementController : ControllerBaseWithPersistence<WorkOrder, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Constructors

        public WBSElementController(ControllerBaseWithPersistenceArguments<IRepository<WorkOrder>, WorkOrder, User> args) : base(args) {}

        #endregion

        #region Private Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<SAPProjectType>("SAPProjectType", x => x.GetAllSorted(), x => x.SAPCode, x => x.Description);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchWBSElement search)
        {
            var repo = _container.GetInstance<ISAPWBSElementRepository>();
            var operatingCenter = _container.GetInstance<IOperatingCenterRepository>().Find(search.OperatingCenter);
            var repoSearch = new SAPWBSElement {
                WBSDescription = search.WBSDescription,
                WBSNumber = search.WBSNumber,
                ProjectType = search.SAPProjectType,
                ProjectDefintion = search.ProjectDefinition,
                PlanningPlant = search.PlanningPlant
            };
            if (search.IsOpen.HasValue)
                repoSearch.SystemStatus = (search.IsOpen == true) ? "I0002" : "I0045";
            if (search.Year.HasValue)
                repoSearch.Year = search.Year.ToString();

            var result = repo.Search(repoSearch);

            // NOTE: We need to mock out the result for regression test purposes, but we can keep
            //       the actual SAP call in here to make sure that isn't throwing an error.
#if DEBUG
            if (MMSINC.MvcApplication.IsInTestMode)
            {
                if (MMSINC.MvcApplication.RegressionTestFlags.Contains("sap returns a valid functional location"))
                {
                    result.Items.Clear();
                    result.Items.Add(new SAPWBSElement() {
                        SAPErrorCode = "Successful",
                        Status = "Open",
                        WBSDescription = search.WBSDescription,
                        WBSNumber = search.WBSNumber
                    });
                }
                else if (MMSINC.MvcApplication.RegressionTestFlags.Contains("sap returned a valid functional location not found"))
                {
                    result.Items.Clear();
                    result.Items.Add(new SAPWBSElement() {
                        SAPErrorCode = "No record was returned from the SAP Web Service"
                    });
                }
            }
#endif 
            
            return this.RespondTo((formatter) => {
                formatter.Json(() => Json(new {
                    Data = result
                }, JsonRequestBehavior.AllowGet));
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        [OutputCache(Location = OutputCacheLocation.None, Duration = 0, NoStore = true)]
        public PartialViewResult Find(int? operatingCenterId)
        {
            if (operatingCenterId.HasValue)
            {
                this.AddDynamicDropDownData<PlanningPlant, PlanningPlantDisplayItem>(x => x.Code, x => x.Display, filter: z => z.OperatingCenter.Id == operatingCenterId.Value);
            }
            SetLookupData(ControllerAction.Search);
            return PartialView("_Find");
        }

        #endregion
    }

    public class SearchWBSElement
    {
        [DropDown]
        public string PlanningPlant { get; set; }
        [Description("Use * for wildcards")]
        public string WBSNumber { get; set; }
        public string WBSDescription { get; set; }
        public int OperatingCenter { get; set; }
        public int AssetType { get; set; }
        [DropDown, View(DisplayName = "Project Type")]
        public string SAPProjectType { get; set; }
        public string ProjectDefinition { get; set; }
        public int? Year { get; set; }
        [View(DisplayName = "Status"), BoolFormat("Open", "Complete")]
        public bool? IsOpen { get; set; }
    }
}