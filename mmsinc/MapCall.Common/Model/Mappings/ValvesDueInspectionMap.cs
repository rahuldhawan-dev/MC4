using MapCall.Common.Model.Migrations._2019;
using MapCall.Common.Model.Migrations._2022;
using MMSINC.ClassExtensions.StringExtensions;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Mappings
{
    public class ValvesDueInspectionMap : AbstractAuxiliaryDatabaseObject
    {
        public struct Sql
        {
            // public const string CREATE_VIEW_FORMAT = PairDownAssetStatusTablesForMC1381.Sql.Valves.CREATE_VALVE_VIEW_SQL_FORMAT;
            public const string CREATE_VIEW_FORMAT =
                MC1393RenamingValveInspectionColumnName.Valves.NEW_CREATE_VIEW;

            public const string DROP_VIEW = MC1725FixAssetsDueInspection.Valves.DROP_VALVES_DUE_INSPECTION_VIEW;
        }

        public override string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            return string.Format(Sql.CREATE_VIEW_FORMAT, "GetDate()").ToSqlite();
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return Sql.DROP_VIEW;
        }
    }
}
