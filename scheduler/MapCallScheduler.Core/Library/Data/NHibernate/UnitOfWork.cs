using NHibernate;
using StructureMap;

namespace MapCallScheduler.Library.Data.NHibernate
{
    public class UnitOfWork : MMSINC.Data.V2.NHibernate.UnitOfWork
    {
        public UnitOfWork(IContainer container, ISession session) : base(container, session) { }

        protected override void SetResolver()
        {
            // noop, this seems to make a mess
        }

        protected override void DisposeSession()
        {
            // noop, the session doesn't belong to only us here
        }
    }
}