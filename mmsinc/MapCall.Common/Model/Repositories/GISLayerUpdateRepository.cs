using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class GISLayerUpdateRepository : RepositoryBase<GISLayerUpdate>, IGISLayerUpdateRepository
    {
        #region Constructors

        public GISLayerUpdateRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public override GISLayerUpdate Save(GISLayerUpdate entity)
        {
            if (entity.IsActive)
            {
                var active = Where(x => x.IsActive && x.Id != entity.Id);
                active.Each(update => {
                    update.IsActive = false;
                    Save(update);
                });
            }

            return base.Save(entity);
        }

        public GISLayerUpdate GetCurrent()
        {
            return Where(x => x.IsActive).Single();
        }

        #endregion
    }

    public interface IGISLayerUpdateRepository : IRepository<GISLayerUpdate>
    {
        #region Abstract Methods

        GISLayerUpdate GetCurrent();

        #endregion
    }
}
