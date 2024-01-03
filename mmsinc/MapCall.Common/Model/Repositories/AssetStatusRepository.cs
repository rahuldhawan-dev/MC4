using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IAssetStatusRepository : IRepository<AssetStatus> { }

    public static class AssetStatusRepositoryExtensions
    {
        public static AssetStatus GetActiveStatus(this IRepository<AssetStatus> that)
        {
            return that.Find(AssetStatus.Indices.ACTIVE);
        }

        public static AssetStatus GetCancelledStatus(this IRepository<AssetStatus> that)
        {
            return that.Find(AssetStatus.Indices.CANCELLED);
        }

        public static AssetStatus GetPendingStatus(this IRepository<AssetStatus> that)
        {
            return that.Find(AssetStatus.Indices.PENDING);
        }

        public static AssetStatus GetRemovedStatus(this IRepository<AssetStatus> that)
        {
            return that.Find(AssetStatus.Indices.REMOVED);
        }

        public static AssetStatus GetRetiredStatus(this IRepository<AssetStatus> that)
        {
            return that.Find(AssetStatus.Indices.RETIRED);
        }

        public static AssetStatus GetRequestRetirementStatus(this IRepository<AssetStatus> that)
        {
            return that.Find(AssetStatus.Indices.REQUEST_RETIREMENT);
        }
    }

    public class AssetStatusRepository : RepositoryBase<AssetStatus>, IAssetStatusRepository
    {
        #region Constructor

        public AssetStatusRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion
    }
}
