﻿// ReSharper disable CheckNamespace
namespace AuthorizeNet
{
// ReSharper restore CheckNamespace
    /// <summary>
    /// A request representing a Void of a previously authorized transaction
    /// </summary>
    public class VoidRequest:GatewayRequest {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoidRequest"/> class.
        /// </summary>
        /// <param name="transactionId">The transaction id.</param>
        public VoidRequest(string transactionId) {
            this.SetApiAction(RequestAction.Void);
            this.Queue(ApiFields.TransactionID, transactionId);
        }
    }
}