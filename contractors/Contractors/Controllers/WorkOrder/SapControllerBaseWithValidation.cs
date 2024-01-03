using System;
using System.Configuration;
using System.Net.Mail;
using Contractors.Data.DesignPatterns.Mvc;

using Contractors.Data.Models.Repositories;
using Contractors.Models;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using StructureMap;

namespace Contractors.Controllers.WorkOrder
{
    public abstract class SapControllerWithValidationBase<TRepository, TEntity> : ControllerBaseWithValidation<TRepository, TEntity>
        where TRepository : class, IRepository<TEntity>
    where TEntity : class, new()
    {
        #region Constants

        public const string SAP_ERROR_CODE = "SAPErrorCode Occurred",
            SAP_UPDATE_FAILURE = "RETRY::UPDATE FAILURE: ";


        #endregion

        #region Constructors

        public SapControllerWithValidationBase(ControllerBaseWithPersistenceArguments<TRepository, TEntity, ContractorUser> args) : base(args) { }

        #endregion

        #region Exposed Methods

        protected void UpdateSAP(int workOrderId)
        {
            var workOrder = GetWorkOrder(workOrderId);
            if (workOrder == null)
                throw new InvalidOperationException($"{workOrderId}: The entity was reported as saved but could not be retrieved. SAP was not updated and the entity could not be marked as such.");
            if (!workOrder.OperatingCenter.SAPEnabled || !workOrder.OperatingCenter.SAPWorkOrdersEnabled || workOrder.OperatingCenter.IsContractedOperations)
                return;
            try
            {
                var sapWorkOrder = _container.GetInstance<ISAPWorkOrderRepository>().Update(new SapProgressWorkOrder(workOrder));
                workOrder.SAPErrorCode = sapWorkOrder.Status;
            }
            catch (Exception ex)
            {
                workOrder.SAPErrorCode = SAP_UPDATE_FAILURE + ex.Message;
            }
            // workOrder.SapWorkOrderStepId = SAPWorkOrderStep.Indices.UPDATE;
            workOrder.SAPWorkOrderStep = _container.GetInstance<IRepository<SAPWorkOrderStep>>().Find(SAPWorkOrderStep.Indices.UPDATE);
            _container.GetInstance<IWorkOrderRepository>().Save(workOrder);
            SendSapErrorNotification(workOrder, workOrderId);
        }

        protected void SendSapErrorNotification(ISAPEntity entity, int workOrderId)
        {
            if (!entity.SAPErrorCode.StartsWith("RETRY") && !entity.SAPErrorCode.ToUpper().Contains("SUCCESSFUL"))
            {
                using (var mm = new MailMessage())
                {
                    mm.Subject = "Contractors - Work Order - SAP Error";
                    mm.To.Add(!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AllEmailsGoTo"]) ? ConfigurationManager.AppSettings["AllEmailsGoTo"] : "mapcall@amwater.com");
                    mm.Sender = new MailAddress(ConfigurationManager.AppSettings[NotifierBase.FROM_ADDRESS_KEY]);
                    mm.Body = "Work Order: " + new SAPEntity().GetShowUrl("WorkOrder", workOrderId) + Environment.NewLine;
                    mm.Body += "Contractor Email: " + AuthenticationService.CurrentUser.Email;
                    using (var sc = new SmtpClient())
                    {
                        sc.Send(mm);
                    }
                }
            }
        }
        #endregion
    }
}