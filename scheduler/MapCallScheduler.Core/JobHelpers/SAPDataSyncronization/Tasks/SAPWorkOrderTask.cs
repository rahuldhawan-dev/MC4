using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks
{
    public class SAPWorkOrderTask : SAPSyncronizationTaskBase<WorkOrder, IRepository<WorkOrder>>
    {
        #region Private Members

        private readonly ISAPNewServiceInstallationRepository _nmiRepo;
        private readonly ISAPWorkOrderRepository _sapRepo;

        #endregion

        #region Constructors

        public SAPWorkOrderTask(IRepository<WorkOrder> repository, ISAPNewServiceInstallationRepository nmiRepo,
            ISAPWorkOrderRepository sapRepo, ILog log) : base(repository, log)
        {
            _nmiRepo = nmiRepo;
            _sapRepo = sapRepo;
        }

        #endregion

        #region Private Methods

        protected override IEnumerable<WorkOrder> GetEntities()
        {
            return _repository.GetWorkOrdersWithSapRetryIssuesImpl();
        }

        private void TryUpdateServiceInstallation(WorkOrder entity)
        {
            var nmiWorkOrder = _nmiRepo.Save(new SAPNewServiceInstallation(entity.ServiceInstallations.FirstOrDefault()));
            entity.SAPErrorCode = nmiWorkOrder.SAPStatus;

        }

        protected override void UpdateEntityForSap(WorkOrder entity)
        {
            try
            {
                switch (entity.SAPWorkOrderStep.Id)
                {
                    case SAPWorkOrderStep.Indices.CREATE:
                        var createSapWorkOrder = _sapRepo.Save(new SAPWorkOrder(entity));
                        createSapWorkOrder.MapToWorkOrder(entity);
                        // if this was just created successfully, we need to change the step to update
                        if (entity.SAPErrorCode.ToLower().EndsWith("successfully"))
                        {
                            entity.SAPWorkOrderStep = new SAPWorkOrderStep { Id = SAPWorkOrderStep.Indices.UPDATE };
                        }
                        break;
                    case SAPWorkOrderStep.Indices.APPROVE_GOODS:
                        var sapGoodsIssue = new SAPGoodsIssue(entity);
                        if (sapGoodsIssue.sapMaterialsUsed == null || !sapGoodsIssue.sapMaterialsUsed.Any())
                            return;
                        var goodsIssued = _sapRepo.Approve(sapGoodsIssue);
                        goodsIssued?.MapToWorkOrder(entity);
                        break;
                    case SAPWorkOrderStep.Indices.COMPLETE:
                        var completeSapWorkOrder = _sapRepo.Complete(new SAPCompleteWorkOrder(entity));
                        completeSapWorkOrder.MapToWorkOrder(entity);
                        break;
                    case SAPWorkOrderStep.Indices.UPDATE:
                        var sapWorkOrder = _sapRepo.Update(new SAPProgressWorkOrder(entity));
                        sapWorkOrder.MapToWorkOrder(entity);
                        break;
                    case SAPWorkOrderStep.Indices.UPDATE_WITH_NMI:
                        var sapUpdateNMI = _sapRepo.Update(new SAPProgressWorkOrder(entity));
                        sapUpdateNMI.MapToWorkOrder(entity);
                        if (entity.ServiceInstallations != null && entity.ServiceInstallations.Any() &&
                            WorkDescriptionRepository.NEW_SERVICE_INSTALLATIONS.Contains(entity.WorkDescription.Id))
                        {
                            TryUpdateServiceInstallation(entity);
                        }
                        break;
                    case SAPWorkOrderStep.Indices.NMI:
                        TryUpdateServiceInstallation(entity);
                        break;
                }
            }
            catch (Exception ex)
            {
                entity.SAPErrorCode = string.Format(SAP_UPDATE_FAILURE + ex.Message);
            }
        }

        #endregion
    }
}