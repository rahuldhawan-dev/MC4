using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AuditLogEntryMap : ClassMap<AuditLogEntry>
    {
        public const string TABLE_NAME = "AuditLogEntries";

        public AuditLogEntryMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.AuditEntryType).Length(AuditLogEntry.StringLengths.AUDIT_ENTRY_TYPE).Not.Nullable();
            Map(x => x.EntityName).Length(AuditLogEntry.StringLengths.ENTITY_NAME_MAX_LENGTH).Not.Nullable();
            Map(x => x.EntityId).Not.Nullable();
            Map(x => x.FieldName).Nullable().Length(AuditLogEntry.StringLengths.FIELD_NAME_MAX_LENGTH).Nullable();
            Map(x => x.OldValue).Length(int.MaxValue).CustomSqlType("text").Nullable().Nullable();
            Map(x => x.NewValue).Length(int.MaxValue).CustomSqlType("text").Nullable().Nullable();
            Map(x => x.Timestamp).Not.Nullable();
            References(x => x.User).Nullable();
            References(x => x.ContractorUser).Nullable();
        }
    }
}
