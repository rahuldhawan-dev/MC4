using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ApplicationMap : ClassMap<Application>
    {
        #region Constructors

        public ApplicationMap()
        {
            Id(x => x.Id, "ApplicationID").GeneratedBy.Assigned();

            Map(x => x.Name).Not.Nullable().Unique();
        }

        #endregion
    }
}
