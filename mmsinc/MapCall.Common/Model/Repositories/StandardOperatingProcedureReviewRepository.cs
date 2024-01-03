using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IStandardOperatingProcedureReviewRepository : IRepository<StandardOperatingProcedureReview>
    {
        IEnumerable<StandardOperatingProcedure> GetStandardOperatingProcedureReviewsDueForUser(User user);
    }

    public class StandardOperatingProcedureReviewRepository :
        MapCallSecuredRepositoryBase<StandardOperatingProcedureReview>, IStandardOperatingProcedureReviewRepository
    {
        public override RoleModules Role
        {
            get { return RoleModules.ManagementGeneral; }
        }

        public override IQueryable<StandardOperatingProcedureReview> Linq
        {
            get
            {
                if (CurrentUserIsUserAdministrator())
                {
                    return base.Linq;
                }

                // Performance: Don't reference property getter in linq as it does a lot of extra processing.
                var user = CurrentUser;
                return base.Linq.Where(x => x.AnsweredBy == user);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                // Don't use the base CurrentUserCan
                if (CurrentUserIsUserAdministrator())
                {
                    return base.Criteria;
                }

                return base.Criteria.Add(Restrictions.Eq("AnsweredBy", CurrentUser));
            }
        }

        #region Private Methods

        private bool CurrentUserIsUserAdministrator()
        {
            if (CurrentUser.IsAdmin)
            {
                return true;
            }

            return MatchingRolesForCurrentUser.Matches.Any(x => x.Action.Id == (int)RoleActions.UserAdministrator);
        }

        #region Public Methods

        public IEnumerable<StandardOperatingProcedure> GetStandardOperatingProcedureReviewsDueForUser(User user)
        {
            // These exceptions may need to become validation related.
            var employee = user.Employee;
            if (employee == null)
            {
                throw new StandardOperatingProcedureReviewException(
                    $"User#{user.Id} does not have a linked employee record.");
            }

            if (employee.PositionGroup == null)
            {
                throw new StandardOperatingProcedureReviewException(
                    $"Unable to get reviews because the employee('{employee.EmployeeId}') does not have a position group set.");
            }

            if (employee.PositionGroup.CommonName == null)
            {
                throw new StandardOperatingProcedureReviewException(
                    $"Unable to get reviews because the employee('{employee.EmployeeId}') has a position group('{employee.PositionGroup}') without a common name.");
            }

            var actualPgcn = employee.PositionGroup.CommonName;
            StandardOperatingProcedurePositionGroupCommonNameRequirement soppgcn = null;
            RecurringFrequencyUnit freqUnit = null;

            var query = Session.QueryOver<StandardOperatingProcedure>()
                               .JoinAlias(x => x.PGCNRequirements, () => soppgcn)
                               .JoinAlias(() => soppgcn.FrequencyUnit, () => freqUnit)
                               .Where(() => soppgcn.PositionGroupCommonName == actualPgcn);

            // Need a view model(yaaaaaaaaaaaaaaaaaaaay)
            //  - SOP
            //  - Frequency
            //  - NeedByDate?

            // I don't think I even understand how frequency is supposed to work here. 

            // DONE: needs to return only SOP with the matching PGCN

            // Needs to return only SOP where the user has not done a review in the time of the frequency

            return query.List<StandardOperatingProcedure>();
        }

        #endregion

        #endregion

        public StandardOperatingProcedureReviewRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public class StandardOperatingProcedureReviewException : Exception
    {
        public StandardOperatingProcedureReviewException(string message) : base(message) { }
    }
}
