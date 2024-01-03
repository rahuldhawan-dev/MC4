using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations._2023;

namespace MapCall.Common.Model.Mappings
{
    public class CurrentAssignmentMap : ClassMap<CurrentAssignment>
    {
        public CurrentAssignmentMap()
        {
            ReadOnly();
            Table(MC5434_CreateCurrentAssignmentView.VIEW_NAME);

            Id(x => x.Id).Column("WorkOrderID");

            References(x => x.Crew).Not.Nullable().Not.Insert().Not.Update();
            References(x => x.CrewAssignment).Not.Nullable().Not.Insert().Not.Update();

            Map(x => x.AssignedFor).Nullable();
            Map(x => x.CrewName).Not.Nullable();
            // ensure this isn't created as a table during schema export since it uses a view
            SchemaAction.None();
        }
    }
}
