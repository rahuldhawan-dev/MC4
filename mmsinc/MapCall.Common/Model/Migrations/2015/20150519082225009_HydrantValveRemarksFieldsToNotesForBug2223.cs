using FluentMigrator;

namespace MapCall.Common.Model.Migrations
{
    [Migration(20150519082225009), Tags("Production")]
    public class HydrantValveRemarksFieldsToNotesForBug2223 : Migration
    {
        public override void Up()
        {
            // Copy hydrant notes.
            Execute.Sql(@"
                declare @noteDataTypeId int
                set @noteDataTypeId = (select top 1 DataTypeId from DataType where Table_Name = 'Hydrants');

                insert into Note (Note, Date_Added, DataLinkID, DataTypeID, CreatedBy)
                select
	                '[Hydrant Record Note] ' + CONVERT(VARCHAR(MAX),Remarks) as Note,
	                getdate() as Date_Added,
	                Id as DataLinkID,
	                @noteDataTypeId as DataTypeId,
	                'mcadmin' as CreatedBy
                from Hydrants 
                where
	                Remarks is not null");
            // Copy valve notes.
            Execute.Sql(@"
                declare @noteDataTypeId int
                set @noteDataTypeId = (select top 1 DataTypeId from DataType where Table_Name = 'Valves');

                insert into Note (Note, Date_Added, DataLinkID, DataTypeID, CreatedBy)
                select
	                '[Valve Record Note] ' + CONVERT(VARCHAR(MAX),Remarks) as Note,
	                getdate() as Date_Added,
	                Id as DataLinkID,
	                @noteDataTypeId as DataTypeId,
	                'mcadmin' as CreatedBy
                from Valves 
                where
	                Remarks is not null");

            Delete.Column("Remarks").FromTable("Hydrants");
            Delete.Column("Remarks").FromTable("Valves");
        }

        public override void Down()
        {
            Alter.Table("Valves").AddColumn("Remarks").AsCustom("ntext").Nullable();
            Execute.Sql(@"
                declare @noteDataTypeId int
				set @noteDataTypeId = (select top 1 DataTypeId from DataType where Table_Name = 'Valves');
				update
					valves
				set
					Remarks = Replace(cast(Note as varchar(max)), '[Valve Record Note]', '')
				from 
					valves V
				join
					Note N
				on	
					V.Id = N.DataLinkId
				where 
					N.DataTypeID = @noteDataTypeId 
				and 
					left(cast(Note as varchar), 19) = '[Valve Record Note]'

				Delete From Note where DataTypeID = @noteDataTypeId and left(cast(Note as varchar), 19) = '[Valve Record Note]'");
            Alter.Table("Hydrants").AddColumn("Remarks").AsCustom("ntext").Nullable();
            Execute.Sql(@"
                declare @noteDataTypeId int
                set @noteDataTypeId = (select top 1 DataTypeId from DataType where Table_Name = 'Hydrants');
				update
					hydrants
				set
					Remarks = Replace(cast(Note as varchar(max)), '[Hydrant Record Note]', '')
				from 
					hydrants H
				join
					Note N
				on	
					H.Id = N.DataLinkId
				where 
					N.DataTypeID = @noteDataTypeId 
				and 
					left(cast(Note as varchar), 21) = '[Hydrant Record Note]'

				Delete From Note where DataTypeID = @noteDataTypeId and left(cast(Note as varchar), 21) = '[Hydrant Record Note]'");
        }
    }
}
