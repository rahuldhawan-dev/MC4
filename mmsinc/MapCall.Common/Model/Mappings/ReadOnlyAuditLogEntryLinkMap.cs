using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate.Dialect;
using NHibernate.Engine;

namespace MapCall.Common.Model.Mappings
{
    public class ReadOnlyAuditLogEntryLinkMap : ClassMap<ReadOnlyAuditLogEntryLink>
    {
        #region Constructors

        public ReadOnlyAuditLogEntryLinkMap()
        {
            Table("AuditLogEntryLinkView");
            Id(x => x.Id);
            References(x => x.AuditLogEntry, "Id").ReadOnly();

            DiscriminateSubClassesOnColumn("EntityName").AlwaysSelectWithValue();

            SchemaAction.None();
        }

        #endregion
    }

    public class AuditLogEntryLinkViewMap : NHibernate.Mapping.AbstractAuxiliaryDatabaseObject
    {
        public override string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            return Migrations.AddViewForAuditLogEntriesForBug2475.CREATE_SQL;
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return Migrations.AddViewForAuditLogEntriesForBug2475.DROP_SQL;
        }
    }
}
