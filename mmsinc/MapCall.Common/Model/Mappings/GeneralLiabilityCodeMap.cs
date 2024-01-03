using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class GeneralLiabilityCodeMap : ClassMap<GeneralLiabilityCode>
    {
        #region Constructors

        public GeneralLiabilityCodeMap()
        {
            Id(x => x.Id);

            Map(x => x.Description)
               .Column("Description")
                // ReSharper disable once AccessToStaticMemberViaDerivedType
               .Length(GeneralLiabilityCode.StringLengths.DESCRIPTION)
               .Not.Nullable();
        }

        #endregion
    }
}
