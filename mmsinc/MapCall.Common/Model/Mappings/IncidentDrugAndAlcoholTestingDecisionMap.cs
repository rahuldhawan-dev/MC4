using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentDrugAndAlcoholTestingDecisionMap : ClassMap<IncidentDrugAndAlcoholTestingDecision>
    {
        #region Constructors

        public IncidentDrugAndAlcoholTestingDecisionMap()
        {
            Id(x => x.Id);

            Map(x => x.Description)
                // ReSharper disable once AccessToStaticMemberViaDerivedType
               .Length(IncidentDrugAndAlcoholTestingDecision.StringLengths.DESCRIPTION)
               .Not.Nullable();
        }

        #endregion
    }
}
