using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ContractorLaborCostRepository : RepositoryBase<ContractorLaborCost>, IContractorLaborCostRepository
    {
        public ContractorLaborCostRepository(ISession session, IContainer container) : base(session, container) { }

        public IEnumerable<ContractorLaborCost> FindByStockNumberUnitOrDescription(string partial)
        {
            return (from c in Linq
                    where c.StockNumber.Contains(partial) || c.Unit.Contains(partial) ||
                          c.JobDescription.Contains(partial)
                    select c).Take(10);
        }

        public IEnumerable<ContractorLaborCost> FindByOperatingCenterId(int operatingCenterId)
        {
            return (from c in Linq where c.OperatingCenters.Any(oc => oc.Id == operatingCenterId) select c);
        }
    }

    public interface IContractorLaborCostRepository : IRepository<ContractorLaborCost>
    {
        IEnumerable<ContractorLaborCost> FindByStockNumberUnitOrDescription(string partial);
        IEnumerable<ContractorLaborCost> FindByOperatingCenterId(int operatingCenterId);
    }
}
