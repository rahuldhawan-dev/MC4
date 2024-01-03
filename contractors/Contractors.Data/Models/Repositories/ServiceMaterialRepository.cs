using System.Collections.Generic;
using System.Linq;

using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class ServiceMaterialRepository : SecuredRepositoryBase<ServiceMaterial,ContractorUser>, IServiceMaterialRepository
    {
        #region Constructors

        public ServiceMaterialRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<ServiceMaterial> GetAllButUnknown()
        {
            return (from sm in Linq
                where sm.Description.ToUpper() != "UNKNOWN"
                select sm);
        }

        #endregion
    }
}