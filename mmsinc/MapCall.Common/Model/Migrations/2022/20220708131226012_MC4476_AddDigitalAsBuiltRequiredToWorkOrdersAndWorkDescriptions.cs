using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220708131226012), Tags("Production")]
    public class MC4476_AddDigitalAsBuiltRequiredToWorkOrdersAndWorkDescriptions : AutoReversingMigration
    {
        public override void Up()
        {
            Alter
               .Table("WorkOrders")
               .AddColumn("DigitalAsBuiltRequired").AsBoolean().NotNullable().WithDefaultValue(false);
            Alter
               .Table("WorkDescriptions")
               .AddColumn("DigitalAsBuiltRequired").AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}