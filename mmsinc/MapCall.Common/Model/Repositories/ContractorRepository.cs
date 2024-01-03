using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ContractorRepository : RepositoryBase<Contractor>, IContractorRepository
    {
        #region Constructors

        public ContractorRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<Contractor> GetByOperatingCenterId(int operatingCenterId)
        {
            return (from c in Linq
                    where c.OperatingCenters.Any(x => x.Id == operatingCenterId)
                    select c);
        }

        public IEnumerable<Contractor> GetActiveContractorsByOperatingCenterId(int operatingCenterId)
        {
            return (from c in Linq
                    where c.IsActive &&
                          c.OperatingCenters.Any(x => x.Id == operatingCenterId)
                    select c);
        }

        public IEnumerable<Contractor> GetAwrContractorsForDropDown()
        {
            return from c in Linq where c.AWR == true select new Contractor {Id = c.Id, Name = c.Name};
        }

        public IEnumerable<Contractor> GetFrameworkContractorsByOperatingCenterId(int operatingCenterId)
        {
            return from c in Linq where c.FrameworkOperatingCenters.Any(x => x.Id == operatingCenterId) select c;
        }

        #endregion
    }

    public interface IContractorRepository : IRepository<Contractor>
    {
        IEnumerable<Contractor> GetByOperatingCenterId(int operatingCenterId);
        IEnumerable<Contractor> GetActiveContractorsByOperatingCenterId(int operatingCenterId);
        IEnumerable<Contractor> GetAwrContractorsForDropDown();
        IEnumerable<Contractor> GetFrameworkContractorsByOperatingCenterId(int operatingCenterId);
    }
}
