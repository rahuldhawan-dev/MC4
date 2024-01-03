using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class EquipmentTypeRepository : RepositoryBase<EquipmentType>, IEquipmentTypeRepository
    {
        #region Constructors

        public EquipmentTypeRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IQueryable<EquipmentType> GetWithNoEquipmentPurpose()
        {
            return from t in Linq where !t.EquipmentPurposes.Any() select t;
        }

        #endregion
    }

    public interface IEquipmentTypeRepository : IRepository<EquipmentType>
    {
        #region Abstract Methods

        IQueryable<EquipmentType> GetWithNoEquipmentPurpose();

        #endregion
    }
}
