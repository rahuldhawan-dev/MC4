using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PositionHistoryMap : ClassMap<PositionHistory>
    {
        public const string TABLE_NAME = "tblPosition_History";

        public PositionHistoryMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id, "Position_History_Id");

            Map(x => x.StartDate, "Position_Start_Date");
            Map(x => x.EndDate, "Position_End_Date");
            Map(x => x.StatusChangeReason, "Status_Change_Reason");
            Map(x => x.PositionSubcategory, "Position_Sub_Category");
            Map(x => x.VacationGrouping, "Vacation_Grouping");
            Map(x => x.FullyQualified, "Fully_Qualified");
            Map(x => x.OnCallRequirement, "On_Call_Requirement");

            References(x => x.Position, "Position_Id");
            References(x => x.Employee, "tblEmployeeId");
            References(x => x.Department, "DepartmentName");
            References(x => x.ReportingFacility);
            References(x => x.ScheduleType);

            HasMany(x => x.PositionHistoryDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.PositionHistoryNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
