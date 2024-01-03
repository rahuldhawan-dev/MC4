// ReSharper disable CheckNamespace
namespace AuthorizeNet
{
// ReSharper restore CheckNamespace
    /// <summary>
    /// Captures a prior authorization
    /// </summary>
    public class CardPresentPriorAuthCapture:GatewayRequest {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardPresentPriorAuthCapture"/> class.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <param name="amount">The amount.</param>
        public CardPresentPriorAuthCapture(string transactionID, decimal amount) {
            this.SetApiAction(RequestAction.PriorAuthCapture);
            this.Queue("x_ref_trans_id", transactionID);
        }
    }
}
