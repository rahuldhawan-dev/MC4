using FluentMigrator;
using MapCall.Common.ClassExtensions;

namespace MapCall.Common.Model.Migrations._2017
{
    [Migration(20170123160942637), Tags("Production")]
    public class Bug3090Incidents : Migration
    {
        public override void Up()
        {
            this.CreateLookupTableWithValues("IncidentEmployeeAvailabilityTypes", "Lost Time", "Restrictive Duty");

            Create.Table("IncidentEmployeeAvailabilities")
                  .WithColumn("Id").AsIdColumn()
                  .WithColumn("StartDate").AsDateTime().NotNullable()
                  .WithColumn("EndDate").AsDateTime().Nullable()
                  .WithColumn("IncidentId").AsInt32()
                  .ForeignKey("FK_IncidentEmployeeAvailabilities_Incidents_Id", "Incidents", "Id").NotNullable()
                  .WithColumn("IncidentEmployeeAvailabilityTypeId").AsInt32()
                  .ForeignKey("FK_IncidentEmployeeAvailabilities_IncidentEmployeeAvailabilityTypes_Id",
                       "IncidentEmployeeAvailabilityTypes", "Id").NotNullable();

            Execute.Sql(@"
declare @lostTime int;
set @lostTime = (select top 1 Id from IncidentEmployeeAvailabilityTypes where Description = 'Lost Time')

insert into IncidentEmployeeAvailabilities (IncidentId, StartDate, EndDate, IncidentEmployeeAvailabilityTypeId)
select 
IncidentId = Incidents.Id,
StartDate = Incidents.LostTimeStartDate,
EndDate = Incidents.LostTimeEndDate,
IncidentEmployeeAvailabilityTypeId = @lostTime
from Incidents
where LostTimeStartDate is not null
");

            Execute.Sql(@"
declare @rd int;
set @rd = (select top 1 Id from IncidentEmployeeAvailabilityTypes where Description = 'Restrictive Duty')
insert into IncidentEmployeeAvailabilities (IncidentId, StartDate, EndDate, IncidentEmployeeAvailabilityTypeId)
select 
IncidentId = Incidents.Id,
StartDate = Incidents.RestrictiveDutyStartDate,
EndDate = Incidents.RestrictiveDutyEndDate,
IncidentEmployeeAvailabilityTypeId = @rd
from Incidents
where RestrictiveDutyStartDate is not null
");

            Delete.Column("LostTimeStartDate").FromTable("Incidents");
            Delete.Column("LostTimeEndDate").FromTable("Incidents");
            Delete.Column("RestrictiveDutyStartDate").FromTable("Incidents");
            Delete.Column("RestrictiveDutyEndDate").FromTable("Incidents");
            Delete.Column("NumberOfLostWorkDays").FromTable("Incidents");
        }

        public override void Down()
        {
            Alter.Table("Incidents")
                 .AddColumn("LostTimeStartDate").AsDateTime().Nullable()
                 .AddColumn("LostTimeEndDate").AsDateTime().Nullable()
                 .AddColumn("RestrictiveDutyStartDate").AsDateTime().Nullable()
                 .AddColumn("RestrictiveDutyEndDate").AsDateTime().Nullable()
                 .AddColumn("NumberOfLostWorkDays").AsDecimal(18, 9).NotNullable().WithDefaultValue(decimal.Zero);

            Execute.Sql(@"
declare @lostTime int;
set @lostTime = (select top 1 Id from IncidentEmployeeAvailabilityTypes where Description = 'Lost Time')

declare @rd int;
set @rd = (select top 1 Id from IncidentEmployeeAvailabilityTypes where Description = 'Restrictive Duty')

update Incidents set LostTimeStartDate = (select top 1 StartDate from IncidentEmployeeAvailabilities where IncidentId = Incidents.Id and IncidentEmployeeAvailabilityTypeId = @lostTime)
update Incidents set LostTimeEndDate = (select top 1 EndDate from IncidentEmployeeAvailabilities where IncidentId = Incidents.Id and IncidentEmployeeAvailabilityTypeId = @lostTime)
update Incidents set RestrictiveDutyStartDate = (select top 1 StartDate from IncidentEmployeeAvailabilities where IncidentId = Incidents.Id and IncidentEmployeeAvailabilityTypeId = @rd)
update Incidents set RestrictiveDutyEndDate = (select top 1 EndDate from IncidentEmployeeAvailabilities where IncidentId = Incidents.Id and IncidentEmployeeAvailabilityTypeId = @rd)
");

            Delete.ForeignKey("FK_IncidentEmployeeAvailabilities_Incidents_Id")
                  .OnTable("IncidentEmployeeAvailabilities");
            Delete.ForeignKey("FK_IncidentEmployeeAvailabilities_IncidentEmployeeAvailabilityTypes_Id")
                  .OnTable("IncidentEmployeeAvailabilities");
            Delete.Table("IncidentEmployeeAvailabilities");
            Delete.Table("IncidentEmployeeAvailabilityTypes");
        }
    }
}
