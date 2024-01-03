using System;
using System.Configuration;
using System.Net.Mail;

using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace Contractors.Controllers.WorkOrder
{
    public abstract class SapSyncronizedControllerBaseWithValidation<TRepository,TEntity> : MMSINC.Controllers.ControllerBaseWithPersistence
            <TRepository, TEntity, ContractorUser>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class, ISAPEntity, new()
    {
        #region Constants

        public const string SAP_ERROR_CODE = "SAPErrorCode Occurred",
            SAP_UPDATE_FAILURE = "RETRY::UPDATE FAILURE: ";

        #endregion

        #region Constructors

        protected SapSyncronizedControllerBaseWithValidation(ControllerBaseWithPersistenceArguments<TRepository, TEntity, ContractorUser> args) : base(args) {}

        #endregion

        #region Private Methods

        protected abstract void UpdateEntityForSap(TEntity entity);

        protected void UpdateSAP(int id)
        {
            var entity = Repository.Find(id);
            if (entity == null)
                throw new InvalidOperationException("The entity was reported as saved but could not be retrieved. SAP was not updated and the entity could not be marked as such.");
            try
            {
                UpdateEntityForSap(entity);
            }
            catch (Exception ex)
            {
                entity.SAPErrorCode = SAP_UPDATE_FAILURE + ex.Message;
            }
            Repository.Save(entity);
            SendSapErrorNotification(entity, id);
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

        protected void DisplaySapErrorIfApplicable(TEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.SAPErrorCode) && !entity.SAPErrorCode.StartsWith("RETRY") && !entity.SAPErrorCode.ToUpper().Contains("SUCCESSFUL"))
            {
                DisplayErrorMessage("An SAP Error has occurred and must be addressed before SAP is updated.");
            }
        }
        #endregion
    }
}