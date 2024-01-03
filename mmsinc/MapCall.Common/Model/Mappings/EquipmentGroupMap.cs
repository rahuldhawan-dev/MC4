using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentGroupMap : EntityLookupMap<EquipmentGroup>
    {
        protected override int DescriptionLength => EquipmentGroup.StringLengths.DESCRIPTION;

        #region Constructors

        public EquipmentGroupMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Code).Not.Nullable();
            Map(x => x.Definition).Length(EquipmentGroup.StringLengths.DEFINITION).Nullable();
        }

        #endregion
    }
}