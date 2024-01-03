using FluentMigrator;
using MMSINC.ClassExtensions.FluentMigratorExtensions;

namespace MapCall.Common.Model.Migrations._2018
{
    [Migration(20180327114752101), Tags("Production")]
    public class CreateCompanySubsidiaries : Migration
    {
        public override void Up()
        {
            //Creates table
            Create.Table("CompanySubsidiaries")
                  .WithIdentityColumn("Id")
                  .WithColumn("Description").AsString();
            //Inserts list of subsidiaries into table
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('California American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Hawaii American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Illinois American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Indiana American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Iowa American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Kentucky American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Maryland American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Michigan American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Missouri American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('New Jersey American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('New York American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Pennsylvania American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Tennessee American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('Virginia American Water')");
            Execute.Sql("INSERT INTO CompanySubsidiaries (Description) VALUES ('West Virginia American Water')");
            //creates new int CompanySubsidiary column
            Alter.Table("tblFacilities").AddForeignKeyColumn("CompanySubsidiaryID", "CompanySubsidiaries");
            // update exisiting values to 
            Execute.Sql(
                "UPDATE tblFacilities SET tblFacilities.CompanySubsidiaryID = (select Id FROM CompanySubsidiaries WHERE Description = 'New Jersey American Water') WHERE tblFacilities.CompanySubsidiary = 'New Jersey American Water Company'");
            Execute.Sql(
                "UPDATE tblFacilities SET tblFacilities.CompanySubsidiaryID = (select Id FROM CompanySubsidiaries WHERE Description = 'New York American Water') WHERE tblFacilities.CompanySubsidiary = 'Long Island Water Corporation'");
            //deletes old string CompanySubsidiary column
            Delete.Column("CompanySubsidiary").FromTable("tblFacilities");
        }

        public override void Down()
        {
            //Adds back string CompanySubsidiary column 
            Alter.Table("tblFacilities").AddColumn("CompanySubsidiary").AsString().Nullable();

            //reverts CompanySubsidiary back to original string data
            Execute.Sql(
                "UPDATE tblFacilities SET tblFacilities.CompanySubsidiary = 'New Jersey American Water Company' WHERE CompanySubsidiaryID = 10");
            Execute.Sql(
                "UPDATE tblFacilities SET tblFacilities.CompanySubsidiary = 'Long Island Water Corporation' WHERE CompanySubsidiaryID = 11");

            //deletes int CompanySubsidiary column
            Delete.ForeignKeyColumn("tblFacilities", "CompanySubsidiaryID", "CompanySubsidiaries");

            //Deletes new table
            Delete.Table("CompanySubsidiaries");
        }
    }
}
