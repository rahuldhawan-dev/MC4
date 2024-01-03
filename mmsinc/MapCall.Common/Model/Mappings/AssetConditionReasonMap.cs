using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.Mappings
{
    public class AssetConditionReasonMap : ClassMap<AssetConditionReason>
    {
        #region Constructors

        public AssetConditionReasonMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Code).Length(AssetConditionReason.StringLengths.CODE).Not.Nullable();
            Map(x => x.Description).Length(ReadOnlyEntityLookup.StringLengths.DESCRIPTION).Not.Nullable();
            References(x => x.ConditionDescription).Not.Nullable();
        }

        #endregion
    }
}
