using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170207155811782), Tags("Production")]
    public class AddDateRejectedToWorkOrderForBug3579 : Migration
    {
        public override void Up()
        {
            Alter.Table("WorkOrders")
                 .AddColumn("DateRejected")
                 .AsDateTime()
                 .Nullable();
        }

        public override void Down()
        {
            Delete.Column("DateRejected").FromTable("WorkOrders");
        }
    }
}
