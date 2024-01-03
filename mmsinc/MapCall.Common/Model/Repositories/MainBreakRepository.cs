using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class MainBreakRepository : RepositoryBase<MainBreak>, IMainBreakRepository
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public MainBreakRepository(ISession session, IContainer container, IDateTimeProvider dateTimeProvider) : base(
            session, container)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<MainBreak> GetPowerPlantMainBreaks(ISearchSet<MainBreak> search)
        {
            WorkOrder workOrder = null;
            WorkDescription workDescription = null;

            var query = Session.QueryOver<MainBreak>();

            query.JoinAlias(x => x.WorkOrder, () => workOrder);
            query.JoinAlias(x => workOrder.WorkDescription, () => workDescription);

            query.Where(x => workDescription.Id == (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE);
            query.Where(x => workOrder.ApprovedOn != null);

            return Search(search, query);
        }

        public IQueryable<MainBreak> GetFromPastDay()
        {
            var yesterday = _dateTimeProvider.GetCurrentDate().AddDays(-1).Date;
            return from m in Linq
                   where
                       m.WorkOrder.DateCompleted.HasValue && m.WorkOrder.DateCompleted < yesterday.AddDays(1) &&
                       m.WorkOrder.DateCompleted >= yesterday
                   select m;
        }

        #endregion
    }

    public interface IMainBreakRepository : IRepository<MainBreak>
    {
        IEnumerable<MainBreak> GetPowerPlantMainBreaks(ISearchSet<MainBreak> search);
        IQueryable<MainBreak> GetFromPastDay();
    }
}
