using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Metadata;

namespace MapCallMVC.Controllers
{
    public class AuditLogEntryController : ControllerBaseWithPersistence<IAuditLogEntryRepository, AuditLogEntry, User>
    {
        #region Search/Index/Show

        // TODO: Creating a SecureAuditLogEntryController might make more sense. -Ross 4/7/2020
        // NOTE: This is used by Views/Shared/AuditLogEntry/Index and WithLog.
        [HttpGet, RequiresSecureForm]
        public ActionResult SecureIndexForSingleRecord(SecureSearchAuditLogEntryForSingleRecord search)
        {
            return this.RespondTo(
                f => f.Fragment(() => ActionHelper.DoIndex(search, new MMSINC.Utilities.ActionHelperDoIndexArgs {
                    SearchOverrideCallback = () => Repository.SearchLogsForSpecificEntityRecord(search),
                    ViewName = "_SecureIndexForSingleRecord",
                    IsPartial = true,
                    // We don't want this to redirect back to the search page as this is loading in a tab.
                    // We also don't want this redirecting to the individual record for the same reason.
                    RedirectSingleItemToShowView = false,
                    OnNoResults = () => new EmptyResult()
                })));
        }

        #endregion

        public AuditLogEntryController(ControllerBaseWithPersistenceArguments<IAuditLogEntryRepository, AuditLogEntry, User> args) : base(args) { }
    }
}
