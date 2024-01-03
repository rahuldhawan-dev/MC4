using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230907113441796), Tags("Production")]
    public class MC6063_AddDataTypeForUsers : Migration
    {
        private const string USERS_TABLE = "tblPermissions";

        public override void Up()
        {
            this.AddDataType(USERS_TABLE);
            this.AddDocumentType("General", USERS_TABLE);
        }

        public override void Down()
        {
            this.RemoveDocumentTypeAndAllRelatedDocuments("General", USERS_TABLE);
            this.RemoveDataType(USERS_TABLE);
        }
    }
}

