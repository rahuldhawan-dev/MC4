using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ScheduleOfValueCategoryMap : EntityLookupMap<ScheduleOfValueCategory>
    {
        public const string TABLE_NAME = "ScheduleOfValueCategories";

        public ScheduleOfValueCategoryMap()
        {
            Table(TABLE_NAME);
            References(x => x.ScheduleOfValueType).Not.Nullable();
        }
    }
}
