using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160930150011958), Tags("Production")]
    public class AddDataTypeForPositionGroupsForBug3201 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"declare @newDataTypeId int;
insert into DataType (Data_Type, Table_Name) select Data_Type, 'PositionGroups' from DataType where Table_Name = 'tblPositions_Classifications'
select @newDataTypeId = @@IDENTITY;
insert into DocumentType (Document_Type, DataTypeId) select doc.Document_Type, @newDataTypeId from DocumentType doc inner join DataType dat on doc.DataTypeID = dat.DataTypeID where dat.Table_Name = 'tblPositions_Classifications'");
        }

        public override void Down()
        {
            Execute.Sql(
                @"delete from DocumentType where DataTypeId = (select DataTypeId from DataType where Table_Name = 'PositionGroups');
delete from DataType where Table_Name = 'PositionGroups';");
        }
    }
}
