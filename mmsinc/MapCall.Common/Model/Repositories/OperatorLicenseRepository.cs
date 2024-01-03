using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class OperatorLicenseRepository : MapCallEmployeeSecuredRepositoryBase<OperatorLicense>,
        IOperatorLicenseRepository
    {
        public override RoleModules Role => RoleModules.HumanResourcesEmployeeLimited;

        public OperatorLicenseRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService) : base(session, container, authenticationService,
            roleRepo) { }

        public IEnumerable<OperatorLicense> SearchOperatorLicenseReport(ISearchOperatorLicenseReport search)
        {
            var query = Session.QueryOver<OperatorLicense>();

            if (search.Expired.HasValue)
            {
                // Expiration based on the date, time is 00:00:00:000 for this field
                // license normally expires at the end of the month so if the expiration date is 4/30 and it's 4/30/20 - the license should still be valid
                var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                if (search.Expired == true)
                {
                    query = query.Where(x => now.Date > x.ExpirationDate);
                }
                else
                {
                    query = query.Where(x => now.Date <= x.ExpirationDate);
                }
            }

            return Search(search, query);
        }
    }

    public interface IOperatorLicenseRepository : IRepository<OperatorLicense>
    {
        IEnumerable<OperatorLicense> SearchOperatorLicenseReport(ISearchOperatorLicenseReport search);
    }
}
