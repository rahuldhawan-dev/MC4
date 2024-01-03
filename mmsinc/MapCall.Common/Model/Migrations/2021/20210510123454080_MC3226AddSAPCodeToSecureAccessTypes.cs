using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210510123454080), Tags("Production")]
    public class MC3226AddSAPCodeToSecureAccessTypes : Migration
    {
        public override void Up()
        {
            Alter.Table("ShortCycleSecureAccessTypes")
                 .AddColumn("SAPCode").AsAnsiString(2).Nullable();
            Execute.Sql("UPDATE ShortCycleSecureAccessTypes SET SAPCode = right('00' + cast(Id as varchar), 2);");
            Alter.Column("SAPCode").OnTable("ShortCycleSecureAccessTypes").AsAnsiString(2).NotNullable();
            Insert.IntoTable("ShortCycleSecureAccessTypes").Row(new { Description = "Not Secure", SAPCode = "00" });
        }

        public override void Down()
        {
            Delete.Column("SAPCode").FromTable("ShortCycleSecureAccessTypes");
        }
    }
}