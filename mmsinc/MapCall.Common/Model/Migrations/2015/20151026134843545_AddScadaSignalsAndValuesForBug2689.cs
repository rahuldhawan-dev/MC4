using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151026134843545), Tags("Production")]
    public class AddScadaSignalsAndValuesForBug2689 : Migration
    {
        public struct TableNames
        {
            public const string SCADA_SIGNALS = "ScadaSignals",
                                SCADA_SIGNAL_VALUES = "ScadaSignalValues";
        }

        public struct StringLengths
        {
            public const int TAG_NAME = 50, DESCRIPTION = 255, ENGINEERING_UNITS = 20, TAG_ID = 55;
        }

        public override void Up()
        {
            Create.Table(TableNames.SCADA_SIGNALS)
                  .WithIdentityColumn()
                  .WithColumn("TagName").AsAnsiString(StringLengths.TAG_NAME).NotNullable()
                  .WithColumn("Description").AsAnsiString(StringLengths.DESCRIPTION).NotNullable()
                  .WithColumn("EngineeringUnits").AsAnsiString(StringLengths.ENGINEERING_UNITS).Nullable()
                  .WithColumn("TagId").AsAnsiString(StringLengths.TAG_ID).NotNullable();

            Create.Table(TableNames.SCADA_SIGNAL_VALUES)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ScadaSignalId", TableNames.SCADA_SIGNALS, nullable: false)
                  .WithColumn("DateTimeStamp").AsDateTime().NotNullable()
                  .WithColumn("Value").AsDecimal(18, 8).NotNullable();
        }

        public override void Down()
        {
            Delete.Table(TableNames.SCADA_SIGNAL_VALUES);
            Delete.Table(TableNames.SCADA_SIGNALS);
        }
    }
}
