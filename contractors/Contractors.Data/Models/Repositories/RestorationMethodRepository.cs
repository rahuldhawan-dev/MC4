using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class RestorationMethodRepository : SecuredRepositoryBase<RestorationMethod, ContractorUser>, IRestorationMethodRepository
    {
        #region Constructors

        public RestorationMethodRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<RestorationMethod> GetByRestorationTypeID(int restorationTypeId)
        {
            return (from rm in Linq
                    where
                        rm.RestorationTypes.Any(
                            rt => rt.Id == restorationTypeId)
                    select rm);
        }

        #endregion
    }
}