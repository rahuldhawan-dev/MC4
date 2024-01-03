using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentStatusMap : ClassMap<EquipmentStatus>
    {
        #region Constants

        public const string TABLE_NAME = "EquipmentStatuses";

        #endregion

        #region Constructors

        public EquipmentStatusMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id, "EquipmentStatusID").GeneratedBy.Assigned();

            Map(x => x.Description).Not.Nullable();

            HasMany(x => x.Equipment).KeyColumn("StatusID");
        }

        #endregion
    }
}
