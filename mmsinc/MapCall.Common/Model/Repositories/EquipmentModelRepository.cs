using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class EquipmentModelRepository : RepositoryBase<EquipmentModel>, IEquipmentModelRepository
    {
        #region Constructors

        public EquipmentModelRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<EquipmentModel> GetByEquipmentManufacturerId(int equipmentManufacturerId)
        {
            return (from em in Linq where em.EquipmentManufacturer.Id == equipmentManufacturerId select em);
        }

        #endregion
    }

    public interface IEquipmentModelRepository : IRepository<EquipmentModel>
    {
        IEnumerable<EquipmentModel> GetByEquipmentManufacturerId(int equipmentManufacturerId);
    }
}
