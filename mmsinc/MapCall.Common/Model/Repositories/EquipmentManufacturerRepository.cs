using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class EquipmentManufacturerRepository : RepositoryBase<EquipmentManufacturer>,
        IEquipmentManufacturerRepository
    {
        #region Constructors

        public EquipmentManufacturerRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<EquipmentManufacturer> GetByEquipmentTypeId(int equipmentTypeId)
        {
            return from m in Linq
                   where m.EquipmentType != null && m.EquipmentType != null &&
                         m.EquipmentType.Id == equipmentTypeId
                   select m;
        }

        public void FindOrCreate(IEnumerable<string> equipmentManufacturers, EquipmentType equipmentType)
        {
            var existing = GetByEquipmentTypeId(equipmentType.Id);
            var unmatched = equipmentManufacturers.Except(existing.Select(x => x.Description));
            foreach (var manufacturer in unmatched)
            {
                if (manufacturer != null)
                {
                    var manu = new EquipmentManufacturer {
                        EquipmentType = equipmentType,
                        Description = manufacturer
                    };
                    if (!string.IsNullOrWhiteSpace(manufacturer) && manufacturer.ToUpper() == "UNKNOWN")
                        manu.MapCallDescription = "UNKNOWN";
                    Save(manu);
                }
            }
        }

        #endregion
    }

    public interface IEquipmentManufacturerRepository : IRepository<EquipmentManufacturer>
    {
        #region Abstract Methods

        IEnumerable<EquipmentManufacturer> GetByEquipmentTypeId(int equipmentTypeId);

        /// <summary>
        /// SAP decided they are the source of record on this. This method will take their string list for
        /// a particular equipmentTypeId and check if any are missing from MapCall. If so, it will create
        /// them. It's up to business to update models after this.
        /// </summary>
        /// <param name="equipmentManufacturers"></param>
        /// <param name="equipmentType"></param>
        void FindOrCreate(IEnumerable<string> equipmentManufacturers, EquipmentType equipmentType);

        #endregion
    }
}
