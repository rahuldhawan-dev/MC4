using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class CountyRepository : SecuredRepositoryBase<County, ContractorUser>, ICountyRepository
    {
        #region Constructors

        public CountyRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<County> GetByStateId(int stateId)
        {
            return (from c in Linq where c.State.Id == stateId select c);
        }

        #endregion
    }
}
