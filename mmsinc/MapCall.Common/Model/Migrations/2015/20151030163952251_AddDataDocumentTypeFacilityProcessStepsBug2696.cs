using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20151030163952251), Tags("Production")]
    public class AddDataDocumentTypeFacilityProcessStepsBug2696 : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                declare @dataTypeId int                
                INSERT INTO DataType VALUES('FacilityProcessStep', 'FacilityProcessSteps', null);
                select @dataTypeId = (select @@Identity)
                INSERT INTO [DocumentType](Document_Type, DataTypeId) Values('Script', @dataTypeId);
                ");
        }

        public override void Down()
        {
            this.RemoveDataType("FacilityProcessSteps");
        }
    }
}
