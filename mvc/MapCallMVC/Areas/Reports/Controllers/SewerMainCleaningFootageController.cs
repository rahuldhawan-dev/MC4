using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("Footage of Sewer Main Inspected / Cleaned")]
    public class SewerMainCleaningFootageController : ControllerBaseWithPersistence<ISewerMainCleaningRepository, SewerMainCleaning, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData("Year", Repository.GetDistinctYearsCompleted().OrderByDescending(x => x), x => x, x => x);
                    break;
            }
            base.SetLookupData(action);
        }

        #region Search/Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch<SearchSewerMainCleaningFootageReport>();
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchSewerMainCleaningFootageReport search)
        {
            return this.RespondTo(f => {
                var model = Repository.SearchSewerMainCleaningFootageReport(search);
                f.View(() => View(model));
                f.Excel(() => this.Excel(model.Select(x => new {
                    x.OperatingCenter,
                    x.Town,
                    x.InspectionType,
                    x.Year,
                    jan = $"{x.Jan:N0}",
                    feb = $"{x.Feb:N0}",
                    mar = $"{x.Mar:N0}",
                    apr = $"{x.Apr:N0}",
                    may = $"{x.May:N0}",
                    jun = $"{x.Jun:N0}",
                    jul = $"{x.Jul:N0}",
                    aug = $"{x.Aug:N0}",
                    sep = $"{x.Sep:N0}",
                    oct = $"{x.Oct:N0}",
                    nov = $"{x.Nov:N0}",
                    dec = $"{x.Dec:N0}",
                    Total = $"{x.Total:N0}"
                })));
            });
        }

        #endregion

        #endregion

        public SewerMainCleaningFootageController(ControllerBaseWithPersistenceArguments<ISewerMainCleaningRepository, SewerMainCleaning, User> args) : base(args) { }
    }
}