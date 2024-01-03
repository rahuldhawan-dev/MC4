using MMSINC.ClassExtensions.StringExtensions;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using Migration =
    MapCall.Common.Model.Migrations._2023.MC1425_FixOperatingCenterZonesInHydrantsDuePaintingView;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantsDuePaintingMap : AbstractAuxiliaryDatabaseObject
    {
        public struct Sql
        {
            public const string CREATE_VIEW_FORMAT =
                Migration.CREATE_VIEW;

            public const string DROP_VIEW =
                Migration.DROP_VIEW;
        }

        public override string SqlCreateString(
            Dialect dialect,
            IMapping p,
            string defaultCatalog,
            string defaultSchema)
        {
            return Sql.CREATE_VIEW_FORMAT.ToSqlite();
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return Sql.DROP_VIEW;
        }
    }
}
