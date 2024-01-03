using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Recurring Project List")]
    public class RecurringProjectListController : ControllerBaseWithPersistence<IRepository<RecurringProject>, RecurringProject, User>
    {
        #region Constants

        public const string NOT_FOUND = "Recurring Project with the id '{0}' was not found.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddOperatingCenterDropDownData();
                    this.AddDropDownData<FoundationalFilingPeriod>("FoundationalFilingPeriod",
                        r => r.GetAllSorted(x => x.Description), x => x.Id, x => x.Description);
                    this.AddDropDownData<RecurringProjectStatus>("Status",
                        r => r.GetAllSorted(x => x.Description), x => x.Id, x => x.Description);
                    this.AddDropDownData<AssetCategory>();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesProjects)]
        public ActionResult Search(SearchRecurringProjectList search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.FieldServicesProjects)]
        public ActionResult Index(SearchRecurringProjectList search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search)
                        .Select(x => new {
                            x.Id, 
                            x.OperatingCenter,
                            x.District,
                            x.Town,
                            x.EstimatedInServiceDate,
                            x.WBSNumber,
                            x.ProjectTitle,
                            x.NJAWEstimate,
                            x.RecurringProjectType,
                            x.ProposedLength,
                            x.ProposedDiameter,
                            x.ProposedPipeMaterial,
                            x.DecadeInstalled,
                            x.ExistingDiameter,
                            x.ExistingPipeMaterial,
                            x.AcceleratedAssetInvestmentCategory,
                            x.SecondaryAssetInvestmentCategory,
                            x.Justification,
                            x.EstimatedInServicePeriod,
                            x.FoundationalFilingPeriod,
                            x.Status,
                            x.FinalCriteriaScore,
                            x.FinalRawScore,
                            x.EstimatedVariableScore,
                            x.EstimatedPriorityWeightedScore,
                            x.ActualInServiceDate,
                            x.CreatedBy,
                            Latitude = (x.Coordinate != null ? x.Coordinate.Latitude : (decimal?)null),
                            Longitude = (x.Coordinate != null ? x.Coordinate.Longitude : (decimal?)null)
                        });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        public RecurringProjectListController(ControllerBaseWithPersistenceArguments<IRepository<RecurringProject>, RecurringProject, User> args) : base(args) {}
    }
}
