using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2021
{
    [Migration(20210430151236438), Tags("Production")]
    public class MC2975ReorderAddressLocationTypeLookup : Migration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE SampleSiteAddressLocationTypes SET [Description] = 'Facility' WHERE Id = 1");
            Execute.Sql("UPDATE SampleSiteAddressLocationTypes SET [Description] = 'Premise' WHERE Id = 2");
        }

        public override void Down()
        {
            // noop, wouldn't need to roll this back
        }
    }
}

