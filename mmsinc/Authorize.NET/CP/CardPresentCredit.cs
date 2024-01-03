// ReSharper disable CheckNamespace
namespace AuthorizeNet
{
// ReSharper restore CheckNamespace
    /// <summary>
    /// A Credit transaction
    /// </summary>
    public class CardPresentCredit:GatewayRequest {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardPresentCredit"/> class.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        public CardPresentCredit(string transactionID) {
            this.SetApiAction(RequestAction.Credit);
            this.Queue("x_ref_trans_id", transactionID);
        }
    }
}
