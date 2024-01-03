using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Operations.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class OccurrenceController : ControllerBaseWithPersistence<IAbsenceNotificationRepository, AbsenceNotification, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsManagement;

        #endregion

        #region Constructors

        public OccurrenceController(ControllerBaseWithPersistenceArguments<IAbsenceNotificationRepository, AbsenceNotification, User> args) : base(args) { }

        #endregion

        #region Exposed Methods
        
        public override void SetLookupData(ControllerAction action)
        {
            this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchOccurrence search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchOccurrence search)
        {
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => { Repository.GetNonFMLAAbsencesLessThanAYearOld(search); }
            };
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search, args));
                formatter.Excel(() => ActionHelper.DoExcel(search, args));
            });
        }

        #endregion
    }
}