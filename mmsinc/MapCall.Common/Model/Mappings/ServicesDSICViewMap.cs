using MapCall.Common.Model.Migrations._2020;
using MMSINC.ClassExtensions.StringExtensions;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace MapCall.Common.Model.Mappings
{
    public class ServicesDSICViewMap : AbstractAuxiliaryDatabaseObject
    {
        public struct Sql
        {
            public const string CREATE_VIEW = CreateViewsForDSICForMC1960.CREATE_SERVICES_VIEW_SQLITE_BEGIN +
                                              CreateViewsForDSICForMC1960.CREATE_SERVICES_VIEW +
                                              CreateViewsForDSICForMC1960.CREATE_SERVICES_VIEW_SQLITE_END;

            public const string DROP_VIEW = "DROP VIEW ServicesDSIC";
        }

        public override string SqlCreateString(Dialect dialect, IMapping p, string defaultCatalog, string defaultSchema)
        {
            return Sql.CREATE_VIEW.ToSqlite();
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return Sql.DROP_VIEW;
        }
    }
}
