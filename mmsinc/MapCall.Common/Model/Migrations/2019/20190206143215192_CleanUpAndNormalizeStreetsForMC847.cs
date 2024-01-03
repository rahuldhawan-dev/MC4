using FluentMigrator;
using MapCall.Common.ClassExtensions;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190206143215192), Tags("Production")]
    public class CleanUpAndNormalizeStreetsForMC847 : Migration
    {
        public override void Up()
        {
            Execute.Sql("update WorkOrders set StreetNumber = '41 B' where WorkorderId = 406165");
            Execute.Sql("update Streets set FullStName = 'PARK CHARLES', StreetPrefix = NULL WHERE StreetId = 193546");
            Execute.Sql("update Streets set StreetName = 'ST THOMAS', StreetPrefix = NULL where StreetId = 242546");
            Execute.Sql("update streets set FullStName = 'NE MAIN ST', StreetPrefix = 'NE' WHERE StreetId = 110973");
            Execute.Sql(
                "update Streets set StreetSuffix = 'PKWY', FullStName = StreetPrefix + ' ' + StreetName + ' PKWY' where StreetSuffix = 'PWKY'");
            Execute.Sql(
                "insert into tblNJAWStreetSuf (Suffix) select distinct ltrim(rtrim(s.StreetSuffix)) from Streets s where s.StreetSuffix IS NOT NULL and ltrim(rtrim(s.StreetSuffix)) <> '' and not exists (select 1 from tblNJAWStreetSuf where rtrim(ltrim(s.StreetSuffix)) = Suffix)");

            Create.Table("StreetSuffixes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(10).NotNullable();
            Create.Table("StreetPrefixes")
                  .WithIdentityColumn()
                  .WithColumn("Description").AsString(2).NotNullable();

            Execute.Sql(
                "insert into StreetSuffixes (Description) select distinct o.Suffix from tblNJAWStreetSuf o order by o.Suffix");
            Execute.Sql(
                "insert into StreetPrefixes (Description) select distinct o.Prefix from tblNJAWStreetPre o order by o.Prefix");

            Delete.Table("tblNJAWStreetSuf");
            Delete.Table("tblNJAWStreetPre");

            Alter.Table("Streets").AddForeignKeyColumn("SuffixId", "StreetSuffixes");
            Alter.Table("Streets").AddForeignKeyColumn("PrefixId", "StreetPrefixes");

            Execute.Sql(
                "update Streets set SuffixId = ss.Id from StreetSuffixes ss where Streets.StreetSuffix = ss.Description");
            Execute.Sql(
                "update Streets set PrefixId = sp.Id from StreetPrefixes sp where Streets.StreetPrefix = sp.Description");

            this.DeleteIndexIfItExists("Streets", "_dta_index_Streets_6_1365579903__K1_3_5_6_7_11");

            Create.Index("IX_Streets_PrefixId_StreetName_SuffixId_TownId").OnTable("Streets").OnColumn("PrefixId")
                  .Ascending()
                  .OnColumn("StreetName").Ascending().OnColumn("SuffixId").Ascending().OnColumn("TownId").Ascending();

            Delete.Column("StreetSuffix").FromTable("Streets");
            Delete.Column("StreetPrefix").FromTable("Streets");
        }

        public override void Down()
        {
            this.DeleteIndexIfItExists("Streets", "IX_Streets_PrefixId_StreetName_SuffixId_TownId");

            Alter.Table("StreetSuffixes").AddColumn("RecOrd").AsInt32().Nullable();
            Alter.Table("StreetPrefixes").AddColumn("RecOrd").AsInt32().Nullable();

            Create.Column("StreetSuffix").OnTable("Streets").AsString(10).Nullable();
            Create.Column("StreetPrefix").OnTable("Streets").AsString(10).Nullable();

            Execute.Sql(
                "update Streets set StreetSuffix = ss.Description from StreetSuffixes ss where Streets.StreetSuffixId = ss.Id");
            Execute.Sql(
                "update Streets set StreetPrefix = sp.Description from StreetPrefixes sp where Streets.StreetPrefixId = sp.Id");

            Rename.Column("Description").OnTable("StreetSuffixes").To("Suffix");
            Rename.Column("Description").OnTable("StreetPrefixes").To("Prefix");

            Create.Index("_dta_index_Streets_6_1365579903__K1_3_5_6_7_11").OnTable("Streets").OnColumn("StreetPrefix")
                  .Ascending()
                  .OnColumn("StreetName").Ascending().OnColumn("StreetSuffix").Ascending().OnColumn("TownId")
                  .Ascending();

            Delete.ForeignKeyColumn("Streets", "SuffixId", "StreetSuffixes");
            Delete.ForeignKeyColumn("Streets", "PrefixId", "StreetPrefixes");

            Rename.Column("ID").OnTable("StreetSuffixes").To("RecID");
            Rename.Column("ID").OnTable("StreetPrefixes").To("RecID");

            Rename.Table("StreetSuffixes").To("tblNJAWStreetSuf");
            Rename.Table("StreetPrefixes").To("tblNJAWStreetPrefixes");
        }
    }
}
