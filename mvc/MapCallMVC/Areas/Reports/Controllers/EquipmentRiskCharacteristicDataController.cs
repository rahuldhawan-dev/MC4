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
    [DisplayName("Equipment Risk Characteristic Data")]
    public class EquipmentRiskCharacteristicDataController : ControllerBaseWithPersistence<IRepository<Equipment>, Equipment, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionEquipment;

        #endregion

        #region Constructor

        public EquipmentRiskCharacteristicDataController(ControllerBaseWithPersistenceArguments<IRepository<Equipment>, Equipment, User> args) : base(args) { }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchEquipmentRiskCharacteristicData search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchEquipmentRiskCharacteristicData search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search)
                                            .Select(e => new {
                                                 e.Id,
                                                 e.OperatingCenter,
                                                 e.Facility.PlanningPlant,
                                                 FacilityName = e.Facility,
                                                 e.EquipmentType,
                                                 EquipmentName = e.Description,//this line for EquipmentName
                                                 FacilityAMStrategyTier = e.Facility.StrategyTier,//Facility AM Strategy Tier
                                                 e.EquipmentPurpose,
                                                 e.ConsequenceOfFailure,
                                                 e.LikelyhoodOfFailure,
                                                 e.RiskCharacteristicsLastUpdatedBy,
                                                 e.RiskCharacteristicsLastUpdatedOn
                                             });
                    return this.Excel(results);
                });
            });
        }

        #endregion
    }
}