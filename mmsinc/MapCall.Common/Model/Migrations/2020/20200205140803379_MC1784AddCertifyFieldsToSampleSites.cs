using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2020
{
    [Migration(20200205140803379), Tags("Production")]
    public class MC1784AddCertifyFieldsToSampleSites : Migration
    {
        public override void Up()
        {
            Alter.Table("tblWQSample_Sites")
                 .AddColumn("CertifiedByUserId").AsInt32().Nullable()
                 .ForeignKey("tblWQSample_Sites_tblPermissions_CertifiedByUserId", "tblPermissions", "RecId")
                 .AddColumn("CertifiedDate").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.ForeignKey("tblWQSample_Sites_tblPermissions_CertifiedByUserId").OnTable("tblWQSample_Sites");
            Delete.Column("CertifiedByUserId").FromTable("tblWQSample_Sites");
            Delete.Column("CertifiedDate").FromTable("tblWQSample_Sites");
        }
    }
}
