using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220516120847814), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC4446GeneralFieldUpdates : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"set identity_insert SampleSiteAddressLocationTypes ON;
                          insert into SampleSiteAddressLocationTypes (Id, Description) VALUES (7, 'Pending Acquisition');
                          set identity_insert SampleSiteAddressLocationTypes OFF;");
        }

        public override void Down()
        {
            Execute.Sql("update SampleSites set SampleSiteAddressLocationTypeId = null where SampleSiteAddressLocationTypeId = 7");
            Execute.Sql("delete from SampleSiteAddressLocationTypes where id = 7");
        }
    }
}

