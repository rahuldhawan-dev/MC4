using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class MaterialRepository : SecuredRepositoryBase<Material, ContractorUser>, IMaterialRepository
    {
        #region Fields

        private readonly IRepository<MaterialUsed> _materialsUsedRepo;
        private readonly IWorkOrderRepository _workOrderRepository;

        #endregion

        #region Properties

        public override IQueryable<Material> Linq
        {
            get
            {
                return (from m in base.Linq
                        where
                            m.OperatingCenters.Any(
                                o =>
                                    CurrentUser.Contractor.OperatingCenters.
                                        Contains(o))
                        select m);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .CreateAlias("OperatingCenters", "oc")
                    .Add(Subqueries.Exists(
                        DetachedCriteria.For<OperatingCenter>()
                            .SetProjection(Projections.Id())
                            .CreateAlias("Contractors", "c")
                            .Add(Restrictions.EqProperty("Id",
                                "oc.Id"))
                            .Add(Restrictions.Eq("c.Id",
                                CurrentUser.Contractor.Id))));
            }
        }

        #endregion

        #region Constructors

        public MaterialRepository(ISession session,
            IAuthenticationService<ContractorUser> authenticationService,
            IContainer container, IRepository<MaterialUsed> materialsUsedRepo,
            IWorkOrderRepository workOrderRepository) :
            base(session, authenticationService, container)
        {
            _materialsUsedRepo = materialsUsedRepo;
            _workOrderRepository = workOrderRepository;
        }

        #endregion

        #region Exposed Methods
    
        public IEnumerable<Material> GetBySearchAndOperatingCenterId(string search, int operatingCenterID, bool? isActive = true)
        {
            return (from m in Linq
                    where 
                        m.OperatingCenters.Any(x => x.Id == operatingCenterID)
                        &&
                        (m.PartNumber.Contains(search) || m.Description.Contains(search)
                        &&
                        (isActive == null || m.IsActive == isActive.Value))
                    select m);
        }

        public IEnumerable<Material> GetBySearchAndMaterialUsedId(string search, int materialUsedId, bool? isActive = true)
        {
            var operatingCenterId =
                _materialsUsedRepo.Find(materialUsedId).WorkOrder.OperatingCenter.Id;
            return GetBySearchAndOperatingCenterId(search, operatingCenterId, isActive);
        }

        public IEnumerable<Material> GetBySearchAndWorkOrderId(string search, int workOrderId, bool? isActive = true)
        {
            var operatingCenterId = _workOrderRepository.Find(workOrderId).OperatingCenter.Id;
            return GetBySearchAndOperatingCenterId(search, operatingCenterId, isActive);
        }

        #endregion
    }
}
