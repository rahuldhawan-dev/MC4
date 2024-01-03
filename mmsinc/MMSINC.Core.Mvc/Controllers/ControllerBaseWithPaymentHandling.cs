using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using AuthorizeNet;
using AuthorizeNet.Utility.NotProvided;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Controllers
{
    public abstract class
        ControllerBaseWithPaymentHandling<TRepository, TEntity, TUser> : ControllerBaseWithPersistence<TRepository,
            TEntity, TUser>
        where TRepository : class, IRepository<TEntity>
        where TEntity : class, IValidatableObject, IEntityWithPayment, new()
        where TUser : class, IAdministratedUser, IUserWithProfile
    {
        #region Constants

        public const string ENTITY_NOT_FOUND = "Unable to locate the data for id: {0}";
        public const string PROFILE_NOT_FOUND = "Payment profile not found.";

        #endregion

        #region Private Members

        private Customer _currentCustomer;

        #endregion

        #region Properties

        public ICustomerGateway CustomerGateway
        {
            get { return _container.GetInstance<IExtendedCustomerGateway>(); }
        }

        public Customer CurrentCustomer
        {
            get
            {
                return _currentCustomer ??
                       (_currentCustomer =
                           CustomerGateway.GetCustomer(AuthenticationService.CurrentUser.CustomerProfileId.ToString()));
            }
        }

        #endregion

        #region Private Methods

        protected ActionResult VerifyEntityAndProfile(int entityId, string profileId,
            Antlr.Runtime.Misc.Func<ActionResult> onValid)
        {
            if (!Repository.Exists(entityId))
                return HttpNotFound(String.Format(ENTITY_NOT_FOUND, entityId));

            if (!CurrentCustomer.PaymentProfiles.Any(x => x.ProfileID == profileId))
            {
                return HttpNotFound(PROFILE_NOT_FOUND);
            }

            return onValid();
        }

        protected ActionResult DoProcessPayment(VerifyPaymentSummary<TEntity> entity)
        {
            IGatewayResponse response;

            entity.Entity = Repository.Find(entity.Id);

            response =
                CustomerGateway.AuthorizeAndCapture(new Order(CurrentCustomer.ProfileID,
                    entity.SelectedPaymentProfileId,
                    null) {
                    Amount = entity.Entity.TotalCharged.Value,
                    InvoiceNumber = GetInvoiceNumber(entity.Entity),
                    Description = GetDescription(entity.Entity),
                    PONumber = GetPONumber(entity.Entity)
                });
            return HandlePaymentCaptureResult(entity, response);
        }

        /// <summary>
        /// Override this to set the description that will appear on authorize.net 
        /// for the transaction.
        /// TODO: Bug 2642 - permits needs to implement this
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDescription(TEntity entity)
        {
            return string.Empty;
        }

        protected virtual string GetInvoiceNumber(TEntity entity)
        {
            return entity.InvoiceNumber;
        }

        protected virtual string GetPONumber(TEntity entity)
        {
            return null;
        }

        protected ActionResult HandlePaymentCaptureResult(VerifyPaymentSummary<TEntity> entity,
            IGatewayResponse response)
        {
            var model = new Payment<TEntity> {
                Entity = entity.Entity,
                Response = response
            };

            if (!response.Approved)
            {
                return PartialView("~/Views/Authorize/_RejectedPayment.cshtml", model.Response);
            }

            UpdatePaymentInformation(entity.Entity, response, entity.SelectedPaymentProfileId);
            // ReSharper disable once Mvc.ActionNotResolved
            return RedirectToAction("SuccessfulPayment", new {entity.Id});
        }

        protected void UpdatePaymentInformation(TEntity entity, IGatewayResponse response, string paymentProfileId)
        {
            entity.PaymentTransactionId = response.TransactionID;
            entity.PaymentAuthorizationCode = response.AuthorizationCode;
            entity.PaymentReceivedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.PaymentProfileId = paymentProfileId;
            Repository.Save(entity);
        }

        #endregion

        #region Constructors

        protected ControllerBaseWithPaymentHandling(
            ControllerBaseWithPersistenceArguments<TRepository, TEntity, TUser> args) : base(args) { }

        #endregion
    }

    public class VerifyPaymentSummary<TEntity> where TEntity : IEntityWithPayment
    {
        #region Properties

        public int Id { get; set; }
        public TEntity Entity { get; set; }
        public string SelectedPaymentProfileId { get; set; }
        public PaymentProfile SelectedPaymentProfile { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents an entity and the response from Authorize.Net after the
    /// system has tried to authorize payment for it.
    /// </summary>
    [Serializable]
    public class Payment<TEntity> : IPayment<TEntity>
        where TEntity : IEntityWithPayment
    {
        #region Properties

        public TEntity Entity { get; set; }
        public IGatewayResponse Response { get; set; }

        #endregion
    }

    public interface IPayment<TEntity>
    {
        TEntity Entity { get; set; }
        IGatewayResponse Response { get; set; }
    }
}
