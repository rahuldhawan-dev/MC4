using MapCall.Common.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapCall.SAP.Model.Entities
{
    public enum SAPNotificationCollectionResult
    {
        Success,
        Error,
        NoResults
    }

    /// <summary>
    /// GetNotificationAggregate holds both input and output from SAP
    /// as Invoker needs input and output Entity to be same
    /// </summary>
    public class GetNotificationAggregate : SAPEntity, ISAPServiceEntity
    {
        public SearchSapNotification SearchSapNotifications { get; set; }
        public SAPNotificationCollection SAPNotificationCollections { get; set; }
        public SAPNotification SAPNotification { get; set; }
        public string SAPErrorCode { get; set; }
    }

    public class SAPNotificationCollection : IEnumerable<SAPNotification>
    {
        #region Public Members

        public List<SAPNotification> Items { get; set; }

        /// <summary>
        /// Returns the SAPErrorCode of the first SAPNotification in the Items list.
        /// </summary>
        /// <remarks>
        /// This is based off the implementation that was in SAPNotificationController.
        /// 
        /// This is operating under the assumption that SAP will always return at least one
        /// SAPNotification record, regardless of success/error. The first result will always
        /// have the error message, including if it's a query that does not return any items.
        /// 
        /// </remarks>
        public string SAPErrorCode
        {
            get { return Items.First().SAPErrorCode; }
        }

        /// <summary>
        /// Returns the Result as parsed from the SAPErrorCode.
        /// </summary>
        /// <remarks>
        /// 
        /// At some point it may make sense to move this to SAPNotification instead.
        /// 
        /// </remarks>
        public SAPNotificationCollectionResult Result
        {
            get
            {
                // This is incredibly fragile as we don't have a complete list of what
                // type of SAPErrorCode messages are returned from SAP.

                var errorCode = SAPErrorCode;

                if (errorCode.StartsWith("Success"))
                {
                    return SAPNotificationCollectionResult.Success;
                }

                if (errorCode == "No Records found in SAP for given selection")
                {
                    return SAPNotificationCollectionResult.NoResults;
                }

                return SAPNotificationCollectionResult.Error;
            }
        }

        #endregion

        #region Public Methods

        public SAPNotificationCollection()
        {
            Items = new List<SAPNotification>();
        }

        public IEnumerator<SAPNotification> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            // Why is this not implemented? -Ross 1/4/2018
            throw new NotImplementedException();
        }

        #endregion
    }
}
