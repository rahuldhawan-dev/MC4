using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class PressureSurgePotentialTypeMap : EntityLookupMap<PressureSurgePotentialType>
    {
        protected override int DescriptionLength => PressureSurgePotentialType.StringLengths.DESCRIPTION;
    }
}
