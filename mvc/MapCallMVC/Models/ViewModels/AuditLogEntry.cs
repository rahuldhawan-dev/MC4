using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchAuditLogEntry : SearchSet<AuditLogEntry>
    {
        #region Properties

        [DropDown]
        public virtual string EntityName { get; set; }
        public virtual int? EntityId { get; set; }

        [DropDown, Description("User responsible the change or action.")]
        public int? User { get; set; }
        [DropDown]
        public string AuditEntryType { get; set; }

        [Description("Use this for searching email notification email addresses.")]
        public string FieldName { get; set; }

        public DateRange Timestamp { get; set; }

        #endregion

        #region Public Methods

        public override void ModifyValues(ISearchMapper mapper)
        {
            base.ModifyValues(mapper);
            // SearchMapper automatically sets properties with "EntityId" to "Id"
            // because that's how we generally want to search for things. In this case,
            // AuditLogEntry has an actual column called EntityId that needs to be searched
            // so we want to set it back to EntityId.
            mapper.MappedProperties[nameof(EntityId)].ActualName = "EntityId";
        }

        #endregion
    }

    public class SecureSearchAuditLogEntryForSingleRecord : SearchSet<AuditLogEntry>, ISearchAuditLogEntryForSingleRecord
    {
        #region Public Methods

        /// <summary>
        /// The name of entity's type.
        /// </summary>
        [Required, Secured]
        public string EntityTypeName { get; set; }

        /// <summary>
        /// The Id for a specific entity record.
        /// </summary>
        [Required, Secured]
        public int? EntityId { get; set; }

        /// <summary>
        /// The controller name. This is sometimes different from the entity name and
        /// is used for Show actions.
        ///
        /// ex: WorkOrder vs WorkOrderSupervisorApproval
        /// </summary>
        [Required, Secured]
        public string ControllerName { get; set; }

        public override string DefaultSortBy => nameof(AuditLogEntry.Timestamp);

        #endregion
    }
}