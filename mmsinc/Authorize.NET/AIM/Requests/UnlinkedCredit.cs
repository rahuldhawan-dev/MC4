// ReSharper disable CheckNamespace
namespace AuthorizeNet
{
// ReSharper restore CheckNamespace
    public class UnlinkedCredit:GatewayRequest {
        public UnlinkedCredit(decimal amount, string cardNumber) {
            this.SetApiAction(RequestAction.UnlinkedCredit);
            this.Queue(ApiFields.Amount, amount.ToString());
            this.Queue(ApiFields.CreditCardNumber, cardNumber);
        }
    }
}
