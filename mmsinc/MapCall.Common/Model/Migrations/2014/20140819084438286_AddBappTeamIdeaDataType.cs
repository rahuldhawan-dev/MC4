using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20140819084438286), Tags("Production")]
    public class AddBappTeamIdeaDataType : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                SET IDENTITY_INSERT DataType ON;
                INSERT INTO DataType(DataTypeID, Data_Type, Table_name) Values(166, 'BappTeamIdeas', 'BappTeamIdeas')
                SET IDENTITY_INSERT DataType OFF;
                INSERT INTO DocumentType(Document_Type, DataTypeID) Select 'Bapp Team Idea', 166");
        }

        public override void Down()
        {
            this.DeleteDataType("BappTeamIdeas");
        }
    }
}
