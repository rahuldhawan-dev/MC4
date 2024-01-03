using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ShortCycleCustomerMaterialRepository
        : RepositoryBase<ShortCycleCustomerMaterial>, IShortCycleCustomerMaterialRepository
    {
        public ShortCycleCustomerMaterialRepository(ISession session, IContainer container)
            : base(session, container) { }

        public ShortCycleCustomerMaterial FindByWorkOrderNumber(int workOrderNumber)
        {
            return Where(m => m.ShortCycleWorkOrderNumber == workOrderNumber)
               .SingleOrDefault();
        }
    }

    public interface IShortCycleCustomerMaterialRepository : IRepository<ShortCycleCustomerMaterial>
    {
        ShortCycleCustomerMaterial FindByWorkOrderNumber(int workOrderNumber);
    }
}
