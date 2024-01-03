using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130514145626), Tags("Production")]
    public class AddWaterSystemColumnToHydrantsSAP : Migration
    {
        #region Constants

        public struct Tables
        {
            public const string HYDRANTS_SAP = "tblNJAWHydrantSAP";
        }

        public struct Columns
        {
            public const string WATER_SYSTEM = "WaterSystem";
        }

        public struct StringLengths
        {
            public const int WATER_SYSTEM = 4;
        }

        #endregion

        public override void Up()
        {
            Alter.Table(Tables.HYDRANTS_SAP)
                 .AddColumn(Columns.WATER_SYSTEM)
                 .AsAnsiString(StringLengths.WATER_SYSTEM).Nullable();
        }

        public override void Down()
        {
            Execute.Sql(
                "IF EXISTS (SELECT 1 FROM sysindexes WHERE name = '_dta_index_tblNJAWHydrantSAP_6_7775185__K1_2_3_4') DROP INDEX tblNJAWHydrantSAP._dta_index_tblNJAWHydrantSAP_6_7775185__K1_2_3_4");
            Delete.Column(Columns.WATER_SYSTEM)
                  .FromTable(Tables.HYDRANTS_SAP);
        }
    }
}
