using NHibernate;

namespace MMSINC.Data.V2.NHibernate
{
    public static class UnitOfWorkExtensions
    {
        public static void Evict<TEntity>(this IUnitOfWork that, TEntity entity)
        {
            that.Container.GetInstance<ISession>().Evict(entity);
        }
    }
}
