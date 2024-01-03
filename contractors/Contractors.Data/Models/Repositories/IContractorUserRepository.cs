using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface IContractorUserRepository : IRepository<ContractorUser>
    {
        // Ya'll can deal with my intellisense documentation!

        #region Methods

        /// <summary>
        /// Gets an existing user that has the given email. Returns null if the user does not exist.
        /// </summary>
        ContractorUser GetUser(string email);

        /// <summary>
        /// Gets whether the user is an administrator.
        /// </summary>
        bool IsAdmin(string email);

        #endregion
    }
}