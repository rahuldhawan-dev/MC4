using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ServiceRestorationContractorRepository : RepositoryBase<ServiceRestorationContractor>,
        IServiceRestorationContractorRepository
    {
        public ServiceRestorationContractorRepository(ISession session, IContainer container) :
            base(session, container) { }

        public IEnumerable<ServiceRestorationContractor> GetByOperatingCenterId(int operatingCenterId)
        {
            return (from src in Linq where src.OperatingCenter.Id == operatingCenterId select src);
        }
    }

    public interface IServiceRestorationContractorRepository : IRepository<ServiceRestorationContractor>
    {
        IEnumerable<ServiceRestorationContractor> GetByOperatingCenterId(int operatingCenterId);
    }
}
