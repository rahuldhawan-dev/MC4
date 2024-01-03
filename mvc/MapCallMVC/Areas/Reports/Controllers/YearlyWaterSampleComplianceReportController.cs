using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class YearlyWaterSampleComplianceReportController : ControllerBaseWithPersistence<IPublicWaterSupplyRepository, PublicWaterSupply, User>
    {
        #region Consts

        public const RoleModules ROLE = RoleModules.EnvironmentalGeneral;

        #endregion

        #region Constructor

        public YearlyWaterSampleComplianceReportController(ControllerBaseWithPersistenceArguments<IPublicWaterSupplyRepository, PublicWaterSupply, User> args) : base(args) { }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchYearlyWaterSampleComplianceReport>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchYearlyWaterSampleComplianceReport search)
        {
            search.EnablePaging = false;
            return ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.SearchYearlyWaterSampleComplianceReport(search)
            });
        }

        #endregion

    }
}