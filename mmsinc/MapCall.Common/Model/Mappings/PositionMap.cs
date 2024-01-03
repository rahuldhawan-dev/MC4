using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PositionMap : ClassMap<Position>
    {
        public const string TABLE_NAME = "tblPositions_Classifications";

        public PositionMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "PositionId");

            Map(x => x.Category);
            Map(x => x.PositionDescription, "Position");
            Map(x => x.Essential, "EssentialPosition");
            Map(x => x.OpCode);

            References(x => x.EmergencyResponsePriority, "EmergencyResponsePriority");
            References(x => x.Local, "Local_ID");
            References(x => x.LicenseRequirementAttainment, "License_Requirement_Attainment");
        }
    }
}
