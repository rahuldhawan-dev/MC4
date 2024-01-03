using System;

namespace MMSINC.Data.NHibernate
{
    public class DummyUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Build()
        {
            throw new InvalidOperationException("Cannot create a new UnitOfWork from within the context of another");
        }

        public IUnitOfWork BuildMemoized()
        {
            throw new InvalidOperationException("Cannot create a new UnitOfWork from within the context of another");
        }
    }
}
