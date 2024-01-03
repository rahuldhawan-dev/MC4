using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class AuditLogEntry : IEntity, IValidatableObject
    {
        #region Consts

        public struct StringLengths
        {
            public const int FIELD_NAME_MAX_LENGTH = 255,
                             AUDIT_ENTRY_TYPE = 255,
                             ENTITY_NAME_MAX_LENGTH = 255;
        }

        public readonly struct DisplayNames
        {
            public const string
                TIME_STAMP_EST = "Time Stamp (EST)";
        }

        protected static internal readonly string[] INVALID_ENTITY_NAMES = {
            "Note",
            "Document",
            "EmployeeLink",
            "DocumentLink"
        };

        protected static internal readonly string[] INVALID_AUDIT_ENTRY_TYPES = {
            "Index",
            "Delete"
        };

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string AuditEntryType { get; set; }
        public virtual string EntityName { get; set; }
        public virtual int EntityId { get; set; }
        public virtual string FieldName { get; set; }
        public virtual string OldValue { get; set; }
        public virtual string NewValue { get; set; }

        [View(DisplayNames.TIME_STAMP_EST, FormatStyle.DateTimeWithSecondsWithEstTimezone)]
        public virtual DateTime Timestamp { get; set; }
        public virtual User User { get; set; }
        public virtual ContractorUser ContractorUser { get; set; }

        #region Logical Properties

        public virtual bool IsLinkable
        {
            get
            {
                if (INVALID_ENTITY_NAMES.Contains(EntityName) ||
                    INVALID_AUDIT_ENTRY_TYPES.Contains(AuditEntryType))
                    return false;
                return true;
            }
        }

        public virtual string PropertyEntityType
        {
            get
            {
                var entity = Type.GetType("MapCall.Common.Model.Entities." + EntityName);
                if (entity != null && FieldName != null)
                {
                    var property = entity.GetProperty(FieldName);
                    if (property != null && typeof(IEntity).IsAssignableFrom(property.PropertyType))
                    {
                        return property.PropertyType.Name;
                    }
                }

                return String.Empty;
            }
        }

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public interface IAuditLogEntryLink
    {
        #region Abstract Properties

        int Id { get; }
        AuditLogEntry AuditLogEntry { get; set; }
        int EntityId { get; }

        #endregion
    }
}
