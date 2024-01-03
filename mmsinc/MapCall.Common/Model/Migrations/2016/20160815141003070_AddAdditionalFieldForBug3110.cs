using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160815141003070), Tags("Production")]
    public class AddAdditionalFieldForBug3110 : Migration
    {
        public override void Up()
        {
            Alter.Table("tblWQSample_Sites")
                 .AddColumn("FieldVerifiedCustomerPlumbingMaterial")
                 .AsBoolean()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("FieldVerifiedCustomerPlumbingMaterial").FromTable("tblWQSample_Sites");
        }
    }
}
