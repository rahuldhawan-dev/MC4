using FluentMigrator;

namespace MapCall.Common.Model.Migrations._2019
{
    [Migration(20190906102601372), Tags("Production")]
    public class MC1502SplitUnknownOtherEquipmentManufacturers : Migration
    {
        public override void Up()
        {
            // Delete the TypeAndDescription constraint because it will no longer be valid.
            Delete.ForeignKey("TypeAndDescription").OnTable("SAPEquipmentManufacturers");

            // Rename the existing SapEquipmentManufacturers from "UNKNOWN/OTHER" to just "UNKNOWN"
            Update.Table("SAPEquipmentManufacturers").Set(new {MapCallDescription = "UNKNOWN"})
                  .Where(new {MapCallDescription = "UNKNOWN/OTHER"});

            // Create new SapEquipmentManufacturers that are the same as the old ones but with "OTHER"
            Execute.Sql(@"
insert into SAPEquipmentManufacturers (SAPEquipmentTypeID, Description, MapCallDescription)
select
    SAPEquipmentTypeID,
    'UNKNOWN',
    'OTHER'
from SAPEquipmentManufacturers
where MapCallDescription = 'UNKNOWN'");

            // Update existing Equipment to have "OTHER" if ManufacturerOther is not null
            Execute.Sql(@"
update 
	e
set SAPEquipmentManufacturerId = (select sm2.Id from SAPEquipmentManufacturers sm2 where sm2.MapCallDescription = 'OTHER' and sm2.SAPEquipmentTypeId = sm.SAPEquipmentTypeId)
from Equipment e
inner join SAPEquipmentManufacturers sm on sm.Id = e.SAPEquipmentManufacturerId
where e.ManufacturerOther is not null and sm.MapCallDescription = 'UNKNOWN' 
");
        }

        public override void Down()
        {
            // Reset Equipment values back to their unsplit versions
            Execute.Sql(@"
update 
	e
set SAPEquipmentManufacturerId = (select sm2.Id from SAPEquipmentManufacturers sm2 where sm2.MapCallDescription = 'UNKNOWN' and sm2.SAPEquipmentTypeId = sm.SAPEquipmentTypeId)
from Equipment e
inner join SAPEquipmentManufacturers sm on sm.Id = e.SAPEquipmentManufacturerId
where e.ManufacturerOther is not null and sm.MapCallDescription = 'OTHER' 
");

            // Remove the "OTHER" manufacturers.
            Delete.FromTable("SAPEquipmentManufacturers").Row(new {MapCallDescription = "OTHER"});

            // Rename back to original
            Update.Table("SAPEquipmentManufacturers").Set(new {MapCallDescription = "UNKNOWN/OTHER"})
                  .Where(new {MapCallDescription = "UNKNOWN"});

            // Add back the TypeAndDescription constraint
            Execute.Sql(
                @"ALTER TABLE [dbo].[SAPEquipmentManufacturers] ADD  CONSTRAINT [TypeAndDescription] UNIQUE NONCLUSTERED 
(
	[SAPEquipmentTypeId] ASC,
	[Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
");
        }
    }
}
