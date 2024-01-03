using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20131125092307), Tags("Production")]
    public class CreateDocumentDataTable : Migration
    {
        public const int HASH_STRING_LENGTH = 40;

        public override void Up()
        {
            Create.Table("DocumentData")
                  .WithColumn("Id")
                  .AsInt32()
                  .Identity()
                  .NotNullable()
                  .PrimaryKey()
                  .WithColumn("Hash")
                  .AsFixedLengthString(HASH_STRING_LENGTH)
                  .NotNullable()
                  .Unique()
                  .WithColumn("FileSize")
                  .AsInt32()
                  .NotNullable();
        }

        public override void Down()
        {
            Delete.Table("DocumentData");
        }
    }
}
