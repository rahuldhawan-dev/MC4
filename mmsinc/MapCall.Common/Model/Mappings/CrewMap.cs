using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CrewMap : ClassMap<Crew>
    {
        #region Constructors

        public CrewMap()
        {
            Id(x => x.Id, "CrewID");

            References(x => x.Contractor);
            References(x => x.OperatingCenter);

            Map(x => x.Description).Not.Nullable();
            Map(x => x.Availability).Not.Nullable();
            Map(x => x.Active).Not.Nullable();

            HasMany(x => x.CrewAssignments).KeyColumn("CrewID");
        }

        #endregion
    }
}
