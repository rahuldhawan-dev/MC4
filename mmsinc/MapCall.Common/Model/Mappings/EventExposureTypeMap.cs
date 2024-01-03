using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EventExposureTypeMap : ClassMap<EventExposureType>
    {
        #region Constructors

        public EventExposureTypeMap()
        {
            Table("EventExposureTypes");
            Id(x => x.Id);
            Map(x => x.Description)
                // ReSharper disable once AccessToStaticMemberViaDerivedType
               .Length(IncidentType.StringLengths.DESCRIPTION)
               .Not.Nullable();
        }

        #endregion
    }
}
