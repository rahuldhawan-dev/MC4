using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface ISpoilStorageLocationRepository : IRepository<SpoilStorageLocation>
    {
        /// <summary>
        /// Returns all SpoilStorageLocations in a given operating center.
        /// </summary>
        IQueryable<SpoilStorageLocation> GetAllInOperatingCenter(int operatingCenterId);
    }
}