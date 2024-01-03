using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PipeDataLookupTypeMap : ClassMap<PipeDataLookupType>
    {
        #region Constructors

        public PipeDataLookupTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            Map(x => x.Description).Not.Nullable();

            HasMany(x => x.PipeDataLookupValues).KeyColumn("PipeDataLookupTypeID");
        }

        #endregion
    }
}
