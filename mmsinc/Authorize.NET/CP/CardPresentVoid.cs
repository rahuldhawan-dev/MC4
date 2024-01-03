// ReSharper disable CheckNamespace
namespace AuthorizeNet
{
// ReSharper restore CheckNamespace
    /// <summary>
    /// A Cardpresent Void transaction
    /// </summary>
    public class CardPresentVoid:GatewayRequest {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardPresentVoid"/> class.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        public CardPresentVoid(string transactionID) {
            this.SetApiAction(RequestAction.Void);
            this.Queue("x_ref_trans_id", transactionID);
        }
    }
}
