using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140910085734204), Tags("Production")]
    public class CreateUserTypesTable : Migration
    {
        #region Consts

        public const int MAX_DESCRIPTION_LENGTH = 50;

        #endregion

        public override void Up()
        {
            Create.Table("UserTypes")
                  .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                  .WithColumn("Description").AsString(MAX_DESCRIPTION_LENGTH).NotNullable().Unique();

            Insert.IntoTable("UserTypes").Row(new {Description = "Internal"});
            Insert.IntoTable("UserTypes").Row(new {Description = "Internal Contractor"});
            Insert.IntoTable("UserTypes").Row(new {Description = "External Contractor"});
        }

        public override void Down()
        {
            Delete.Table("UserTypes");
        }
    }
}
