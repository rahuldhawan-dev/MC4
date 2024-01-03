using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2022
{
    [Migration(20220328141225008), Tags("Production")]
    // ReSharper disable once InconsistentNaming
    public class MC3896LinkSampleSiteToAnAsset : Migration
    {
        public override void Up()
        {
            Alter.Table("SampleSites").AddForeignKeyColumn("HydrantId", "Hydrants");
            Alter.Table("SampleSites").AddForeignKeyColumn("ValveId", "Valves");

            Execute.Sql(@"set identity_insert SampleSiteAddressLocationTypes ON;
                          insert into SampleSiteAddressLocationTypes (Id, Description) VALUES (4, 'Hydrant');
                          insert into SampleSiteAddressLocationTypes (Id, Description) VALUES (6, 'Valve');
                          set identity_insert SampleSiteAddressLocationTypes OFF;");
        }

        public override void Down()
        {
            Delete.ForeignKeyColumn("SampleSites", "ValveId", "Valves");
            Delete.ForeignKeyColumn("SampleSites", "HydrantId", "Hydrants");
            Execute.Sql("delete from SampleSiteAddressLocationTypes where id in (4, 6)");
        }
    }
}

