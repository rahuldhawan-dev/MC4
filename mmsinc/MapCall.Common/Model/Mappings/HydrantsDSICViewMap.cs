using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using CurrentViewMigration =
    MapCall.Common.Model.Migrations._2023.MC2780_FixHydrantsDSICView;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantsDSICViewMap : AbstractAuxiliaryDatabaseObject
    {
        public override string SqlCreateString(
            Dialect dialect,
            IMapping p,
            string defaultCatalog,
            string defaultSchema)
        {
            return
                $"CREATE VIEW [{CurrentViewMigration.VIEW_NAME}] " +
                $"AS{CurrentViewMigration.NEW_VIEW_SQL}{CurrentViewMigration.NEW_VIEW_SQLITE_END}";
        }

        public override string SqlDropString(Dialect dialect, string defaultCatalog, string defaultSchema)
        {
            return $"DROP VIEW [{CurrentViewMigration.VIEW_NAME}]";
        }
    }
}
