using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131231101506), Tags("Production")]
    public class AddFunctionalLocationToHydrantsValvesSAP : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string VALVES_SAP = "ValvesSAP",
                                HYDRANTS_SAP = "tblNJAWHydrantSAP",
                                SEWER_MANHOLES = "SewerManHoles",
                                FUNCTIONAL_LOCATIONS = "FunctionalLocations";
        }

        public struct Columns
        {
            public const string FUNCTIONAL_LOCATION_ID = "FunctionalLocationID",
                                DESCRIPTION = "Description";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50;
        }

        public struct ForeignKeys
        {
            public const string FK_HYDRANTS_FUNCTIONAL_LOCATIONS =
                                    "FK_tblNJAWHydrantSAP_FunctionalLocations_FunctionalLocationID",
                                FK_VALVES_FUNCTIONAL_LOCATIONS =
                                    "FK_ValvesSAP_FunctionalLocations_FunctionalLocationID",
                                FK_SEWER_MANHOLES_FUNCTIONAL_LOCATIONS =
                                    "FK_SewerManholes_FunctionalLocations_FunctionalLocationID";
        }

        #endregion

        public override void Up()
        {
            Create.Table(Tables.FUNCTIONAL_LOCATIONS)
                  .WithColumn(Columns.FUNCTIONAL_LOCATION_ID).AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn(Columns.DESCRIPTION).AsAnsiString(StringLengths.DESCRIPTION).NotNullable();

            Alter.Table(Tables.HYDRANTS_SAP).AddColumn(Columns.FUNCTIONAL_LOCATION_ID).AsInt32().Nullable();
            Alter.Table(Tables.VALVES_SAP).AddColumn(Columns.FUNCTIONAL_LOCATION_ID).AsInt32().Nullable();
            Alter.Table(Tables.SEWER_MANHOLES).AddColumn(Columns.FUNCTIONAL_LOCATION_ID).AsInt32().Nullable();

            Create.ForeignKey(ForeignKeys.FK_HYDRANTS_FUNCTIONAL_LOCATIONS)
                  .FromTable(Tables.HYDRANTS_SAP).ForeignColumn(Columns.FUNCTIONAL_LOCATION_ID)
                  .ToTable(Tables.FUNCTIONAL_LOCATIONS).PrimaryColumn(Columns.FUNCTIONAL_LOCATION_ID);
            Create.ForeignKey(ForeignKeys.FK_VALVES_FUNCTIONAL_LOCATIONS)
                  .FromTable(Tables.VALVES_SAP).ForeignColumn(Columns.FUNCTIONAL_LOCATION_ID)
                  .ToTable(Tables.FUNCTIONAL_LOCATIONS).PrimaryColumn(Columns.FUNCTIONAL_LOCATION_ID);
            Create.ForeignKey(ForeignKeys.FK_SEWER_MANHOLES_FUNCTIONAL_LOCATIONS)
                  .FromTable(Tables.SEWER_MANHOLES).ForeignColumn(Columns.FUNCTIONAL_LOCATION_ID)
                  .ToTable(Tables.FUNCTIONAL_LOCATIONS).PrimaryColumn(Columns.FUNCTIONAL_LOCATION_ID);
        }

        public override void Down()
        {
            Delete.ForeignKey(ForeignKeys.FK_HYDRANTS_FUNCTIONAL_LOCATIONS).OnTable(Tables.HYDRANTS_SAP);
            Delete.ForeignKey(ForeignKeys.FK_VALVES_FUNCTIONAL_LOCATIONS).OnTable(Tables.VALVES_SAP);
            Delete.ForeignKey(ForeignKeys.FK_SEWER_MANHOLES_FUNCTIONAL_LOCATIONS).OnTable(Tables.SEWER_MANHOLES);

            Delete.Column(Columns.FUNCTIONAL_LOCATION_ID).FromTable(Tables.HYDRANTS_SAP);
            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sysindexes WHERE name = '_dta_index_ValvesSAP_7_596301284__K1_8') DROP INDEX ValvesSAP._dta_index_ValvesSAP_7_596301284__K1_8");
            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sys.stats WHERE name = '_dta_stat_596301284_8_1') DROP STATISTICS {0}._dta_stat_596301284_8_1",
                Tables.VALVES_SAP);
            Delete.Column(Columns.FUNCTIONAL_LOCATION_ID).FromTable(Tables.VALVES_SAP);
            Delete.Column(Columns.FUNCTIONAL_LOCATION_ID).FromTable(Tables.SEWER_MANHOLES);

            Delete.Table(Tables.FUNCTIONAL_LOCATIONS);
        }
    }
}
