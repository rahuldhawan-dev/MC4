using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IContractorUserRepository : IRepository<ContractorUser>
    {
        #region Methods

        ContractorUser TryGetUserByEmail(string email);

        #endregion
    }

    public class ContractorUserRepository : RepositoryBase<ContractorUser>, IContractorUserRepository
    {
        #region Constructors

        public ContractorUserRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public ContractorUser TryGetUserByEmail(string email)
        {
            email = email.SanitizeAndDowncase();
            // SingleOrDefault because Email is a unique field.
            return Linq.SingleOrDefault(x => x.Email == email);
        }

        #endregion
    }
}
