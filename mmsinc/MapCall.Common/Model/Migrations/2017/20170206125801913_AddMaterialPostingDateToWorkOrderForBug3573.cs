using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170206125801913), Tags("Production")]
    public class AddMaterialPostingDateToWorkOrderForBug3573 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddColumn("MaterialPostingDate")
                 .AsDateTime()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("MaterialPostingDate").FromTable("WorkOrders");
        }
    }
}
