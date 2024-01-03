using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190506114001606), Tags("Production")]
    public class MC1168MoreFieldsForShortCycleCompletions : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrderCompletions")
                 .AddColumn("NeedTwoManCrew").AsBoolean().Nullable()
                 .AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("ShortCycleWorkOrderCompletions", "CoordinateId", "Coordinates", "CoordinateID");
            Delete.Column("NeedTwoManCrew").FromTable("ShortCycleWorkOrderCompletions");
        }
    }
}
