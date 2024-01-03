using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;
using System.Collections.Generic;
using System.Linq;

namespace MapCall.Common.Model.Repositories
{
    public class MeasurementPointEquipmentTypeRepository : RepositoryBase<MeasurementPointEquipmentType>,
        IMeasurementPointEquipmentTypeRepository
    {
        #region Constructors

        public MeasurementPointEquipmentTypeRepository(ISession session, IContainer container) : base(session, container) { }
        
        #endregion

        #region Exposed Methods
        
        /// <summary>
        /// Gets all MeasurementPointEquipmentType Ids for a given EquipmentType that are being used by one or more ProductionWorkOrders.
        /// </summary>
        /// <param name="equipmentTypeId"></param>
        /// <returns>An IEnumerable of unique Ids for all MeasurementPointEquipmentTypes that are currently in use.</returns>
        public IEnumerable<int> GetAllInUse(int equipmentTypeId)
        {
            var equipmentType = _container.GetInstance<EquipmentTypeRepository>().Find(equipmentTypeId);
            
            return _container.GetInstance<ProductionWorkOrderRepository>()
                             .GetAll()
                             .SelectMany(x => x.ProductionWorkOrderMeasurementPointValues)
                             .Where(x => equipmentType.MeasurementPoints.Contains(x.MeasurementPointEquipmentType))
                             .Select(x => x.MeasurementPointEquipmentType.Id)
                             .Distinct();
        }

        public bool IsCurrentlyInUse(int measurementPointId, int equipmentTypeId) => 
            GetAllInUse(equipmentTypeId).Contains(measurementPointId);
        
        #endregion
    }

    public interface IMeasurementPointEquipmentTypeRepository : IRepository<MeasurementPointEquipmentType>
    {
        IEnumerable<int> GetAllInUse(int equipmentTypeId);
        bool IsCurrentlyInUse(int measurementPointId, int equipmentTypeId);
    }
}
