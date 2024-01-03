using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180328121923925), Tags("Production")]
    public class MC287 : Migration
    {
        public override void Up()
        {
            //Adds new columns
            Alter.Table("PDESSystems").AddColumn("IsConsentOrder").AsBoolean().Nullable();
            Alter.Table("PDESSystems").AddColumn("IsNewAcquisition").AsBoolean().Nullable();
        }

        public override void Down()
        {
            //Deletes new columns
            Delete.Column("IsConsentOrder").FromTable("PDESSystems");
            Delete.Column("IsNewAcquisition").FromTable("PDESSystems");
        }
    }
}
