using FluentMigrator;
using MapCall.Common.Data;

namespace MapCall.Common.Model.Migrations._2016
{
    [Migration(20160715085549178), Tags("Production")]
    public class AddEmergencyEquipmentDataType : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                declare @dataTypeId int
                insert into [DataType] (Data_Type, Table_Name) values('EmergencyEquipment', 'EmergencyEquipment')
                set @dataTypeId = (select @@IDENTITY)
                insert into [DocumentType] (Document_Type, DataTypeID) values('Document', @dataTypeId)");
        }

        public override void Down()
        {
            this.DeleteDataType("EmergencyEquipment");
        }
    }
}
