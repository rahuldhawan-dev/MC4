using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ScheduleOfValueRepository : RepositoryBase<ScheduleOfValue>, IScheduleOfValueRepository
    {
        public IEnumerable<ScheduleOfValue> GetByScheduleOfValueCategoryId(int scheduleOfValueCategoryId)
        {
            return
                (from sov in Linq where sov.ScheduleOfValueCategory.Id == scheduleOfValueCategoryId select sov)
               .ToList();
        }

        public ScheduleOfValueRepository(ISession session, IContainer container) : base(session, container) { }
    }

    public interface IScheduleOfValueRepository : IRepository<ScheduleOfValue>
    {
        IEnumerable<ScheduleOfValue> GetByScheduleOfValueCategoryId(int scheduleOfValueCategoryId);
    }
}
