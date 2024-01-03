using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20130606094344), Tags("Production")]
    public class CreateAddressesTable : Migration
    {
        #region Consts

        public const string TABLE_NAME = "Addresses";

        public const int MAX_ADDRESS1_LENGTH = 255,
                         MAX_ADDRESS2_LENGTH = 255,
                         MAX_ZIP_CODE = 10;

        private const string TOWN_FOREIGN_KEY = "FK_Addresses_Towns_TownId";

        #endregion

        public override void Up()
        {
            Create.Table(TABLE_NAME)
                  .WithColumn("Id").AsInt32().Identity().PrimaryKey().NotNullable()
                  .WithColumn("Address1").AsString(MAX_ADDRESS1_LENGTH).NotNullable()
                  .WithColumn("Address2").AsString(MAX_ADDRESS2_LENGTH).Nullable()
                  .WithColumn("ZipCode").AsString(MAX_ZIP_CODE).NotNullable()
                  .WithColumn("TownId").AsInt32().NotNullable();

            Create.ForeignKey(TOWN_FOREIGN_KEY)
                  .FromTable(TABLE_NAME).ForeignColumn("TownId")
                  .ToTable("Towns").PrimaryColumn("TownId");

            this.Grant(TABLE_NAME, "MCUser");
        }

        public override void Down()
        {
            Delete.ForeignKey(TOWN_FOREIGN_KEY).OnTable("Addresses");
            Delete.Table(TABLE_NAME);
        }
    }
}
