using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Repositories
{
    public static class MostRecentlyInstalledServiceRepositoryExtensions
    {
        public static IQueryable<MostRecentlyInstalledService> ByInstallationNumberAndOperatingCenter(
            this IRepository<MostRecentlyInstalledService> that,
            string installation,
            int operatingCenterId)
        {
            return that.Where(s =>
                s.Premise.StatusCode.Id == PremiseStatusCode.Indices.ACTIVE &&
                s.Premise.Installation == installation &&
                (s.Premise.OperatingCenter.Id == operatingCenterId ||
                 s.Service.OperatingCenter.Id == operatingCenterId));
        }
    }
}
