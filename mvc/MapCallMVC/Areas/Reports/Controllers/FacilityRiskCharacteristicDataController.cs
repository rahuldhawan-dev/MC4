using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Facility Risk Characteristic Data")]
    public class FacilityRiskCharacteristicDataController : ControllerBaseWithPersistence<IRepository<Facility>, Facility, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region Constructor

        public FacilityRiskCharacteristicDataController(ControllerBaseWithPersistenceArguments<IRepository<Facility>, Facility, User> args) : base(args){ }
        
        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchFacilityRiskCharacteristicData search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchFacilityRiskCharacteristicData search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search)
                                            .Select(e => new {
                                                 e.Id,
                                                 e.OperatingCenter,
                                                 e.PlanningPlant,
                                                 e.FacilityName,
                                                 e.Performance,
                                                 e.Condition,
                                                 e.LikelihoodOfFailure,
                                                 e.ConsequenceOfFailure,
                                                 e.MaintenanceRiskOfFailure,
                                                 e.StrategyTier,
                                                 e.ConsequenceOfFailureFactor,
                                                 e.WeightedRiskOfFailureScore
                                             });
                    return this.Excel(results);
                });
            });
        }

        #endregion
    }
}
