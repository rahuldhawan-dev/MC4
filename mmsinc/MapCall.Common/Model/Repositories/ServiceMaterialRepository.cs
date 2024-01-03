using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Repositories
{
    public static class ServiceMaterialRepositoryExtensions
    {
        public static ServiceMaterial GetLeadServiceMaterial(this IRepository<ServiceMaterial> repo)
        {
            // NOTE: DO NOT USE THE ID FOR THIS SEARCH OR YOU WILL BE IN A WORLD OF TESTING PAIN.
            // Users are allowed to enter new service materials via the generic EntityLookupController pages.
            // ServiceMaterial can not be a ReadOnlyEntityLookup
            // ServiceMaterialFactory can not be a StaticListEntityLookupFactory
            // ServiceMaterialMap can not have Id set to GeneratedBy.Assigned.

            return repo.Where(x => x.Description == "Lead").SingleOrDefault();
        }
    }
}