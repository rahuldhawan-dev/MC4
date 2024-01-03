using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class FunctionalLocationRepository : RepositoryBase<FunctionalLocation>, IFunctionalLocationRepository
    {
        // Must use RepositoryBase<Facility> here in order to skip the criteria/linq filtering.
        private readonly RepositoryBase<Facility> _facilityRepository;

        #region Constructors

        public FunctionalLocationRepository(ISession session, IContainer container,
            RepositoryBase<Facility> facilityRepository) : base(session, container)
        {
            _facilityRepository = facilityRepository;
        }

        #endregion

        #region Exposed Methods

        public IQueryable<FunctionalLocation> GetByAssetTypeId(int assetTypeId)
        {
            return (from fl in Linq where fl.AssetType.Id == assetTypeId orderby fl.Description select fl);
        }

        public IEnumerable<FunctionalLocation> GetByTownIdAndAssetTypeId(int townId, int assetTypeId)
        {
            return (from fl in Linq
                    where fl.Town.Id == townId && fl.AssetType.Id == assetTypeId
                    orderby fl.Description
                    select fl).ToList();
        }

        public IEnumerable<FunctionalLocation> GetActiveByTownIdAndAssetTypeId(int townId, int assetTypeId)
        {
            return (from fl in Linq
                    where fl.Town.Id == townId && fl.AssetType.Id == assetTypeId && fl.IsActive
                    orderby fl.Description
                    select fl).ToList();
        }

        public IEnumerable<FunctionalLocation> GetByTownId(int townId)
        {
            return (from fl in Linq where fl.Town.Id == townId orderby fl.Description select fl).ToList();
        }

        public IEnumerable<FunctionalLocation> GetActiveByTownId(int townId)
        {
            return (from fl in Linq where fl.Town.Id == townId && fl.IsActive orderby fl.Description select fl)
               .ToList();
        }

        public IEnumerable<FunctionalLocation> GetByFacilityId(int facilityId)
        {
            var facility = _facilityRepository.Find(facilityId);
            var townId = (facility != null && facility.Town != null) ? facility.Town.Id : 0;
            return (from fl in Linq where fl.Town.Id == townId || fl.Town == null orderby fl.Description select fl);
        }

        #endregion
    }

    public interface IFunctionalLocationRepository : IRepository<FunctionalLocation>
    {
        IQueryable<FunctionalLocation> GetByAssetTypeId(int assetTypeId);
        IEnumerable<FunctionalLocation> GetByTownIdAndAssetTypeId(int townId, int assetTypeId);
        IEnumerable<FunctionalLocation> GetActiveByTownIdAndAssetTypeId(int townId, int assetTypeId);
        IEnumerable<FunctionalLocation> GetByTownId(int townId);
        IEnumerable<FunctionalLocation> GetActiveByTownId(int townId);
        IEnumerable<FunctionalLocation> GetByFacilityId(int facilityId);
    }
}
