using System;
using System.Collections.Generic;
using System.Web.Mvc;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public abstract class SapSyncronizedControllerBaseWithPersisence<TRepository, TEntity, TUser> : ControllerBaseWithPersistence<TRepository, TEntity, TUser>
        where TRepository : class, IRepository<TEntity>
        where TUser : IAdministratedUser
        where TEntity : class, ISAPEntity, new()
    {
        #region Constants

        public const string SAP_ERROR_CODE = "SAPErrorCode Occurred", 
                            SAP_UPDATE_FAILURE = "RETRY::UPDATE FAILURE: ",
                            ASSETS_SAP_ERROR_CODE = "Assets SAPErrorCode Occurred",
                            FACILITIES_SAP_ERROR_CODE = "Facilities SAPErrorCode Occurred";
        public const string WORK1VIEW_LOG_FORMAT_STRING = "{0}-MapCallMVC-{1}-{2}";

        #endregion

        #region Constructors

        public SapSyncronizedControllerBaseWithPersisence(ControllerBaseWithPersistenceArguments<TRepository, TEntity, TUser> args) : base(args)
        {
            _log = _container.GetInstance<ILog>();
            _dateTimeProvider = _container.GetInstance<IDateTimeProvider>();
        }

        #endregion

        #region Private Members 

        protected ILog _log { get; set; }

        protected IDateTimeProvider _dateTimeProvider { get; set; }

        #endregion

        #region Abstract Methods

        protected abstract void UpdateEntityForSap(TEntity entity);
        
        #endregion
        
        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">WorkOrderID</param>
        /// <param name="module">What module for notifications?</param>
        /// <param name="stage">What stage are we at? SAPEntity.WorkOrdersStage</param>
        protected void UpdateSAP(int id, RoleModules module)
        {
            var entity = Repository.Find(id) ?? throw new InvalidOperationException("The entity was reported as saved but could not be retrieved. SAP was not updated and the entity could not be marked as such.");
            try
            {
                UpdateEntityForSap(entity);
            }
            catch (Exception ex)
            {
                entity.SAPErrorCode = SAP_UPDATE_FAILURE +ex.Message;
            }

            Repository.Save(entity);
            SendSapErrorNotification(entity, module);
        }

        protected void SendSapErrorNotification(TEntity entity, RoleModules module)
        {
            //TODO: Move this to a logical formula property and add to the searches.
            if (!string.IsNullOrWhiteSpace(entity.SAPErrorCode) && !entity.SAPErrorCode.StartsWith("RETRY") && !entity.SAPErrorCode.ToUpper().Contains("SUCCESSFUL"))
            {
                var notifier = _container.GetInstance<INotificationService>();
                notifier.Notify(new NotifierArgs
                {
                    //If we have something with an operating center, lets pass along that operating center to the notifier
                    OperatingCenterId = (entity as IThingWithOperatingCenter)?.OperatingCenter?.Id ?? 0,
                    Subject = SAP_ERROR_CODE,
                    Purpose = GetSapPurpose(module),
                    Module = module,
                    Data = new
                    {
                        RecordUrl = GetUrlForModel(entity, "Show", typeof(TEntity).Name, RouteData?.Values["Area"]?.ToString()),
                        entity.SAPErrorCode
                    }
                });
            }
        }
            
        protected void DisplaySapErrorIfApplicable(TEntity entity)
        {
            if (!string.IsNullOrEmpty(entity.SAPErrorCode) && !entity.SAPErrorCode.StartsWith("RETRY") && !entity.SAPErrorCode.ToUpper().Contains("SUCCESSFUL"))
            {
                DisplayErrorMessage("An SAP Error has occurred and must be addressed before SAP is updated.");
            }
        }

        // This method is used by the ShortCycle controllers. I'm not really sure why this
        // one displays the actual message while the DisplaySapErrorIfApplicable method
        // does not.
        protected void TryDisplaySAPErrorCode(string sapErrorCode)
        {
            if (!string.IsNullOrWhiteSpace(sapErrorCode) && !sapErrorCode.ToUpper().Contains("SUCCESS"))
            {
                DisplayErrorMessage(SAP_ERROR_CODE);
            }
        }

        private string GetSapPurpose(RoleModules module)
        {
            var map = new Dictionary<RoleModules, string>() {
                {RoleModules.ProductionFacilities, FACILITIES_SAP_ERROR_CODE},
                {RoleModules.FieldServicesWorkManagement, SAP_ERROR_CODE},
                {RoleModules.FieldServicesAssets, ASSETS_SAP_ERROR_CODE}
            };

            return map.TryGetValue(module, out string output) ? output : SAP_ERROR_CODE;
        }

        #endregion
    }
}