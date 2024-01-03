using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ReadOnlyAuditLogEntryLink : IEntity, IValidatableObject, IAuditLogEntryLink
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual AuditLogEntry AuditLogEntry { get; set; }
        public virtual int EntityId { get; set; }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield return new ValidationResult("Cannot validate a read-only object.");
        }

        #endregion
    }

    //[Serializable]
    //public class AuditLogEntry<T> : ReadOnlyAuditLogEntryLink
    //{
    //    public virtual T Entity { get; set; }
    //}

    // NOTE: If you're inheriting from ReadOnlyAuditLogEntryLink or AuditLogEntry<T> then you're probably doing it wrong now. -Ross 10/6/2016
}
