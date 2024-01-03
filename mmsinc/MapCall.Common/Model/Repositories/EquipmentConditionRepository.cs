using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class EquipmentConditionRepository : RepositoryBase<EquipmentCondition>, IEquipmentConditionRepository
    {
        public EquipmentConditionRepository(ISession session, IContainer container) : base(session, container) { }

        public override IQueryable<EquipmentCondition> GetAllSorted()
        {
            return GetAll().OrderBy(x => x.Id);
        }
    }

    public interface IEquipmentConditionRepository : IRepository<EquipmentCondition> { }
}
