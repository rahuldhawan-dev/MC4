using FluentMigrator;
using CreateMostRecentlyInstalledServicesView =
    MapCall.Common.Model.Migrations._2022.MC4972_FixMostRecentlyInstalledServicesView;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20230314092846119), Tags("Production")]
    public class MC5116_RevertDocumentLinkViewAndMostRecentlyInstalledServicesViewColumnNameChanges : Migration
    {
        public override void Up()
        {
            // NOOP, this only exists so that we can recreate views using columns whose names were changed
            // in `MC5116_RenameChangeTrackingColumns`.  If we attempt to do this in the `Down()` of
            // `MC5116_FixDocumentLinkViewForNewChangeTrackingColumns` instead we'll get errors because the
            // columns won't have been renamed back yet
        }

        public override void Down()
        {
            Execute.Sql(CreateDocumentLinkView.CREATE_SQL.Replace("CREATE VIEW", "ALTER VIEW"));
            Execute.Sql($"ALTER VIEW [MostRecentlyInstalledServicesView] AS{CreateMostRecentlyInstalledServicesView.NEW_VIEW_SQL}");
        }
    }
}
