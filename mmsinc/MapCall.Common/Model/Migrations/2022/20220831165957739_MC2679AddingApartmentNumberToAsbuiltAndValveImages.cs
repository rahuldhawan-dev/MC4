using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220831165957739), Tags("Production")]
    public class MC2679AddingApartmentNumberToAsbuiltAndValveImages : Migration
    {
        public override void Up()
        {
            Alter.Table("AsBuiltImages").AddColumn("ApartmentNumber").AsAnsiString(50).Nullable();
            Alter.Table("ValveImages").AddColumn("ApartmentNumber").AsAnsiString(50).Nullable();
        }

        public override void Down()
        {
            Delete.Column("ApartmentNumber").FromTable("ValveImages");
            Delete.Column("ApartmentNumber").FromTable("AsBuiltImages");
        }
    }
}

