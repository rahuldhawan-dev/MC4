using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IUserViewedRepository : IRepository<UserViewed>
    {
        IEnumerable<UserViewedDailyRecordItem> SearchDailyReportItems(ISearchUserViewedDailyRecordItem search);
        IEnumerable<UserViewed> SearchWithImages(ISearchSet<UserViewed> search);
    }

    public class UserViewedRepository : RepositoryBase<UserViewed>, IUserViewedRepository
    {
        #region Constructors

        public UserViewedRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public methods

        /// <summary>
        /// Accepts basic search parameters, except it does all of the joins for the images and their towns
        /// for performance. Used by the UserViewedController in MVC.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public IEnumerable<UserViewed> SearchWithImages(ISearchSet<UserViewed> search)
        {
            // This method is for making sure the search results do proper joins
            // so we aren't sending out a million queries.
            // NOTE: There is a unit test for this to ensure that only a single query is generated
            // and that the results don't also cause additional queries. If you update this, make sure
            // the test is still valid.
            var query = Session.QueryOver<UserViewed>();
            TapImage tap = null;
            ValveImage valve = null;
            AsBuiltImage asBuilt = null;
            Town tapTown = null;
            Town valveTown = null;
            Town asBuiltTown = null;

            query.JoinAlias(x => x.TapImage, () => tap, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
                 .JoinAlias(x => x.ValveImage, () => valve, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
                 .JoinAlias(x => x.AsBuiltImage, () => asBuilt, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
                 .JoinAlias(() => tap.Town, () => tapTown, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
                 .JoinAlias(() => valve.Town, () => valveTown, NHibernate.SqlCommand.JoinType.LeftOuterJoin)
                 .JoinAlias(() => asBuilt.Town, () => asBuiltTown, NHibernate.SqlCommand.JoinType.LeftOuterJoin);

            return Search(search, query);
        }

        public IEnumerable<UserViewedDailyRecordItem> SearchDailyReportItems(ISearchUserViewedDailyRecordItem search)
        {
            var query = Session.QueryOver<UserViewed>();
            UserViewedDailyRecordItem result = null;
            User user = null; // SearchAlias on viewmodel relies on this being "user".
            query.JoinAlias(x => x.User, () => user);
            Employee emp = null;
            query.JoinAlias(() => user.Employee, () => emp,
                NHibernate.SqlCommand.JoinType.LeftOuterJoin); // Not all users have employees.

            var zero = Projections.Constant(0);
            var one = Projections.Constant(1);

            query.SelectList(x => x
                                  // Casting to date is much cleaner and works on SQL Server. It does not work on sqlite(only returns year for some reason), so we have to do the garbage below.
                                  //.Select(Projections.Cast(NHibernateUtil.Date, Projections.GroupProperty("ViewedAt"))).WithAlias(() => result.ViewedAt)
                                 .Select(Projections.GroupProperty(Projections.SqlFunction("date",
                                      NHibernateUtil.DateTime, Projections.Property(nameof(UserViewed.ViewedAt)))))
                                 .WithAlias(() => result.ViewedAt)
                                 .SelectGroup(() => user.Id).WithAlias(() => result.UserId)
                                 .SelectGroup(() => user.UserName).WithAlias(() => result.Username)
                                 .SelectGroup(() => user.Address).WithAlias(() => result.UserAddress)
                                 .SelectGroup(() => emp.EmployeeId).WithAlias(() => result.EmployeeId)
                                 .Select(Projections.Sum(
                                      Projections.Conditional(Restrictions.IsNull(nameof(UserViewed.TapImage)), zero,
                                          one))).WithAlias(() => result.TapImages)
                                 .Select(Projections.Sum(Projections.Conditional(
                                      Restrictions.IsNull(nameof(UserViewed.ValveImage)),
                                      Projections.Constant(0),
                                      Projections.Constant(1)
                                  ))).WithAlias(() => result.ValveImages)
                                 .Select(Projections.Sum(Projections.Conditional(
                                      Restrictions.IsNull(nameof(UserViewed.AsBuiltImage)),
                                      Projections.Constant(0),
                                      Projections.Constant(1)
                                  ))).WithAlias(() => result.AsBuiltImages)
            );

            query = query.OrderBy(x => x.ViewedAt).Desc().ThenBy(() => user.UserName).Asc();

            query = query.TransformUsing(Transformers.AliasToBean<UserViewedDailyRecordItem>());
            return Search(search, query);
        }

        #endregion
    }
}
