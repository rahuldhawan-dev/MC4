using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160428101403288), Tags("Production")]
    public class GetRidOfOldScadaSignalValuesForBug2896 : Migration
    {
        #region Constants

        public const string TABLE_NAME = "ScadaSignalValues";

        #endregion

        #region Exposed Methods

        public override void Up()
        {
            Delete.Table(TABLE_NAME);
        }

        public override void Down()
        {
            Create.Table(TABLE_NAME)
                  .WithIdentityColumn()
                  .WithForeignKeyColumn("ScadaSignalId", "ScadaSignals", nullable: false)
                  .WithColumn("DateTimeStamp").AsDateTime().NotNullable()
                  .WithColumn("Value").AsDecimal(18, 8).NotNullable();
        }

        #endregion
    }
}
