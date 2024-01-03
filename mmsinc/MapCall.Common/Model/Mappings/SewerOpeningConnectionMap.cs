using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SewerOpeningConnectionMap : ClassMap<SewerOpeningConnection>
    {
        #region Constructors

        public SewerOpeningConnectionMap()
        {
            Id(x => x.Id);

            // TODO: Both DownstreamOpenening and UpstreamOpening should not be nullable. 
            // They're required fields and none of the values are null in the database.
            References(x => x.DownstreamOpening).Column("DownstreamSewerOpeningID").Nullable();
            References(x => x.UpstreamOpening).Column("UpstreamSewerOpeningID").Nullable();
            References(x => x.SewerPipeMaterial).Nullable();
            References(x => x.SewerTerminationType).Nullable();
            References(x => x.InspectionFrequencyUnit).Nullable();
            References(x => x.CreatedBy).Column("CreatedBy").Nullable();

            Map(x => x.IsInlet).Nullable();
            Map(x => x.Size).Nullable();
            Map(x => x.Invert).Nullable();
            Map(x => x.Route).Nullable();
            Map(x => x.Stop).Nullable();
            Map(x => x.InspectionFrequency).Column("InspFreq").Nullable();
            Map(x => x.CreatedAt).Not.Nullable();
        }

        #endregion
    }
}
