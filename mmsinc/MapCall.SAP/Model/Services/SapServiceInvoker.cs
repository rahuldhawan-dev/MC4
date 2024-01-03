using System;
using System.Linq;
using System.ServiceModel;
using MapCall.SAP.CreateBPEMWS;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAP.SAPEquipmentWS;

namespace MapCall.SAP.Model.Services
{
    public interface ISAPServiceInvoker<TSAPEntity, TSAPServiceClient, TChannel>
        where TSAPEntity : ISAPServiceEntity
        where TSAPServiceClient : ClientBase<TChannel>, TChannel
        where TChannel : class { }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSAPEntity">The SAP entity class being used for the service request</typeparam>
    /// <typeparam name="TSAPServiceClient">The SAP service class that is generated from the WSDL</typeparam>
    /// <typeparam name="TChannel">
    /// The generic type parameter that the TSapServiceClient uses when inheriting from ClientBase{T}. 
    /// This is the interface that contains the actual service methods and that TSapServiceClient implements as well.
    /// </typeparam>
    /// <remarks>
    /// 
    /// The TChannel parameter is only required due to the way the auto-generated service code is created. The 
    /// client must be cast to the TChannel interface in order to call any methods.
    /// 
    /// </remarks>
    public abstract class
        SAPServiceInvoker<TSAPEntity, TSAPServiceClient, TChannel> : ISAPServiceInvoker<TSAPEntity, TSAPServiceClient,
            TChannel>
        where TSAPEntity : ISAPServiceEntity
        where TSAPServiceClient : ClientBase<TChannel>, TChannel
        where TChannel : class
    {
        #region Consts/Structs

        protected const string NO_RECORDS_FOUND = "No records found",
                               RETRY_ERROR_TEXT = SAPHttpClient.RETRY_ERROR_TEXT;

        #endregion

        #region Fields

        private readonly ISAPHttpClient _sapHttpClient;

        #endregion

        #region Properties

        public TimeSpan? SendTimeOut;

        #endregion

        #region Constructors

        protected SAPServiceInvoker(ISAPHttpClient sapHttpClient)
        {
            _sapHttpClient = sapHttpClient;
        }

        #endregion

        #region Private Methods

        protected abstract TSAPEntity InvokeInternal(TSAPEntity sapEntity, TChannel client);

        /// <summary>
        /// Called when the SAP service code encounters an exception. Only override this if
        /// you need to do additional custom logic when an exception occurs. 
        /// </summary>
        /// <param name="sapEntity"></param>
        /// <param name="ex"></param>
        protected virtual void OnException(TSAPEntity sapEntity, Exception ex)
        {
            sapEntity.SAPErrorCode = RETRY_ERROR_TEXT + ex;
        }

        /// <summary>
        /// Called after the SAP service has been called and dealt with regardless of success/failure 
        /// of the service call.
        /// </summary>
        /// <param name="sapEntity"></param>
        protected virtual void AfterInvoke(TSAPEntity sapEntity) { }

        /// <summary>
        /// Do not override this. This is specifically for unit testing where we need to replace
        /// the client with a mocked version in order to test properly.
        /// </summary>
        /// <param name="serviceClient"></param>
        /// <returns></returns>
        protected virtual TChannel CastServiceClientToChannel(TSAPServiceClient serviceClient)
        {
            return serviceClient;
        }

        protected virtual TSAPServiceClient CreateServiceClient()
        {
            if (SendTimeOut.HasValue)
            {
                _sapHttpClient.SendTimeOut = SendTimeOut;
            }

            return _sapHttpClient.CreateServiceClient<TSAPServiceClient, TChannel>();
        }

        #endregion

        #region Public Methods

        public TSAPEntity Invoke(TSAPEntity sapEntity)
        {
            try
            {
                using (var client = CreateServiceClient())
                {
                    var castedClientThatInPracticeShouldBeExactlyTheSameInstance = CastServiceClientToChannel(client);

                    // I do not know if the sapEntity returned is ever a different
                    // instance from the one provided.
                    sapEntity = InvokeInternal(sapEntity, castedClientThatInPracticeShouldBeExactlyTheSameInstance);

                    // NOTE: Disposing the client also closes the connection. There is no need to call Close.
                }
            }
            catch (FaultException ex)
            {
                OnException(sapEntity, ex);
            }

            catch (Exception ex)
            {
                // Apparently we're supposed to eat all exceptions.
                OnException(sapEntity, ex);
            }
            finally
            {
                // Some invokers, like EquipmentResponseInvoker, need to set properties on the
                // object whether or not it failed/succeeded.
                AfterInvoke(sapEntity);
            }

            return sapEntity;
        }

        #endregion
    }
}
