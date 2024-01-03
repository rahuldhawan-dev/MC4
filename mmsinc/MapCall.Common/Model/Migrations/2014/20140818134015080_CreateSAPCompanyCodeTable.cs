using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140818134015080), Tags("Production")]
    public class CreateSAPCompanyCodeTable : Migration
    {
        public const int MAX_DESCRIPTION_LENGTH = 50;

        private void AddCompanyCode(string code)
        {
            Insert.IntoTable("SAPCompanyCodes").Row(new {Description = code});
        }

        public override void Up()
        {
            Create.Table("SAPCompanyCodes")
                  .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("Description").AsString(MAX_DESCRIPTION_LENGTH).NotNullable().Unique();

            // All of these were cut of at 25 characters in SAP.
            AddCompanyCode("American Water Works Comp");
            AddCompanyCode("American Water Works Serv");
            AddCompanyCode("California-American Water");
            AddCompanyCode("Hawaii-American Water Co.");
            AddCompanyCode("Illinois-American Water C");
            AddCompanyCode("Indiana-American Water Co");
            AddCompanyCode("Iowa-American Water Compa");
            AddCompanyCode("Kentucky-American Water C");
            AddCompanyCode("Maryland-American Water C");
            AddCompanyCode("Michigan-American Water C");
            AddCompanyCode("Missouri-American Water C");
            AddCompanyCode("New Jersey-American Water");
            AddCompanyCode("New York American Water C");
            AddCompanyCode("Pennsylvania-American Wat");
            AddCompanyCode("Tennessee-American Water");
            AddCompanyCode("Virginia-American Water C");
            AddCompanyCode("West Virginia-American Wa");
        }

        public override void Down()
        {
            Delete.Table("SAPCompanyCodes");
        }
    }
}
