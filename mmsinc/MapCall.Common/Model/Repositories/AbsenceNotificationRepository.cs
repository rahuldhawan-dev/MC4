using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class AbsenceNotificationRepository : MapCallEmployeeSecuredRepositoryBase<AbsenceNotification>,
        IAbsenceNotificationRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsManagement;

        #endregion

        #region Fields

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        public override RoleModules Role => ROLE;

        #endregion

        #region Constructors

        public AbsenceNotificationRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IDateTimeProvider dateTimeProvider) : base(session,
            container,
            authenticationService, roleRepo)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        #region Reports

        public IEnumerable<OccurrenceReportItem> GetNonFMLAAbsencesLessThanAYearOld(
            ISearchSet<OccurrenceReportItem> search)
        {
            var yearAgo = _dateTimeProvider.GetCurrentDate().SubtractYears(1).BeginningOfDay();

            OccurrenceReportItem result = null;
            AbsenceNotification absenceNotification = null;

            Employee employee = null;
            OperatingCenter operatingCenter = null;
            ProgressiveDiscipline progressiveDiscipline = null;

            var query = Session.QueryOver(() => absenceNotification);

            query.JoinAlias(x => absenceNotification.Employee, () => employee);
            query.JoinAlias(x => absenceNotification.ProgressiveDiscipline, () => progressiveDiscipline,
                JoinType.LeftOuterJoin);
            query.JoinAlias(x => employee.OperatingCenter, () => operatingCenter, JoinType.LeftOuterJoin);

            query.SelectList(x => x
                                 .Select(e => e.Id).WithAlias(() => result.AbsenceNotificationId)
                                 .Select(e => e.Employee).WithAlias(() => result.Employee)
                                 .Select(e => employee.OperatingCenter).WithAlias(() => result.OperatingCenter)
                                 .Select(e => e.StartDate).WithAlias(() => result.StartDate)
                                 .Select(e => e.EndDate).WithAlias(() => result.EndDate)
                                 .Select(e => e.TotalHoursOfAbsence).WithAlias(() => result.TotalHoursOfAbsence)
                                 .Select(e => e.ProgressiveDiscipline).WithAlias(() => result.ProgressiveDiscipline)
                                 .Select(e => e.SupervisorNotes).WithAlias(() => result.SupervisorNotes)
                                 .Select(e => e.HumanResourcesNotes).WithAlias(() => result.Notes)
            );

            query.Where(_ => absenceNotification.StartDate != null);
            query.Where(_ => absenceNotification.StartDate > yearAgo);
            query.Where(_ => absenceNotification.FamilyMedicalLeaveActCase == null);

            query.TransformUsing(Transformers.AliasToBean<OccurrenceReportItem>());

            return Search(search, query);
        }

        #endregion

        #endregion
    }

    public interface IAbsenceNotificationRepository : IRepository<AbsenceNotification>
    {
        #region Abstract Methods

        IEnumerable<OccurrenceReportItem> GetNonFMLAAbsencesLessThanAYearOld(ISearchSet<OccurrenceReportItem> search);

        #endregion
    }
}
