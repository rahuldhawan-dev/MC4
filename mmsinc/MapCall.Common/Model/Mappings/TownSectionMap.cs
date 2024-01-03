using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class TownSectionMap : ClassMap<TownSection>
    {
        #region Constructors

        public TownSectionMap()
        {
            Id(x => x.Id).Column("TownSectionID");

            Map(x => x.Name).Length(30).Not
                            .Nullable(); // This is a lie. It's nullable in the database for some reason. -Ross 12/20/2014
            Map(x => x.Abbreviation).Nullable();
            Map(x => x.ZipCode).Column("Zip").Length(10).Nullable();
            Map(x => x.Active).Not.Nullable();
            Map(x => x.MainSAPEquipmentId).Nullable();
            Map(x => x.SewerMainSAPEquipmentId).Nullable();

            References(x => x.Town).Not.Nullable();
            References(x => x.MainSAPFunctionalLocation).Nullable();
            References(x => x.SewerMainSAPFunctionalLocation).Nullable();
            References(x => x.DistributionPlanningPlant).Nullable();
            References(x => x.SewerPlanningPlant).Nullable();
        }

        #endregion
    }
}
