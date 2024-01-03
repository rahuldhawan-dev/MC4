using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190313091613111), Tags("Production")]
    public class MC1063AddNewShortCycleWorkOrderColumns : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleWorkOrders")
                 .AddColumn("BeforeAddress").AsAnsiString().Nullable()
                 .AddColumn("AfterAddress").AsAnsiString().Nullable()
                 .AddForeignKeyColumn("CoordinateId", "Coordinates", "CoordinateID")
                 .AddColumn("MatCodeEscalator").AsAnsiString()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("MatCodeEscalator").FromTable("ShortCycleWorkOrders");
            Delete.ForeignKeyColumn("ShortCycleWorkOrders", "CoordinateId", "Coordinates", "CoordinateID");
            Delete.Column("AfterAddress").FromTable("ShortCycleWorkOrders");
            Delete.Column("BeforeAddress").FromTable("ShortCycleWorkOrders");
        }
    }
}
