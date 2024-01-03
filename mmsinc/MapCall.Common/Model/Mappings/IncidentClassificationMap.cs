using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class IncidentClassificationMap : ClassMap<IncidentClassification>
    {
        #region Constructors

        public IncidentClassificationMap()
        {
            Id(x => x.Id);

            Map(x => x.Description)
               .Column("Description")
                // ReSharper disable once AccessToStaticMemberViaDerivedType
               .Length(IncidentClassification.StringLengths.DESCRIPTION)
               .Not.Nullable();
        }

        #endregion
    }
}
