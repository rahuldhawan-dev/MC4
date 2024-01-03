using System.Collections.Generic;
using System.Linq;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using NHibernate;
using NHibernate.SqlCommand;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class NotificationConfigurationRepository : 
        RepositoryBase<NotificationConfiguration>, INotificationConfigurationRepository
    {
        #region Constructors

        public NotificationConfigurationRepository(ISession session, IContainer container) :
            base(session, container) { }

        #endregion

        #region Properties

        public override ICriteria Criteria =>
            base.Criteria
                .CreateAlias("OperatingCenter", "criteriaOperatingCenter", JoinType.LeftOuterJoin)
                .CreateAlias("NotificationPurposes", "criteriaNotificationPurposes", JoinType.LeftOuterJoin)
                .CreateAlias("criteriaNotificationPurposes.Module", "criteriaModule", JoinType.LeftOuterJoin)
                .CreateAlias("criteriaModule.Application", "criteriaApplication", JoinType.LeftOuterJoin);

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Returns all NotificationConfigurations that match an operating center, module, and purpose.
        /// If a NotificationConfiguration has a NULL operating center, then that is also considered a match.
        /// </summary>
        public IEnumerable<NotificationConfiguration> FindByOperatingCenterModuleAndPurpose(int operatingCenterId, RoleModules module, string purpose)
        {
            return Linq.Where(x => (x.OperatingCenter == null || x.OperatingCenter.Id == operatingCenterId) &&
                                   x.NotificationPurposes.Any(y => y.Module.Id == (int)module &&
                                                                   y.Purpose == purpose));
        }

        public IEnumerable<NotificationConfiguration> FindByModuleAndPurpose(RoleModules module, string purpose)
        {
            return Linq.Where(x => x.NotificationPurposes.Any(y => y.Module.Id == (int)module &&
                                                                   y.Purpose == purpose));
        }

        public IEnumerable<NotificationConfigurationSearchResultViewModel> SearchNotificationConfigurations(SearchSet<NotificationConfiguration> searchSet)
        {
            searchSet.EnablePaging = false;
            return Search(searchSet).DistinctBy(nc => nc.Id)
                                    .SelectMany(nc => nc.NotificationPurposes.Select(np =>
                                         new NotificationConfigurationSearchResultViewModel {
                                             Id = nc.Id,
                                             ContactName = nc.Contact.ContactName,
                                             OperatingCenter = nc.OperatingCenter,
                                             Application = np.Module.Application,
                                             Module = np.Module,
                                             Purpose = np.Purpose
                                         }))
                                    .OrderBy(vm => vm.Id)
                                     // NOTE: For some reason, if you only sort by "vm.Application" or
                                     // "vm.Module"(without the Name property) you will eventually get an
                                     // error about something needing to implement IComparable. I could not
                                     // track down what data in particularly triggered this to happen.
                                    .ThenBy(vm => vm.Application.Name)
                                    .ThenBy(vm => vm.Module.Name)
                                    .ThenBy(vm => vm.Purpose);
        }

        #endregion
    }

    public interface INotificationConfigurationRepository : IRepository<NotificationConfiguration>
    {
        IEnumerable<NotificationConfiguration> FindByOperatingCenterModuleAndPurpose(int operatingCenterId, RoleModules module, string purpose);

        IEnumerable<NotificationConfiguration> FindByModuleAndPurpose(RoleModules module, string purpose);

        IEnumerable<NotificationConfigurationSearchResultViewModel> SearchNotificationConfigurations(
            SearchSet<NotificationConfiguration> searchSet);
    }
}