using NHibernate;
using StructureMap;

namespace MapCallImporter.Library.Data
{
    public class UnitOfWorkFactory : MMSINC.Data.V2.NHibernate.UnitOfWorkFactory
    {
        public UnitOfWorkFactory(IContainer container) : base(container) { }
    }
}
