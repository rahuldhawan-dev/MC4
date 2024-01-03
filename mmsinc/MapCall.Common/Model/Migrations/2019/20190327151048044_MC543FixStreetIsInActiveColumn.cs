using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190327151048044), Tags("Production")]
    public class MC543FixStreetIsInActiveColumn : Migration
    {
        public override void Up()
        {
            Alter.Table("Streets")
                 .AddColumn("IsActive").AsBoolean().WithDefaultValue(true).NotNullable();
            Execute.Sql("UPDATE Streets set IsActive = 0 WHERE InactSt = 'ON'");
            Execute.Sql("DROP STATISTICS [Streets].[_dta_stat_1365579903_4_3]");
            Delete.Column("InactSt").FromTable("Streets");
        }

        public override void Down()
        {
            Alter.Table("Streets")
                 .AddColumn("InactSt").AsAnsiString(2).Nullable();
            Execute.Sql("UPDATE Streets set InactSt = 'ON' where IsActive = 0");
            Execute.Sql("CREATE STATISTICS [_dta_stat_1365579903_4_3] ON [Streets]([InactSt], [FullStName])");
            Delete.Column("IsActive").FromTable("Streets");
        }
    }
}
