using System;
using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230606065520828), Tags("Production")]
    public class MC5818_MakeDocumentLinkUpdatedAtNotAllowNull : Migration
    {
        public override void Up()
        {
            Update.Table("DocumentLink")
                  .Set(new {UpdatedAt = new DateTime(1900, 1, 1)})
                  .Where(new {UpdatedAt = (DateTime?)null});
            Alter.Table("DocumentLink")
                 .AlterColumn("UpdatedAt")
                 .AsDateTime()
                 .NotNullable();
        }

        public override void Down()
        {
            // no need to revert
        }
    }
}

