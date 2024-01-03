using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PremiseTypeMap : ClassMap<PremiseType>
    {
        #region Constructors

        public PremiseTypeMap()
        {
            Id(x => x.Id, "PremiseTypeID");

            Map(x => x.Description).Not.Nullable().Unique().Length(PremiseType.StringLengths.DESCRIPTION);
            Map(x => x.Abbreviation).Not.Nullable().Unique().Length(PremiseType.StringLengths.ABBREVIATION);
        }

        #endregion
    }
}
