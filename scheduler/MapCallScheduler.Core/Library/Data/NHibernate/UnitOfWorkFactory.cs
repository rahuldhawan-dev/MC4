using NHibernate;
using StructureMap;

namespace MapCallScheduler.Library.Data.NHibernate
{
    public class UnitOfWorkFactory : MMSINC.Data.V2.NHibernate.UnitOfWorkFactory
    {
        public UnitOfWorkFactory(IContainer container) : base(container) { }
    }
}
