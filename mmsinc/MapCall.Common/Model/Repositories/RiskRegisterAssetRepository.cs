using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class RiskRegisterAssetRepository : RepositoryBase<RiskRegisterAsset>, IRiskRegisterAssetRepository
    {
        public RiskRegisterAssetRepository(ISession session, IContainer container) : base(session, container) { }

        public override void Delete(RiskRegisterAsset entity)
        {
            // Action items need to be deleted with the record. Reports based on action items
            // start throwing NHibernate errors when you need to reference the ActionItem.Entity property 
            // and the entity no longer exists. We need to find a better way of dealing with this, if possible.
            // Otherwise we need to implement this deletion for everything that uses action items. -Ross 8/7/2020
            this.DeleteAllActionItems(Session, entity);

            base.Delete(entity);
        }
    }

    public interface IRiskRegisterAssetRepository : IRepository<RiskRegisterAsset> {}
}
