using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class AssetTypeRepository : RepositoryBase<AssetType>, IAssetTypeRepository
    {
        public IEnumerable<AssetType> GetByOperatingCenterId(int operatingCenterId)
        {
            return (from t in Linq
                    where t.OperatingCenterAssetTypes.Any(x => x.OperatingCenter.Id == operatingCenterId)
                    select t);
        }

        public IEnumerable<AssetType> GetByStateId(int stateId)
        {
            return from t in Linq
                   where t.OperatingCenterAssetTypes.Any(x => x.OperatingCenter.State.Id == stateId)
                   select t;
        }

        public AssetTypeRepository(ISession session, IContainer container) : base(session, container) { }
    }

    public interface IAssetTypeRepository : IRepository<AssetType>
    {
        IEnumerable<AssetType> GetByOperatingCenterId(int operatingCenterId);

        IEnumerable<AssetType> GetByStateId(int stateId);
    }
}
