using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentDrugAndAlcoholTestingResultMap : ClassMap<IncidentDrugAndAlcoholTestingResult>
    {
        #region Constructors

        public IncidentDrugAndAlcoholTestingResultMap()
        {
            Id(x => x.Id);

            Map(x => x.Description)
                // ReSharper disable once AccessToStaticMemberViaDerivedType
               .Length(IncidentDrugAndAlcoholTestingResult.StringLengths.DESCRIPTION)
               .Not.Nullable();
        }

        #endregion
    }
}
