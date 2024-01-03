using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MarkoutTypeMap : ClassMap<MarkoutType>
    {
        #region Constructors

        public MarkoutTypeMap()
        {
            Id(x => x.Id, "MarkoutTypeID").GeneratedBy.Assigned();

            Map(x => x.Description)
               .Length(MarkoutType.StringLengths.DESCRIPTION)
               .Not.Nullable();
        }

        #endregion
    }
}
