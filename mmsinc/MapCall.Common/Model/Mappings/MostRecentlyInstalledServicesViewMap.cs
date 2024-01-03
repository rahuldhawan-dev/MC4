using MapCall.Common.Model.Migrations._2022;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using CurrentViewMigration =
    MapCall.Common.Model.Migrations._2022.MC5116_FixMostRecentlyInstalledServicesViewForNewChangeTrackingColumnNames;

namespace MapCall.Common.Model.Mappings
{
    public class MostRecentlyInstalledServicesViewMap : AbstractAuxiliaryDatabaseObject
    {
        #region Exposed Methods

        public override string SqlCreateString(
            Dialect dialect,
            IMapping p,
            string defaultCatalog,
            string defaultSchema)
        {
            return
                $"CREATE VIEW [{CurrentViewMigration.VIEW_NAME}] " + 
                $"AS{CurrentViewMigration.NEW_VIEW_SQL}";
        }

        public override string SqlDropString(
            Dialect dialect,
            string defaultCatalog,
            string defaultSchema)
        {
            return MC4687_CreateMostRecentlyInstalledServicesView.DROP_SQL;
        }

        #endregion
    }
}
