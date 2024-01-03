using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class EnvironmentalNonComplianceEventActionItemRepository : 
        MapCallSecuredRepositoryBase<EnvironmentalNonComplianceEventActionItem>, IEnvironmentalNonComplianceEventActionItemRepository
    {
        public EnvironmentalNonComplianceEventActionItemRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        public override RoleModules Role => RoleModules.EnvironmentalGeneral;
    }

    public interface IEnvironmentalNonComplianceEventActionItemRepository : IRepository<EnvironmentalNonComplianceEventActionItem> { }

    public static class EnvironmentalNonComplianceEventActionItemRepositoryExtensions
    {
        public static IQueryable<EnvironmentalNonComplianceEventActionItem> GetAllActionItemsEvery30DaysFromEstimatedCompletion(this IRepository<EnvironmentalNonComplianceEventActionItem> that, IDateTimeProvider dateTimeProvider)
        {
            return that.Where(x =>
                x.In30DayIntervalFromTargetDate == true && x.DateCompleted == null);
        }
    }
}