using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class NearMissRepository : RepositoryBase<NearMiss>, INearMissRepository
    {
        public NearMissRepository(ISession session, IContainer container) : base(session, container) { }

        public override void Delete(NearMiss entity)
        {
            // Action items need to be deleted with the record. Reports based on action items
            // start throwing NHibernate errors when you need to reference the ActionItem.Entity property 
            // and the entity no longer exists. We need to find a better way of dealing with this, if possible.
            // Otherwise we need to implement this deletion for everything that uses action items. -Ross 8/7/2020
            this.DeleteAllActionItems(Session, entity);

            base.Delete(entity);
        }
    }

    public interface INearMissRepository : IRepository<NearMiss> { }

    public static class NearMissRepositoryExtensions
    {
        public static IQueryable<NearMiss> GetNearMissesInPriorOneDay(
            this IRepository<NearMiss> that,
            IDateTimeProvider dateTimeProvider, int nearMissTypeId)
        {
            return that.Where(x =>
                x.CreatedAt >= dateTimeProvider.GetCurrentDate().AddDays(-1) &&
                x.Type.Id == nearMissTypeId);
        }
    }
}
