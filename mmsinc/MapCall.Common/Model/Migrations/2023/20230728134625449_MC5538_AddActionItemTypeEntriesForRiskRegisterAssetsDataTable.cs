using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2023
{
    [Migration(20230728134625449), Tags("Production")]
    public class MC5538_AddActionItemTypeEntriesForRiskRegisterAssetsDataTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"declare @dataTypeId int;
            select @dataTypeId = DataTypeID from DataType Where Table_Name = 'RiskRegisterAssets';
            insert into ActionItemTypes (Description,DataTypeId) values ('Study / inspection',@dataTypeId);
            insert into ActionItemTypes (Description,DataTypeId) values ('Initiate capital project (IP/RP)',@dataTypeId);
            insert into ActionItemTypes (Description,DataTypeId) values ('Update ERP',@dataTypeId);
            insert into ActionItemTypes (Description,DataTypeId) values ('Initiate Training / Tabletop Review',@dataTypeId);
            insert into ActionItemTypes (Description,DataTypeId) values ('Initiate operational change',@dataTypeId);
            insert into ActionItemTypes (Description,DataTypeId) values ('Transfer risk to 3rd party',@dataTypeId);");
        }
        
        public override void Down()
        {
            Execute.Sql(@"declare @dataTypeId int;
            select @dataTypeId = DataTypeID from DataType Where Table_Name = 'RiskRegisterAssets';
            delete from ActionItemTypes where DataTypeId = @dataTypeId;");
        }
    }
}