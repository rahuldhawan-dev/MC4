using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class TypicalOperatingPressureRangeMap : EntityLookupMap<TypicalOperatingPressureRange>
    {
        protected override int DescriptionLength => TypicalOperatingPressureRange.StringLengths.DESCRIPTION;
    }
}
