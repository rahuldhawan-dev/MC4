using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IProductionWorkDescriptionRepository : IRepository<ProductionWorkDescription>
    {
        ProductionWorkDescription GetMaintenancePlanWorkDescription();
        ProductionWorkDescription GetCorrectiveActionWorkDescription(int? equipmentTypeId);
    }

    public class ProductionWorkDescriptionRepository : RepositoryBase<ProductionWorkDescription>, IProductionWorkDescriptionRepository
    {
        #region Constants

        private const string GENERAL_REPAIR_DESCRIPTION = "GENERAL REPAIR";

        #endregion

        #region Constructors

        public ProductionWorkDescriptionRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        public ProductionWorkDescription GetMaintenancePlanWorkDescription()
        {
            var result = Where(x => x.Description == ProductionWorkDescription.StaticDescriptions.MAINTENANCE_PLAN && x.OrderType.Id == OrderType.Indices.ROUTINE_13).FirstOrDefault();

            if (result == null)
            {
                throw new InvalidOperationException($"The Maintenance Plan work description could not be found. A ProductionWorkDescription with Description '{ProductionWorkDescription.StaticDescriptions.MAINTENANCE_PLAN}' and Order Type '{OrderType.SAPCodes.ROUTINE_13}' must exist, but could not be found.");
            }

            return result;
        }

        public ProductionWorkDescription GetCorrectiveActionWorkDescription(int? equipmentTypeId)
        {
            var result = Where(x =>
                             x.Description == GENERAL_REPAIR_DESCRIPTION && x.EquipmentType.Id == equipmentTypeId).FirstOrDefault() ??
                         Where(x => x.Description == GENERAL_REPAIR_DESCRIPTION).FirstOrDefault();

            if (result == null)
            {
                throw new InvalidOperationException($"The Production Work Order description could not be found. A ProductionWorkDescription with Order Type '{OrderType.SAPCodes.CORRECTIVE_ACTION_20}' must exist, but could not be found.");
            }

            return result;
        }
    }
}
