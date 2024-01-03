using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchAuditLogEntryForSingleRecord : ISearchSet<AuditLogEntry>
    {
        // NOTE: All properties are set to not map because we need the automagical search
        // stuff from the repo, but we're using a specific repo method and manually mapping
        // everything.

        /// <summary>
        /// The name of entity's type.
        /// </summary>
        [Search(CanMap = false)]
        string EntityTypeName { get; set; }

        /// <summary>
        /// The Id for a specific entity record.
        /// </summary>
        [Search(CanMap = false)]
        int? EntityId { get; set; }

        /// <summary>
        /// The controller name. This is sometimes different from the entity name and
        /// is used for Show actions.
        ///
        /// ex: WorkOrder vs WorkOrderSupervisorApproval
        /// </summary>
        [Search(CanMap = false)]
        string ControllerName { get; set; }
    }
}
