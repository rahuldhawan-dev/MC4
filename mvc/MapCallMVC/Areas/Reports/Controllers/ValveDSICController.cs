using System.ComponentModel;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Reports.Models;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Reports.Controllers
{
    [DisplayName("DSIC Reporting – Valves")]
    public class ValveDSICController : ControllerBaseWithPersistence<IRepository<ValveDSICReportItem>, ValveDSICReportItem, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesAssets;

        #endregion

        #region Constructors

        public ValveDSICController(ControllerBaseWithPersistenceArguments<IRepository<ValveDSICReportItem>, ValveDSICReportItem, User> args) : base(args) { }

        #endregion

        #region Search/Index

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchValveDSIC search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchValveDSIC search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion
    }
}